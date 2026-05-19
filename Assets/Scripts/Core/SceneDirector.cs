using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Milehigh.Data;
using Milehigh.Characters;

namespace Milehigh.Core
{
    public class SceneDirector : MonoBehaviour
    {
        [Header("Setup")]
        public List<GameObject> characterPrefabs = new List<GameObject>();
        public Transform characterSpawnRoot = null!;

        private Dictionary<string, GameObject?> _objectCache = new Dictionary<string, GameObject?>();
        private Dictionary<string, GameObject?> _prefabCache = new Dictionary<string, GameObject?>();
        private Dictionary<int, CharacterControllerBase?> _controllerCache = new Dictionary<int, CharacterControllerBase?>();

        private static readonly Regex SafeNameRegex = new Regex(@"^[a-zA-Z0-9_\s\(\)\-\.\[\]]+$", RegexOptions.Compiled);

        private void Awake()
        {
            InitializePrefabCache();
        }

        private void InitializePrefabCache()
        {
            if (characterPrefabs != null)
            {
                foreach (var prefab in characterPrefabs)
                {
                    if (prefab != null && !string.IsNullOrEmpty(prefab.name))
                    {
                        _prefabCache[prefab.name] = prefab;
                    }
                }
            }
        }

        private void Start()
        {
            var data = CampaignManager.Instance.currentCampaignData;
            if (data != null && data.scenarios != null && data.scenarios.Count > 0)
            {
                SetupScene(data.scenarios[0]);
            }
        }

        private void OnDestroy()
        {
            _objectCache.Clear();
            _prefabCache.Clear();
            _controllerCache.Clear();
        }

        public void SetupScene(SceneScenario scenario)
        {
            if (scenario == null) return;

            _objectCache.Clear();
            foreach (var go in Object.FindObjectsOfType<GameObject>())
            {
                if (go != null && !string.IsNullOrEmpty(go.name))
                {
                    _objectCache[go.name] = go;
                }
            }

            if (CampaignManager.Instance.currentCampaignData != null)
            {
                foreach (var charProfile in CampaignManager.Instance.currentCampaignData.characters)
                {
                    SpawnOrUpdateCharacter(charProfile);
                }
            }

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
            if (profile == null || string.IsNullOrEmpty(profile.name)) return;

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

        private GameObject? GetCachedObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName)) return null;

            if (objectName.Length > 128 || !SafeNameRegex.IsMatch(objectName))
            {
                return null;
            }

            if (_objectCache.TryGetValue(objectName, out GameObject? obj) && obj != null)
            {
                return obj;
            }

            GameObject? foundObj = GameObject.Find(objectName);
            _objectCache[objectName] = foundObj;
            return foundObj;
        }

        private GameObject? GetPrefab(string profileName)
        {
            if (_prefabCache.TryGetValue(profileName, out GameObject? prefab)) return prefab;

            prefab = characterPrefabs?.Find(p => p != null && p.name.Contains(profileName));
            if (prefab != null) _prefabCache[profileName] = prefab;
            return prefab;
        }

        private CharacterControllerBase? GetCharacterController(GameObject characterObj)
        {
            if (characterObj == null) return null;
            int objId = characterObj.GetInstanceID();

            if (_controllerCache.TryGetValue(objId, out CharacterControllerBase? controller) && controller != null)
            {
                return controller;
            }

            controller = characterObj.GetComponent<CharacterControllerBase>();
            if (controller != null) _controllerCache[objId] = controller;
            return controller;
        }

        private void ApplyInteraction(ObjectInteraction interaction)
        {
            // 🛡️ Sentinel: Prevent Insecure Direct Object Reference (IDOR) by sanitizing untrusted external object IDs
            if (interaction.objectId == "CampaignManager" || interaction.objectId == "SceneDirector" ||
                interaction.objectId == "CameraManager" || interaction.objectId == "AlliancePowerManager")
            {
                Debug.LogError($"[Security] Blocked unauthorized interaction attempt to system object: {interaction.objectId}");
                return;
            }

            GameObject target = GetCachedObject(interaction.objectId);
            if (interaction == null || string.IsNullOrEmpty(interaction.objectId)) return;

            GameObject? target = GetCachedObject(interaction.objectId);
            if (target != null)
            {
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
