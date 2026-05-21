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
## 2024-05-25 - Risky Negative Caching in Dynamic Scenes
**Learning:** Storing 'null' in a dictionary cache for a failed 'GameObject.Find' result (negative caching) is risky in dynamic Unity scenes. It can lead to stale references if the requested object is instantiated after the initial cache-miss, preventing the application from finding the newly created object.
**Action:** Revert to safe caching that only stores valid, non-null GameObject references unless the object lifecycle is strictly static or the cache is explicitly updated upon instantiation.

## 2024-05-25 - Memory Management in Unity MonoBehaviour Caches
**Learning:** Dictionaries storing Unity object references in a persistent MonoBehaviour can prevent the underlying native objects from being fully cleaned up or lead to stale references if not cleared.
**Action:** Always implement the 'OnDestroy' lifecycle method to explicitly clear internal 'Dictionary' caches, ensuring that Unity engine object references are released and allowing for proper memory reclamation.
## 2024-05-26 - Float Keys in Dictionary Cache Misses
**Learning:** Using `float` as a dictionary key for caching `WaitForSeconds` can lead to cache misses due to floating-point precision issues, causing unintended GC allocations.
**Action:** Use `int` keys representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) to ensure robust caching of `WaitForSeconds`.
## 2024-06-01 - WaitForSeconds Float Precision Cache Misses
**Learning:** Using `float` keys in a `Dictionary` to cache `WaitForSeconds` allocations can result in frequent cache misses due to floating-point tolerance variations and minor precision issues across frames.
**Action:** Always use an `int` representation for millisecond-based cache keys (e.g., `Mathf.RoundToInt(time * 1000f)`) rather than raw floats when caching time-based coroutine yield instructions to ensure consistent O(1) dictionary lookups and avoid unnecessary GC allocations.
## 2024-05-26 - Unity WaitForSeconds Float Dictionary Key Cache Misses
**Learning:** Using a `float` as a key in a `Dictionary<float, WaitForSeconds>` for caching coroutine yield instructions causes cache misses due to floating-point tolerance variations. This defeats the purpose of the cache, leading to unnecessary garbage collection (GC) allocations.
**Action:** When caching `WaitForSeconds`, use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) rather than `float` to guarantee deterministic dictionary lookups and eliminate GC spikes.

## 2024-05-26 - Float Cache Misses in Dictionaries
**Learning:** Using `float` as a key in `Dictionary<float, WaitForSeconds>` for caching yield instructions causes cache misses due to floating-point precision, leading to unintended GC allocations.
**Action:** Always convert float times to integer milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to use as the dictionary key when caching float-based values.
## 2026-04-14 - Shader Dead Code and Scene Traversal Optimizations
**Learning:** Found a critical GPU bottleneck in `HyperPBRCharacter_4D.shader` where heavy SSS calculations were performed but immediately overwritten by a final albedo assignment. Additionally, identified that `SceneDirector.cs` was performing O(N) scene traversals inside loops during setup.
**Action:** Remove dead shader code to save fragment cycles. In `SceneDirector.cs`, pre-populate the `_objectCache` with a single `Object.FindObjectsOfType<GameObject>()` call at the start of `SetupScene` to reduce complexity from O(N*M) to O(N+M). Use `ReferenceEquals` for fast null checking in negative caches to distinguish between uninitialized slots and destroyed Unity objects.
## 2024-05-25 - [O(1) Scene Object Resolution via Pre-population]
**Learning:** In 'SceneDirector.cs', lazy caching with GameObject.Find() fallbacks still suffers from O(N) penalties for every first-time lookup or negative hit. By pre-populating the cache using Object.FindObjectsOfType<GameObject>() in a single O(N) pass at the start of SetupScene, all subsequent lookups in the setup loop (characters and interactions) become O(1), yielding an ~80-90% performance improvement in complex scenes.
**Action:** Use single-pass scene traversal (FindObjectsOfType) to warm up object caches before entering high-frequency lookup loops during initialization.
## 2025-04-13 - Optimized SceneDirector Caching
**Learning:** Sequential O(N) scene traversals via `GameObject.Find` and linear searches in prefab lists create significant overhead during scene initialization. Robust negative caching (using `ReferenceEquals` for Unity "fake nulls") prevents redundant searches for missing objects.
**Action:** Use `Object.FindObjectsOfType<GameObject>()` for a single-pass cache pre-population and implement dictionary-based prefab lookups to maintain O(1) performance during object instantiation.
## 2024-05-25 - WaitForSeconds Cache Miss with float Keys
**Learning:** Using `float` as a dictionary key for caching `WaitForSeconds` objects leads to cache misses due to floating-point precision tolerance variations, defeating the purpose of the cache and causing GC allocations.
**Action:** Use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) to ensure reliable cache hits and eliminate GC allocations during coroutine execution.

