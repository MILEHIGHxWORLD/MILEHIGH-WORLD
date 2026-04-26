with open(".jules/bolt.md", "a") as f:
    f.write("\n## 2024-05-24 - Unity WaitForSeconds Float Dictionary Cache Issue\n")
    f.write("**Learning:** When caching `WaitForSeconds` in a Dictionary to avoid GC allocations during coroutines, using a `float` as the key can lead to cache misses due to floating-point precision issues. This undermines the optimization and still generates garbage.\n")
    f.write("**Action:** Use an `int` key for caching floating point durations, calculated by converting the float to milliseconds (e.g., `Mathf.RoundToInt(time * 1000f)`). This ensures consistent cache hits regardless of minor floating-point inaccuracies.\n")
