## 2024-05-24 - Unity GameObject.Find Performance Bottleneck
**Learning:** In Unity 3D scripts like `SceneDirector.cs` within this project, methods frequently call `GameObject.Find()` to search for characters or interactive objects by name during scene setups. This forces Unity to traverse the entire scene hierarchy repeatedly (O(N) operation), which can cause noticeable load times or frame drops if called continuously or on large scenes.
**Action:** Replace direct `GameObject.Find()` calls with a `Dictionary<string, GameObject>` cache when objects are fetched by name multiple times or instantiated dynamically. Check `cachedObj != null` to handle Unity's custom object destruction logic safely before returning a cached reference.

## 2024-03-16 - Unity Coroutine Yield Instruction Caching
**Learning:** Found that using inline `new WaitForSeconds(time)` repeatedly inside Unity coroutines (especially in heavily choreographed cinematic sequences) generates unnecessary Garbage Collection allocations every time the coroutine hits that yield instruction, which can lead to GC spikes and frame stuttering.
**Action:** Always cache frequently used yield instructions like `WaitForSeconds` in a shared static dictionary or similar caching mechanism, so that existing yield objects are reused and no extra garbage is created per sequence execution.
## 2025-05-14 - [SceneDirector Code Rot Cleanup]
**Learning:** Found significant code rot and syntax errors in SceneDirector.cs, including stray characters and broken loop nesting that would have blocked compilation.
**Action:** Always perform a manual code health check on core scripts before implementing tests to ensure a stable baseline.

## 2025-05-14 - [Unity Testing via .NET CLI]
**Learning:** In environments without the Unity Editor, logic can be verified by bootstrapping a temporary .NET console project and stubbing Unity-specific dependencies.
**Action:** Use this 'stubbing' pattern to gain confidence in C# logic when the full Unity engine is unavailable.

## 2025-05-14 - [Nullable Reference Type (NRT) in Unity Caches]
**Learning:** Using `GameObject?` in Dictionaries helps satisfy NRT requirements and clarifies the intent of 'negative caching' (explicitly caching a missing object).
**Action:** Prefer nullable types for cache values when the cache is expected to store null results from lookups like `GameObject.Find`.
## 2024-05-15 - Shader 'pow()' optimization
**Learning:** Using `pow(x, c)` for small constant integer exponents in pixel-evaluated fragment shaders can be highly unoptimized depending on the graphics API.
**Action:** Always rewrite integer-based exponentiation as explicit variable multiplications. For example, use `x * x * x` instead of `pow(x, 3.0)` to save ALU cycles and improve GPU fragment processing times.

## 2025-03-27 - Dead Code Elimination in Surface Shaders
**Learning:** Identified a performance bottleneck in `HyperPBRCharacter_4D.shader` where a complex Subsurface Scattering (SSS) block was being calculated but immediately overwritten by a final Albedo assignment. This caused wasted GPU fragment shader cycles and redundant texture samples per pixel.
**Action:** Always audit surface shader functions for value overwrites. Removing dead logic blocks that do not contribute to the final output is a high-impact optimization. When optimizing, preserve `Properties` to maintain material backward compatibility.
## 2026-03-25 - [Redundant Member Clutter Performance Impact]
**Learning:** The 'SceneDirector.cs' file was severely cluttered with over a dozen redundant dictionary declarations and duplicate helper methods for GameObject caching. This not only increases memory overhead but also creates a "state fragmentation" risk where different parts of the initialization loop use different caches, leading to redundant O(N) traversals despite the caching intent.
**Action:** Always audit caching implementations for redundancy. Consolidate into a single, unified caching pattern to ensure O(1) lookups are consistent across the entire system.

