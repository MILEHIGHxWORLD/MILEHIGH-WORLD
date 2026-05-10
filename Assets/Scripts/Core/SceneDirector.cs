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

        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and linear searches
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
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
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);
            // BOLT: Implement negative caching by storing null results.
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

            // BOLT: Clear lookup caches at start of setup to avoid stale references and memory leaks across scenes.
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();

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
                // BOLT: Optimized prefab lookup using O(1) dictionary cache instead of O(P) linear search
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
                }

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object to prevent redundant searches
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Avoid expensive GetComponent calls by caching character controllers using InstanceID
                int instanceId = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(instanceId, out CharacterControllerBase controller))
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    _controllerCache[instanceId] = controller;
                }

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
