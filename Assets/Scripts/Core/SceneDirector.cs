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

        // BOLT: Cache for character prefabs to turn O(P) list searches into O(1) lookups
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        // BOLT: Cache for character controllers to avoid redundant GetComponent calls
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

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

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            // Note: ReferenceEquals(obj, null) checks if the managed reference is null,
            // while obj == null (Unity's override) checks if the native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                if (!ReferenceEquals(obj, null) && obj != null)
                {
                    return obj;
                }
                _objectCache.Remove(objectName); // Clean up stale/destroyed references
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);
            if (obj != null)
            {
                _objectCache[objectName] = obj;
            }
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

            // BOLT: Clean up controller cache to avoid memory leaks of destroyed objects
            _controllerCache.Clear();

            // BOLT: Populate prefab cache once per initialization
            EnsurePrefabCache();

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
                // BOLT: Optimized O(1) prefab lookup via dictionary
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact name not in cache
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
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
                // BOLT: Optimized component access using GetInstanceID() to avoid redundant GetComponent calls
                int instanceId = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(instanceId, out CharacterControllerBase controller) || controller == null)
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    if (controller != null) _controllerCache[instanceId] = controller;
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
