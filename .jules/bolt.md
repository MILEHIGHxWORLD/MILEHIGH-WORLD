## 2025-05-14 - [SceneDirector Code Rot Cleanup]
**Learning:** Found significant code rot and syntax errors in SceneDirector.cs, including stray characters and broken loop nesting that would have blocked compilation.
**Action:** Always perform a manual code health check on core scripts before implementing tests to ensure a stable baseline.
## 2024-05-26 - [SceneDirector Caching Optimization]
**Learning:** [SceneDirector.cs was performing expensive O(N) `GameObject.Find()` calls and linear prefab searches inside character instantiation loops. Unity's native object destruction creates "fake nulls" (C# reference is not null, but native object is destroyed), which complicates dictionary-based caching.]
**Action:** [Implemented a multi-level caching system in `SceneDirector.cs`. 1. `_objectCache` for GameObjects with negative caching and `ReferenceEquals` checks for fake nulls. 2. `_prefabCache` to avoid repeated linear searches on the prefab list. 3. `_controllerCache` using `GetInstanceID()` to avoid redundant `GetComponent` calls. All caches are cleared during scenario setup to prevent memory leaks and stale references.]
## 2024-05-24 - Unity GameObject.Find Performance Bottleneck
**Learning:** In Unity 3D scripts like `SceneDirector.cs` within this project, methods frequently call `GameObject.Find()` to search for characters or interactive objects by name during scene setups. This forces Unity to traverse the entire scene hierarchy repeatedly (O(N) operation), which can cause noticeable load times or frame drops if called continuously or on large scenes.
**Action:** Replace direct `GameObject.Find()` calls with a `Dictionary<string, GameObject>` cache when objects are fetched by name multiple times or instantiated dynamically. Check `cachedObj != null` to handle Unity's custom object destruction logic safely before returning a cached reference.
## 2024-05-24 - Unity GameObject.Find Bottleneck
**Learning:** `GameObject.Find` is heavily used in setup scripts like `SceneDirector.cs` which can cause significant frame drops during scenario initialization or interaction processing. Unity's overridden `!= null` check works properly for detecting destroyed objects in a Dictionary cache.
**Action:** Always consider replacing `GameObject.Find` inside loops or repetitive functions with a cached Dictionary approach in Unity C# scripts.
## 2024-05-23 - GameObject.Find caching in iterative setups
**Learning:** Using `GameObject.Find` in setup loops (like iterating over character profiles and interactive objects) can cause noticeable hitches during scene initialization, particularly in complex scenes. It is extremely expensive as it iterates through the whole scene hierarchy.
**Action:** Always use dictionary-caching or direct references for multiple lookups by name instead of repeated `GameObject.Find` calls to speed up scene initialization by making repeated lookups O(1) instead of O(N).
## 2024-03-16 - Unity Coroutine Yield Instruction Caching
**Learning:** Found that using inline `new WaitForSeconds(time)` repeatedly inside Unity coroutines (especially in heavily choreographed cinematic sequences) generates unnecessary Garbage Collection allocations every time the coroutine hits that yield instruction, which can lead to GC spikes and frame stuttering.
**Action:** Always cache frequently used yield instructions like `WaitForSeconds` in a shared static dictionary or similar caching mechanism, so that existing yield objects are reused and no extra garbage is created per sequence execution.
## 2024-05-24 - Avoid O(n) GameObject.Find in Initialization/Setup Loops
**Learning:** Unity's `GameObject.Find()` operation is O(n) and deeply iterates the scene. Repeated calls for the same objects, especially when setting up many characters or interactive objects from JSON configuration loops, leads to significant and unnecessary latency. This pattern is common in dynamic data-driven scene instantiators like `SceneDirector`.
**Action:** When dynamically instantiating or initializing a scene from a list of identifiers, cache `GameObject.Find()` results and instantiated references into a `Dictionary<string, GameObject>`. This converts repetitive O(n) string-based scene lookups into an O(1) dictionary retrieval.
## 2024-05-24 - Unity GameObject.Find Performance Bottleneck in Setup Loops
**Learning:** `GameObject.Find()` in Unity executes an O(N) full scene graph traversal across all active objects. In data-driven setup loops (e.g., parsing JSON to build a scene or instantiating/finding characters and interactive objects in `SceneDirector.cs`), using it repeatedly causes significant CPU spikes on initialization.
**Action:** Always cache `GameObject.Find()` results in a generic `Dictionary<string, GameObject>` specifically for repeating setups, and check the cache before issuing the underlying `GameObject.Find()` call.

## 2025-05-14 - [Unity Testing via .NET CLI]
**Learning:** In environments without the Unity Editor, logic can be verified by bootstrapping a temporary .NET console project and stubbing Unity-specific dependencies.
**Action:** Use this 'stubbing' pattern to gain confidence in C# logic when the full Unity engine is unavailable.

## 2025-05-14 - [Nullable Reference Type (NRT) in Unity Caches]
**Learning:** Using `GameObject?` in Dictionaries helps satisfy NRT requirements and clarifies the intent of 'negative caching' (explicitly caching a missing object).
**Action:** Prefer nullable types for cache values when the cache is expected to store null results from lookups like `GameObject.Find`.
## 2024-05-15 - Shader 'pow()' optimization
**Learning:** Using `pow(x, c)` for small constant integer exponents in pixel-evaluated fragment shaders can be highly unoptimized depending on the graphics API.
**Action:** Always rewrite integer-based exponentiation as explicit variable multiplications. For example, use `x * x * x` instead of `pow(x, 3.0)` to save ALU cycles and improve GPU fragment processing times.
## 2024-05-24 - Expensive Scene Traversal in Initialization Loops
**Learning:** `GameObject.Find` causes O(N) scene hierarchy traversals, which becomes a severe bottleneck when called repeatedly inside initialization loops (e.g., `SceneDirector.SetupScene` iterating over character profiles and interactive objects).
**Action:** Always use a dictionary cache (`Dictionary<string, GameObject>`) to store and retrieve object references during scene initialization sequences, ensuring to check `obj != null` on retrieval to handle natively destroyed objects correctly.
## 2024-05-24 - Expensive GameObject.Find in Loops
**Learning:** `GameObject.Find` is an O(N) operation traversing the scene hierarchy and is especially expensive when called repeatedly inside loops or initialization sequences like `SceneDirector.SetupScene`.
**Action:** Always use dictionary caches (e.g., `Dictionary<string, GameObject>`) to store and retrieve object references to avoid repeated scene traversals.
## 2024-05-25 - Unity WaitForSeconds GC Allocation in Loops
**Learning:** Using `new WaitForSeconds()` inside a tight loop (like a typewriter text effect in a Coroutine) creates a new object allocation on the heap for every single character typed. This causes unnecessary garbage collection (GC) pressure and can lead to frame stuttering during text-heavy cinematic sequences.
**Action:** Always cache `WaitForSeconds` objects outside the loop or use a shared caching mechanism when the duration is constant, returning the cached instance inside the loop.

## 2026-03-25 - [Redundant Member Clutter Performance Impact]
**Learning:** The 'SceneDirector.cs' file was severely cluttered with over a dozen redundant dictionary declarations and duplicate helper methods for GameObject caching. This not only increases memory overhead but also creates a "state fragmentation" risk where different parts of the initialization loop use different caches, leading to redundant O(N) traversals despite the caching intent.
**Action:** Always audit caching implementations for redundancy. Consolidate into a single, unified caching pattern to ensure O(1) lookups are consistent across the entire system.

## 2024-05-25 - [Unity Scene Setup Optimization: Prefab & Negative Caching]
**Learning:** In 'SceneDirector.cs', initialization spikes were caused by O(N*M) lookups where N is the number of characters/objects and M is the scene hierarchy size or prefab list size. `GameObject.Find` is especially expensive when it fails, as it scans the entire scene every time.
**Action:** Use a `Dictionary<string, GameObject>` for both prefab lookups (initialized once) and scene object resolution (with negative caching). In Unity, use `ReferenceEquals(obj, null)` to detect negative cache hits vs. destroyed "fake null" objects to maintain cache integrity.
## 2024-05-26 - Unity WaitForSeconds GC Allocation in Loops
**Learning:** Floating-point keys in Dictionary cache for WaitForSeconds lead to cache misses due to precision issues.
**Action:** Use an int key representing milliseconds for the dictionary cache.
## 2024-05-24 - Cache keys for WaitForSeconds
**Learning:** Using float as a dictionary key causes cache misses due to floating-point tolerance variations.
**Action:** Always use an int key (e.g., milliseconds) when caching float-based objects like WaitForSeconds in a Dictionary to ensure accurate lookups.
## 2024-05-25 - Unity WaitForSeconds GC Allocation in Loops Cache Key
**Learning:** When caching `WaitForSeconds` in a `Dictionary` to prevent GC allocations in Unity Coroutines, using a `float` key is unreliable due to floating-point precision differences causing cache misses.
**Action:** Always use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) rather than `float` when caching temporal objects like `WaitForSeconds` to ensure consistent and reliable O(1) dictionary lookups.
## 2025-02-23 - Avoid float keys in Dictionary caches for Unity
**Learning:** Using `float` keys in a `Dictionary` cache (e.g., for `WaitForSeconds`) can cause frequent cache misses due to floating-point tolerance variations. Even when requesting the same float value (like 0.5f), minor precision differences might lead to a different hash, breaking the cache mechanism.
**Action:** Always convert floats to a stable integer representation (like milliseconds via `Mathf.RoundToInt(time * 1000f)`) when using them as cache keys.

