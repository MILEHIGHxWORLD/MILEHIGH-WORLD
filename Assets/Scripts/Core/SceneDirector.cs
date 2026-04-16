using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = null!; // Assign in Inspector
        public Transform? characterSpawnRoot;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            // We use System.Object.ReferenceEquals(obj, null) to distinguish between a truly null reference
            // and a destroyed Unity object, which allows us to implement "negative caching" for missing objects.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Managed reference is null: object was never found (cached negative result).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // Managed reference exists, check if Unity object is still alive.
                if (obj != null) return obj;

                // Managed reference is non-null but obj == null: object was destroyed.
                return null;
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);
            // Cache the result (including null if not found) for future O(1) retrieval.
            _objectCache[objectName] = obj!;
            return obj;
        }

        private void OnDestroy()
        {
            // BOLT: Ensure references are released to prevent memory leaks, especially with negative caching.
            _objectCache.Clear();
            _prefabCache.Clear();
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

            // BOLT: Optimization - Pre-populate the cache with a single scene traversal.
            // This avoids multiple O(N) GameObject.Find calls later.
            _objectCache.Clear();
            var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.None, FindObjectsSortMode.None);
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

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
                // BOLT: Optimized prefab lookup with O(1) dictionary cache.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
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
