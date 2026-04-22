
## 2026-04-22 - Dialogue Sequence Refactoring & UX Polish
**Learning:** Encapsulating repetitive cinematic sequences into a single helper method (e.g., PlayDialogueLine) not only improves readability but also allows for consistent injection of micro-UX improvements like nameplate 'pop' animations and interruptible wait logic (skip logic) across the entire sequence.
**Action:** Always look to centralize dialogue line handling to ensure UI feedback (like scaling animations or punctuation pauses) remains consistent and easily adjustable.

## 2026-04-22 - CI Failure: Nullable Reference Types
**Learning:** In strict C# environments (CI with <TreatWarningsAsErrors>), all public fields in Unity Inspector classes must be initialized with 'null!' or declared as nullable to satisfy CS8618/CS8625, even if the CI reports a 'billing issue'.
**Action:** Proactively initialize all Inspector fields with 'null!' when creating or refactoring MonoBehaviour scripts.