## 2024-05-15 - Optimizing List.Find with Partial String Matching
**Learning:** In Unity, an O(N) list search with string comparisons (e.g., `List.Find(p => p.name.Contains(searchString))`) inside a loop is expensive. When replacing this with a `Dictionary<string, GameObject>` cache, we can't naively pre-cache using `prefab.name` if partial matching is required. The solution is lazy evaluation: perform the O(N) `.Find()` on a cache miss, and store the result mapped to the `searchString` key for subsequent O(1) lookups. Additionally, storing `null` for missing items (negative caching) prevents the expensive fallback search from executing repeatedly for non-existent assets.
**Action:** When migrating from `List.Find` to a dictionary cache where partial string matching is involved, implement a lazy evaluation approach with negative caching. Key the dictionary with the *search query*, not the resulting object's name.
## 2024-05-24 - Unity List.Find Performance Bottleneck in Setup Loops
**Learning:** Using `List.Find` with string comparisons (like `.Contains()`) inside a scene initialization loop creates an O(N*M) performance bottleneck, as it repeatedly scans the list with string matching operations for every entity being setup.
**Action:** Always replace O(N) list searches for prefabs inside setup loops with a `Dictionary<string, GameObject>` cache. Use lazy evaluation to perform the O(N) lookup only on a cache miss, mapping the result to the search string for subsequent O(1) lookups.

