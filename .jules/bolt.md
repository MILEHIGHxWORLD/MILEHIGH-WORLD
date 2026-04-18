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

## 2024-03-20 - GameObject.Find caching during scene initialization
**Learning:** The SceneDirector repeatedly used the highly expensive `GameObject.Find` to resolve objects dynamically based on campaign JSON data during scene setup. For scenarios with multiple interactive objects or characters, this O(N) hierarchy traversal scales poorly.
**Action:** Implemented a simple dictionary cache (`_objectCache`) to memoize object lookups within `SceneDirector`, falling back to `GameObject.Find` only when necessary and caching newly instantiated objects. Always cache `GameObject.Find` results if they might be queried multiple times in a setup loop.
## 2026-03-20 - [GameObject.Find() Cache Optimization]
**Learning:** [SceneDirector.cs was doing expensive `GameObject.Find()` calls in a loop when spawning characters or applying interactions, which scans the entire scene tree. Unity overrides the == operator for GameObjects, making cached null checks a safe and effective way to see if an object was destroyed.]
**Action:** [Use a `Dictionary<string, GameObject>` to cache GameObjects found or created, substituting repeated `GameObject.Find()` calls with dictionary lookups for faster scene setups.]
## 2024-05-24 - O(N*M) lookup bottleneck in SetupScene
**Learning:** SceneDirector.cs used multiple GameObject.Find() calls inside iterative loops across characters and interactions. Since GameObject.Find() traverses the entire scene hierarchy, nesting this in a loop causes significant setup spikes.
**Action:** Use a Dictionary to cache GameObject lookups. When instantiating new objects dynamically, immediately add them to the dictionary cache to avoid secondary lookups.
## 2025-03-15 - [Coroutines in Unity Cinematics]
**Learning:** Unity coroutine `new WaitForSeconds()` creates a new object instance every time it's called, causing GC pressure during long sequences.
**Action:** Cache the yield instructions or use constant values in long coroutines/cinematics to avoid allocating memory for short delays repeatedly.
## 2024-05-24 - Avoid GameObject.Find in Scene Initialization Loops
**Learning:** Using `GameObject.Find` inside loops or scene initialization sequences (like in `SceneDirector.cs`) causes expensive O(N) scene hierarchy traversals, leading to severe performance bottlenecks during level load or scenario updates.
**Action:** Always use dictionary caches (e.g., `Dictionary<string, GameObject>`) to store and retrieve object references instead of relying on `GameObject.Find` inside loops.
## 2024-05-24 - Expensive `GameObject.Find` in Setup Loops
**Learning:** `GameObject.Find` is an O(N) operation that iterates over the entire scene hierarchy. When executed inside setup loops (e.g. `SceneDirector.cs` reading from JSON configurations), it creates a severe performance bottleneck during scene initialization and updates. Furthermore, caching GameObjects via `Dictionary<string, GameObject>` requires checking `obj != null` on retrieval, as Unity's overloaded equality operator natively checks if the object was destroyed in the C++ layer, even if the managed C# reference still exists.
**Action:** Replace `GameObject.Find` inside loops with O(1) Dictionary lookups (`Dictionary<string, GameObject>`). Always check `obj != null` when retrieving cached objects to avoid NullReferenceExceptions on natively destroyed instances.

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
**Action:** Always cache `WaitForSeconds` objects outside the loop or use a shared caching mechanism when the yield duration is constant, returning the cached instance inside the loop.

## 2026-03-25 - [Redundant Member Clutter Performance Impact]
**Learning:** The 'SceneDirector.cs' file was severely cluttered with over a dozen redundant dictionary declarations and duplicate helper methods for GameObject caching. This not only increases memory overhead but also creates a "state fragmentation" risk where different parts of the initialization loop use different caches, leading to redundant O(N) traversals despite the caching intent.
**Action:** Always audit caching implementations for redundancy. Consolidate into a single, unified caching pattern to ensure O(1) lookups are consistent across the entire system.
## 2024-05-25 - WaitForSeconds Cache Miss with float Keys
**Learning:** Using `float` as a dictionary key for caching `WaitForSeconds` objects leads to cache misses due to floating-point precision tolerance variations, defeating the purpose of the cache and causing GC allocations.
**Action:** Use an `int` key representing milliseconds (e.g., via `Mathf.RoundToInt(time * 1000f)`) to ensure reliable cache hits and eliminate GC allocations during coroutine execution.
