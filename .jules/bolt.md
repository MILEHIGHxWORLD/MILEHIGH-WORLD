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