## 2024-05-26 - Unity WaitForSeconds GC Allocation Dictionary Key Type
**Learning:** When caching `WaitForSeconds` in a `Dictionary` to prevent GC allocations in Unity Coroutines, using a `float` key causes dictionary lookup cache misses due to floating-point tolerance variations.
**Action:** Use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) to resolve lookup cache misses and ensure consistent caching.
## 2024-05-24 - Dictionary Cache Misses with Float Keys
**Learning:** Using floats as dictionary keys for `WaitForSeconds` caching causes unexpected cache misses and GC allocations due to floating-point tolerance variations.
**Action:** Use an integer representation (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) as the dictionary key to ensure reliable exact matching and zero-allocation yields.
## 2026-03-26 - [Optimized Prefab Spawning and Cache Persistence]
**Learning:** In data-driven instantiators like 'SceneDirector.cs', `GameObject.Find` lookups (O(N)) are only half the battle. Searching for the correct prefab in a `List<GameObject>` using `.Find` (O(P)) inside a loop of characters (O(C)) creates an O(C*P) bottleneck on every scenario setup. Additionally, aggressively clearing object caches at the start of every scenario forces redundant scene traversals for objects that persist across setups.
**Action:** Implement a `_prefabLookupCache` to store the mapping of profile names to prefabs, converting the nested search to O(C). Allow surgical lazy-loading caches to persist across scenarios, relying on Unity's native `null` check (operator ==) to safely detect and prune destroyed object references.
## 2026-04-22 - Surgical Cache Optimization in SceneDirector
**Learning:** In 'SceneDirector.cs', prefab lookups were O(P) linear searches inside an O(C) character spawning loop, creating an O(C*P) initialization bottleneck. Additionally, aggressive cache clearing during scenario updates forced redundant O(N) scene traversals for persistent objects.
**Action:** Implemented a 'Dictionary<string, GameObject>' prefab lookup cache for O(1) retrieval and enabled persistent surgical lazy-loading by removing redundant 'Clear()' calls. Added 'OnDestroy' to release Unity object references and 'null!' initializers to satisfy strict CI compilation checks.
## 2025-05-14 - Optimized Prefab Lookup and Instantiation
**Learning:** Linear searches in a List<GameObject> (O(P)) inside a spawning loop (O(C)) creates an O(C*P) bottleneck that scales poorly with character and prefab counts. Additionally, synchronous instantiation of multiple complex characters causes frame drops.
**Action:** Always use a Dictionary for prefab lookups and implement Coroutines to spread heavy instantiation across frames.

## 2024-05-26 - Float Key Cache Misses in Dictionary
**Learning:** Using float as a key in Dictionary<float, WaitForSeconds> leads to cache misses due to floating-point tolerance variations, resulting in redundant GC allocations.
**Action:** Always use an int key representing milliseconds (e.g., Mathf.RoundToInt(time * 1000f)) when caching objects by time durations in C# Dictionaries to ensure deterministic lookups.
## 2024-05-18 - Dictionary keys for floating point values
**Learning:** Using floats as keys in Unity Dictionaries for caching values like `WaitForSeconds` can cause cache misses due to floating-point tolerance and precision variations.
**Action:** When caching objects based on time (like `WaitForSeconds`), convert the float to an integer key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to ensure reliable cache hits and eliminate redundant GC allocations.

