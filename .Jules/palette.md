## 2026-05-20 - Multi-namespace Ambiguity Resolution
**Learning:** In Unity projects with overlapping names (e.g., `CharacterData` as both a `ScriptableObject` and a `struct`), renaming the `ScriptableObject` to something distinct like `CharacterProfileData` prevents compilation errors and reduces the need for verbose fully-qualified names in high-frequency logic.
**Action:** Renamed `CharacterData.cs`'s class to `CharacterProfileData` and updated all dependent controllers and directors.
