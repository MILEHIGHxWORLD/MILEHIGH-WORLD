using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        [Header("References")]
        public List<GameObject> characterPrefabs = null!;
        public Transform characterSpawnRoot = null!;

        private static SceneDirector? _instance;
        public static SceneDirector Instance => _instance!;

        // BOLT: Consolidated triple-cache system to eliminate O(N) scene traversals (GameObject.Find),
        // O(P) linear search for prefabs, and O(N) component lookups (GetComponent).
        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            // BOLT: Warm up prefab cache for O(1) retrieval
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !_prefabCache.ContainsKey(prefab.name))
                        _prefabCache[prefab.name] = prefab;
                }
            }
        }

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }

        public void SetupScene(HorizonGameData data)
        {
            StartCoroutine(SetupSceneCoroutine(data));
        }

        private System.Collections.IEnumerator SetupSceneCoroutine(HorizonGameData data)
        {
            // BOLT: Single pass O(N) scene traversal to warm up object cache
            var allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var go in allObjects)
            {
                if (go != null && go.activeInHierarchy && !string.IsNullOrEmpty(go.name) && !_objectCache.ContainsKey(go.name))
                    _objectCache[go.name] = go;
            }

            if (data.characters != null)
            {
                foreach (var profile in data.characters)
                {
                    SpawnOrUpdateCharacter(profile);
                    yield return null;
                }
            }

            if (data.scenarios != null && data.scenarios.Count > 0)
            {
                var scenario = data.scenarios[0];
                if (scenario.interactiveObjects != null)
                {
                    foreach (var interaction in scenario.interactiveObjects)
                    {
                        ApplyInteraction(interaction);
                    }
                }
            }
        }

        private GameObject? GetCachedObject(string objectName)
        {
            if (_objectCache.TryGetValue(objectName, out GameObject? obj))
            {
                if (obj != null) return obj;
                if (ReferenceEquals(obj, null)) return null;
                _objectCache.Remove(objectName);
            }

            obj = GameObject.Find(objectName);
            _objectCache[objectName] = obj;
            return obj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab))
                return prefab;

            if (characterPrefabs != null)
            {
                prefab = characterPrefabs.Find(p => p.name.Contains(profileName));
                _prefabCache[profileName] = prefab;
            }
            return prefab;
        }

        private void SpawnOrUpdateCharacter(CharacterProfile profile)
        {
            GameObject? characterObj = GetCachedObject(profile.name);

            if (characterObj == null)
            {
                GameObject? prefab = GetPrefab(profile.name);
                if (prefab != null)
                {
                    characterObj = Instantiate(prefab, characterSpawnRoot);
                    characterObj.name = profile.name;
                    _objectCache[profile.name] = characterObj;
                }
            }

            if (characterObj != null)
            {
                int id = characterObj.GetInstanceID();
                if (!_controllerCache.TryGetValue(id, out var controller))
                {
                    controller = characterObj.GetComponent<CharacterControllerBase>();
                    _controllerCache[id] = controller;
                }

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
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            // 🛡️ Sentinel: Prevent IDOR tampering with core system objects
            string[] protectedManagers = { "CampaignManager", "SceneDirector", "CameraManager", "AlliancePowerManager" };
            if (System.Array.Exists(protectedManagers, m => m == interaction.objectId))
            {
                Debug.LogWarning($"[Security] Blocked unauthorized interaction with protected manager: {interaction.objectId}");
                return;
            }

            GameObject? target = GetCachedObject(interaction.objectId);

            if (target != null)
            {
                if (interaction.isVector)
                {
                    target.transform.position = interaction.GetVectorValue();
                }
            }
        }
    }
}
