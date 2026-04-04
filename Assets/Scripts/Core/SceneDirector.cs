using System;
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

        // BOLT: Consolidated cache for GameObjects to prevent expensive O(N) GameObject.Find calls.
        // Performance Insight: Dictionary.TryGetValue is O(1).
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            // Performance Insight: We use negative caching (storing 'null' for missing objects)
            // to avoid repeated O(N) scene traversals for objects that don't exist.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity overrides the == operator for GameObjects. If 'obj' was destroyed, it returns null.
                // We must check if the reference is actually valid.
                if (ReferenceEquals(obj, null))
                {
                    // This is a legitimate negative cache hit (real null).
                    return null;
                }

                if (obj == null)
                {
                    // Object was destroyed (fake null). Remove from cache and continue.
                    _objectCache.Remove(objectName);
                }
                else
                {
                    // Valid object found.
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);

            // Performance Insight: Even if obj is null, we store it to enable negative caching.
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

            // Performance Insight: Clear cache at start of setup to prevent stale references or
            // negative cache leakage between scenarios if objects were created/destroyed.
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

                    // BOLT: Immediately cache the newly instantiated object to prevent redundant scene searches
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
