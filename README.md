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
