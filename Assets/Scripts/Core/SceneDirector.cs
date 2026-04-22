using System.Collections;
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

        // BOLT: Prefab lookup cache for O(1) retrieval instead of O(P) linear search
        private Dictionary<string, GameObject> _prefabLookupCache = new Dictionary<string, GameObject>();

        // BOLT: Simple object pool to reduce GC pressure and instantiation overhead
        private Dictionary<string, Stack<GameObject>> _characterPool = new Dictionary<string, Stack<GameObject>>();

        private void Awake()
        {
            PopulatePrefabCache();
        }

        private void PopulatePrefabCache()
        {
            if (characterPrefabs == null) return;
            foreach (var prefab in characterPrefabs)
            {
                if (prefab != null && !_prefabLookupCache.ContainsKey(prefab.name))
                {
                    _prefabLookupCache[prefab.name] = prefab;
                }
            }
        }

        private GameObject GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (_objectCache.TryGetValue(objectName, out GameObject obj) && obj != null)
            {
                return obj;
            }

            obj = GameObject.Find(objectName);
            if (obj != null)
            {
                _objectCache[objectName] = obj;
            }
            return obj;
        }

        private void Start()
        {
            if (CampaignManager.Instance != null && CampaignManager.Instance.currentCampaignData != null)
            {
                SetupScene(CampaignManager.Instance.currentCampaignData.scenarios[0]);
            }
        }

        public void SetupScene(SceneScenario scenario)
        {
            StartCoroutine(SetupSceneCoroutine(scenario));
        }

        private IEnumerator SetupSceneCoroutine(SceneScenario scenario)
        {
            Debug.Log($"⚡ Bolt: Setting up scenario asynchronously: {scenario.scenarioId}");

            // BOLT: Removed _objectCache.Clear() to allow surgical lazy-loading persistence

            // Instantiate characters across multiple frames if needed to prevent spikes
            var characters = CampaignManager.Instance.currentCampaignData.characters;
            for (int i = 0; i < characters.Count; i++)
            {
                SpawnOrUpdateCharacter(characters[i]);
                // Yield every 2 characters to balance speed and framerate
                if (i % 2 == 1) yield return null;
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
                // BOLT: Try to get from pool first
                if (_characterPool.TryGetValue(profile.name, out var pool) && pool.Count > 0)
                {
                    characterObj = pool.Pop();
                    characterObj.SetActive(true);
                    _objectCache[profile.name] = characterObj;
                }
                else
                {
                    // BOLT: Use O(1) prefab lookup cache
                    _prefabLookupCache.TryGetValue(profile.name, out GameObject prefab);

                    // Fallback to partial match if exact match fails (legacy behavior)
                    if (prefab == null && characterPrefabs != null)
                    {
                        prefab = characterPrefabs.Find(p => p.name.Contains(profile.name));
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

        // BOLT: Public method to return characters to pool
        public void DespawnCharacter(string characterName)
        {
            if (_objectCache.TryGetValue(characterName, out GameObject obj))
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
            // BOLT: Explicitly clear caches to release Unity object references
            _objectCache.Clear();
            _prefabLookupCache.Clear();
            foreach (var pool in _characterPool.Values)
            {
                pool.Clear();
            }
            _characterPool.Clear();
        }
    }
}
