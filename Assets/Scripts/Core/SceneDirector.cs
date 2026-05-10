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

        // BOLT: Consolidated triple-cache system for maximum performance
        // 1. _objectCache: Memoizes GameObject.Find lookups (O(N) -> O(1)) with negative caching.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // 2. _prefabCache: Memoizes character prefab lookups (O(M) -> O(1)).
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // 3. _controllerCache: Memoizes GetComponent<CharacterControllerBase> calls (O(1) lookup via InstanceID).
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Robust negative caching check.
                // Unity's '==' operator returns true for destroyed objects (fake nulls).
                // ReferenceEquals(obj, null) is only true for "real" C# nulls (negative cache hits).
                if (obj != null) return obj;
                if (System.Object.ReferenceEquals(obj, null)) return null;
                // If it's a "fake null", we fall through to re-find it in the scene.
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached or destroyed.
            obj = GameObject.Find(objectName);
            // BOLT: Cache the result even if null to prevent repeated failed lookups (negative caching).
            _objectCache[objectName] = obj;
            return obj;
        }

        private CharacterControllerBase GetCachedController(GameObject characterObj)
        {
            if (characterObj == null) return null;

            int instanceID = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(instanceID, out CharacterControllerBase controller) && controller != null)
            {
                return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null)
            {
                _controllerCache[instanceID] = controller;
            }
            return controller;
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

            // BOLT: Clear scene-specific caches to avoid stale references across scenarios/scenes.
            // Prefab cache is persisted as it remains valid across the session.
            _objectCache.Clear();
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
                // BOLT: O(1) prefab lookup via cache instead of O(M) list search.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab) || prefab == null)
                {
                    prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null) _prefabCache[profile.name] = prefab;
                }

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object to resolve negative cache hits.
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Use GetCachedController to avoid expensive GetComponent calls.
                var controller = GetCachedController(characterObj);
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
