using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefabs to avoid O(P) linear search through characterPrefabs list
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab lookup cache to avoid O(P) linear searches in characterPrefabs list
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null) return null;

                return obj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Cache the result even if null (negative caching) to prevent repeated searches.
            _objectCache[objectName] = obj;

            return obj;
        }

        private void Start()
        {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabLookupCache.Clear();

            // BOLT: Pre-populate prefab cache for O(1) lookup during spawning
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        // We use the full name as the key. Character names usually match or contain the prefab name.
                        if (!_prefabLookupCache.ContainsKey(prefab.name))
                        {
                            _prefabLookupCache[prefab.name] = prefab;
                        }
                    }
                }
            }

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                // Instantiate characters if not already in scene
                foreach (var charProfile in campaignData.characters)
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
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                // instead of characterPrefabs.Find (O(P))
                GameObject? prefab = null;

                // Try exact match first
                if (!_prefabLookupCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact match fails (legacy support)
                    foreach (var kvp in _prefabLookupCache)
                    {
                        if (kvp.Key != null && kvp.Key.Contains(profile.name))
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
    }
}
