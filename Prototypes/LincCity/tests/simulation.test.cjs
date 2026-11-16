'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const { City, BUILDINGS } = require('../simulation.js');

function readyDistrict() {
  const city = new City();
  for (const [kind, tile] of [
    ['habitat',29], ['habitat',33], ['exchange',47], ['power',51],
    ['water',23], ['clinic',48], ['ward',50]
  ]) assert.equal(city.place(kind,tile).ok,true,kind+' placement');
  return city;
}

test('construction is atomic for invalid, occupied, unaffordable and inherited tool names', () => {
  const city = new City();
  for (const [kind,index] of [['habitat',-1],['habitat',81],['habitat',10.5],['habitat',40],
    ['road',0],['habitat',4],['toString',29],['__proto__',29],['constructor',29],['unknown',29]]) {
    const before=city.serialize();assert.equal(city.place(kind,index).ok,false);assert.equal(city.serialize(),before);
  }
  const s=city.state;s.credits=10;const poor=new City(s);
  assert.equal(poor.place('road',13).ok,false);assert.equal(poor.state.credits,10);
});

test('a disconnected street does not activate a building; deleting the bridge removes service', () => {
  const city=new City();
  city.place('road',20);city.place('power',11);
  assert.equal(city.stats().powerSupply,8);
  assert.ok(!city.stats().active.includes(11));
  city.place('road',21);assert.equal(city.stats().powerSupply,28);
  const before=city.state.credits;
  city.place('bulldoze',21);assert.equal(city.stats().powerSupply,8);
  assert.equal(city.state.credits-before,Math.floor(BUILDINGS.road.cost*0.35));
  const removed=city.serialize();assert.equal(city.place('bulldoze',21).ok,false);assert.equal(city.serialize(),removed);
});

test('brownouts reduce water, healthcare and ward output; funding changes capacity and upkeep', () => {
  const city=readyDistrict();const a=city.stats();
  assert.equal(a.power,1);assert.equal(a.water,1);assert.equal(a.care,1);assert.equal(a.mitigation,7);
  city.configure(10,50);const lean=city.stats();
  assert.equal(lean.careCapacity,20);assert.equal(lean.mitigation,3.5);assert.equal(lean.upkeep,a.upkeep-8);
  city.configure(10,100);city.place('bulldoze',51);const brownout=city.stats();
  assert.ok(brownout.power<1);assert.ok(brownout.careCapacity<a.careCapacity);
  assert.ok(brownout.waterCapacity<a.waterCapacity);assert.ok(brownout.mitigation<7);
});

test('cash flow conserves treasury and includes both taxes and trade', () => {
  const city=readyDistrict();const old=city.state.credits;city.step();
  const s=city.state,stats=city.stats();
  assert.equal(s.credits,Math.round((old+stats.income-stats.upkeep)*100)/100);
  assert.ok(stats.taxRevenue>0);assert.ok(stats.tradeRevenue>0);
});

test('a complete reachable district wins and terminal state rejects extra events', () => {
  const city=readyDistrict();let turns=0;
  while(city.state.phase==='playing'&&turns++<25) city.step();
  assert.equal(city.state.phase,'won');assert.ok(city.state.population>=32);assert.equal(city.state.streak,4);
  assert.ok(city.stats().net>0);assert.ok(city.state.pressure<45);
  const terminal=city.serialize();assert.equal(city.step().ok,false);assert.equal(city.place('road',13).ok,false);
  assert.equal(city.configure(12,150),false);assert.equal(city.serialize(),terminal);
  assert.equal(city.continue(),true);assert.equal(city.state.sandbox,true);assert.equal(city.step().ok,true);
});

test('a service failure resets consecutive mission progress', () => {
  const city=readyDistrict();while(city.state.streak<2)city.step();
  city.place('bulldoze',48);city.step();assert.equal(city.state.streak,0);
});

test('surges are deterministic and telegraphed for the fifth cycle', () => {
  const city=new City();for(let i=0;i<4;i++)assert.equal(city.step().surge,false);
  assert.equal(city.stats().surgeIn,1);const before=city.state.pressure;
  assert.equal(city.step().surge,true);assert.equal(city.state.pressure-before,15.5);
  const restored=City.restore(city.serialize());city.step();restored.step();
  assert.equal(restored.serialize(),city.serialize());
});

test('debt failure and new attempts do not carry terminal progress', () => {
  const state=new City().state;state.credits=-400;const city=new City(state);
  for(let i=0;i<3;i++)city.step();assert.equal(city.state.phase,'lost');
  const ended=city.serialize();assert.equal(city.step().ok,false);assert.equal(city.serialize(),ended);
  const fresh=new City();assert.equal(fresh.state.phase,'playing');assert.equal(fresh.state.cycle,0);
  assert.equal(fresh.state.streak,0);assert.equal(fresh.state.debtCycles,0);
});

test('unprotected high pressure can collapse a district and stop the simulation', () => {
  const state=new City().state;state.pressure=100;state.stability=1;
  const city=new City(state);city.step();assert.equal(city.state.phase,'lost');assert.equal(city.state.stability,0);
});

test('checkpoints round-trip playing, won and lost states without sharing mutable arrays', () => {
  const states=[readyDistrict()];const win=readyDistrict();while(win.state.phase==='playing')win.step();states.push(win);
  const failed=new City().state;failed.credits=-400;const lose=new City(failed);for(let i=0;i<3;i++)lose.step();states.push(lose);
  for(const city of states){const copy=City.restore(city.serialize());assert.deepEqual(copy.state,city.state);const s=copy.state;s.grid[40]='road';assert.equal(copy.state.grid[40],'core');}
});

test('corrupt or unsupported saves and invalid budget inputs fail without mutation', () => {
  for(const bad of ['not json','null','[]','{}','x'.repeat(20001)])assert.throws(()=>City.restore(bad));
  for(const change of [s=>s.version=2,s=>s.grid.pop(),s=>s.grid[40]='road',s=>s.grid[0]='empty',
    s=>s.credits=null,s=>s.population=2.5,s=>s.tax=17,s=>s.funding=75,s=>s.phase='mystery']){
    const s=new City().state;change(s);assert.throws(()=>City.restore(JSON.stringify(s)));
  }
  const city=new City();const before=city.serialize();
  for(const values of [[NaN,100],[10,75],[10.5,100],[6,Infinity]])assert.equal(city.configure(...values),false);
  assert.equal(city.serialize(),before);
});
