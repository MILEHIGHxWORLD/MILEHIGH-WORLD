using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        [Header("Settings")]
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public Transform? characterSpawnRoot;

        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals and linear searches
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // 🛡️ Sentinel: Whitelist regex for object names to prevent malicious input and DoS via complex GameObject.Find queries.
        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\-\[\]\.\$\/]+$", RegexOptions.Compiled);

        private void Awake()
        {
            InitializePrefabCache();

            // BOLT: Capture singleton property for NRT flow analysis
            var campaignData = CampaignManager.Instance?.currentCampaignData;
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
                if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                {
                    if (!_prefabCache.ContainsKey(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            // BOLT: O(1) exact match lookup
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: Fallback to O(P) linear search for partial matches
            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profileName));
                // Cache the result (including negative results) to prevent repeated linear scans
                _prefabCache[profileName] = prefab;
                return prefab;
            }

            return null;
        }

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: DoS Mitigation - Limit string length and validate against whitelist
            if (objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            // BOLT: O(1) lookup
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Managed reference is null: object was never found (cached negative result).
                if (ReferenceEquals(obj, null)) return null;

                // Managed reference exists, check if Unity object is still alive.
                if (obj != null) return obj;

                // Managed reference is non-null but obj == null: object was destroyed.
                _objectCache.Remove(objectName);
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = obj;

            return obj;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;

            int objId = characterObj.GetInstanceID();

            // BOLT: O(1) component cache lookup
            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller))
            {
                if (controller != null) return controller;
                _controllerCache.Remove(objId);
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();

            // BOLT: Cache result even if null (negative caching) to avoid redundant GetComponent calls
            _controllerCache[objId] = controller;

            return controller;
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear scene-specific caches to avoid stale references across scenarios.
            _objectCache.Clear();
            _controllerCache.Clear();

            var campaignData = CampaignManager.Instance?.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
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
                    if (interaction != null)
                    {
                        ApplyInteraction(interaction);
                    }
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache to avoid searching the scene next time
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
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

            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR)
            if (interaction.objectId == "CampaignManager" ||
                interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" ||
                interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on protected object: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);
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
            // BOLT: Clear caches to release references and prevent memory leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }
    }
}
