# Validation record

Prepared November 13, 2026 (America/Los_Angeles).

## Passed

- Eleven Node simulation tests pass with no dependencies: atomic construction, disconnection/reconnection, service degradation, service funding, treasury conservation, a complete win, consecutive-cycle reset, deterministic surges, debt and stability failure, terminal protection, clean new attempts, checkpoint round-trips, and corrupt save rejection.
- The documented layout reaches victory at cycle 9 with 32 residents, 499.2 credits, approximately 92.8% city stability, 10.5% pressure, and +12.4 credits per cycle at standard funding and 10% tax.
- `node build.cjs` produces one HTML file with the full simulation, UI, styles, and procedural canvas art included. No external assets or dependencies are required.

## Unverified / blocked

- **Actual browser rendering and interactions:** the remote browser could not connect to the local HTTP preview (`ERR_CONNECTION_REFUSED`). An attempted local-file preview was rejected by its protocol security policy. No alternate browser mechanism was used to bypass that policy.
- **Mobile device behavior and responsive visual QA:** mobile CSS, touch pointer controls, and keyboard controls are implemented, but have not been exercised in a browser or on an Android device in this environment.
- **Browser storage:** save failures and invalid payloads have handling; the checkpoint data model is tested, but actual storage availability, persistence across browser sessions, and Resume controls need browser verification.
- **Unity/Quest:** this is a browser prototype, not a Unity editor compilation, player build, or VR performance result. The repository tree inspected at `eced0a70cbedd3d1e32a184eb6071f90e3a08e0d` has no editor-version file, Unity package manifest, scenes, or assembly definitions. Unity source and gameplay integrations remain unverified.
- **Production hosting:** no public deployment was made.

## Manual browser acceptance

1. Open the portable HTML in current Chrome/Edge/Safari/Firefox, at desktop width and a 360–430 px mobile width. Confirm readable controls, a rendered map, and no page overflow or console errors.
2. Tap/select buildings and place the documented win layout. Drag streets; remove a connected street and confirm isolated buildings lose services. Try an occupied tile and a fracture; treasury must not change.
3. Advance to cycle 9 and verify victory pauses the city. Continue must enter sandbox mode without repeating victory. New city must reset the attempt.
4. Save mid-game, advance a cycle, Resume, and confirm the earlier map, treasury, population, and cycle return. Reload the page and Resume again. Verify an unavailable/full browser storage state reports a visible save failure.
5. Run automatic play, open help, and hide the tab. Confirm automatic advancement pauses. Exercise keyboard controls and reduced-motion mode.
