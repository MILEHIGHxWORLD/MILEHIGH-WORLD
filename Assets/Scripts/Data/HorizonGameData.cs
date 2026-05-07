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

    [System.Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = string.Empty;
        public int systemParity;
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            // SECURITY: Prevent resource exhaustion by limiting string length
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment string is too long.");
                return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class CharacterProfile
    {
        public string name = string.Empty;
        public string role = string.Empty;
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = string.Empty;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            return true;
        }
    }

    [System.Serializable]
    public class ObjectInteraction
    {
        public string objectId = string.Empty;
        public string action = string.Empty;

        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        public Vector3 GetVectorValue()
        {
            return new Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 64) return false;
            if (!string.IsNullOrEmpty(action) && action.Length > 64) return false;
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speaker = string.Empty;
        public string text = string.Empty;
        public string trigger = string.Empty;

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text) || text.Length > 1024) return false;
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId = string.Empty;
        public string description = string.Empty;
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;

            if (interactiveObjects != null)
            {
                foreach (var interaction in interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
            }

            if (dialogue != null)
            {
                foreach (var d in dialogue)
                {
                    if (d == null || !d.IsValid()) return false;
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public class HorizonGameData
    {
        public string sceneId = string.Empty;
        public Metadata metadata = new Metadata();
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (metadata == null)
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing.");
                return false;
            }

            if (!metadata.IsValid())
            {
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of character profiles.");
                return false;
            }

            // 🛡️ Sentinel: Enforce list size limit to mitigate DoS
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of scenarios.");
                return false;
            }

            foreach (var charProfile in characters)
            {
                if (charProfile == null || !charProfile.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid character profile detected.");
                    return false;
                }
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid scenario detected.");
                    return false;
                }
            }

            return true;
        }
    }
}