## 2024-05-26 - Robust Negative Caching in Unity
**Learning:** Unity's '==' operator for GameObjects checks if the native C++ object has been destroyed, which can lead to "fake nulls" in a dictionary cache. Standard null checks might incorrectly return a destroyed object as a valid reference if the managed wrapper still exists.
**Action:** Use 'ReferenceEquals(obj, null)' to identify "real nulls" (negative cache hits for objects that never existed) vs 'obj == null' for "fake nulls" (objects that were destroyed). This allows for efficient negative caching while still permitting cache invalidation and re-fetching of destroyed objects.
## 2024-05-25 - Robust Negative Caching for Unity Lookups
**Learning:** In Unity, simply caching the result of 'GameObject.Find' is insufficient if the object is missing. Without storing the 'null' result (Negative Caching), the system continues to perform O(N) scene traversals for every query of the missing object. Additionally, 'ReferenceEquals(obj, null)' is the only reliable way to check if a cached reference is a 'Real Null' (never existed) vs a 'Fake Null' (Unity object was destroyed).
**Action:** Implement negative caching by storing 'null' results in lookups. Use 'ReferenceEquals' to distinguish cache hits for missing objects from invalidated references of destroyed objects.

## 2024-05-25 - Fragment Shader Dead Work Elimination
**Learning:** In HLSL fragment shaders, assigning to 'o.Albedo' at the end of a function overwrites all previous additive operations (like Subsurface Scattering) applied to that member. This creates "dead work" where expensive GPU calculations are performed but then discarded.
**Action:** Always assign the base albedo at the start of the 'surf' function. This ensures subsequent additive or multiplicative effects are correctly layered on the final output and prevents cycles from being wasted on discarded results.
## 2024-05-27 - Caching O(N) list searches with lazy evaluation and negative caching
**Learning:** In Unity setup scripts (e.g., `SceneDirector.cs`), using an O(N) list search with string comparisons (like `List.Find(p => p.name.Contains(searchString))`) inside initialization loops creates a significant performance bottleneck. Naively precaching the list using `prefab.name` won't work correctly if partial matching (e.g. `Contains`) is required by the logic.
**Action:** Replace the loop-based list search with a dictionary cache (`Dictionary<string, GameObject>`). Use lazy evaluation: perform the expensive O(N) list search ONLY on a cache miss, and store the result mapped to the original `searchString` key. Importantly, also cache `null` results (negative caching) to prevent repeated expensive searches for assets that do not exist.
## 2024-05-24 - Unity Coroutine Yield Instruction Caching with Floats
**Learning:** Found that using a `Dictionary<float, WaitForSeconds>` to cache Unity yield instructions is an anti-pattern. Floating-point precision inaccuracies can cause cache misses, leading to redundant Garbage Collection (GC) allocations.
**Action:** Always use an integer key (e.g., milliseconds calculated via `Mathf.RoundToInt(time * 1000f)`) when caching time-based objects like `WaitForSeconds` in Unity to ensure accurate cache hits and eliminate GC overhead.
## 2024-05-24 - Unity WaitForSeconds float key dictionary cache anti-pattern
**Learning:** Using `Dictionary<float, WaitForSeconds>` to cache yield instructions causes cache misses due to floating-point precision inaccuracies, leading to the creation of unintended garbage allocations (GC pressure) during high frequency calls like cinematic loops or typewriters.
**Action:** Use an integer key (e.g. milliseconds `Dictionary<int, WaitForSeconds>`) to correctly cache float-based values safely, avoiding precision-related cache misses while still eliminating GC allocation within loops.
## 2024-05-25 - Float-keyed Dictionary Cache Misses for WaitForSeconds
**Learning:** In `Cinematic_IntoTheVoid.cs`, using a `Dictionary<float, WaitForSeconds>` for caching yield instructions resulted in cache misses due to floating-point precision inaccuracies. While intended to prevent GC allocations, the float keys caused redundant instantiation of `WaitForSeconds` objects.
**Action:** Always use an integer-keyed dictionary (e.g., converting seconds to milliseconds via `Mathf.RoundToInt(time * 1000f)`) when caching temporal objects like `WaitForSeconds` to ensure reliable cache hits and truly eliminate GC allocations.

