using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;
using System.Text.RegularExpressions;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // 🛡️ Sentinel: Pre-compiled regex for object name validation to improve performance.
        private static readonly Regex _nameValidator = new Regex(@"^[a-zA-Z0-9_\s\(\)\-\$\.\/\[\]]+$", RegexOptions.Compiled);

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void Start()
        {
            if (CampaignManager.Instance?.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR) to critical system singletons
            if (objectName == "CampaignManager" || objectName == "SceneDirector" ||
                objectName == "CameraManager" || objectName == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Access to protected core manager '{objectName}' is denied.");
                return null;
            }

            // 🛡️ Sentinel: DoS Mitigation - Limit string length and validate against whitelist
            if (objectName.Length > 128 || !_nameValidator.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching using ReferenceEquals.
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // Unity's == operator checks if the native object is destroyed.
                if (obj != null) return obj;

                _objectCache.Remove(objectName);
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = obj;

            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            // BOLT: Perform O(1) dictionary lookup
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                // Handle negative caching and destroyed objects
                if (ReferenceEquals(prefab, null)) return null;
                if (prefab != null) return prefab;
            }

            // BOLT: Fallback to O(P) linear search with partial match support
            GameObject? foundPrefab = characterPrefabs?.Find(p => p != null && (p.name == profileName || p.name.Contains(profileName)));

            // BOLT: Cache result (including null for negative caching)
            _prefabCache[profileName] = foundPrefab;
            return foundPrefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;

            // BOLT: Use InstanceID as the dictionary key to avoid garbage generation
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller))
            {
                if (ReferenceEquals(controller, null)) return null;
                if (controller != null) return controller;
            }

            CharacterControllerBase? foundController = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[objId] = foundController;

            return foundController;
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();

            // BOLT: Pre-populate object cache with existing scene objects to avoid lazy O(N) lookups
            foreach (var go in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // BOLT: Performance Boost - Populate cache in a single pass before lookups
            PreWarmCache();

            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance?.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                GameObject? prefab = GetPrefab(profile.name);

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object.
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Use O(1) controller cache to avoid redundant GetComponent.
                var controller = GetCharacterController(characterObj);
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

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            // 🛡️ Sentinel: Prevent IDOR (Insecure Direct Object Reference) tampering with core systems.
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" ||
                interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on core system: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                Debug.Log($"Applying {interaction.action} to {interaction.objectId}");
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

        private void PreWarmCache()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    GameObject[] roots = scene.GetRootGameObjects();
                    foreach (GameObject root in roots)
                    {
                        if (root != null) CacheHierarchyRecursive(root.transform);
                    }
                }
            }
        }

        private void CacheHierarchyRecursive(Transform t)
        {
            // Match GameObject.Find behavior: only cache active objects
            if (t.gameObject.activeInHierarchy && !_objectCache.ContainsKey(t.name))
            {
                _objectCache[t.name] = t.gameObject;
            }

            for (int i = 0; i < t.childCount; i++)
            {
                CacheHierarchyRecursive(t.GetChild(i));
            }
        }

        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches to release Unity object references and prevent leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }
    }
}
