# LINC — Into the Void city prototype

## Design brief

Build one small, replayable city-management loop: connect streets to the city core, place buildings, support residents, balance revenue and upkeep, and hold against periodic Void surges. The default is **steady mode**: time stays paused until the player advances a cycle. Optional automatic play advances once every eight seconds.

This is a **proposed experimental district scenario**, separate from the Micah mission in `Into-the-Void-First-Playable.md`. LINC and Into the Void belong to the established project; the district staging, building catalogue, economic values, city stability metric, pressure model, and victory conditions here are new prototype rules. They are not psyche stats, dialogue gates, combat mechanics, or canon decisions. No character values or lore files are changed.

## Play

Open `index.html` in a current desktop or mobile browser. All source files are local; there are no dependencies, external assets, accounts, network requests, or paid resources. On a web server, keep `simulation.js` beside `index.html`.

To produce **one portable HTML game** that works without the companion script:

```sh
node Prototypes/LincCity/build.cjs
```

Open the resulting `Prototypes/LincCity/dist/LINC-City-Prototype.html` in a browser. The build output is generated, not committed.

Select a tool and tap a tile. Drag to place streets or reclaim tiles. Keyboard: 1–8 select tools, arrow keys move map selection, Enter places the selected tool. Space advances the city unless the map or a form control has focus. The play button runs the city automatically; opening help, starting a new attempt, and hiding the page pause automatic play.

**One reachable win:** add Habitats at C4 and G4, an Exchange at C6, a Power core at G6, Waterworks at F3, a Clinic at D6, and a Rift ward at F6. The starting streets already connect all seven sites. At standard funding and 10% tax, advance nine cycles to reach the first win. Other layouts work too.

## Simulation

- Streets connected orthogonally to the immutable central core form the service network. Buildings must touch a street and only operate when that street connects to the core. Isolated buildings still incur upkeep.
- Construction spends credits immediately and cannot exceed available funds. Reclaim returns 35% of construction cost. Removing a street can disconnect services; removing housing also caps the population.
- The core supplies 8 power and water for 16 residents. Power cores add 20 power; powered Waterworks add 44 water capacity. Habitats house 8 people each. Underpowered buildings reduce water, care, trade capacity, and ward protection.
- An Exchange serves up to 24 residents and earns 1.35 credits per served resident each cycle. At 10% tax, each resident generates one credit per cycle. All built streets and buildings incur upkeep.
- Clinics serve 40 residents and wards remove 7 pressure per cycle at full power and standard funding. Lean/expanded funding adjusts only clinic and ward capacity and upkeep. Excessive taxes reduce stability.
- Pressure rises by 2.5 each cycle, plus 1.5 per isolated building. Every fifth cycle adds a telegraphed 13-point surge before ward protection. High pressure damages city stability.
- Success: at least 32 residents, full power/water/care, positive cycle balance, stability at least 70%, and pressure below 45% for **four consecutive cycles**. Success pauses the city; Continue enters sandbox play. Failure: stability reaches zero, or treasury remains below −250 credits for three cycles. Terminal events cannot advance the city again.
- Save/Resume uses a versioned local browser checkpoint. Invalid or unsupported saves are rejected without replacing the current city. Storage errors are visible. New city resets the attempt but keeps the saved slot until the player explicitly saves again. This is local prototype storage, not account sync or the existing Unity persistence service.

## Validation

```sh
node --test Prototypes/LincCity/tests/simulation.test.cjs
node Prototypes/LincCity/build.cjs
```

The simulation tests cover atomic construction, street connectivity, service loss, budget conservation, a reachable win, mission streak reset, surge timing, deterministic checkpoint replay, terminal protection, failure/retry, and corrupted saves. Browser smoke coverage and viewport results are recorded in `VALIDATION.md`.

## Integration boundary

The inspected repository contains Unity C# source but no `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, scenes, or assembly definitions in the inspected tree. The root `AGENTS.md` directs a lightweight HTML/CSS/JavaScript browser game. This prototype therefore lives outside `Assets/` and does not claim to be a compiled Unity or Quest build. `simulation.js` has no DOM dependencies so its rules can be reviewed or ported after the actual Unity configuration and authoritative subsystem contracts are available.
