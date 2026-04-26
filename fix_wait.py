import re

with open("Assets/Scripts/Cinematics/Cinematic_IntoTheVoid.cs", "r") as f:
    content = f.read()

# Replace float key with int key for WaitForSeconds cache
# _waitForSecondsCache = new Dictionary<float, WaitForSeconds>(); -> Dictionary<int, WaitForSeconds>();
content = re.sub(
    r"private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>\(\);",
    r"private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();",
    content
)

# Replace GetWait method
new_getwait = """    private WaitForSeconds GetWait(float time)
    {
        int key = UnityEngine.Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        }
        return wait;
    }"""
content = re.sub(
    r"    private WaitForSeconds GetWait\(float time\)\s*\{\s*if \(!_waitForSecondsCache\.TryGetValue\(time, out var wait\)\)\s*\{\s*wait = new WaitForSeconds\(time\);\s*_waitForSecondsCache\[time\] = wait;\s*\}\s*return wait;\s*\}",
    new_getwait,
    content
)

with open("Assets/Scripts/Cinematics/Cinematic_IntoTheVoid.cs", "w") as f:
    f.write(content)
