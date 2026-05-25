## 2025-05-15 - [Cinematic Typewriter Allocation-Free Pacing]
**Learning:** Using `string.Substring` or string concatenation inside a high-frequency loop (like a typewriter effect) re-introduces $O(n^2)$ memory pressure and defeats the purpose of `maxVisibleCharacters` optimization.
**Action:** Use manual character indexing and multi-character comparison logic (e.g., `content[i-4] == 'S'`) to implement look-ahead or look-behind logic in loops without allocating temporary strings.
