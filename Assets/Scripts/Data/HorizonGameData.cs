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

        /// <summary>
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                Debug.LogError($"[Security] Metadata validation failed: environment name is invalid or too long.");
                return false;
            }

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
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

        /// <summary>
        /// 🛡️ Sentinel: Validates character profile data for security and integrity.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 64) return false;
            if (behaviorScript != null && behaviorScript.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;

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

        public Vector3 GetVectorValue()
        {
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// 🛡️ Sentinel: Validates interaction data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            if (string.IsNullOrEmpty(action) || action.Length > 64) return false;
            return true;
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker;
        public string text;
        public string trigger;

        /// <summary>
        /// 🛡️ Sentinel: Validates dialogue data for security and length limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text) || text.Length > 1024) return false;
            if (trigger != null && trigger.Length > 128) return false;
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

        /// <summary>
        /// 🛡️ Sentinel: Validates scenario data and collection sizes.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;

            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 50) return false;
                foreach (var obj in interactiveObjects)
                {
                    if (obj == null || !obj.IsValid()) return false;
                }
            }

            if (dialogue != null)
            {
                if (dialogue.Count > 50) return false;
                foreach (var d in dialogue)
                {
                    if (d == null || !d.IsValid()) return false;
                }
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

        /// <summary>
        /// 🛡️ Sentinel: Performs hierarchical integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character profile count.");
                return false;
            }

            foreach (var charProfile in characters)
            {
                if (charProfile == null || !charProfile.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid character profile.");
                    return false;
                }
            }

            if (scenarios == null || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid scenario count.");
                return false;
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid())
                {
                    Debug.LogError("[Security] Game data validation failed: Invalid scene scenario.");
                    return false;
                }
            }

            return true;
        }
    }
}
