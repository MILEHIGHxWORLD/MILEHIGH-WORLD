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

        // BOLT: Consolidated caches to prevent expensive O(N) scene traversals and O(P) linear searches.
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: O(1) dictionary lookup.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Check if the cached reference is a destroyed Unity object (fake null)
                // vs a legitimate negative cache entry (real null).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                if (obj == null) // Unity object was destroyed
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result (including negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) linear search only if not pre-cached.
            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out var controller)) return controller;

            controller = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[objId] = controller;
            return controller;
        }

        private void Start()
        {
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during any scene setup
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

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear dynamic caches at start of setup
            _objectCache.Clear();
            _controllerCache.Clear();

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

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
                GameObject? prefab = GetPrefab(profile.name);

                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                var controller = GetCharacterController(characterObj);
                if (controller != null)
                {
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