## 2026-04-02 - Float Keys in Caches Cause Misses
**Learning:** Using floats for Dictionary keys (e.g., `Dictionary<float, WaitForSeconds>`) causes cache misses due to floating point precision issues, leading to the GC allocations the cache is meant to avoid.
**Action:** When caching objects using float parameters, multiply by a suitable constant and round to an integer (like milliseconds for time) to use as the Dictionary key.
## 2024-05-26 - WaitForSeconds Dictionary Cache Float Precision Issue
**Learning:** Using a float as a Dictionary key for caching `WaitForSeconds` objects (e.g., `Dictionary<float, WaitForSeconds>`) can lead to cache misses due to floating-point precision inaccuracies, unintentionally creating new GC allocations.
**Action:** Always use an integer-keyed dictionary (e.g., representing milliseconds by multiplying the float time by 1000 and rounding) to safely and accurately cache float-based values without precision issues.

## 2026-03-26 - Cache List Searches inside Instantiation Loops
**Learning:** In Unity, using O(N) list searches with string comparisons (e.g., `List.Find(p => p.name.Contains(...))`) inside instantiation or scene setup loops causes significant performance bottlenecks due to unnecessary iterations.
**Action:** Always pre-cache the list items into a `Dictionary<string, GameObject>` to achieve O(1) lookups and prevent performance bottlenecks during repetitive lookups in loops.
## 2024-05-26 - Unity WaitForSeconds Float Cache Misses
**Learning:** Using a `Dictionary<float, WaitForSeconds>` to cache coroutine yield instructions is an anti-pattern due to floating-point precision inaccuracies. Even identical float literal arguments can yield tiny precision differences, resulting in cache misses and continuous heap allocations (GC spikes) that the cache was explicitly designed to prevent.
**Action:** Always key floating-point delay caches using integers (e.g., convert `float` seconds to `int` milliseconds using `Mathf.RoundToInt(time * 1000f)`). This ensures robust cache hits and zero GC allocations for repeated coroutine delays.

