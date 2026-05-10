using System;
using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState
    {
        Day,
        Night,
        Dynamic
    }

    [Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment;
        public int systemParity;
        public float voidSaturationLevel;

        private const int MAX_STRING_LENGTH = 128;

        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            if (environment != null && environment.Length > MAX_STRING_LENGTH)
            {
                Debug.LogError($"[Security] Metadata validation failed: environment string exceeds {MAX_STRING_LENGTH} characters.");
                return false;
            }

            return true;
        }
    }

    [Serializable]
    public class CharacterProfile
    {
        public string name;
        public string role;
        public string[] traits;
        public string behaviorScript;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > MAX_STRING_LENGTH) return false;
            if (role != null && role.Length > MAX_STRING_LENGTH) return false;
            if (behaviorScript != null && behaviorScript.Length > MAX_STRING_LENGTH) return false;
            return true;
        }
    }

    [Serializable]
    public class ObjectInteraction
    {
        public string objectId;
        public string action;

        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > MAX_STRING_LENGTH) return false;
            if (action != null && action.Length > MAX_STRING_LENGTH) return false;
            return true;
        }

        public Vector3 GetVectorValue()
        {
            return new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker;
        public string text;
        public string trigger;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (speaker != null && speaker.Length > MAX_STRING_LENGTH) return false;
            if (trigger != null && trigger.Length > MAX_STRING_LENGTH) return false;
            // Dialogue text can be longer, but we might want to cap it too for DoS
            if (text != null && text.Length > 2048) return false;
            return true;
        }
    }

    [Serializable]
    public class SceneScenario
    {
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        private const int MAX_STRING_LENGTH = 128;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > MAX_STRING_LENGTH) return false;

            if (interactiveObjects != null)
            {
                foreach (var obj in interactiveObjects) if (!obj.IsValid()) return false;
            }

            if (dialogue != null)
            {
                foreach (var d in dialogue) if (!d.IsValid()) return false;
            }

            return true;
        }
    }

    [Serializable]
    public class HorizonGameData
    {
        public string sceneId;
        public Metadata metadata;
        public List<CharacterProfile> characters;
        public List<SceneScenario> scenarios;

        private const int MAX_STRING_LENGTH = 128;
        private const int MAX_CHARACTERS = 50;
        private const int MAX_SCENARIOS = 100;

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > MAX_STRING_LENGTH)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid sceneId.");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > MAX_CHARACTERS)
            {
                Debug.LogError($"[Security] Game data validation failed: Character count {characters?.Count} out of bounds.");
                return false;
            }

            foreach (var character in characters)
            {
                if (!character.IsValid())
                {
                    Debug.LogError($"[Security] Game data validation failed: Invalid character profile {character.name}.");
                    return false;
                }
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > MAX_SCENARIOS)
            {
                Debug.LogError($"[Security] Game data validation failed: Scenario count {scenarios?.Count} out of bounds.");
                return false;
            }

            foreach (var scenario in scenarios)
            {
                if (!scenario.IsValid())
                {
                    Debug.LogError($"[Security] Game data validation failed: Invalid scenario {scenario.scenarioId}.");
                    return false;
                }
            }

            return true;
        }
    }
}
