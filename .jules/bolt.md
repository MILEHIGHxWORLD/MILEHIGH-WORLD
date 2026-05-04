## 2025-05-14 - [SceneDirector Code Rot Cleanup]
**Learning:** Found significant code rot and syntax errors in SceneDirector.cs, including stray characters and broken loop nesting that would have blocked compilation.
**Action:** Always perform a manual code health check on core scripts before implementing tests to ensure a stable baseline.

## 2025-05-14 - [Unity Testing via .NET CLI]
**Learning:** In environments without the Unity Editor, logic can be verified by bootstrapping a temporary .NET console project and stubbing Unity-specific dependencies.
**Action:** Use this 'stubbing' pattern to gain confidence in C# logic when the full Unity engine is unavailable.

## 2025-05-14 - [Nullable Reference Type (NRT) in Unity Caches]
**Learning:** Using `GameObject?` in Dictionaries helps satisfy NRT requirements and clarifies the intent of 'negative caching' (explicitly caching a missing object).
**Action:** Prefer nullable types for cache values when the cache is expected to store null results from lookups like `GameObject.Find`.