## 2024-06-12 - List.Find with string allocation in instantiation loop
**Learning:** Using `List.Find` combined with a string operation (like `p.name.Contains`) inside a scene initialization or instantiation loop scales at O(N) and creates repeated overhead per instantiated entity.
**Action:** Cache prefab lookups into a `Dictionary<string, GameObject>` keyed by the search name to convert sequential iterations into an O(1) hash map lookup, preventing performance hitches during dynamic scene setups.
## 2026-04-02 - Unity Negative Caching and ReferenceEquals
**Learning:** Unity's 'GameObject.Find' is expensive, especially when it fails. Implementing negative caching by storing 'null' in the dictionary prevents repeated O(N) scene traversals for non-existent objects. Additionally, using 'System.Object.ReferenceEquals(obj, null)' alongside the '== null' check is critical for robustly identifying truly null entries versus destroyed Unity objects (fake nulls).
**Action:** Use negative caching in GameObject caches and combine '== null' with 'ReferenceEquals' to safely manage the lifecycle of cached Unity objects.

## 2026-03-27 - [Triple-Layer Caching & Unity Null Safety]
**Learning:** Effective scene initialization in Unity requires a triple-layer caching strategy: 1) Negative caching for `GameObject.Find` to avoid repeated O(N) traversals for missing objects, 2) Prefab memoization to replace O(M) list searches, and 3) `InstanceID`-based component caching to optimize `GetComponent` calls. Crucially, robust negative caching MUST distinguish between "real nulls" (negative cache hits) and "fake nulls" (destroyed objects) using `System.Object.ReferenceEquals(obj, null)`.
**Action:** Implement unified `_objectCache`, `_prefabCache`, and `_controllerCache` patterns. Clear scene-specific caches (`_objectCache`, `_controllerCache`) during scenario transitions while persisting asset-based caches (`_prefabCache`). Always use `ReferenceEquals` for deep null-safety in Unity dictionaries.
## 2026-03-30 - Unity WaitForSeconds Anti-pattern
**Learning:** Caching `WaitForSeconds` with a Dictionary using float keys is an anti-pattern due to floating-point precision issues.
**Action:** Pre-calculate `WaitForSeconds` outside of loops to eliminate GC allocations without float dictionary keys.

## 2024-05-25 - Unity WaitForSeconds Float Dictionary Cache Anti-Pattern
**Learning:** Using `Dictionary<float, WaitForSeconds>` to cache yield instructions is an anti-pattern due to floating-point precision inaccuracies, leading to cache misses and subtle memory leaks/GC pressure.
**Action:** Instead of dictionary caching for `WaitForSeconds`, instantiate and cache specific `WaitForSeconds` objects as local variables outside of loops or as fields if the durations are static, avoiding dictionary lookups with float keys.
## 2026-03-26 - [Unity Component Caching and Lifecycle Management]
**Learning:** While caching `GetComponent` calls into a `Dictionary<int, T>` using `GetInstanceID()` as a key is a powerful O(1) optimization, it introduces a memory leak risk if not cleared during scene or scenario transitions. Unity's "fake null" objects (destroyed on the native side but alive in managed memory) persist in dictionaries, leading to redundant processing and memory bloat.
**Action:** Always clear component and object caches during scenario setups or scene transitions. In `SceneDirector.cs`, calling `_controllerCache.Clear()` ensures that only active objects are tracked, preventing leaks of destroyed character controllers.
## 2024-05-25 - Dictionary<float, WaitForSeconds> Anti-Pattern
**Learning:** Using `Dictionary<float, WaitForSeconds>` to cache yield instructions is an anti-pattern due to floating-point precision inaccuracies, which can cause cache misses and still result in per-iteration GC allocations.
**Action:** When variable wait times are needed in a tight loop (e.g., rhythmic punctuation delays in a typewriter effect), pre-calculate and cache the specific `WaitForSeconds` instances in local variables outside the loop instead of using a float-keyed dictionary.
## 2024-05-24 - Dead Code in Shaders
**Learning:** In 'HyperPBRCharacter_4D.shader', a Subsurface Scattering (SSS) block was performing expensive texture lookups and mathematical operations only for the result to be immediately overwritten by a subsequent albedo assignment. This "dead code" wastes GPU cycles and memory bandwidth without contributing to the final frame.
**Action:** Always audit surface shaders for assignment overwrites. Removing high-cost logic that is logically unreachable or overwritten is a high-impact optimization for rendering performance.
## 2024-05-25 - Unity WaitForSeconds GC Allocation in Loops vs Static Cache
**Learning:** Using `new WaitForSeconds()` inside a tight loop (like a typewriter text effect in a Coroutine) creates a new object allocation on the heap for every single character typed, causing unnecessary GC pressure. However, using a `Dictionary<float, WaitForSeconds>` for caching is an anti-pattern due to floating-point precision inaccuracies, and it risks a static memory leak if typing speeds vary continuously.
**Action:** Remove static `Dictionary<float, WaitForSeconds>` caches. Always safely cache `WaitForSeconds` objects in a local variable *outside* the loop (e.g., `var wait = new WaitForSeconds(time);`) instead of relying on unverified custom static dictionary caching methods.
## 2026-03-28 - Unity Component and Prefab Caching in SceneDirector
**Learning:** Even with GameObject caching, O(N) operations like `GetComponent` and O(P) operations like `List.Find` (especially with string operations like `Contains`) inside character setup loops create measurable initialization spikes.
**Action:** Use `Dictionary<int, T>` (using `GetInstanceID`) for component caching and `Dictionary<string, GameObject>` for prefab lookups to ensure all repetitive setup operations are O(1).

