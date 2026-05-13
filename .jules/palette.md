## 2025-05-14 - Responsive 'Fast Skip' Dialogue Pattern
**Learning:** In dialogue systems with typewriter effects and subsequent pauses, users often want to skip both with a single input. Moving the 'skipRequested' flag reset from the end of the reveal animation to the end of the pause (via a custom 'WaitForSecondsOrSkip' coroutine) creates a much more responsive and less frustrating 'fast skip' experience.
**Action:** Always implement a 'WaitForSecondsOrSkip' pattern for dialogue-related pauses in skippable cinematics, ensuring that the skip intent carries through from the text reveal to the beat pause.

## 2025-05-14 - Subtle Visual Anchors with Pop Scaling
**Learning:** A subtle 0.2s scaling animation (approx. 1.15x) on UI text elements like speaker names during state changes provides a clear visual anchor that improves interface polish without interrupting user flow.
**Action:** Use short, non-intrusive scaling animations to draw attention to UI state changes (like speaker transitions) to make the interface feel more reactive and polished.
