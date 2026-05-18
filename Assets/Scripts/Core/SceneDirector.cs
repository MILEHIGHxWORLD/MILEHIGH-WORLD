using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;
using System.Text.RegularExpressions;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public Transform characterSpawnRoot = null!;

        // BOLT: Consolidated O(1) caches to prevent expensive O(N) scene traversals and linear searches.
        private readonly Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private readonly Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        // 🛡️ Sentinel & BOLT: Protected managers hashset for fast and secure O(1) lookups
        private static readonly HashSet<string> _protectedManagers = new HashSet<string>
        {
            "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager", "GlobalResonanceManager", "CombatManager"
        };

        // 🛡️ Sentinel: Regex for whitelisting safe object names to prevent DoS via expensive GameObject.Find operations.
        private static readonly Regex _nameValidator = new Regex(@"^[a-zA-Z0-9_\s\(\)\-$\.\/\[\]]+$", RegexOptions.Compiled);

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void Start()
        {
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(campaignData.scenarios[0]);
            }
        }

        private void OnDestroy()
        {
            // BOLT: Explicitly clear caches to release Unity object references and prevent memory leaks.
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }

        private void InitializePrefabCache()
        {
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during any scene setup
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null) _prefabCache[prefab.name] = prefab;
            }

            if (CampaignManager.Instance != null &&
                CampaignManager.Instance.currentCampaignData != null &&
                CampaignManager.Instance.currentCampaignData.scenarios != null &&
                CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups during scene setup
            // BOLT: Pre-populate prefab cache to ensure O(1) lookups
            _prefabCache.Clear();
            if (characterPrefabs != null)
            {
                if (CampaignManager.Instance.currentCampaignData.scenarios != null &&
                    CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
                {
                    SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                }
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null)
                    {
                        _prefabCache[prefab.name] = prefab;
                        _prefabLookupCache[prefab.name] = prefab;
                    }
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabLookupCache[prefab.name] = prefab;
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }

            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            var campaignData = CampaignManager.Instance?.currentCampaignData;
            // NRT Pattern: Capture singleton property in local variable before null check
            if (CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.scenarios.Count > 0)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.scenarios != null && campaignData.scenarios.Count > 0)
            {
                SetupScene(data.scenarios[0]);
            }
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        private void OnDestroy()
        {
            // BOLT: Clear caches to release Unity object references for GC
            _objectCache?.Clear();
            _prefabLookupCache?.Clear();
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;
            UnityEngine.Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Performance optimization - Pre-populate the cache with a single O(N) pass
            // instead of calling GameObject.Find (O(N)) repeatedly in loops (O(N*M)).
            StartCoroutine(SetupSceneCoroutine(scenario));
        }

        private IEnumerator SetupSceneCoroutine(SceneScenario scenario)
        {
            Debug.Log($"⚡ Bolt: Setting up scenario asynchronously: {scenario.scenarioId}");

            if (CampaignManager.Instance.currentCampaignData == null) yield break;

            // Instantiate characters across multiple frames if needed to prevent spikes
            var characters = CampaignManager.Instance.currentCampaignData.characters;
            for (int i = 0; i < characters.Count; i++)
            {
                SpawnOrUpdateCharacter(characters[i]);
                // Yield every 2 characters to balance speed and framerate
                if (i % 2 == 1) yield return null;
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
            if (scenario == null) return;
            Debug.Log($"⚡ Bolt: Setting up scenario: {scenario.scenarioId}");
            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Removed redundant _objectCache.Clear() to allow surgical lazy-loading cache
            // to persist across multiple scenario updates for incremental performance.
            // BOLT: Initialize prefab lookup cache once per setup
            InitializePrefabCache();

            // Clear cache at start of setup to avoid stale references across scenes
            if (scenario == null || !scenario.IsValid()) return;

            Debug.Log($"Setting up scenario: {scenario.scenarioId}");

            // BOLT: Clear scene-specific caches at start of setup to avoid stale references.
            _objectCache.Clear();
            GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (go != null) _objectCache[go.name] = go;
            }

            // BOLT: Pre-cache prefabs for faster lookup.
            _prefabCache.Clear();
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null) _prefabCache[prefab.name] = prefab;
                }
            }

            // BOLT: Pre-populate object cache with existing scene objects to avoid lazy O(N) lookups
            // Note: FindObjectsOfType is legacy but highly compatible across Unity versions.
            foreach (var go in FindObjectsOfType<GameObject>())
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // BOLT: Performance Boost - Pre-populate object cache with existing scene objects
            // in a single pass to avoid lazy O(N) lookups later.
            foreach (var go in UnityEngine.Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // Instantiate characters if not already in scene
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null && CampaignManager.Instance.currentCampaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            if (CampaignManager.Instance.currentCampaignData != null)
            if (CampaignManager.Instance.currentCampaignData?.characters != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    SpawnCharacter(charProfile);
                }
            }

            // Execute interactive objects logic
            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    ApplyInteraction(interaction);
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            var data = CampaignManager.Instance.currentCampaignData;
            if (data != null)
            {
                foreach (var charProfile in data.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabCache.Clear();

            // BOLT: Pre-populate object cache with a single O(N) pass to avoid multiple GameObject.Find calls
            // Using FindObjectsOfType for maximum compatibility across Unity versions.
            GameObject[] allObjects = Object.FindObjectsOfType<GameObject>();
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
            // BOLT: Removed redundant .Clear() to allow lazy-loading caches to persist across scenario updates.
            // Unity's null check (obj != null) safely handles destroyed objects from previous scenes.

            // BOLT: Batch pre-populate object cache from existing scene objects to avoid multiple O(N) Find calls
            foreach (var go in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                if (go != null && !string.IsNullOrEmpty(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            // Instantiate characters if not already in scene
            var campaignData = CampaignManager.Instance?.currentCampaignData;
            if (campaignData?.characters != null)
            if (CampaignManager.Instance.currentCampaignData != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null)
                        SpawnOrUpdateCharacter(charProfile);
                    {
                        SpawnOrUpdateCharacter(charProfile);
                    }
                    SpawnOrUpdateCharacter(charProfile);
            // BOLT: Optimization - Pre-populate the cache with a single scene traversal.
            // This avoids multiple O(N) GameObject.Find calls later.
            _objectCache.Clear();
            var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsInactive.None, FindObjectsSortMode.None);
            foreach (var go in allObjects)
            {
                if (!_objectCache.ContainsKey(go.name))
            _objectCache.Clear();
            _controllerCache.Clear();

            // BOLT: Pre-populate object cache with existing scene objects to avoid lazy O(N) lookups
            // Note: FindObjectsOfType is legacy but highly compatible across Unity versions.
            foreach (var go in FindObjectsOfType<GameObject>())
            {
                if (go != null && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

        private GameObject? GetCachedGameObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // Check cache first; safely handle natively destroyed objects via Unity's overloaded == operator
            if (_objectCache.TryGetValue(objectName, out GameObject? obj) && obj != null)
            {
                return obj;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            if (foundObj != null)
            // Instantiate characters if not already in scene
            if (CampaignManager.Instance.currentCampaignData != null)
            // NRT Pattern: Capture singleton property in local variable
            if (CampaignManager.Instance.currentCampaignData != null)
            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                }
            }

            // BOLT: Clean up controller cache to avoid memory leaks of destroyed objects
            _controllerCache.Clear();

            // BOLT: Populate prefab cache once per initialization
            EnsurePrefabCache();
            // Clear caches at start of setup to avoid stale references across scenes
            _objectCache.Clear();
            _prefabLookupCache.Clear();

            // BOLT: Pre-populate prefab cache for O(1) lookup during spawning
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        // We use the full name as the key. Character names usually match or contain the prefab name.
                        if (!_prefabLookupCache.ContainsKey(prefab.name))
                        {
                            _prefabLookupCache[prefab.name] = prefab;
                        }
                    }
                }
            }

            var campaignData = CampaignManager.Instance.currentCampaignData;
            if (campaignData != null && campaignData.characters != null)
            {
                // Instantiate characters if not already in scene
                foreach (var charProfile in campaignData.characters)
                {
                    if (charProfile != null) SpawnOrUpdateCharacter(charProfile);
                }
            }

            if (scenario.interactiveObjects != null)
            {
                foreach (var interaction in scenario.interactiveObjects)
                {
                    if (interaction != null)
                        ApplyInteraction(interaction);
                    {
                        ApplyInteraction(interaction);
                    }
                    if (interaction != null) ApplyInteraction(interaction);
                }
            }
        }

        public void SpawnCharacter(CharacterProfile profile)
        {
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

            GameObject characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                // BOLT: Use a prefab cache to avoid O(P) linear searches through the prefab list.
                if (!_prefabCache.TryGetValue(profile.name, out GameObject prefab))
                {
                // BOLT: O(1) exact match lookup from prefab cache.
                GameObject prefab = null;
                if (!_prefabCache.TryGetValue(profile.name, out prefab))
                {
                    // Fallback to O(N) partial match if exact name not found.
                    foreach (var kvp in _prefabCache)
                    {
                        if (kvp.Key.Contains(profile.name))
                        {
                            prefab = kvp.Value;
                            break;
                        }
                    }
                }
            if (profile == null || !profile.IsValid()) return;

            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                if (prefab != null)
                {
                    // Unity Performance Pattern: Use generic Instantiate for type safety
                    characterObj = Instantiate<GameObject>(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;

                    // BOLT: Immediately cache the newly instantiated object.
                    // BOLT: Immediately cache the newly instantiated object to avoid subsequent searches
                    _objectCache[profile.name] = characterObj;
                }
                else
                {
                    // BOLT: Use O(1) prefab lookup cache
                    _prefabLookupCache.TryGetValue(profile.name, out GameObject? prefab);

                    // Fallback to partial match if exact match fails (legacy behavior)
                    if (prefab == null && characterPrefabs != null)
                    {
                        prefab = characterPrefabs.Find(p => p != null && p.name.Contains(profile.name));
                    }

                    if (prefab != null)
                    {
                        characterObj = Instantiate(prefab, characterSpawnRoot);
                        characterObj.name = profile.name;
                        _objectCache[profile.name] = characterObj;
                    }
                }
            }

            if (characterObj != null)
            {
                var controller = characterObj.GetComponent<CharacterControllerBase>();
                // Assign data to controllers
                var controller = characterObj.GetComponent<MonoBehaviour>();
                if (controller is CharacterControllerBase charController)
                // BOLT: O(1) controller lookup avoids redundant GetComponent calls
                var controller = GetCharacterController(characterObj);
                // BOLT: Optimized component access using GetInstanceID() to avoid redundant GetComponent calls
                int instanceId = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(instanceId, out CharacterControllerBase controller) || controller == null)
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    if (controller != null) _controllerCache[instanceId] = controller;
                }

                if (controller != null)
                {
                    // Create a dummy CharacterData for runtime initialization
                    CharacterData data = UnityEngine.ScriptableObject.CreateInstance<CharacterData>();
                    CharacterData data = ScriptableObject.CreateInstance<CharacterData>();
                    data.characterName = profile.name;
                    data.role = profile.role;
                    data.traits = profile.traits;
                    data.behaviorScript = profile.behaviorScript;

                    charController.Initialize(data);
                    controller.Initialize(data);
                }
                GetCharacterController(characterObj);
            }
        }

        // BOLT: Public method to return characters to pool
        public void DespawnCharacter(string characterName)
        {
            if (_objectCache.TryGetValue(characterName, out GameObject? obj) && obj != null)
            {
                obj.SetActive(false);
                _objectCache.Remove(characterName);

                if (!_characterPool.ContainsKey(characterName))
                {
                    _characterPool[characterName] = new Stack<GameObject>();
                }
                _characterPool[characterName].Push(obj);
            }
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            GameObject target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                UnityEngine.Debug.Log($"Applying {interaction.action} to {interaction.objectId}");
            GameObject? target = GetCachedGameObject(interaction.objectId);
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;
            // 🛡️ Sentinel: Prevent IDOR (Insecure Direct Object Reference) tampering with core systems.
            // Explicitly block critical managers to prevent unauthorized manipulation via external data.
            // We trim leading slashes to prevent bypasses using path-like IDs (e.g., "/CampaignManager").
            string sanitizedId = interaction.objectId.TrimStart('/');

            // BOLT: Use cached HashSet for efficient lookup and zero per-call allocation.
            // 🛡️ Sentinel: Enhanced IDOR protection - check if any part of the path targets a protected manager.
            string[] segments = sanitizedId.Split('/');
            foreach (string segment in segments)
            {
                if (_protectedManagers != null && _protectedManagers.Contains(segment))
                {
                    Debug.LogWarning($"[Security] Blocked unauthorized interaction attempt on core system: {sanitizedId} (targeted protected segment: {segment})");
                    return;
                }
            }

            GameObject? target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                if (interaction.isVector)
                {
                    target.transform.position = interaction.GetVectorValue();
                }
                else
                {
                    target.transform.localScale = UnityEngine.Vector3.one * interaction.floatValue;
                }
            }
        }

        private void OnDestroy()
        {
            // Clear caches to release references
            _objectCache.Clear();
            // Clean up to prevent memory leaks if objects are held by the dictionary
            _objectCache.Clear();
            // BOLT: Clear the cache on destroy to release references and prevent memory leaks.
            _objectCache.Clear();
            // BOLT: Explicitly clear dictionaries to release Unity object references for GC
            _objectCache?.Clear();
            _prefabLookupCache?.Clear();
            // BOLT: Explicitly clear caches to release Unity object references
            _objectCache.Clear();
            _prefabLookupCache.Clear();
            foreach (var pool in _characterPool.Values)
            {
                pool.Clear();
            }
            _characterPool.Clear();
        // BOLT: Explicitly clear caches on destroy to prevent memory leaks in the Unity Editor
        private void OnDestroy()
        {
        private void OnDestroy()
        {
            // BOLT: Clear caches to release references
            _objectCache.Clear();
            _controllerCache.Clear();
            _prefabLookupCache.Clear();
            // BOLT: Clear caches to allow Unity to garbage collect native objects
            _objectCache.Clear();
            _prefabLookupCache.Clear();
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // 🛡️ Sentinel: DoS Mitigation - Enforce length limit and character whitelist on object names.
            if (objectName.Length > 128 || !_nameValidator.IsMatch(objectName))
            {
                Debug.LogWarning($"[Security] GetCachedObject blocked potentially malicious input: {objectName}");
                return null;
            }

            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                // BOLT: Surgical negative caching using ReferenceEquals.
                if (ReferenceEquals(obj, null)) return null;

                // If it's a Unity null (native object destroyed), we should try to find it again.
                if (obj != null) return obj;
            }

            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return null;

            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
            {
                if (ReferenceEquals(prefab, null)) return null;
                if (prefab != null) return prefab;
            }

            prefab = characterPrefabs?.Find(p => p != null && (p.name == profileName || p.name.Contains(profileName)));
            _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            int id = characterObj.GetInstanceID();
            if (_controllerCache.TryGetValue(id, out CharacterControllerBase? controller))
            {
                if (ReferenceEquals(controller, null)) return null;
                if (controller != null) return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            _controllerCache[id] = controller;
            return controller;
        }
    }
}