## 2026-03-28 - Performance vs. Correctness in Shader Logic
**Learning:** Fixing a "dead code" bug (e.g., an albedo overwrite that discarded an expensive SSS calculation) can unintentionally decrease performance by enabling high-cost GPU operations that were previously optimized out by the compiler.
**Action:** When optimizing, verify if a "fix" actually enables expensive paths. If performance is the priority, consider removing the unused logic entirely instead of enabling it.
## 2024-05-25 - Unity WaitForSeconds float Dictionary Anti-Pattern
**Learning:** While caching `WaitForSeconds` is necessary to avoid per-iteration Garbage Collection inside Unity coroutines, using a `Dictionary<float, WaitForSeconds>` is an anti-pattern. Floating-point precision inaccuracies make `float` unreliable as a dictionary key, potentially causing cache misses or unexpected behavior.
**Action:** Instead of using a float-keyed dictionary, pre-calculate specific `WaitForSeconds` instances (e.g., normal delay, short pause, long pause) into local variables outside the coroutine loop, and yield those local variables directly inside the loop.
## 2026-03-26 - [Advanced Unity Caching & Memory Efficiency]
**Learning:** When caching Unity components, using `InstanceID` (int) as a dictionary key is significantly more efficient than strings or `ToString()` calls, as it avoids heap allocations during every lookup. Furthermore, robust negative caching for Unity objects (which override `== null`) requires using `ReferenceEquals(obj, null)` to distinguish between a legitimate `null` cache entry and a reference to a natively destroyed object (fake null).
**Action:** Use `int` (GetInstanceID) for component cache keys and `ReferenceEquals` to manage the lifecycle of cached Unity objects correctly.
## 2026-03-26 - Unity Negative Caching Pitfalls
**Learning:** In Unity managers like `SceneDirector.cs` that both find and instantiate objects, "negative caching" (storing `null` in the dictionary when `GameObject.Find` fails) is a dangerous anti-pattern. If an object is instantiated later in the same frame or scenario, subsequent lookups will incorrectly return the cached `null` instead of the newly created object. Furthermore, Unity's `obj != null` check is essential even for cached references to detect if the native C++ object was destroyed.
**Action:** When caching `GameObject.Find` results, always use the `if (_cache.TryGetValue(key, out obj) && obj != null)` pattern. Do not cache `null` results if there is any chance the object will be created later. Ensure the cache is updated immediately after any `Instantiate` calls.
## 2024-05-24 - Redundant Caching in SceneDirector
**Learning:** The `SceneDirector.cs` file contained multiple redundant dictionary definitions for the same cache and duplicated `GetCachedObject` logic with conflicting implementations, which leads to unpredictable O(N) traversals and bugs with Unity's null checks.
**Action:** Removed redundant caches and consolidated the GameObject caching strategy to ensure O(1) dictionary lookups are consistent.

## 2024-05-23 - Dictionary cache misses with float keys
**Learning:** When caching WaitForSeconds in a Dictionary to prevent GC allocations in Unity Coroutines, float keys can cause lookup cache misses due to floating-point tolerance variations.
**Action:** Use an int key representing milliseconds (e.g., via Mathf.RoundToInt(time * 1000f)) rather than float for reliable cache hits.
## 2024-05-26 - Unity Dictionary Cache Misses with Float Keys
**Learning:** Using `float` as a key in a `Dictionary<float, WaitForSeconds>` causes frequent cache misses due to floating-point tolerance variations. This defeats the purpose of caching yield instructions and still creates garbage collection allocations.
**Action:** When caching `WaitForSeconds` or anything time-related, convert the float into an integer representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to serve as a reliable, exact-match dictionary key.
## 2024-05-26 - Float keys in Dictionary cache
**Learning:** Using float as a dictionary key for WaitForSeconds cache can cause cache misses due to floating point inaccuracies, leading to GC allocations despite the cache.
**Action:** Convert float time to integer milliseconds using Mathf.RoundToInt(time * 1000f) for the dictionary key to ensure deterministic cache hits.
## 2024-05-28 - Unity WaitForSeconds Dictionary Key Optimization
**Learning:** When caching `WaitForSeconds` in a `Dictionary` to prevent GC allocations in Unity Coroutines, using a `float` as the key can lead to cache misses due to floating-point tolerance variations.
**Action:** Use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) rather than `float` to resolve dictionary lookup cache misses and ensure zero-allocation yields.

