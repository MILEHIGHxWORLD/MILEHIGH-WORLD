/* Proposed LINC city prototype. City metrics are not psyche-system values. */
(function (root) {
  'use strict';
  const SIZE = 9;
  const CORE = 40;
  const RIFTS = Object.freeze([0, 1, 8, 9, 71, 72, 79, 80]);
  const BUILDINGS = Object.freeze({
    road: { name: 'Street', cost: 12, upkeep: 0.5, power: 0 },
    habitat: { name: 'Habitat', cost: 160, upkeep: 2, power: 2, homes: 8 },
    exchange: { name: 'Exchange', cost: 220, upkeep: 6, power: 3, trade: 24 },
    power: { name: 'Power core', cost: 240, upkeep: 10, power: 0, supply: 20 },
    water: { name: 'Waterworks', cost: 180, upkeep: 8, power: 2, water: 44 },
    clinic: { name: 'Clinic', cost: 260, upkeep: 8, power: 3, care: 40 },
    ward: { name: 'Rift ward', cost: 300, upkeep: 8, power: 4, ward: 7 }
  });
  const clamp = (v, lo, hi) => Math.max(lo, Math.min(hi, v));
  const money = v => Math.round(v * 100) / 100;
  const adjacent = index => {
    const x = index % SIZE, y = Math.floor(index / SIZE), out = [];
    if (x > 0) out.push(index - 1);
    if (x < SIZE - 1) out.push(index + 1);
    if (y > 0) out.push(index - SIZE);
    if (y < SIZE - 1) out.push(index + SIZE);
    return out;
  };
  function initialState() {
    const grid = Array(SIZE * SIZE).fill('empty');
    RIFTS.forEach(i => { grid[i] = 'rift'; });
    grid[CORE] = 'core';
    [22, 31, 38, 39, 41, 42, 49, 58].forEach(i => { grid[i] = 'road'; });
    grid[30] = grid[32] = 'habitat';
    return { version: 1, grid, credits: 2000, population: 8, cycle: 0,
      stability: 70, pressure: 38, tax: 10, funding: 100,
      streak: 0, debtCycles: 0, phase: 'playing', sandbox: false, reason: '' };
  }
  function validate(input) {
    if (!input || typeof input !== 'object' || input.version !== 1)
      throw new Error('This checkpoint uses an unsupported format.');
    const valid = new Set(['empty', 'core', 'rift', ...Object.keys(BUILDINGS)]);
    if (!Array.isArray(input.grid) || input.grid.length !== SIZE * SIZE ||
      input.grid.some((cell, i) => !valid.has(cell) ||
        (i === CORE ? cell !== 'core' : cell === 'core') ||
        (RIFTS.includes(i) ? cell !== 'rift' : cell === 'rift')))
      throw new Error('The checkpoint map is damaged.');
    const ranges = { credits: [-1000000, 10000000], population: [0, 648],
      cycle: [0, 100000], stability: [0, 100], pressure: [0, 100],
      tax: [6, 16], funding: [50, 150], streak: [0, 4], debtCycles: [0, 3] };
    for (const [key, range] of Object.entries(ranges)) {
      if (!Number.isFinite(input[key]) || input[key] < range[0] || input[key] > range[1])
        throw new Error('The checkpoint has an invalid ' + key + ' value.');
    }
    for (const key of ['population', 'cycle', 'tax', 'streak', 'debtCycles'])
      if (!Number.isInteger(input[key])) throw new Error('The checkpoint has an invalid ' + key + ' value.');
    if (![50, 100, 150].includes(input.funding) ||
      !['playing', 'won', 'lost'].includes(input.phase) || typeof input.sandbox !== 'boolean' ||
      typeof input.reason !== 'string' || input.reason.length > 180)
      throw new Error('The checkpoint has an invalid mission state.');
    // Copy only the explicit schema. Never merge untrusted save properties.
    const output = initialState();
    for (const key of Object.keys(output)) output[key] = key === 'grid' ? input.grid.slice() : input[key];
    return output;
  }
  class City {
    constructor(checkpoint) { this._state = checkpoint === undefined ? initialState() : validate(checkpoint); }
    get state() { return JSON.parse(JSON.stringify(this._state)); }
    snapshot() { return this.state; }
    serialize() { return JSON.stringify(this._state); }
    static restore(text) {
      if (typeof text !== 'string' || text.length > 20000) throw new Error('The checkpoint is too large or unreadable.');
      let data;
      try { data = JSON.parse(text); } catch (_) { throw new Error('The checkpoint could not be read.'); }
      return new City(data);
    }
    stats() {
      const s = this._state, connected = new Set([CORE]), queue = [CORE];
      // Only streets and the immutable city core carry network connectivity.
      for (let q = 0; q < queue.length; q++) {
        for (const i of adjacent(queue[q])) {
          if (s.grid[i] === 'road' && !connected.has(i)) { connected.add(i); queue.push(i); }
        }
      }
      const active = new Set();
      s.grid.forEach((kind, i) => {
        if (BUILDINGS[kind] && kind !== 'road' && adjacent(i).some(n => connected.has(n) && s.grid[n] === 'road')) active.add(i);
      });
      let capacity = 0, powerSupply = 8, demand = 0, waterBase = 16, careBase = 0,
        tradeBase = 0, wardBase = 0, upkeep = 0, isolated = 0;
      const funding = s.funding / 100;
      s.grid.forEach((kind, i) => {
        const b = BUILDINGS[kind];
        if (!b) return;
        upkeep += b.upkeep * (kind === 'clinic' || kind === 'ward' ? funding : 1);
        if (!active.has(i)) { if (kind !== 'road') isolated++; return; }
        capacity += b.homes || 0;
        powerSupply += b.supply || 0;
        demand += b.power;
        waterBase += b.water || 0;
        careBase += (b.care || 0) * funding;
        tradeBase += b.trade || 0;
        wardBase += (b.ward || 0) * funding;
      });
      const power = Math.min(1, powerSupply / Math.max(1, demand));
      const waterCapacity = 16 + (waterBase - 16) * power;
      const water = Math.min(1, waterCapacity / Math.max(1, capacity, s.population));
      const careCapacity = careBase * power;
      const care = Math.min(1, careCapacity / Math.max(1, capacity, s.population));
      const commerce = Math.min(1, tradeBase * power / Math.max(1, s.population));
      const taxRevenue = s.population * s.tax / 10;
      const tradeRevenue = Math.min(s.population, tradeBase * power) * 1.35;
      const income = money(taxRevenue + tradeRevenue);
      upkeep = money(upkeep);
      const net = money(income - upkeep);
      const ready = s.population >= 32 && power >= 0.99 && water >= 0.99 && care >= 0.99 &&
        s.stability >= 70 && s.pressure < 45 && net > 0;
      return { connected: [...connected], active: [...active], capacity, powerSupply, demand,
        power, waterCapacity, water, careCapacity, care, commerce, taxRevenue: money(taxRevenue),
        tradeRevenue: money(tradeRevenue), income, upkeep, net, isolated,
        mitigation: wardBase * power, ready, surgeIn: 5 - s.cycle % 5 };
    }
    place(kind, index) {
      const s = this._state;
      if (s.phase !== 'playing') return { ok: false, message: 'Choose Continue or New city before building.' };
      if (!Number.isInteger(index) || index < 0 || index >= SIZE * SIZE)
        return { ok: false, message: 'Choose a tile inside the district.' };
      if (typeof kind !== 'string' || (!Object.prototype.hasOwnProperty.call(BUILDINGS, kind) && kind !== 'bulldoze'))
        return { ok: false, message: 'Choose a building tool.' };
      const old = s.grid[index];
      if (kind === 'bulldoze') {
        if (!BUILDINGS[old]) return { ok: false, message: old === 'core' ? 'The city core cannot be removed.' : 'There is nothing removable on this tile.' };
        const refund = Math.floor(BUILDINGS[old].cost * 0.35);
        s.grid[index] = 'empty'; s.credits = money(s.credits + refund);
        s.population = Math.min(s.population, this.stats().capacity);
        return { ok: true, message: BUILDINGS[old].name + ' removed. Reclaimed ' + refund + ' credits.' };
      }
      if (old !== 'empty') return { ok: false, message: old === 'rift' ? 'The Void fracture cannot be built on.' : 'This tile is already occupied.' };
      if (kind !== 'road' && !adjacent(index).some(i => s.grid[i] === 'road'))
        return { ok: false, message: 'Buildings need a street beside them.' };
      if (s.credits < BUILDINGS[kind].cost) return { ok: false, message: 'Not enough credits. Advance a cycle or reclaim a building.' };
      s.grid[index] = kind; s.credits = money(s.credits - BUILDINGS[kind].cost);
      const online = kind === 'road' ? this.stats().connected.includes(index) : this.stats().active.includes(index);
      return { ok: true, message: BUILDINGS[kind].name + ' built.' + (online ? '' : ' Connect its street to the city core to bring it online.') };
    }
    configure(tax, funding) {
      if (this._state.phase !== 'playing') return false;
      if (!Number.isInteger(tax) || tax < 6 || tax > 16 || ![50, 100, 150].includes(funding)) return false;
      this._state.tax = tax; this._state.funding = funding; return true;
    }
    step() {
      const s = this._state;
      if (s.phase !== 'playing') return { ok: false, message: 'The mission has ended.' };
      let stats = this.stats();
      s.cycle++;
      const surge = s.cycle % 5 === 0;
      s.pressure = clamp(s.pressure + 2.5 + stats.isolated * 1.5 + (surge ? 13 : 0) - stats.mitigation, 0, 100);
      const target = clamp(100 * (0.3 * stats.power + 0.3 * stats.water + 0.15 * stats.care +
        0.1 * stats.commerce + 0.15 * (1 - s.pressure / 100)) - Math.max(0, s.tax - 10) * 3, 0, 100);
      s.stability = clamp(s.stability + (target - s.stability) * 0.22 - Math.max(0, s.pressure - 65) * 0.45, 0, 100);
      const supported = Math.min(stats.capacity, Math.floor(stats.waterCapacity));
      if (stats.power >= 0.8 && stats.water >= 0.8 && s.stability >= 45)
        s.population = Math.min(supported, s.population + 4);
      else s.population = Math.max(0, Math.min(stats.capacity, s.population - 2));
      stats = this.stats();
      s.credits = money(s.credits + stats.net);
      s.debtCycles = s.credits < -250 ? Math.min(3, s.debtCycles + 1) : 0;
      s.streak = stats.ready ? Math.min(4, s.streak + 1) : 0;
      if (s.stability <= 0 || s.debtCycles >= 3) {
        s.phase = 'lost';
        s.reason = s.stability <= 0 ? 'City stability reached zero.' : 'The city remained more than 250 credits in debt for three cycles.';
      } else if (s.streak >= 4 && !s.sandbox) {
        s.phase = 'won'; s.reason = 'LINC held steady for four cycles. The district is secure.';
      }
      return { ok: true, surge, message: surge ? 'Void surge: +13 pressure before ward protection.' : 'Cycle complete. Budget and services updated.' };
    }
    continue() {
      if (this._state.phase !== 'won') return false;
      this._state.phase = 'playing'; this._state.sandbox = true; this._state.reason = ''; return true;
    }
  }
  const api = Object.freeze({ City, SIZE, CORE, RIFTS, BUILDINGS, adjacent });
  if (typeof module !== 'undefined' && module.exports) module.exports = api;
  else root.LincCity = api;
})(typeof globalThis !== 'undefined' ? globalThis : this);
