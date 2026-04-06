## 2025-11-23 - Null Reference Protection in OmegaOneController

**Vulnerability:** OmegaOneController.CheckStability would crash with a NullReferenceException if the passed 'nearestShard' was null, as it accessed 'shard.name' in a Debug.Log statement.

**Learning:** Even simple logging statements can cause crashes if they dereference objects without null checks.

**Prevention:** Implement explicit null checks for GameObjects before accessing their properties, especially in methods that might be called with dynamic scene references.
