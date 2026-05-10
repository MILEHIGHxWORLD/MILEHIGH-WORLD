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
        public string environment = "";
        public int systemParity;
        public float voidSaturationLevel;

        public bool IsValid()
        {
        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints and resource limits.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range.
            // Consolidated multiple redundant checks into one.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Limit environment string length to prevent DoS via memory exhaustion
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: environment is null or exceeds 128 characters.");
                return false;
            }
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
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

        /// <summary>
        /// 🛡️ Sentinel: Security validation for individual character data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Implement resource exhaustion protection (DoS prevention)
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 128) return false;
            if (behaviorScript != null && behaviorScript.Length > 128) return false;
        public string name = string.Empty;
        public string role = string.Empty;
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = string.Empty;
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;
        public string name = "";
        public string role = "";
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = "";
        public float health;
        public float resonance;
        public float integrity;
        public float vanguardMultiplier;

        /// <summary>
        /// 🛡️ Sentinel: Validates character profile data for security and integrity.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            // SECURITY: behaviorScript limit increased to 2048 to support complex AI behaviors while maintaining DoS protection.
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            // BOLT: Increased behaviorScript length limit to support complex AI while maintaining safety threshold
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 1024) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;
            // SECURITY: Enforce string length limits to prevent resource exhaustion (DoS)
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                Debug.LogError("[Security] Character validation failed: Name is missing or exceeds 128 characters.");
                return false;
            }

            if (role != null && role.Length > 128)
            {
                Debug.LogError("[Security] Character validation failed: Role exceeds 128 characters.");
                return false;
            }

            // SECURITY: behaviorScript can be long (it's code) but we still enforce a maximum for DoS prevention.
            if (behaviorScript != null && behaviorScript.Length > 2048)
            {
                Debug.LogError("[Security] Character validation failed: behaviorScript too long (max 2048).");
                return false;
            }

            // SECURITY: Limit traits collection size and individual trait lengths
            if (traits != null)
            {
                if (traits.Length > 20)
                {
                    Debug.LogError("[Security] Character validation failed: traits collection exceeds 20 items.");
                    return false;
                }
                foreach (var trait in traits)
                {
                    if (string.IsNullOrEmpty(trait) || trait.Length > 128)
                    {
                        Debug.LogError("[Security] Character validation failed: Individual trait is invalid or exceeds 128 characters.");
                        return false;
                    }
                }
            }

            return true;
        }
    }

    [System.Serializable]
    public class ObjectInteraction
    {
        public string objectId = null!;
        public string action = null!;
        public string objectId = string.Empty;
        public string action = string.Empty;

        public string objectId = "";
        public string action = "";
        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        public Vector3 GetVectorValue() => new Vector3(x, y, z);

        /// <summary>
        /// 🛡️ Sentinel: Validates object interaction data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction validation failed: objectId is missing or too long (max 128).");
                return false;
            }
            if (string.IsNullOrEmpty(action) || action.Length > 128)
            {
                Debug.LogError("[Security] Interaction validation failed: action is missing or too long (max 128).");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        public string speaker = string.Empty;
        public string text = string.Empty;
        public string trigger = string.Empty;
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";

        /// <summary>
        /// 🛡️ Sentinel: Validates dialogue data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 64)
            {
                Debug.LogError("[Security] Dialogue validation failed: speaker is missing or too long (max 64).");
                return false;
            }
            if (string.IsNullOrEmpty(text) || text.Length > 1024)
            {
                Debug.LogError("[Security] Dialogue validation failed: text is missing or too long (max 1024).");
                return false;
            }
            if (trigger != null && trigger.Length > 128)
            {
                Debug.LogError("[Security] Dialogue validation failed: trigger exceeds 128 characters.");
                return false;
            }
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

        /// <summary>
        /// 🛡️ Sentinel: Security validation for individual scenarios.
        public string scenarioId = string.Empty;
        public string description = string.Empty;
        public string scenarioId = null!;
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = null!;
        public List<Dialogue> dialogue = null!;
        public string scenarioId = "";
        public string description = "";
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();

        /// <summary>
        /// 🛡️ Sentinel: Validates scene scenario data and enforces resource limits.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null && interactiveObjects.Count > 50) return false;
            if (dialogue != null && dialogue.Count > 50) return false;
            if (interactiveObjects != null)
            {
                foreach (var i in interactiveObjects) if (i == null || !i.IsValid()) return false;
            }
            if (dialogue != null)
            {
                foreach (var d in dialogue) if (d == null || !d.IsValid()) return false;
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] Scenario validation failed: scenarioId is missing or too long (max 128).");
                return false;
            }

            if (description != null && description.Length > 1024)
            {
                Debug.LogError("[Security] Scenario validation failed: description exceeds 1024 characters.");
                return false;
            }

            // SECURITY: Limit collection sizes to prevent resource exhaustion
            if (interactiveObjects == null || interactiveObjects.Count > 50)
            {
                Debug.LogError("[Security] Scenario validation failed: interactiveObjects collection is null or too large (max 50).");
                return false;
            }
            foreach (var obj in interactiveObjects)
            {
                if (obj == null || !obj.IsValid()) return false;
            }

            if (dialogue == null || dialogue.Count > 100)
            {
                Debug.LogError("[Security] Scenario validation failed: dialogue collection is null or too large (max 100).");
                return false;
            }
            foreach (var d in dialogue)
            {
                if (d == null || !d.IsValid()) return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class HorizonGameData
    {
        public string sceneId = string.Empty;
        public string sceneId = "";
        public Metadata metadata = new Metadata();
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0 || characters.Count > 50) return false;
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100) return false;
            foreach (var c in characters) if (c == null || !c.IsValid()) return false;
            foreach (var s in scenarios) if (s == null || !s.IsValid()) return false;
        /// <summary>
        /// 🛡️ Sentinel: Performs hierarchical integrity and security validation on the entire campaign dataset.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid or too long sceneId.");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid character count (must be between 1 and 50).");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }
            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            // 🛡️ Sentinel: Enforce list size limit to mitigate DoS
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid scenario count (must be between 1 and 100).");
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
