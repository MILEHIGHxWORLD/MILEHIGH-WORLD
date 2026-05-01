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

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls.
        // We use GameObject? to support negative caching (storing 'null' for objects that don't exist).
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Unified prefab cache to replace redundant _prefabCache and _prefabLookupCache.
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) for performance.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: O(1) dictionary lookup.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Robust check for Unity's native object destruction.
                // ReferenceEquals(obj, null) checks if it's a 'true' null (negative cache).
                // obj == null checks if the Unity native object was destroyed.
                if (obj == null && !System.Object.ReferenceEquals(obj, null))
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only when necessary.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals.
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) linear search with delegate allocation happens only once per profile name.
            prefab = characterPrefabs?.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            return controller;
        }

        private void Start()
        {
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during any scene setup
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            var campaign = CampaignManager.Instance.currentCampaignData;
            if (campaign != null && campaign.scenarios.Count > 0)
            {
                SetupScene(campaign.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear dynamic caches at start of setup to avoid stale references across scenes.
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Cache campaign data locally to avoid repeated singleton property lookups in loops.
            var campaignData = CampaignManager.Instance.currentCampaignData;

            // Instantiate characters if not already in scene
            if (campaignData != null)
            {
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
                // BOLT: Use O(1) prefab cache helper.
                GameObject? prefab = GetPrefab(profile.name);

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object.
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Use O(1) controller cache to avoid redundant GetComponent.
                var controller = GetCharacterController(characterObj);
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
