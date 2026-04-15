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
        private readonly Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // BOLT: Cache for prefabs to avoid repeated string matching in the list
        private readonly Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            // Note: Use ReferenceEquals for the cache check to distinguish between a truly missing
            // entry and one that was cached as null (negative caching).
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // Unity's == operator returns true for destroyed objects.
                if (obj != null) return obj;

                // If it's null but the key exists, it means we already searched and found nothing (negative cache),
                // OR the object was destroyed since being cached.
                if (System.Object.ReferenceEquals(obj, null)) return null;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache.
            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj; // Cache the result, even if null (negative caching)
            return obj;
        }

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
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

            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();

            // BOLT: Pre-populate object cache with a single O(N) pass to avoid multiple GameObject.Find calls
            var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.None, FindObjectsSortMode.None);
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

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
                // BOLT: Use prefab cache to avoid repeated string matching in the list
                GameObject prefab;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profile.name));
                    _prefabCache[profile.name] = prefab;
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