## 2024-05-24 - Avoid GC Allocations with WaitForSeconds in Unity
**Learning:** Caching `WaitForSeconds` in a Dictionary using a `float` as a key can lead to cache misses due to floating-point imprecision, which causes unexpected GC allocations when yielding in coroutines.
**Action:** When caching `WaitForSeconds` instances, convert the time to an integer key (e.g., `Mathf.RoundToInt(time * 1000f)`) rather than using a `float` key to avoid boxing and imprecision issues.
## 2024-05-26 - Unity WaitForSeconds Cache Misses with Float Keys
**Learning:** Using floats as keys in a dictionary cache (e.g., `Dictionary<float, WaitForSeconds>`) causes frequent cache misses due to floating-point precision errors. This silently breaks the caching mechanism, leading to continuous GC allocations during frequent coroutine yields (like typewriter effects).
**Action:** Always use integer keys (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) when caching `WaitForSeconds` to ensure consistent O(1) retrieval and prevent unnecessary GC pressure.
## 2024-05-26 - Float Keys in Dictionaries for Yield Instructions
**Learning:** Caching `WaitForSeconds` in a `Dictionary<float, WaitForSeconds>` causes frequent cache misses due to floating-point imprecision (e.g., `0.1f` might evaluate slightly differently). This leads to unintended instantiations and GC spikes, defeating the purpose of the cache.
**Action:** Always use an integer key (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) when caching time-based yield instructions in a dictionary to ensure consistent cache hits and zero allocations.
## 2024-05-04 - [Testing Unity Scripts in Standalone .NET]
**Learning:** Unity scripts (MonoBehaviours) can be unit-tested in a standalone `dotnet` environment by stubbing out core UnityEngine classes (GameObject, MonoBehaviour, Mathf, Debug). This allows for rapid validation of logic-heavy methods without the overhead of the Unity Editor.
**Action:** Use `dotnet new console` or `dotnet test` with custom stubs to verify non-Unity-dependent logic in core scripts when the full Editor environment is unavailable.

## 2024-05-04 - [Fixing Code Rot and Syntax Soup in Core Scripts]
**Learning:** Automated or poorly merged security patches can lead to "code rot" where multiple versions of the same logic (like typewriter reveal or path sanitization) are layered on top of each other, creating uncompilable "syntax soup".
**Action:** When fixing compilation errors in legacy or heavily patched files, prioritize reading the entire file first to identify redundant blocks. Manually consolidate logic into a single, clean path rather than attempting to patch individual errors, as the errors are often symptoms of deeper duplication.
## 2024-05-24 - Avoid float keys in Dictionary caches
**Learning:** Caching `WaitForSeconds` using a `float` as a dictionary key causes cache misses due to floating-point imprecision, leading to unnecessary GC allocations and defeating the purpose of the cache.
**Action:** When caching objects based on time durations, use an integer key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to guarantee consistent and reliable dictionary lookups without precision errors.

