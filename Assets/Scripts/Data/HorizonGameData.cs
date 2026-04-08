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
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment string exceeds 128 characters.");
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
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (role != null && role.Length > 128) return false;
            if (behaviorScript != null && behaviorScript.Length > 128) return false;
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
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            if (action != null && action.Length > 128) return false;
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
            // Text can be longer, maybe 2048
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
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (description != null && description.Length > 512) return false;

            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 100) return false;
                foreach (var interaction in interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
            }

            if (dialogue != null)
            {
                if (dialogue.Count > 100) return false;
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
                Debug.LogError("[Security] Game data validation failed: Invalid sceneId.");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Character count out of bounds (1-50).");
                return false;
            }

            foreach (var character in characters)
            {
                if (character == null || !character.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid or null character profile.");
                    return false;
                }
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Scenario count out of bounds (1-100).");
                return false;
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid or null scenario.");
                    return false;
                }
            }

            return true;
        }
    }
}
