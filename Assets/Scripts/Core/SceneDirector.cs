using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs; // Assign in Inspector
        public Transform characterSpawnRoot;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

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

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
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
        }

        private void Start()
        {
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
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

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            if (profile == null) return;
            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
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

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // Assign data to controllers
                var controller = characterObj.GetComponent<CharacterControllerBase>();
                if (controller != null)
                {
                    // Create a dummy CharacterData for runtime initialization
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
            if (interaction == null) return;
            GameObject target = GetCachedObject(interaction.objectId);

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
    }
}
