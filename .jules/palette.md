
## 2026-04-22 - Dialogue Sequence Refactoring & UX Polish
**Learning:** Encapsulating repetitive cinematic sequences into a single helper method (e.g., PlayDialogueLine) not only improves readability but also allows for consistent injection of micro-UX improvements like nameplate 'pop' animations and interruptible wait logic (skip logic) across the entire sequence.
**Action:** Always look to centralize dialogue line handling to ensure UI feedback (like scaling animations or punctuation pauses) remains consistent and easily adjustable.
