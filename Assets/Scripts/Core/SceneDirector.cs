using UnityEngine;
using UnityEngine.SceneManagement;
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
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();

        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();

        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // Note: Unity overrides == to treat destroyed native objects as null.
                // First handle both managed-null and Unity-null in one check...
                if (obj == null)
                {
                    // ...then distinguish explicit negative cache (true managed null)
                    // from destroyed Unity object (managed ref not actually null).
                    if (System.Object.ReferenceEquals(obj, null)) return null;

                    // Destroyed object: remove stale cache entry so we can find/instantiate again.
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            }

            // BOLT: Fallback to O(N) scene traversal only if not found in cache.
            // This is still needed for objects that might have been activated after PreWarmCache.
            obj = GameObject.Find(objectName);

            // BOLT: Cache result (including null to avoid repeated O(N) failures)
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            // BOLT: O(P) search happens only once per profile name
            prefab = characterPrefabs?.Find(p => p != null && p.name == profileName);
            if (prefab == null)
            {
                prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            }

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
                        _prefabCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        // BOLT: Optimization - One-time O(N) scene traversal to populate the dictionary.
        // This replaces multiple O(M*N) GameObject.Find() calls with a single O(N) pass,
        // making subsequent lookups O(1).
        private void PreWarmCache()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.isLoaded)
                {
                    GameObject[] roots = scene.GetRootGameObjects();
                    foreach (GameObject root in roots)
                    {
                        if (root != null) CacheHierarchyRecursive(root.transform);
                    }
                }
            }
        }

        private void CacheHierarchyRecursive(Transform t)
        {
            // Match GameObject.Find behavior: only cache active objects
            if (t.gameObject.activeInHierarchy && !_objectCache.ContainsKey(t.name))
            {
                _objectCache[t.name] = t.gameObject;
            }

            for (int i = 0; i < t.childCount; i++)
            {
                CacheHierarchyRecursive(t.GetChild(i));
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear dynamic caches at start of setup to avoid stale references
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Performance Boost - Populate cache in a single pass before lookups
            PreWarmCache();

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance?.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
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
            if (profile == null) return;
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);

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
            if (interaction == null) return;
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
