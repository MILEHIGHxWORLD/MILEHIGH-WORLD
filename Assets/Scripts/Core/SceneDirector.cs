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

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: Input validation and DoS protection
            // SECURITY: Limit object name length and restrict characters to prevent DoS via GameObject.Find
            if (objectName.Length > 128)
            {
                Debug.LogWarning($"[Security] GetCachedObject: Name '{objectName.Substring(0, 10)}...' exceeds length limit.");
                return null;
            }

            // Whitelist alphanumeric, underscores, spaces, parentheses, hyphens, periods, and square brackets (common in Unity names)
            if (!Regex.IsMatch(objectName, @"^[a-zA-Z0-9_\s\(\)\-\.\[\]]+$"))
            {
                Debug.LogWarning($"[Security] GetCachedObject: Name '{objectName}' contains potentially dangerous characters.");
                return null;
            }

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // SECURITY: Negative caching - if we already searched and found nothing, return null immediately.
                // Note: ReferenceEquals(obj, null) is true for a legitimate negative cache hit.
                // Unity's == null check is true if the object was destroyed (fake null).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                if (obj != null) return obj;

                // Object was destroyed, remove from cache and fall through to find it again (or not)
                _objectCache.Remove(objectName);
            }

            // BOLT: Fallback to O(N) scene traversal only if not cached.
            obj = GameObject.Find(objectName);

            // SECURITY: Robust negative caching - store null explicitly if not found to prevent future traversals.
            _objectCache[objectName] = obj;

            return obj;
        }

        private void Start()
        {
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                if (CampaignManager.Instance.currentCampaignData.scenarios != null &&
                    CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
                {
                    SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                }
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // Clear cache at start of setup to avoid stale references across scenes
            _objectCache.Clear();

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData?.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                }
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    if (interaction != null) ApplyInteraction(interaction);
                }
            }
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // Try to find prefab if not in scene
                GameObject prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
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

        private void OnDestroy()
        {
            // Clean up to prevent memory leaks if objects are held by the dictionary
            _objectCache.Clear();
        }
    }
}
