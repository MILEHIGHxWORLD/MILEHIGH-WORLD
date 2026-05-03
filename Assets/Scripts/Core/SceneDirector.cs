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
        // We use nullable types to support negative caching (storing a true null for missing objects)
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) linear searches and delegate allocations
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching using ReferenceEquals.
                // We distinguish between a 'true' null (cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj == null)
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            GameObject? foundObj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = foundObj;
            return foundObj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) linear search as fallback if not pre-cached
            GameObject? foundPrefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabCache[profileName] = foundPrefab;

            return foundPrefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            CharacterControllerBase? foundController = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[objId] = foundController;

            return foundController;
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

            // Use NRT flow analysis pattern: capture singleton property in local variable
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear dynamic caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _controllerCache.Clear();

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
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
                // BOLT: Use O(1) prefab cache helper
                GameObject? prefab = GetPrefab(profile.name);

                if (prefab != null)
                {
                    // Use generic Instantiate<T> for better type safety and flow analysis
                    characterObj = Instantiate<GameObject>(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                // BOLT: Use O(1) controller cache to avoid redundant GetComponent
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