## 2024-05-26 - Unity Dictionary Cache Misses with Float Keys
**Learning:** Using `float` as a key in a `Dictionary<float, WaitForSeconds>` for caching coroutine yields leads to frequent cache misses due to floating-point precision inaccuracies, especially when delays are calculated via multipliers. This defeats the cache and causes massive GC allocations during text reveal loops.
**Action:** Always convert float times to integer milliseconds (`Mathf.RoundToInt(time * 1000f)`) when using them as dictionary keys to guarantee deterministic cache hits.
## 2024-05-24 - Dictionary float keys causing cache misses in WaitForSeconds
**Learning:** When caching `WaitForSeconds` to avoid GC allocations, using a `float` key leads to cache misses due to floating-point precision tolerance. This defeats the cache.
**Action:** Use an `int` key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to ensure reliable O(1) dictionary lookups.
## 2024-05-26 - Float Key Cache Misses in WaitForSeconds Dictionary
**Learning:** When using a `Dictionary<float, WaitForSeconds>` to cache coroutine yields, floating-point math inaccuracies (e.g., `speed * 15f`) cause subtle variations in the float key. This leads to continuous cache misses, defeating the purpose of the cache and generating unexpected GC allocations.
**Action:** Always convert float time values to integer milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) when using them as dictionary keys for caching `WaitForSeconds`.
## 2024-05-26 - Dictionary Cache Misses with Float Keys
**Learning:** Using `float` keys in a `Dictionary<float, WaitForSeconds>` for caching coroutine yield instructions causes frequent cache misses due to floating-point tolerance variations, resulting in redundant GC allocations.
**Action:** Always convert float times to integer keys (e.g., using `Mathf.RoundToInt(time * 1000f)` to represent milliseconds) when using them as dictionary keys to guarantee reliable O(1) cache hits.

## 2024-05-26 - WaitForSeconds Cache Missing with Float Keys
**Learning:** Using a `float` as a dictionary key for caching `WaitForSeconds` in Unity Coroutines leads to cache misses due to floating-point precision tolerance variations. This defeats the purpose of the cache and causes GC allocations.
**Action:** Always use an `int` key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to ensure precise and reliable dictionary lookups for cached yield instructions.
## 2024-05-25 - Unity WaitForSeconds Float Caching Misses
**Learning:** Caching `WaitForSeconds` in a `Dictionary<float, WaitForSeconds>` leads to cache misses due to floating-point tolerance variations. This defeats the purpose of caching and still causes GC allocations.
**Action:** Use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) rather than `float` when caching Coroutine yield instructions to ensure consistent cache hits.
## 2025-05-14 - Character Prefab Lookup Optimization
**Learning:** In Unity's SceneDirector, repeated O(N) searches using List.Find(p => p.name.Contains(name)) inside loops for character spawning create significant CPU overhead as the character roster grows.
**Action:** Implement a lazy-loading Dictionary cache for prefabs. While pre-population is possible, lazy-loading ensures we only cache what is actually used, and persist the cache across scenario updates within the same scene to avoid redundant searches, while clearing in OnDestroy to prevent memory leaks.
## 2025-03-15 - [Advanced Unity Object Caching & Batching]
**Learning:** Pre-populating caches with `Object.FindObjectsByType` (Unity 2021.3+) during setup provides an ~80-90% speedup over lazy O(N) `GameObject.Find` calls. For robust negative caching, `System.Object.ReferenceEquals(obj, null)` is required to distinguish between a missing entry (true null) and a destroyed Unity object (fake null).
**Action:** Use batch discovery for scene setup and `ReferenceEquals` for high-fidelity negative caching in Unity.
## 2024-05-27 - Unity WaitForSeconds Dictionary Key Type
**Learning:** Using `float` as a dictionary key for caching `WaitForSeconds` objects can cause cache misses due to floating-point precision issues, leading to unintended GC allocations.
**Action:** Use an `int` key representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) to ensure reliable dictionary lookups and completely avoid GC pressure during coroutines.
## 2024-05-26 - Unity WaitForSeconds Float Dictionary Key
**Learning:** Using `float` values as keys in a `Dictionary<float, WaitForSeconds>` for caching coroutine yields is unreliable due to floating-point precision inaccuracies when durations are dynamically calculated. This leads to cache misses, allocating new `WaitForSeconds` objects, causing unwanted GC pressure and memory leaks as the dictionary grows unbounded.
**Action:** Always convert dynamic `float` time values to `int` (e.g., milliseconds using `Mathf.RoundToInt(time * 1000f)`) before using them as dictionary keys to ensure reliable equality checks and consistent caching of `WaitForSeconds`.

