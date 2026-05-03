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

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
                if (obj == null && !ReferenceEquals(obj, null))
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

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
            if (controller != null) _controllerCache[objId] = controller;
            return controller;
        }

        private void Start()
        {
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            _objectCache.Clear();
            _controllerCache.Clear();

            if (CampaignManager.Instance?.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
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
