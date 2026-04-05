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

        // BOLT: Prefab cache to replace O(M) list searching with O(1) dictionary lookup
        private Dictionary<string, GameObject> _prefabCache;

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Performance Learning - Robust negative caching requires checking both Unity null (destroyed) and C# null (not found)
                // If ReferenceEquals is true, it's a legitimate negative cache hit (real null).
                // If ReferenceEquals is false but obj == null is true, the object was destroyed (fake null).
                if (ReferenceEquals(obj, null)) return null;

                if (obj != null) return obj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);

            // BOLT: Negative caching - store the result even if it is null to avoid repeated O(N) traversals
            _objectCache[objectName] = obj;

            return obj;
        }

        private void InitializePrefabCache()
        {
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
        {
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            InitializePrefabCache();

            // Instantiate characters if not already in scene
            foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
            {
                SpawnOrUpdateCharacter(charProfile);
            }

            // Execute interactive objects logic
            foreach (var interaction in scenario.interactiveObjects)
            {
                ApplyInteraction(interaction);
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Optimized prefab lookup using O(1) dictionary instead of O(M) List.Find
                GameObject prefab = null;

                // Try direct match first
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to fuzzy search if necessary, but keep it minimal
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
