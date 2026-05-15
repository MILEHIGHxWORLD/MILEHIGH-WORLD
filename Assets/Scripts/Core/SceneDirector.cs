using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals and linear searches.
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // BOLT: Cache protected managers in a HashSet for O(1) lookups and to avoid per-interaction allocations.
        private static readonly HashSet<string> _protectedManagers = new HashSet<string> { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager" };

        // 🛡️ Sentinel: Regex for whitelisting safe object names to prevent DoS via expensive GameObject.Find operations.
        private static readonly Regex _nameValidator = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\.\/\[\]]+$", RegexOptions.Compiled);

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void Start()
        {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches to release Unity object references and prevent memory leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
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

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null || !scenario.IsValid()) return;

            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear scene-specific caches at start of setup to avoid stale references.
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Performance Boost - Pre-populate object cache with existing scene objects
            // in a single pass to avoid lazy O(N) lookups later.
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnCharacter(charProfile);
                }
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
                }
            }
        }

        public void SpawnCharacter(CharacterProfile profile)
        {
            if (profile == null || !profile.IsValid()) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                GetCharacterController(characterObj);
            }
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            // 🛡️ Sentinel: Prevent IDOR (Insecure Direct Object Reference) tampering with core systems.
            // We trim leading slashes to prevent bypasses using path-like IDs (e.g., "/CampaignManager").
            string[] protectedManagers = { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager", "GlobalResonanceManager" };
            string sanitizedId = interaction.objectId.TrimStart('/');

            if (System.Array.Exists(protectedManagers, m => m == sanitizedId))
            // BOLT: Use cached HashSet for efficient lookup and zero per-call allocation.
            if (_protectedManagers.Contains(interaction.objectId))
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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: DoS Mitigation - Enforce length limit and character whitelist on object names.
            if (objectName.Length > 128 || !_nameValidator.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching using ReferenceEquals.
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
            }

            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                if (ReferenceEquals(prefab, null)) return null;
                if (prefab != null) return prefab;
            }

            prefab = characterPrefabs?.Find(p => p != null && (p.name == profileName || p.name.Contains(profileName)));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            int id = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(id, out CharacterControllerBase? controller))
            {
                if (ReferenceEquals(controller, null)) return null;
                if (controller != null) return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[id] = controller;
            return controller;
        }
    }
}
