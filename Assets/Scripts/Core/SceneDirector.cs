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

        // ⚡ Bolt: Cache for GameObject lookups to prevent expensive O(N) hierarchy traversals
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

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
        // ⚡ Bolt: Cache GameObjects to avoid expensive GameObject.Find() calls in loops
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        // ⚡ Bolt Optimization: Cache GameObject.Find results to prevent O(N*M) lookups during scene setup
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();

        private GameObject FindObjectCached(string objectName)
        {
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                return obj;
            }

            obj = GameObject.Find(objectName);
            if (obj != null)
            {
                _objectCache[objectName] = obj;
        // Cache to prevent expensive GameObject.Find calls in loops
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // Cache for GameObject references to prevent expensive GameObject.Find calls
        private Dictionary<string, GameObject> _objectCache = new Dictionary<string, GameObject>();
        // Cache to avoid O(N) GameObject.Find calls in loops
        private Dictionary<string, GameObject> objectCache = new Dictionary<string, GameObject>();

        private GameObject GetCachedGameObject(string name)
        {
            if (objectCache.TryGetValue(name, out GameObject obj))
            {
                // Check if obj is null (Unity handles destroyed objects evaluating to null)
                if (obj != null) return obj;
                objectCache.Remove(name);
            }

            GameObject foundObj = GameObject.Find(name);
            if (foundObj != null)
            {
                objectCache[name] = foundObj;
            }
            return foundObj;
        // Cache to prevent expensive GameObject.Find calls in loops
        private Dictionary<string, GameObject> _gameObjectCache = new Dictionary<string, GameObject>();

        private GameObject GetCachedGameObject(string name)
        {
            if (_gameObjectCache.TryGetValue(name, out GameObject cachedObj) && cachedObj != null)
            {
                return cachedObj;
            }

            GameObject obj = GameObject.Find(name);
            if (obj != null)
            {
                _gameObjectCache[name] = obj;
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
            objectCache.Clear();

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

        private GameObject FindCachedObject(string objName)
        {
            if (_objectCache.TryGetValue(objName, out GameObject obj) && obj != null)
        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            // Check cache first; safely handle natively destroyed objects via Unity's overloaded == operator
            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                return obj;
            }

            obj = GameObject.Find(objName);
            if (obj != null)
            {
                _objectCache[objName] = obj;
            }
            return obj;
            // Fallback to Find and cache the result
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
            GameObject characterObj = null;
            if (_objectCache.ContainsKey(profile.name))
            {
                characterObj = _objectCache[profile.name];
            }

            GameObject characterObj = FindObjectCached(profile.name);
            GameObject characterObj = FindCachedObject(profile.name);
            GameObject characterObj = GetCachedObject(profile.name);
            GameObject characterObj = GetCachedGameObject(profile.name);
            if (characterObj == null)
            {
                characterObj = GameObject.Find(profile.name);

                if (characterObj == null)
                {
                    // Try to find prefab
                    GameObject prefab = characterPrefabs?.Find(p => p.name.Contains(profile.name));
                    if (prefab != null)
                    {
                        characterObj = Instantiate(prefab, characterSpawnRoot);
                        characterObj.name = profile.name;
                    }
                }

                if (characterObj != null)
                {
                    _objectCache[profile.name] = characterObj;
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj; // ⚡ Bolt: add instantiated object to cache
                    _objectCache[profile.name] = characterObj; // Cache the newly instantiated object
                    _objectCache[profile.name] = characterObj; // Cache new instance

                    // Add newly instantiated character to cache
                    _objectCache[profile.name] = characterObj;
                    objectCache[profile.name] = characterObj; // Cache newly created object
                    // Cache the newly instantiated object
                    _gameObjectCache[profile.name] = characterObj;
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
            GameObject target = null;
            if (_objectCache.ContainsKey(interaction.objectId))
            {
                target = _objectCache[interaction.objectId];
            }

            if (target == null)
            {
                target = GameObject.Find(interaction.objectId);
                if (target != null)
                {
                    _objectCache[interaction.objectId] = target;
                }
            }

            GameObject target = FindObjectCached(interaction.objectId);
            GameObject target = FindCachedObject(interaction.objectId);
            GameObject target = GetCachedObject(interaction.objectId);
            GameObject target = GetCachedGameObject(interaction.objectId);
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
