## 2024-05-24 - O(N*M) lookup bottleneck in SetupScene
**Learning:** SceneDirector.cs used multiple GameObject.Find() calls inside iterative loops across characters and interactions. Since GameObject.Find() traverses the entire scene hierarchy, nesting this in a loop causes significant setup spikes.
**Action:** Use a Dictionary to cache GameObject lookups. When instantiating new objects dynamically, immediately add them to the dictionary cache to avoid secondary lookups.
