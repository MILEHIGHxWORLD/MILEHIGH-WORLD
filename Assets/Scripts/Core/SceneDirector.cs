using System;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // 🛡️ Sentinel: Regex for whitelisting safe object names to prevent DoS via expensive GameObject.Find operations.
        private static readonly Regex _nameWhitelist = new Regex(@"^[a-zA-Z0-9_\s\-.\[\]\(\)\$]+$", RegexOptions.Compiled);
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private static readonly System.Text.RegularExpressions.Regex _nameWhitelist = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9_\s\(\)\-$\.\/\[\]]+$", System.Text.RegularExpressions.RegexOptions.Compiled);
        // Note: Using GameObject? to support negative caching (storing confirmed nulls)
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // BOLT: Component cache to avoid redundant GetComponent calls.
        // Note: Using CharacterControllerBase? to support negative caching.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // Uses GameObject? for negative caching (null for confirmed missing objects)
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) list searches.
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        public List<GameObject> characterPrefabs; // Assign in Inspector
        public Transform characterSpawnRoot;

        // BOLT: Unified caching system to replace multiple redundant/conflicting declarations.
        // Uses O(1) lookups to eliminate expensive O(N) scene traversals and linear list searches.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();


        // 🛡️ Sentinel: Whitelist regex for object names to prevent malicious input.
        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\-\[\]\.]+$", RegexOptions.Compiled);
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls.
        // Performance Insight: Dictionary.TryGetValue is O(1).
        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();
        // BOLT: Prefab lookup cache to avoid O(P) linear searches
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Prefab lookup cache to avoid O(P) linear searches in characterPrefabs list
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();
        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and linear searches
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefab lookups to prevent O(N) list searches on instantiation
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();
        // BOLT: Consolidated caches for GameObjects, prefabs, and controllers to prevent expensive searches and GetComponent calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefab lookups to avoid O(N) list searches
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
1        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Component cache to avoid redundant GetComponent calls
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            // 🛡️ Sentinel: Implement input length limits and validation to prevent DoS via expensive Find operations.
            if (string.IsNullOrEmpty(objectName) || objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
                return null;
            }

            // 🛡️ Sentinel: DoS Mitigation - Enforce length limit and character whitelist on object names.
            if (objectName.Length > 128 || !_nameWhitelist.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious object name: {objectName}");
            // 🛡️ Sentinel: DoS Mitigation - Limit string length and validate against whitelist
            // to prevent expensive GameObject.Find from being triggered by malicious or oversized input.
            if (objectName.Length > 128 || !_nameWhitelist.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            GameObject? obj;
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            if (_objectCache.TryGetValue(objectName, out obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            if (_objectCache.TryGetValue(objectName, out obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // by removing it from cache and falling through to GameObject.Find.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // BOLT: Perform O(1) lookup
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Return cached reference (Unity's == operator handles destruction check)
                if (obj != null || System.Object.ReferenceEquals(obj, null))
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
        // BOLT: Cache for character prefabs to turn O(P) list searches into O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private void Start()
        private void EnsurePrefabCache()
        {
            if (_prefabCache.Count > 0 || characterPrefabs == null) return;

            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        // BOLT: Cache for prefabs to prevent O(N) list searches during instantiation
        private Dictionary<string, GameObject> _prefabCache;

        private void InitializePrefabCache()
        {
            if (_prefabCache != null) return;

            _prefabCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        // BOLT: Cache to prevent O(N) list searches with string comparisons inside setup loops
        private Dictionary<string, GameObject> _prefabCache;

        // BOLT: Cache for character prefabs to prevent O(N) list searches during scene setup
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private GameObject GetCachedPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName) || characterPrefabs == null) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject prefab))
            {
                return prefab; // Will return null if it was cached as null
            }

            // Fallback to O(N) search and cache the result (even if null)
            prefab = characterPrefabs.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        // BOLT: Cache for prefab lookups to avoid O(N) list search with string comparisons
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for prefabs to replace O(M) list searching with O(1) dictionary lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            // BOLT: Pre-populate prefab cache for O(1) retrieval during character spawning
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        // BOLT: Cache for prefab lookups to avoid O(N) string comparisons in loops
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Prefab cache to replace O(M) list searching with O(1) dictionary lookup
        private Dictionary<string, GameObject> _prefabCache;

        // BOLT: Prefab cache to replace O(M) linear search through characterPrefabs list with O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        // BOLT: Cache for prefabs to prevent expensive O(N) List.Find string matching inside loops
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for prefab lookups to prevent O(N) list searches with string comparisons
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Prefab cache to avoid O(M) linear searches through characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            if (characterPrefabs == null) return;
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        // BOLT: Cache for character prefabs to replace O(M) linear searches with O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs == null) return;

            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
        }

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                if (System.Object.ReferenceEquals(obj, null)) return null;
                if (obj == null)
            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // 🛡️ Sentinel: Mitigate DoS via complex GameObject.Find queries.
            // Restrict name length and characters to prevent expensive scene traversals with complex patterns.
            if (objectName.Length > 128) return null;

            foreach (char c in objectName)
            {
                if (!char.IsLetterOrDigit(c) && c != '_' && c != ' ' && c != '(' && c != ')')
                {
                    return null;
                }
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                // ReferenceEquals(obj, null) is true for real nulls (including negative cache hits).
                if (ReferenceEquals(obj, null))
                {
                    return null; // Negative cache hit
                }

                if (obj != null)
                {
                    return obj;
                }
                // Object was destroyed but still in cache, remove and fallback
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                // BOLT: Unity's custom == operator checks if the native object was destroyed.
                // We use ReferenceEquals to check for literal nulls (negative cache hits).
                if (obj != null) return obj;
                if (ReferenceEquals(obj, null)) return null; // Genuine negative cache hit

                // If obj == null but ReferenceEquals is false, it means the object was destroyed.
                // We remove it and fall back to Find.
                _objectCache.Remove(objectName);
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);
            // BOLT: Cache the result, including nulls (negative caching) to prevent repeated O(N) traversals
            _objectCache[objectName] = obj;
            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Implement negative caching to prevent repeated expensive searches for non-existent objects
            if (obj != null)
            {
                _objectCache[objectName] = obj;
            }
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
            }

                // If it's a Unity null (native object destroyed), we should try to find it again
                if (obj == null)
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Robust negative caching - use ReferenceEquals to check for real nulls
                // vs fake nulls (destroyed Unity objects).
                if (ReferenceEquals(obj, null)) return null; // Negative cache hit

                if (obj != null) return obj; // Valid cached object
            {
                // BOLT: Performance Learning - Robust negative caching requires checking both Unity null (destroyed) and C# null (not found)
                // If ReferenceEquals is true, it's a legitimate negative cache hit (real null).
                // If ReferenceEquals is false but obj == null is true, the object was destroyed (fake null).
                if (ReferenceEquals(obj, null)) return null;

                if (obj != null) return obj;
            // Performance Insight: We use negative caching (storing 'null' for missing objects)
            // to avoid repeated O(N) scene traversals for objects that don't exist.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity overrides the == operator for GameObjects. If 'obj' was destroyed, it returns null.
                // We must check if the reference is actually valid.
                if (ReferenceEquals(obj, null))
                {
                    // This is a legitimate negative cache hit (real null).
                    return null;
                }

                if (obj == null)
                {
                    // Object was destroyed (fake null). Remove from cache and continue.
                    _objectCache.Remove(objectName);
                }
                else
                {
                    // Valid object found.
                    return obj;
                }
            }

            obj = GameObject.Find(objectName);
            if (obj != null) _objectCache[objectName] = obj;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);

            // Performance Insight: Even if obj is null, we store it to enable negative caching.
            _objectCache[objectName] = obj;

            return obj;
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Performance optimization - use ReferenceEquals for native null checks.
                // If both are true, it's a legitimate negative cache hit (real null).
                if (ReferenceEquals(obj, null))
                {
                    return null;
                }

                // If obj == null is true but ReferenceEquals is false, the object was destroyed (fake null)
                if (obj != null)
                {
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Implement negative caching to prevent repeated O(N) lookups for missing objects
            _objectCache[objectName] = obj;

            return obj;
            {
                // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                // If it's not null and not a "fake null" (destroyed), return it.
                if (obj != null) return obj;

                // BOLT: Implement negative caching and handle "fake nulls" (destroyed objects).
                // If obj == null but ReferenceEquals is false, it's a destroyed object; otherwise it's a legitimate null cache.
                if (System.Object.ReferenceEquals(obj, null)) return null; // Legitimate negative cache hit (real null)
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or cache was stale.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals.
            // We store the result in the dictionary. If Find returns null, it's a 'true' null.
            _objectCache[objectName] = obj; // Cache the result, even if it's null (negative caching)

            return obj;
            {
                // BOLT: Robust negative caching check.
                // Unity's '==' operator returns true for destroyed objects (fake nulls).
                // ReferenceEquals(obj, null) is only true for "real" C# nulls (negative cache hits).
                if (obj != null) return obj;
                if (System.Object.ReferenceEquals(obj, null)) return null;
                // If it's a "fake null", we fall through to re-find it in the scene.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Handle "fake nulls" (destroyed native objects) and negative caching.
                // If the reference is not null but Unity says it is, the object was destroyed.
                if (obj == null && !ReferenceEquals(obj, null))
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj; // Return cached object or legitimate null (negative cache hit)
                }

                // If Unity object was destroyed but entry isn't a negative cache entry, remove it
                _objectCache.Remove(objectName);
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null)
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null)
            // Fallback to scene traversal
            obj = GameObject.Find(objectName);
            // Note: ReferenceEquals(obj, null) checks if the managed reference is null,
            // while obj == null (Unity's override) checks if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
1            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Distinguish between negative cache (true null) and destroyed object (Unity null)
                if (ReferenceEquals(obj, null)) return null;
                if (obj == null) // Unity object was destroyed
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
            }

            GameObject foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj; // negative cache if not found
            return foundObj;
        }

            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            GameObject? foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = foundObj;
            return foundObj;
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                if (!ReferenceEquals(obj, null) && obj != null)
                {
                    return obj;
                }
                _objectCache.Remove(objectName); // Clean up stale/destroyed references
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache the result, including nulls for negative caching
            _objectCache[objectName] = obj;

            return obj;
            // BOLT: Negative caching - store the result even if it is null to avoid repeated O(N) traversals
            _objectCache[objectName] = obj;

            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName))!;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));

            GameObject? prefab;
            if (_prefabCache.TryGetValue(profileName, out prefab)) return prefab;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                if (prefab != null) return prefab;
            }

            // BOLT: Optimized prefab lookup using characterPrefabs list
            // We do a partial match search only if the name isn't already cached.
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            }

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search only happens once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
            }
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // Fallback to searching the list if not pre-cached
            if (characterPrefabs != null)
            {
                GameObject? foundPrefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                if (foundPrefab != null)
                {
                    _prefabCache[profileName] = foundPrefab;
                    return foundPrefab;
                }
            }
            return null;
        }

            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p.name.Contains(profileName));

            // BOLT: Cache result (including null for negative caching) to ensure O(1) subsequent lookups
            if (prefab != null)
            {
                _prefabCache[profileName] = prefab;
            }
            return prefab;
        }

            return prefab;
        }

            if (prefab != null) _prefabCache[profileName] = prefab;
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
        private CharacterControllerBase? GetCharacterController(GameObject? characterObj)
            // BOLT: O(P) search only happens once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
            prefab = characterPrefabs.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
        private void InitializePrefabCache()
        {
            if (_prefabCache != null) return;

            _prefabCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
            // BOLT: Cache the result even if null to prevent repeated failed lookups (negative caching).
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // Fallback for partial matches if not in cache
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                _prefabLookupCache[profileName] = prefab;
            }

        private CharacterControllerBase GetCachedController(GameObject characterObj)
        {
            InitializePrefabCache();

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        private void InitializePrefabCache()
        {
            _prefabCache.Clear();
            if (characterPrefabs == null) return;

            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                {
                    _prefabCache[prefab.name] = prefab;
                }
            }
            if (_prefabCache != null) return;
            _prefabCache = new Dictionary<string, GameObject>();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        // Use the prefab name as key for fast lookup
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        private void Start()
        private GameObject? GetPrefab(string profileName)
        {
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            if (string.IsNullOrEmpty(profileName)) return null;
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                if (prefab != null) _prefabCache[profileName] = prefab;
            }
            if (_prefabLookupCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabLookupCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject? characterObj)
        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            CharacterControllerBase? controller;
            if (_controllerCache.TryGetValue(objId, out controller)) return controller;
            if (_controllerCache.TryGetValue(objId, out var controller))
            {
                if (controller != null) return controller;
            }
            // BOLT: Use dictionary lookup with negative caching support
            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();

            // BOLT: Cache component reference to avoid redundant GetComponent calls
            // BOLT: Cache the result even if null (negative caching) to avoid redundant GetComponent calls
            _controllerCache[objId] = controller;
            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            // BOLT: Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            InitializePrefabCache();
            // BOLT: Using InstanceID (int) for cache key to avoid string allocations
            if (_controllerCache.TryGetValue(objId, out var controller) && controller != null)
                return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            // BOLT: Clean up controller cache to avoid memory leaks of destroyed objects
            _controllerCache.Clear();

            InitializePrefabCache();

            // Performance Insight: Clear cache at start of setup to prevent stale references or
            // negative cache leakage between scenarios if objects were created/destroyed.
            // Clear cache at start of setup to avoid stale references across scenes
            // BOLT: Clear scene-specific caches to avoid stale references across scenarios/scenes.
            // Prefab cache is persisted as it remains valid across the session.
            _objectCache.Clear();
            _prefabCache.Clear();
            // BOLT: Clear lookup caches at start of setup to avoid stale references and memory leaks across scenes.
            _objectCache.Clear();
            _prefabCache.Clear();
            // BOLT: Populate prefab cache once per initialization
            EnsurePrefabCache();
            // Clear object and controller caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                SpawnOrUpdateCharacter(charProfile);
            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            if (controller != null)
            {
                _controllerCache[objId] = controller;
            }
            return controller;
        }

        private void Start()
        {
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups
            // BOLT: Pre-populate prefab cache for O(1) lookups
            if (characterPrefabs != null)
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabLookupCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
            // UNITY NRT Flow Analysis Pattern: Capture singleton property in local variable
            var campaignManager = CampaignManager.Instance;
            if (campaignManager != null)
            {
                var data = campaignManager.currentCampaignData;
                if (data != null && data.scenarios != null && data.scenarios.Count > 0)
                {
                    SetupScene(data.scenarios[0]);
                }
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            // 🛡️ Sentinel: Capture singleton property to local for NRT flow analysis
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            // BOLT: Capture property in local variable for NRT flow analysis
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            if (CampaignManager.Instance?.currentCampaignData != null)
            {
                    if (prefab != null)
                    {
                         _prefabCache[prefab.name] = prefab;
                         _prefabLookupCache[prefab.name] = prefab;
                    }
                }
            }

            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
                    if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                    }
                    if (prefab != null) _prefabLookupCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
                if (CampaignManager.Instance.currentCampaignData.scenarios != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
                {
                    SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                }
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            _objectCache.Clear();
            _controllerCache.Clear();

            _objectCache.Clear();
            _controllerCache.Clear();

            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Clear scene-specific caches to avoid stale references and memory leaks.
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Clear dynamic caches to avoid stale references across scenarios
            _objectCache.Clear();
            _controllerCache.Clear();

            // Instantiate characters if not already in scene
            var campaignManager = CampaignManager.Instance;
            if (campaignManager != null)
            {
                var data = campaignManager.currentCampaignData;
                if (data != null && data.characters != null)
                {
                    foreach (var charProfile in data.characters)
                    {
                        if (charProfile != null)
                        {
                            SpawnOrUpdateCharacter(charProfile);
                        }
            if (CampaignManager.Instance.currentCampaignData != null)
            // 🛡️ Sentinel: Capture singleton property to local for NRT flow analysis
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            if (CampaignManager.Instance.currentCampaignData != null)
            if (CampaignManager.Instance?.currentCampaignData != null)
            _objectCache.Clear();
            _controllerCache.Clear();

            if (CampaignManager.Instance?.currentCampaignData?.characters != null)
            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                    if (charProfile != null)
                    {
                        SpawnOrUpdateCharacter(charProfile);
                    }
                }
            }

            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    if (interaction != null) ApplyInteraction(interaction);
                    if (interaction != null)
                    {
                        ApplyInteraction(interaction);
                    }
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            if (profile == null) return;
            GameObject? characterObj = GetCachedObject(profile.name);
            InitializePrefabCache();
            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using dictionary instead of linear search
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback for partial name matching if exact match fails
                if (prefab == null && characterPrefabs != null)
                {
                    prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profile.name));
                }
                // BOLT: Optimized O(1) prefab lookup via dictionary cache
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback for partial name matches if exact match fails (maintaining existing behavior)
                // BOLT: Use O(1) prefab cache helper
                GameObject prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                GameObject? prefab = null;
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Use O(1) prefab cache helper
                // BOLT: O(1) prefab lookup via dictionary
                if (_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                // BOLT: Use O(1) prefab cache lookup instead of O(M) linear Find
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback to partial name match only if exact match fails
                if (prefab == null && characterPrefabs != null)
                {
                    prefab = characterPrefabs.Find(p => p.name.Contains(profile.name));
                }
                // BOLT: Optimized prefab lookup using O(1) dictionary instead of O(M) List.Find
                GameObject prefab = null;

                // Try direct match first
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to fuzzy search if necessary, but keep it minimal
                GameObject? prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using the pre-populated dictionary (O(1) vs O(M))
                // Try exact match first for O(1) performance
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to fuzzy match if exact match fails (O(M))
                    foreach (var kvp in _prefabCache)
                    {
                        if (kvp.Key.Contains(profile.name))
                        {
                            prefab = kvp.Value;
                            break;
                        }
                    }
                }
                // Try to find prefab if not in scene
                GameObject prefab;
                // BOLT: Lazy evaluation dictionary lookup for prefabs to prevent O(N) list search on every instantiation. Includes negative caching.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                GameObject prefab = null;
                // BOLT: Lazy evaluation of prefab caching. Includes negative caching (caching nulls).
                // BOLT: O(1) cache lookup with lazy fallback to O(N) search (including negative caching)
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                }
                if (_prefabCache.TryGetValue(profile.name, out GameObject cachedPrefab))
                {
                    prefab = cachedPrefab;
                GameObject prefab = GetCachedPrefab(profile.name);
                // BOLT: Replaced O(N) list search with O(1) dictionary lookup where possible
                GameObject prefab = null;
                if (_prefabCache != null)
                {
                    // Fast exact match
                    if (!_prefabCache.TryGetValue(profile.name, out prefab))
                    {
                        // Fallback to Contains if exact match fails, and cache the result for O(1) next time
                        prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                        if (prefab != null)
                        {
                            _prefabCache[profile.name] = prefab;
                        }
                    }
                }
                else
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                }
                // Try to find prefab if not in scene, first using O(1) lookup
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    // Fallback to O(N) contains if exact name match fails, then cache the result
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null)
                    {
                        _prefabCache[profile.name] = prefab;
                    }
                }
                // BOLT: O(1) prefab lookup via cache instead of O(M) list search.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab) || prefab == null)
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                }
                // BOLT: Optimized prefab lookup using O(1) dictionary cache instead of O(P) linear search
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                // BOLT: Optimized O(1) prefab lookup via dictionary
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact name not in cache
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                // BOLT: O(1) prefab lookup instead of O(P) list search.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                // BOLT: Use O(1) prefab cache helper
                GameObject prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                GameObject? prefab = GetPrefab(profile.name);

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object to prevent redundant scene searches
                    // BOLT: Immediately cache the newly instantiated object to resolve negative cache hits.
                    // BOLT: Immediately cache the newly instantiated object to prevent redundant searches
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                var controller = GetCharacterController(characterObj);
                var controller = GetCachedController(characterObj);
                if (controller != null)
                {
                    CharacterData data = ScriptableObject.CreateInstance<CharacterData>();
                    data.characterName = profile.name;
                    data.role = profile.role;
                    data.traits = profile.traits;
                    data.behaviorScript = profile.behaviorScript;

                    controller.Initialize(data);
                }
            }
        }

        // 🛡️ Sentinel: Blocklist of critical system managers to prevent Insecure Direct Object Reference (IDOR)
        // by stopping untrusted external JSON data from manipulating core application infrastructure.
        private static readonly HashSet<string> ProtectedManagers = new HashSet<string>
        {
            "CampaignManager",
            "SceneDirector",
            "CameraManager",
            "AlliancePowerManager"
        };

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            // 🛡️ Sentinel: Security validation to prevent IDOR-like tampering of critical system objects
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager")
            {
            // 🛡️ Sentinel: Apply IDOR validation blocklist at the application boundary where external JSON is processed.
            if (string.IsNullOrEmpty(interaction.objectId) || ProtectedManagers.Contains(interaction.objectId))
            {
                Debug.LogWarning($"Security Error: Blocked unauthorized manipulation attempt on protected object '{interaction.objectId}'.");
        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Handle Unity's native object lifetime (fake nulls)
                if (obj != null) return obj;
                _objectCache.Remove(objectName);
            }

            GameObject foundObj = GameObject.Find(objectName);
            if (foundObj != null) _objectCache[objectName] = foundObj;

            return foundObj;
        }

        private CharacterControllerBase GetCachedController(GameObject characterObj)
        {
            if (characterObj == null) return null;

            int instanceID = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(instanceID, out CharacterControllerBase controller))
            {
                if (controller != null) return controller;
                _controllerCache.Remove(instanceID);
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[instanceID] = controller;

            return controller;
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            if (interaction == null) return;
            // 🛡️ Sentinel: Prevent IDOR vulnerabilities by blocking untrusted external data from targeting core architectural singletons.
            // SECURITY: Prevent IDOR tampering with core system managers via untrusted JSON data
            // 🛡️ Sentinel: Prevent IDOR by blocking manipulation of critical system managers
            if (interaction == null) return;
            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR)
            // Block external data from manipulating core architectural managers.
            string[] protectedObjects = { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager" };
            foreach (string protectedObj in protectedObjects)
            {
                if (interaction.objectId == protectedObj)
                {
                    Debug.LogError($"[Security] Blocked unauthorized interaction attempt on protected object: {interaction.objectId}");
                    return;
                }
            // SENTINEL: Prevent IDOR vulnerabilities by blocking untrusted external interactions
            // from manipulating core architectural singletons and managers.
            // SENTINEL: Prevent IDOR by blocking access to core architectural singletons.
            // External JSON data should not be able to manipulate these critical managers.
            // 🛡️ Sentinel: Prevent IDOR (Insecure Direct Object Reference)
            // Block external modification of core architectural singletons.
            // SECURITY: Prevent IDOR (Insecure Direct Object Reference). Block external JSON from manipulating core managers.
            // 🛡️ Sentinel: Prevent IDOR-like tampering of critical system objects.
            // External JSON could attempt to manipulate core managers by passing their exact names.
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" ||
                interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on protected system object: {interaction.objectId}");
                Debug.LogWarning("Security: Blocked unauthorized interaction with core system manager.");
                Debug.LogWarning($"[Security] Blocked unauthorized interaction with protected system object: {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized attempt to interact with protected core system: {interaction.objectId}");
                Debug.LogWarning($"Security Block: Unauthorized interaction attempt on protected object {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized attempt to manipulate protected object: {interaction.objectId}");
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on protected object: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on system object: {interaction.objectId}");
                return;
            }

            // SECURITY: Exact string matching to prevent IDOR-like tampering of critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on critical system object: {interaction.objectId}");
                return;
            }

            // 🛡️ Sentinel: Security enhancement to prevent IDOR-like manipulation of core objects
            // Do not allow external JSON data to manipulate critical system objects
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector")
            {
                Debug.LogWarning($"[Security] Blocked attempt to manipulate protected object: {interaction.objectId}");
                return;
            }

            if (interaction == null) return;
            GameObject target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                if (interaction.isVector)
                {
                    target.transform.position = interaction.GetVectorValue();
                }
                else
                {
                    target.transform.localScale = Vector3.one * interaction.floatValue;
                }
            }
        }

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }
    }
}
