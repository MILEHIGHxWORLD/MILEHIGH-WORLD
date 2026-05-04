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
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
1        // BOLT: Prefab cache to avoid O(P) list searches and delegate allocations
        private Dictionary<string, GameObject> _prefabCache = new Dictionary<string, GameObject>();
        // BOLT: Component cache to avoid redundant GetComponent calls. Key is InstanceID (int) to avoid string allocations.
        private Dictionary<int, CharacterControllerBase> _controllerCache = new Dictionary<int, CharacterControllerBase>();

        // BOLT: Prefab lookup cache to avoid O(P) linear searches in characterPrefabs list
        private Dictionary<string, GameObject?> _prefabLookupCache = new Dictionary<string, GameObject?>();

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // BOLT: Perform an O(1) dictionary lookup first.
1            // Note: Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj))
            {
                // BOLT: Check if the cached reference is a destroyed Unity object (fake null)
                // vs a legitimate negative cache entry (real null).
                if (obj == null && !ReferenceEquals(obj, null))
                {
                    _objectCache.Remove(objectName);
                }
                else
                {
                    return obj;
                }
            // Unity overrides the == operator to check if the underlying native C++ object is destroyed.
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching. We use ReferenceEquals to distinguish between
                // a 'true' null (explicitly cached as missing) and a 'Unity' null (destroyed object).
                if (System.Object.ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again
                // or just return the Unity null which behaves like null.
                if (obj == null) return null;

                return obj;
            }

            // BOLT: Fallback to O(N) scene traversal only if not in cache or if the cached object was destroyed.
            obj = GameObject.Find(objectName);
            // BOLT: Cache result even if null (negative caching) to avoid future O(N) traversals
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject prefab)) return prefab;

            // BOLT: O(P) search and delegate allocation happens only once per profile name
            prefab = characterPrefabs?.Find(p => p.name.Contains(profileName));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase GetCharacterController(GameObject characterObj)
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
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null)
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
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Use O(1) prefab cache helper
                GameObject prefab = GetPrefab(profile.name);
                // BOLT: Optimized prefab lookup using dictionary cache (O(1))
                // instead of characterPrefabs.Find (O(P))
                GameObject? prefab = null;

                // Try exact match first
                if (!_prefabLookupCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to partial match if exact match fails (legacy support)
                    foreach (var kvp in _prefabLookupCache)
                    {
                        if (kvp.Key != null && kvp.Key.Contains(profile.name))
                        {
                            prefab = kvp.Value;
                            break;
                        }
                    }
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
            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR)
            // Block external data from manipulating core architectural managers.
            string[] protectedObjects = { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager" };
            foreach (string protectedObj in protectedObjects)
            {
                if (interaction.objectId == protectedObj)
                {
                    Debug.LogError($"[Security] Blocked unauthorized interaction attempt on protected object: {interaction.objectId}");
                    return;
                }
            }

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