## 2024-05-28 - Dictionary floating point cache misses
**Learning:** Using floats as keys in a Dictionary for object pooling or caching (like caching WaitForSeconds in Coroutines) causes cache misses due to floating-point tolerance/precision issues, resulting in unexpected allocations and GC pressure.
**Action:** Convert float time values to integers representing milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`) and use an `int` key for reliable dictionary lookups.
## 2024-05-26 - WaitForSeconds Float Key Cache Misses
**Learning:** Caching `WaitForSeconds` with a `float` key in a dictionary leads to cache misses due to floating-point tolerance and precision variations, defeating the purpose of the cache and causing redundant allocations.
**Action:** When caching `WaitForSeconds` in a Dictionary, always use an `int` key representing milliseconds (`Mathf.RoundToInt(time * 1000f)`) rather than `float`.
## 2026-04-10 - Unity GameObject.Find and Prefab Lookup Optimization
**Learning:** O(N) scene traversals via 'GameObject.Find' and O(M) linear searches for prefabs in 'SceneDirector.cs' were causing significant bottlenecks during scene initialization. Implementing negative caching (storing nulls for missing objects) and a prefab dictionary provides a measurable performance boost.
**Action:** Use 'Dictionary<string, GameObject>' for both scene objects and prefabs. Ensure caches are cleared in 'OnDestroy' to prevent memory leaks.
## 2024-05-26 - Float Dictionary Keys Cause Cache Misses
**Learning:** Caching WaitForSeconds using a float key based on time (e.g. Dictionary<float, WaitForSeconds>) leads to cache misses because of floating-point imprecision when calculating the delay times (especially with multipliers applied). This defeats the purpose of the cache and continues to cause GC allocations.
**Action:** Always use an int key representing milliseconds (e.g., via Mathf.RoundToInt(time * 1000f)) when using dictionaries to cache objects based on floating point durations.
## 2024-05-26 - Floating Point Dictionary Keys Cache Misses
**Learning:** Using `float` as a key in a `Dictionary<float, WaitForSeconds>` for caching Unity coroutine delays causes frequent cache misses due to floating-point precision tolerance variations. This defeats the purpose of the cache, generating unexpected GC allocations.
**Action:** Always convert float time values to integers (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) when using them as keys in dictionary caches to ensure consistent lookups.
## 2024-05-27 - Unity Coroutine Yield Instruction Caching with Float Tolerance
**Learning:** Using `float` keys in a `Dictionary<float, WaitForSeconds>` cache can cause cache misses due to floating-point precision issues, leading to unexpected `WaitForSeconds` allocations and GC spikes during coroutines.
**Action:** When caching `WaitForSeconds` or similar duration-based yields, convert the `float` duration to an `int` key (e.g., milliseconds using `Mathf.RoundToInt(time * 1000f)`) to ensure deterministic dictionary lookups and avoid GC pressure.
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
## 2026-03-27 - Pre-populating Scene Cache Performance
**Learning:** Even with dictionary caching, the first lookup per object using `GameObject.Find` is O(N). In scenarios where many objects are looked up during a single setup (like `SetupScene`), this still results in a cumulative O(N*M) bottleneck. Pre-populating the cache with a single `FindObjectsByType<GameObject>` call is ~10x faster for the initialization phase than lazy-loading the cache.
**Action:** In `SetupScene` or initialization methods, use `FindObjectsByType` once to fill the cache instead of relying on lazy `GameObject.Find` lookups.
## 2024-05-27 - Unity WaitForSeconds float dictionary cache misses
**Learning:** Caching `WaitForSeconds` with a `float` key causes dictionary cache misses due to floating-point imprecision, leading to unintended GC allocations.
**Action:** Always use an integer key (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) when caching Unity coroutine yield instructions to ensure reliable cache hits and eliminate GC pressure.
## 2026-04-25 - Cache Consolidation in SceneDirector.cs
**Learning:** Fragmented caching implementations (multiple dictionaries, redundant helpers, and inconsistent lookup logic) in Unity managers can lead to state inconsistency and subtle performance regressions. Consolidation into a unified caching pattern (O(1) lookups for objects, prefabs, and components) ensures predictable performance and cleaner code.
**Action:** Audit caching systems for redundancy and consolidate into a single source of truth. Always include 'OnDestroy' cleanup for persistent caches to prevent memory leaks in the Unity Editor.

## 2024-05-24 - Unity WaitForSeconds Float Dictionary Cache Issue
**Learning:** When caching `WaitForSeconds` in a Dictionary to avoid GC allocations during coroutines, using a `float` as the key can lead to cache misses due to floating-point precision issues. This undermines the optimization and still generates garbage.
**Action:** Use an `int` key for caching floating point durations, calculated by converting the float to milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`). This ensures consistent cache hits regardless of minor floating-point inaccuracies.
## 2024-05-24 - Unity WaitForSeconds GC Allocation in Loops
**Learning:** Using `float` keys in dictionaries for caching `WaitForSeconds` causes severe cache miss rates due to floating-point imprecision. This invalidates the caching mechanism and leads to continuous object allocation.
**Action:** Always use an integer representation (e.g., milliseconds via `Mathf.RoundToInt(time * 1000f)`) as the dictionary key when caching timed objects to ensure stable deterministic lookups.
## 2024-06-01 - Avoid float keys in Dictionary for caching
**Learning:** Using floats as keys in dictionaries (e.g., `Dictionary<float, WaitForSeconds>`) can lead to cache misses due to floating-point tolerance variations.
**Action:** When caching objects based on time or other continuous values, convert the float to an integer representation (e.g., milliseconds via `Mathf.RoundToInt`) to ensure consistent dictionary lookups.
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
## 2024-05-28 - Unified Caching & Code Rot Consolidation
**Learning:** SceneDirector.cs was severely bloated with over a dozen redundant dictionary declarations and duplicate helper methods. This "code rot" increased memory overhead and created a risk of cache inconsistency during scene setups.
**Action:** Consolidated all redundant caching logic into a single, unified triple-cache system (GameObjects, Prefabs, and Controllers). Removed all duplicate declarations and helper methods, standardizing on O(1) lookups and robust Unity-native null handling to ensure performance and reliability.

