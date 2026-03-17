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

        // ⚡ Bolt: Cache game objects to prevent expensive O(N) GameObject.Find() calls on every interaction.
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

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

        // ⚡ Bolt: Helper to retrieve or cache GameObjects, avoiding repeated hierarchy traversal.
        private GameObject FindCachedObject(string objectName)
        {
            if (_objectCache.TryGetValue(objectName, out GameObject cachedObj) && cachedObj != null)
            {
                return cachedObj;
            }

            GameObject foundObj = GameObject.Find(objectName);
            if (foundObj != null)
            {
                _objectCache[objectName] = foundObj;
            }
            return foundObj;
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject characterObj = FindCachedObject(profile.name);
            if (characterObj == null)
            {
                // Try to find prefab
                GameObject prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // ⚡ Bolt: Cache newly spawned characters immediately
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
            GameObject target = FindCachedObject(interaction.objectId);
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
