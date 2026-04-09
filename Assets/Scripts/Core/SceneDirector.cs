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
                _objectCache.Remove(objectName);
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);

            // BOLT: Implement negative caching to prevent repeated expensive searches for non-existent objects
            _objectCache[objectName] = obj;

            return obj;
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

            // Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();

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
                // BOLT: Optimized prefab lookup using dictionary instead of linear search
                _prefabCache.TryGetValue(profile.name, out GameObject prefab);

                // Fallback for partial name matching if exact match fails
                if (prefab == null && characterPrefabs != null)
                {
                    prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profile.name));
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