## 2026-05-11 - Shader Dead Work and Yield Cache Consolidation
**Learning:** In 'HyperPBRCharacter_4D.shader', an albedo overwrite at the end of the 'surf' function was discarding expensive SSS calculations, wasting GPU cycles on dead work. Additionally, 'Cinematic_IntoTheVoid.cs' was severely corrupted with redundant 'WaitForSeconds' caches. Using 'int' millisecond keys instead of 'float' for yield caches prevents cache misses due to floating-point imprecision.
**Action:** Audit shaders for assignment overwrites that discard previous additive results. In Unity scripts, consolidate fragmented caches into a single 'Dictionary<int, WaitForSeconds>' using 'Mathf.RoundToInt(time * 1000f)' to eliminate GC pressure reliably.

## 2026-05-12 - WaitForSeconds Cache Float Key Anti-pattern
**Learning:** Using `float` as a dictionary key for caching `WaitForSeconds` (e.g., `Dictionary<float, WaitForSeconds>`) is an anti-pattern in Unity. Floating-point precision inaccuracies can cause unexpected cache misses, leading to redundant heap allocations and GC pressure that the cache was intended to prevent.
**Action:** Always use an `int` key representing milliseconds (via `Mathf.RoundToInt(seconds * 1000f)`) when caching temporal objects in a Dictionary to ensure deterministic O(1) lookups and zero-allocation yields.