## 2026-04-14 - Shader Dead Code and Scene Traversal Optimizations
**Learning:** Found a critical GPU bottleneck in `HyperPBRCharacter_4D.shader` where heavy SSS calculations were performed but immediately overwritten by a final albedo assignment. Additionally, identified that `SceneDirector.cs` was performing O(N) scene traversals inside loops during setup.
**Action:** Remove dead shader code to save fragment cycles. In `SceneDirector.cs`, pre-populate the `_objectCache` with a single `Object.FindObjectsOfType<GameObject>()` call at the start of `SetupScene` to reduce complexity from O(N*M) to O(N+M). Use `ReferenceEquals` for fast null checking in negative caches to distinguish between uninitialized slots and destroyed Unity objects.
## 2025-05-15 - [Unity MaterialPropertyBlock Optimization]
**Learning:** Accessing `Renderer.material` in Unity (e.g., in `CombatManager.TriggerEnemyGlitch`) triggers a material clone on the heap, which increases memory overhead and breaks draw call batching (GPU instancing/SRP batcher).
**Action:** Always use `MaterialPropertyBlock` for per-renderer shader property overrides. Cache property IDs with `Shader.PropertyToID` and reuse a static/shared `MaterialPropertyBlock` instance where possible to eliminate GC allocations during updates.

## 2026-04-22 - Surgical Cache Optimization in SceneDirector
**Learning:** In 'SceneDirector.cs', prefab lookups were O(P) linear searches inside an O(C) character spawning loop, creating an O(C*P) initialization bottleneck. Additionally, aggressive cache clearing during scenario updates forced redundant O(N) scene traversals for persistent objects.
**Action:** Implemented a 'Dictionary<string, GameObject>' prefab lookup cache for O(1) retrieval and enabled persistent surgical lazy-loading by removing redundant 'Clear()' calls. Added 'OnDestroy' to release Unity object references and 'null!' initializers to satisfy strict CI compilation checks.

## 2026-05-10 - SceneDirector Performance and CI Fixes
**Learning:** Found a performance bottleneck in 'SceneDirector.cs' where character prefabs were searched linearly O(P) in an O(C) loop, and caches were cleared unnecessarily. Additionally, identified critical syntax errors in 'HorizonGameData.cs' and 'CampaignManager.cs' that were blocking CI.
**Action:** Implemented a 'Dictionary<string, GameObject>' prefab lookup cache for O(1) retrieval and enabled persistent object caching. Fixed all syntax errors and added 'null!' initializers to satisfy strict CI Nullable Reference Type checks in affected files.
## 2025-05-14 - Optimized Prefab Lookup and Instantiation
**Learning:** Linear searches in a List<GameObject> (O(P)) inside a spawning loop (O(C)) creates an O(C*P) bottleneck that scales poorly with character and prefab counts. Additionally, synchronous instantiation of multiple complex characters causes frame drops.
**Action:** Always use a Dictionary for prefab lookups and implement Coroutines to spread heavy instantiation across frames.

## 2026-03-26 - [Advanced Unity Caching & Memory Efficiency]
**Learning:** When caching Unity components, using `InstanceID` (int) as a dictionary key is significantly more efficient than strings or `ToString()` calls, as it avoids heap allocations during every lookup. Furthermore, robust negative caching for Unity objects (which override `== null`) requires using `ReferenceEquals(obj, null)` to distinguish between a legitimate `null` cache entry and a reference to a natively destroyed object (fake null).
**Action:** Use `int` (GetInstanceID) for component cache keys and `ReferenceEquals` to manage the lifecycle of cached Unity objects correctly.
## 2026-03-26 - Unity Negative Caching Pitfalls
**Learning:** In Unity managers like `SceneDirector.cs` that both find and instantiate objects, "negative caching" (storing `null` in the dictionary when `GameObject.Find` fails) is a dangerous anti-pattern. If an object is instantiated later in the same frame or scenario, subsequent lookups will incorrectly return the cached `null` instead of the newly created object. Furthermore, Unity's `obj != null` check is essential even for cached references to detect if the native C++ object was destroyed.
**Action:** When caching `GameObject.Find` results, always use the `if (_cache.TryGetValue(key, out obj) && obj != null)` pattern. Do not cache `null` results if there is any chance the object will be created later. Ensure the cache is updated immediately after any `Instantiate` calls.

