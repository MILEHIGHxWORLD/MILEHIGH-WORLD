using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;
using System.Text.RegularExpressions;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs; // Assign in Inspector
        public Transform characterSpawnRoot;

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        // 🛡️ Sentinel: Regex for white-listing safe characters in object names to prevent DoS via GameObject.Find
        private static readonly Regex _safeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\.\-\[\]]+$");
        private const int MAX_NAME_LENGTH = 128;

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: Sanitize input to mitigate DoS risks and ensure data integrity
            if (objectName.Length > MAX_NAME_LENGTH || !_safeNameRegex.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject: Rejected potentially unsafe object name '{objectName}'");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                return obj;
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
                // Try to find prefab if not in scene
                GameObject prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
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