## 2024-05-28 - WaitForSeconds Cache Floating Point Imprecision
**Learning:** Using `float` as a dictionary key for caching `WaitForSeconds` instances causes cache misses due to floating-point imprecision, resulting in unnecessary GC allocations.
**Action:** Always use an `int` key for dictionary caches representing time (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) to ensure O(1) lookups are reliable.
## 2024-05-26 - Unity Coroutine Float Cache Pitfall
**Learning:** Caching `WaitForSeconds` instances using a `float` as the dictionary key leads to cache misses due to floating-point imprecision. This nullifies the caching optimization and causes unintended GC allocations.
**Action:** When caching yield instructions by duration, always convert the `float` time into an `int` key (e.g., `Mathf.RoundToInt(time * 1000f)`) to ensure perfect cache hits.
## 2024-05-26 - Unity Pool Integrity and Null Checking
**Learning:** When using object pools in Unity, cached references in a Queue or List can become "fake nulls" if the object is destroyed externally (e.g., by another script or scene cleanup). Dequeuing a destroyed object and calling SetActive(true) will fail.
**Action:** Always use a `while (_pool.Count > 0)` loop to dequeue and check `if (obj != null)` before reusing a pooled object. This ensures the pool remains robust even if its members are destroyed by outside logic.
## 2024-05-24 - Float to Int Key in Dictionary for Unity Caching
**Learning:** Using a float as a key in a Dictionary for caching (like caching WaitForSeconds in Cinematic_IntoTheVoid.cs) can lead to dictionary cache misses due to floating-point precision issues, generating unnecessary garbage collection allocations.
**Action:** Convert the float to an integer (e.g., using `Mathf.RoundToInt(time * 1000f)`) before using it as a key in the Dictionary to ensure consistent cache hits and prevent memory allocations during runtime.
## 2024-05-26 - Robust Unity Object Caching and Pooling
**Learning:** When implementing caches for Unity GameObjects, it is critical to distinguish between a 'true' null (explicitly cached as missing) and a 'Unity' null (native object destroyed). Using `ReferenceEquals(obj, null)` identifies true nulls, while `obj == null` (via Unity's operator overload) identifies destroyed objects. This prevents unnecessary O(N) `GameObject.Find` calls for objects confirmed missing while allowing re-acquisition of destroyed ones. Additionally, when recycling from an object pool, always verify the dequeued object is not a Unity null.
**Action:** Use the `ReferenceEquals` check for negative caching and verify pool integrity before reuse to ensure robust performance in dynamic scene managers.
## 2026-05-07 - Unity WaitForSeconds float key cache miss
**Learning:** When caching `WaitForSeconds` instances in a Dictionary for Unity coroutines, using a `float` key can lead to cache misses due to floating-point imprecision. This defeats the purpose of the cache and still causes redundant Garbage Collection (GC) allocations on every creation of `WaitForSeconds` during frame stuttering sequences.
**Action:** Use an integer key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) rather than a `float` to guarantee consistent cache hits and prevent memory allocations.
## 2024-05-26 - Unity WaitForSeconds Float Caching Pitfalls
**Learning:** When caching `WaitForSeconds` instances in a Dictionary for Unity coroutines, using a `float` as the key can lead to cache misses due to floating-point imprecision, causing unnecessary GC allocations.
**Action:** Use an integer key instead (e.g., representing milliseconds via `Mathf.RoundToInt(time * 1000f)`) to ensure reliable dictionary lookups and prevent garbage creation.
## 2024-05-28 - Unity WaitForSeconds float key imprecision
**Learning:** Caching `WaitForSeconds` in a Dictionary using a `float` as the key leads to cache misses due to floating-point imprecision. This defeats the purpose of the cache and causes unexpected GC allocations when yielding delays.
**Action:** Use an integer key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to guarantee stable lookups and prevent unnecessary object allocations in performance-critical loops.
## 2026-05-06 - Float Keys in WaitForSeconds Cache
**Learning:** Using floats as keys in a Dictionary for caching WaitForSeconds causes cache misses due to floating-point imprecision, leading to unnecessary GC allocations and defeating the purpose of the cache.
**Action:** Use an integer key representing milliseconds (via `Mathf.RoundToInt(time * 1000f)`) to ensure cache hits.
## 2024-05-26 - WaitForSeconds Float Dictionary Cache Misses
**Learning:** Caching `WaitForSeconds` using a `float` as the dictionary key can lead to cache misses due to floating-point imprecision, causing unintended `WaitForSeconds` instantiations and unnecessary GC allocations.
**Action:** Always use an integer key (e.g., representing milliseconds via `Mathf.RoundToInt(time * 1000f)`) instead of a `float` when caching `WaitForSeconds` instances in a dictionary.
## 2026-05-10 - [Unified Caching & Code Rot Consolidation]
**Learning:** SceneDirector.cs was severely bloated with over a dozen redundant dictionary declarations and duplicate helper methods. This "code rot" increased memory overhead and created a risk of cache inconsistency during scene setups.
**Action:** Consolidated all redundant caching logic into a single, unified triple-cache system (GameObjects, Prefabs, and Controllers). Removed all duplicate declarations and helper methods, standardizing on O(1) lookups and robust Unity-native null handling to ensure performance and reliability.