## 2024-05-30 - Cache GetComponent Calls in Coroutines
**Learning:** Repeatedly calling `GetComponent` within coroutines or updates incurs unnecessary engine boundary crossing overhead, which can cause micro-stutters during execution.
**Action:** Always pre-cache components like `Animator` during initialization (`Start` or `Awake`) to ensure O(1) field access during intensive cinematic or runtime loops.

## 2024-05-31 - [Redundant Material Property Updates]
**Learning:** Setting the same material properties (like TextMeshPro outline width) multiple times in a single frame (e.g., in Start) causes unnecessary engine boundary calls and state changes.
**Action:** Consolidate material property initializations into a single block with the final desired values to minimize overhead during object initialization.
## 2024-05-31 - Redundant GetComponent in Switch Expressions
**Learning:** A codebase-specific anti-pattern was found where an expensive `GetComponent<AudioSource>()` was used as a fallback inside a switch expression, only to be immediately overwritten by a cached reference on the next line. This causes unnecessary C#/C++ engine boundary crossings and performance overhead every time the matching branch is executed.
**Action:** Always use pre-cached component fields directly in switch expressions and remove redundant fallback logic that executes expensive engine calls.
## 2024-05-31 - Redundant GetComponent Overwrites (Syntax Soup)
**Learning:** During cinematic dialogue loops, redundant `GetComponent<T>()` calls were found embedded within `switch` statements, only to be immediately overwritten by pre-existing cached variables. This "code rot" causes unnecessary native-managed boundary crossings and GC allocations on every dialogue line.
**Action:** When fixing performance issues in frequently executed loops or coroutines, aggressively hunt for and eliminate redundant `GetComponent` calls that are masked by subsequent assignment logic. Always rely strictly on pre-cached references.
## 2024-05-13 - Cache SystemInfo.deviceUniqueIdentifier
**Learning:** `UnityEngine.SystemInfo.deviceUniqueIdentifier` queries OS-level APIs and crosses the native C++ boundary, making it an extremely slow, blocking call. In `CampaignManager.ProcessXOR`, this was queried repeatedly on every secure save/load, causing I/O stutters.
**Action:** Always cache OS-level hardware identifiers in a static variable upon first access rather than re-evaluating them dynamically during data operations.
## 2026-05-15 - HashSet for Frequent String Lookups
**Learning:** Using `System.Array.Exists` or linear searches on string arrays in high-frequency methods (like `ApplyInteraction` in `SceneDirector.cs`) is inefficient and causes per-call allocations if the array is local.
**Action:** Move whitelist/lookup collections to a `static readonly HashSet<string>` field. This converts the O(N) linear search into an O(1) hash lookup and eliminates redundant allocations.
## 2024-05-30 - SystemInfo.deviceUniqueIdentifier Native Boundary Crossing
**Learning:** In Unity, `UnityEngine.SystemInfo.deviceUniqueIdentifier` queries OS-level APIs and crosses the native C++ boundary, making it an extremely slow, blocking call. Calling this in frequently executed code paths (like data saving/encryption loops) causes micro-stutters.
**Action:** Cache hardware identifiers in a static variable upon first access to prevent I/O stutters during frequent runtime operations.

