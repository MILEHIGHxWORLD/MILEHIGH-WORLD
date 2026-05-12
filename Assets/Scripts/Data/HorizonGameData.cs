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
        [UnityEngine.Tooltip("The lighting state for the scene.")]
        public LightingState lighting;
        [UnityEngine.Tooltip("The name of the environment.")]
        public string environment = null!;
        [UnityEngine.Tooltip("System parity level for synchronization.")]
        public int systemParity;
        [UnityEngine.Tooltip("The saturation level of the Void effect (0 to 1).")]
        [UnityEngine.Range(0.0f, 1.0f)]
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized metadata meets business constraints and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure environment string is present and within safe length limits (DoS mitigation)
            if (string.IsNullOrEmpty(environment))
            {
                Debug.LogError("[Security] Metadata validation failed: environment is missing.");
                return false;
            }
            if (environment.Length > 128)
            {
                Debug.LogError($"[Security] Metadata validation failed: Environment name length {environment.Length} exceeds 128 characters.");
                return false;
            }

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range to prevent out-of-bounds visual artifacts or logic errors.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            if (string.IsNullOrEmpty(environment) || environment.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] Metadata validation failed: environment is missing or too long.");
                return false;
            }

            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
        public bool IsValid()
        {
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }

            if (string.IsNullOrEmpty(environment))
            {
                Debug.LogError("[Security] Metadata validation failed: environment is missing.");
                return false;
            }

            if (environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: Environment name exceeds 128 characters.");
                return false;
            }

            return true;
        }
    }

    [System.Serializable]
    public class CharacterProfile
    {
        [UnityEngine.Tooltip("The name of the character.")]
        public string name = null!;
        [UnityEngine.Tooltip("The role or class of the character.")]
        public string role = null!;
        [UnityEngine.Tooltip("A list of traits defining the character's abilities or attributes.")]
        public string[] traits = null!;
        [UnityEngine.Tooltip("The script defining the character's AI behavior.")]
        [UnityEngine.TextArea(3, 10)]
        public string behaviorScript = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("[Security] CharacterProfile validation failed: Name is missing.");
                return false;
            }
            if (name.Length > 64)
            {
                Debug.LogError($"[Security] CharacterProfile validation failed: Name '{name.Substring(0, 10)}...' exceeds 64 characters.");
                return false;
            }
            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            // BOLT: Increased behaviorScript length to 2048 to support complex AI behaviors while maintaining safety.
            if (string.IsNullOrEmpty(name) || name.Length > 64)
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: name is invalid.");
                return false;
            }
            if (!string.IsNullOrEmpty(role) && role.Length > 64)
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: role is too long.");
                return false;
            }
            if (traits != null && traits.Length > 10)
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: too many traits.");
                return false;
            }
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048)
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: behaviorScript is too long.");
                return false;
            }
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
        [UnityEngine.Tooltip("The ID of the object to interact with.")]
        public string objectId = null!;
        [UnityEngine.Tooltip("The action to perform on the object.")]
        public string action = null!;

        [UnityEngine.Tooltip("Whether the interaction uses a vector or a float value.")]
        public bool isVector;
        [UnityEngine.Tooltip("The float value for the interaction.")]
        public float floatValue;
        [UnityEngine.Tooltip("The X component of the vector value.")]
        public float x;
        [UnityEngine.Tooltip("The Y component of the vector value.")]
        public float y;
        [UnityEngine.Tooltip("The Z component of the vector value.")]
        public float z;

        public Vector3 GetVectorValue()
        {
            return new Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId))
            {
                Debug.LogError("[Security] ObjectInteraction validation failed: objectId is missing.");
                return false;
            }
            if (objectId.Length > 128) return false;
            if (string.IsNullOrEmpty(action))
            {
                Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': action is missing.");
                return false;
            }
            if (action.Length > 128) return false;
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] ObjectInteraction validation failed: objectId is invalid.");
                return false;
            }
            if (string.IsNullOrEmpty(action) || action.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] ObjectInteraction validation failed: action is invalid.");
                return false;
            }
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 64) return false;
            if (string.IsNullOrEmpty(action) || action.Length > 64) return false;
            return true;
        }
    }

    [System.Serializable]
    public class Dialogue
    {
        [UnityEngine.Tooltip("The name of the speaker.")]
        public string speaker = null!;
        [UnityEngine.Tooltip("The dialogue text content.")]
        [UnityEngine.TextArea(2, 5)]
        public string text = null!;
        [UnityEngine.Tooltip("The event trigger for this dialogue.")]
        public string trigger = null!;

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("[Security] Dialogue validation failed: text is missing.");
                return false;
            }
            if (text.Length > 1024) return false;
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            if (string.IsNullOrEmpty(action) || action.Length > 128) return false;
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: speaker name is too long.");
                return false;
            }
            if (string.IsNullOrEmpty(text) || text.Length > 1024)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: text is invalid or too long.");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string name = null!;
        [UnityEngine.Tooltip("Unique ID for the scenario.")]
        public string scenarioId = null!;
        [UnityEngine.Tooltip("Description of the scenario.")]
        [UnityEngine.TextArea(2, 5)]
        public string description = null!;
        [UnityEngine.Tooltip("List of interactive objects in this scenario.")]
        public List<ObjectInteraction> interactiveObjects = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId))
            {
                Debug.LogError("[Security] SceneScenario validation failed: scenarioId is missing.");
                return false;
            }
            if (scenarioId.Length > 128) return false;
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (interactiveObjects == null) return false;
            foreach (var interaction in interactiveObjects)
        [UnityEngine.Tooltip("List of dialogue lines in this scenario.")]
        public List<Dialogue> dialogue = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                if (interactiveObjects.Count > 50)
                {
                    Debug.LogError($"[Security] SceneScenario '{scenarioId}' validation failed: too many interactive objects ({interactiveObjects.Count}).");
                    return false;
                }
                foreach (var interaction in interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: scenarioId is invalid.");
                return false;
            }
            if (interactiveObjects != null && interactiveObjects.Count > 50)
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: too many interactive objects.");
                return false;
            }
            if (dialogue != null && dialogue.Count > 50)
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: too many dialogue lines.");
                return false;
            }

            if (interactiveObjects != null)
            {
                if (dialogue.Count > 50)
                {
                    Debug.LogError($"[Security] SceneScenario '{scenarioId}' validation failed: too many dialogue lines ({dialogue.Count}).");
                    return false;
                }
                foreach (var d in dialogue)
                {
                    if (d == null || !d.IsValid()) return false;
                }
                if (interaction == null || !interaction.IsValid()) return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class HorizonGameData
    {
        [UnityEngine.Tooltip("Unique ID for the scene.")]
        public string sceneId = null!;
        [UnityEngine.Tooltip("Metadata for the scene.")]
        public Metadata metadata = null!;
        [UnityEngine.Tooltip("List of character profiles in this campaign.")]
        public List<CharacterProfile> characters = null!;
        [UnityEngine.Tooltip("List of scenarios in this campaign.")]
        public List<SceneScenario> scenarios = null!;

        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid())
            {
                Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                return false;
            }
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Metadata is missing or invalid.");
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null) return false;

            if (characters == null || characters.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }
            if (characters.Count > 50)
            {
                Debug.LogError($"[Security] Game data validation failed: character count {characters.Count} exceeds 50.");
                return false;
            }

            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (scenarios.Count > 100)
            {
                Debug.LogError($"[Security] Game data validation failed: scenario count {scenarios.Count} exceeds 100.");
                return false;

            foreach (var character in characters)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No scenarios defined or too many scenarios.");
                return false;
            }

            foreach (var character in characters)
            {
                if (character == null || !character.IsValid())
                {
                    UnityEngine.Debug.LogError("[Security] Game data validation failed: A character profile is invalid.");
                    return false;
                }
            }

            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
                if (scenario == null || !scenario.IsValid())
                {
                    UnityEngine.Debug.LogError("[Security] Game data validation failed: A scenario is invalid.");
                    return false;
                }
                if (character == null || !character.IsValid()) return false;
            }
            if (scenarios == null) return false;

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
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
