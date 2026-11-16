# MILEHIGH.WORLD: INTO THE VOID

This repository contains the Unity C# scripts and campaign data for the "Into the Void" cinematic sequence.

## Project Structure

- `Assets/Scripts/Cinematics`: Contains the main cinematic controller `Cinematic_IntoTheVoid.cs`.
- `Assets/Scripts/Core`: Core managers for the campaign, cameras, and abilities.
- `Assets/Scripts/Data`: JSON campaign data and C# data models.
- `Packages/`: Unity package metadata.

## Micro-UX Enhancements (Palette)

The cinematic sequence features a polished dialogue system with:
- **Rhythmic Typewriter Effect:** Pacing adjusted for natural speech (punctuation pauses).
- **Speaker Identification:** Color-coded speaker names and matching completion cues.
- **Layout Stability:** Pre-calculated text layout to prevent jarring shifts during reveal.
- **Optimized Performance:** Cached yields and zero-allocation typewriter loops.

## Playable city prototype

[LINC — city building and management](Prototypes/LincCity/README.md) is a proposed experimental browser scenario: lay streets, build homes and services, balance the budget, and hold against Void surges. Open `Prototypes/LincCity/index.html` with its companion script, or run `node Prototypes/LincCity/build.cjs` to generate one portable HTML game. Time starts paused; desktop and mobile layouts are included.

This prototype lives outside the Unity assets and does not change character, combat, narrative, or psyche systems. See its [validation record](Prototypes/LincCity/VALIDATION.md) for verified behavior and remaining device checks.