## 2026-04-29 - Unity Native Null Caching Efficiency
**Learning:** In managers that use dictionary-based caching for Unity objects, simply checking `if (obj != null)` is insufficient if an object was destroyed but its C# wrapper persists. Using `ReferenceEquals(obj, null)` alongside Unity's overloaded equality operator allows for robust negative caching and cache invalidation of destroyed objects without redundant O(N) scene traversals.
**Action:** Implement a dual-check in dictionary lookups: use Unity's `== null` to detect destroyed objects and `ReferenceEquals` to identify explicit negative cache entries.
## 2026-04-25 - Cache Consolidation in SceneDirector.cs
**Learning:** Fragmented caching implementations (multiple dictionaries, redundant helpers, and inconsistent lookup logic) in Unity managers can lead to state inconsistency and subtle performance regressions. Consolidation into a unified caching pattern (O(1) lookups for objects, prefabs, and components) ensures predictable performance and cleaner code.
**Action:** Audit caching systems for redundancy and consolidate into a single source of truth. Always include 'OnDestroy' cleanup for persistent caches to prevent memory leaks in the Unity Editor.
## 2026-05-10 - Unified Caching and Batch Pre-population in SceneDirector
**Learning:** In Unity managers handling multiple dynamic lookups (Find, GetPrefab, GetComponent), fragmented caching logic causes state fragmentation and redundant scene traversals. Batch pre-population (e.g., using 'FindObjectsByType' once in 'SetupScene') converts multiple O(N) operations into a single O(N) pass followed by O(1) lookups, providing a significant performance win for complex scenes.
**Action:** Consolidate all caching (GameObjects, Prefabs, Components) into a unified dictionary-based system. Use batch pre-population during scene setup to minimize expensive engine calls.
## 2025-05-14 - Character Prefab Lookup Optimization
**Learning:** In Unity's SceneDirector, repeated O(N) searches using List.Find(p => p.name.Contains(name)) inside loops for character spawning create significant CPU overhead as the character roster grows.
**Action:** Implement a lazy-loading Dictionary cache for prefabs. While pre-population is possible, lazy-loading ensures we only cache what is actually used, and persist the cache across scenario updates within the same scene to avoid redundant searches, while clearing in OnDestroy to prevent memory leaks.
## 2025-03-15 - [Advanced Unity Object Caching & Batching]
**Learning:** Pre-populating caches with `Object.FindObjectsByType` (Unity 2021.3+) during setup provides an ~80-90% speedup over lazy O(N) `GameObject.Find` calls. For robust negative caching, `System.Object.ReferenceEquals(obj, null)` is required to distinguish between a missing entry (true null) and a destroyed Unity object (fake null).
**Action:** Use batch discovery for scene setup and `ReferenceEquals` for high-fidelity negative caching in Unity.
## 2026-03-26 - [Unity Component Caching and Lifecycle Management]
**Learning:** While caching `GetComponent` calls into a `Dictionary<int, T>` using `GetInstanceID()` as a key is a powerful O(1) optimization, it introduces a memory leak risk if not cleared during scene or scenario transitions. Unity's "fake null" objects (destroyed on the native side but alive in managed memory) persist in dictionaries, leading to redundant processing and memory bloat.
**Action:** Always clear component and object caches during scenario setups or scene transitions. In `SceneDirector.cs`, calling `_controllerCache.Clear()` ensures that only active objects are tracked, preventing leaks of destroyed character controllers.

## 2026-05-13 - [Unity WaitForSeconds GC Optimization]
**Learning:** In terminal-style UI components like 'OtisTerminal.cs' that use coroutine-based typewriter effects, creating 'new WaitForSeconds' for every character revealed leads to significant Garbage Collection (GC) allocations. This can cause frame stuttering during long text reveals.
**Action:** Implement a static Dictionary cache using milliseconds (int) as keys for 'WaitForSeconds' objects to eliminate redundant heap allocations while avoiding floating-point lookup precision issues.
## 2024-05-15 - OtisTerminal Yield Instruction Caching
**Learning:** Repeatedly calling 'new WaitForSeconds(time)' in high-frequency coroutines like the typewriter effect in 'OtisTerminal.cs' causes unnecessary GC allocations on every character reveal.
**Action:** Implement a static 'Dictionary<int, WaitForSeconds>' cache using millisecond-rounded keys (Mathf.RoundToInt(time * 1000f)) to reuse yield instructions and eliminate heap allocations.
