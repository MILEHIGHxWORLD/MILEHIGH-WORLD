## 2025-01-20 - Renderer.material Allocation Pattern
**Learning:** In Unity, accessing `Renderer.material` at runtime instantiates a material clone on the heap. This breaks draw call batching (SRP/GPU instancing) and causes GC allocations for every access.
**Action:** Use a `MaterialPropertyBlock` (via `GetPropertyBlock` and `SetPropertyBlock`) to apply per-renderer shader modifications without cloning the material.
