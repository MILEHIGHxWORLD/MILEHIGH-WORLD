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
        public string environment;
        public int systemParity;
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Invalid voidSaturationLevel detected: {voidSaturationLevel}. Must be between 0.0 and 1.0.");
                return false;
            }

            if (environment != null && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata environment string exceeds 128 characters.");
                return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class CharacterProfile
    {
        public string name;
        public string role;
        public string[] traits;
        public string behaviorScript;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                Debug.LogError("[Security] Character name is null, empty or exceeds 128 characters.");
                return false;
            }
            if (role != null && role.Length > 128)
            {
                Debug.LogError("[Security] Character role exceeds 128 characters.");
                return false;
            }
            if (behaviorScript != null && behaviorScript.Length > 1024) // Allowing more for script, but still limited
            {
                Debug.LogError("[Security] Character behaviorScript exceeds 1024 characters.");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class ObjectInteraction
    {
        public string objectId;
        public string action;

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
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction objectId is null, empty or exceeds 128 characters.");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speaker;
        public string text;
        public string trigger;

        public bool IsValid()
        {
            if (speaker != null && speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] ScenarioId is null, empty or exceeds 128 characters.");
                return false;
            }

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
        public string sceneId;
        public Metadata metadata;
        public List<CharacterProfile> characters;
        public List<SceneScenario> scenarios;

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is null, empty or exceeds 128 characters.");
                return false;
            }

            if (metadata == null)
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing.");
                return false;
            }

            if (!metadata.IsValid())
            {
                return false;
            }

            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Too many characters defined (>50).");
                return false;
            }

            foreach (var charProfile in characters)
            {
                if (charProfile == null || !charProfile.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Too many scenarios defined (>100).");
                return false;
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }

            return true;
        }
    }
}