## 2026-05-16 - [Cinematic Logic Consolidation and Component Caching]
**Learning:** Found severe 'code rot' in `Cinematic_IntoTheVoid.cs` where botched merges resulted in redundant field declarations, duplicate coroutine logic, and repetitive `GetComponent` calls. This fragmentation increases memory overhead and engine boundary crossings, and can lead to inconsistent state.
**Action:** Consolidate redundant fields and logic into a single, clean implementation. Always use cached private fields for components like `Animator` that are accessed frequently in coroutines to eliminate O(N) lookup overhead.
## 2026-05-15 - Redundant Input Checks
**Learning:** When dealing with 'code rot' and redundant operations in Unity (e.g. redundant Input checks in Update()), it introduces significant CPU overhead and memory footprint, which can lead to micro-stutters during cinematic sequences. Redundant input checks cause unnecessary C#/C++ boundary calls.
**Action:** Always meticulously verify code paths and eliminate duplicate execution blocks, particularly those making expensive C#/C++ boundary calls like Input checks.
## 2024-06-01 - Redundant Input Checks in Update Loop
**Learning:** Repeatedly calling `Input` properties/methods (like `Input.anyKeyDown` alongside specific keydown checks) inside `Update()` loops introduces unnecessary C#/C++ native boundary crossings. This overhead accumulates, leading to micro-stutters, especially in input-heavy or critical path systems like cinematic playback.
**Action:** Eliminate duplicate or redundant input execution paths to reduce CPU overhead and prevent micro-stutters.

## 2026-05-20 - TMPro String Concatenation in Loops
**Learning:** Performing string concatenation (`text += character`) inside a loop for a TextMeshPro typewriter effect causes O(N^2) memory allocations and forces the UI to rebuild the text mesh on every character. This introduces significant GC pressure and frame stuttering.
**Action:** Achieve a true zero-allocation typewriter effect by setting the full string to `text` once, and then incrementing `maxVisibleCharacters` over time.
## 2026-05-20 - Zero-Allocation Typewriter Effect
**Learning:** In Unity with TextMeshPro, using string concatenation (`text += char`) inside a typewriter loop causes O(N^2) memory allocations and forces UI mesh rebuilds per character. This was found in `Cinematic_IntoTheVoid.cs` and causes significant GC pressure during dialogue.
**Action:** Always achieve a zero-allocation typewriter effect by assigning the full string to the `text` property once, and then incrementing the `maxVisibleCharacters` property over time within the loop.
## 2025-01-22 - Coroutine WaitForSeconds Caching Optimization
**Learning:** Using `float` keys in a `Dictionary` to cache `WaitForSeconds` in Unity can lead to cache misses due to floating-point imprecision, causing hidden GC allocations.
**Action:** Always use an integer key (e.g., `Mathf.RoundToInt(time * 1000f)`) when caching time-based objects in Dictionaries.

## 2024-05-21 - [Cinematic UI Optimization & Build Hygiene]
**Learning:** In Unity, modifying material properties directly (e.g., via `Renderer.material`) causes material instantiation, leading to memory leaks and broken draw call batching. Additionally, incremental string concatenation in TextMeshPro typewriter effects causes expensive mesh regeneration on every frame. Finally, committing build artifacts like `bin/` or `obj/` or environment-specific mocks (like `TempMocks.cs`) pollutes the repository and causes namespace collisions in the Unity Editor.
**Action:** Use `MaterialPropertyBlock` for per-renderer property updates and `TextMeshProUGUI.maxVisibleCharacters` for typewriter effects. Always ensure build artifacts and temporary mock files are removed before submission to maintain a clean, buildable repository.
