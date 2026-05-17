using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState { Day, Night, Dynamic }

    [System.Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = "";
        [UnityEngine.Tooltip("The lighting state for the scene.")]
        public Milehigh.Data.LightingState lighting;
        [UnityEngine.Tooltip("The name of the environment.")]
        public string environment = null!;
        [UnityEngine.Tooltip("System parity level for synchronization.")]
        public int systemParity;
        [UnityEngine.Tooltip("The saturation level of the Void effect (0 to 1).")]
        [UnityEngine.Range(0.0f, 1.0f)]
        public float voidSaturationLevel;

        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized metadata meets business constraints.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            if (string.IsNullOrEmpty(environment))
            {
                Debug.LogError("[Security] Metadata validation failed: environment is missing.");
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
        /// 🛡️ Sentinel: Validates metadata integrity and safety bounds.
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Input validation for environment string length (DoS mitigation)
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata validation failed: Environment name exceeds 128 characters.");
                return false;
            }

            // Void saturation must be within a safe 0.0 to 1.0 range.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range.
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (voidSaturationLevel < 0f || voidSaturationLevel > 1f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            // 🛡️ Sentinel: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
        public bool IsValid()
        {
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f) return false;
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128) return false;
        /// <summary>
        /// 🛡️ Sentinel: Security validation to ensure deserialized data meets business constraints.
        /// Validates metadata integrity and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                Debug.LogError($"[Security] Invalid voidSaturationLevel detected: {voidSaturationLevel}. Must be between 0.0 and 1.0.");
                return false;
            }

            if (environment != null && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata environment string exceeds 128 characters.");
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                Debug.LogError("[Security] Metadata validation failed: environment string is too long.");
        /// 🛡️ Sentinel: Security validation to ensure deserialized metadata meets business constraints and safety bounds.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Ensure environment string is present and within safe length limits (DoS mitigation)
            if (string.IsNullOrEmpty(environment))
            {
                UnityEngine.Debug.LogError("[Security] Metadata validation failed: environment is missing.");
                return false;
            }
            if (environment.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: Environment name length {environment.Length} exceeds 128 characters.");
                return false;
            }

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
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

        /// <summary>
        /// 🛡️ Sentinel: Security validation for character profile data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Enforce string length limits to prevent memory exhaustion/DoS
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (role != null && role.Length > 128) return false;
            if (behaviorScript != null && behaviorScript.Length > 128) return false;
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
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 128) return false;
        public string name = "";
        public string role = "";
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = "";
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;

        /// <summary>
        /// 🛡️ Sentinel: Security validation for individual character data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Implement resource exhaustion protection (DoS prevention)
            if (string.IsNullOrEmpty(name) || name.Length > 128) return false;
            if (string.IsNullOrEmpty(role) || role.Length > 128) return false;
            if (behaviorScript != null && behaviorScript.Length > 128) return false;
        public string name = null!;
        public string role = null!;
        public string[] traits = null!;
        public string behaviorScript = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 64) return false;
            if (!string.IsNullOrEmpty(role) && role.Length > 64) return false;
            if (traits != null && traits.Length > 10) return false;
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048) return false;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name))
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: Name is missing.");
                return false;
            }
            if (name.Length > 64)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Name '{name}' exceeds 64 characters.");
                return false;
            }
            if (!string.IsNullOrEmpty(role) && role.Length > 64)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Role for '{name}' exceeds 64 characters.");
                return false;
            }
            if (traits != null && traits.Length > 20)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Too many traits for '{name}'.");
                return false;
            }
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Behavior script for '{name}' exceeds 2048 characters.");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class ObjectInteraction
    {
        public string objectId = "";
        public string action = "";
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

        public UnityEngine.Vector3 GetVectorValue()
        {
            return new UnityEngine.Vector3(x, y, z);
        public Vector3 GetVectorValue() => new Vector3(x, y, z);

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId)) return false;
            return true;
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128) return false;
            return true;
        {
            return new UnityEngine.Vector3(this.x, this.y, this.z);
        }

        public bool IsValid()
        {
8            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction objectId is null, empty or exceeds 128 characters.");
            if (string.IsNullOrEmpty(objectId))
            {
                UnityEngine.Debug.LogError("[Security] ObjectInteraction validation failed: objectId is missing.");
                return false;
            }
            if (objectId.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': objectId exceeds 128 characters.");
                return false;
            }
            if (string.IsNullOrEmpty(action))
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': action is missing.");
                return false;
            }
            if (action.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': action exceeds 128 characters.");
                return false;
            }
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
            if (speaker != null && speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
            if (string.IsNullOrEmpty(speaker) || speaker.Length > 128) return false;
            if (text != null && text.Length > 2048) return false;
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;
        public string speaker = "";
        public string text = "";
        public string trigger = "";
        public string speaker = null!;
        public string text = null!;
        public string trigger = null!;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: speaker name is too long.");
                return false;
            }
            if (string.IsNullOrEmpty(text))
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: text is missing.");
                return false;
            }
            if (text.Length > 1024)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: text exceeds 1024 characters.");
                return false;
            }
            if (!string.IsNullOrEmpty(trigger) && trigger.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: trigger exceeds 128 characters.");
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
        [UnityEngine.Tooltip("List of dialogue lines in this scenario.")]
        public List<Dialogue> dialogue = null!;

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
            }
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        public string scenarioId = "";
        public string description = "";
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();
        public List<ObjectInteraction> interactiveObjects = new();
        public List<Dialogue> dialogue = new();
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();
        public string scenarioId;
        public string description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        /// <summary>
        /// 🛡️ Sentinel: Security validation for scenario data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Enforce string length limits
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            return true;
        }
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] ScenarioId is null, empty or exceeds 128 characters.");
            if (string.IsNullOrEmpty(scenarioId))
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: scenarioId is missing.");
                return false;
            }
            if (scenarioId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: scenarioId too long.");
                return false;
            }
            if (!string.IsNullOrEmpty(name) && name.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] SceneScenario '{scenarioId}' validation failed: name too long.");
                return false;
            }

            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 50)
                {
                    UnityEngine.Debug.LogError($"[Security] SceneScenario '{scenarioId}' validation failed: too many interactive objects ({interactiveObjects.Count}).");
                    return false;
                }
                foreach (var interaction in interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
            }

            if (dialogue != null)
            {
                if (dialogue.Count > 100)
                {
                    UnityEngine.Debug.LogError($"[Security] SceneScenario '{scenarioId}' validation failed: too many dialogue lines ({dialogue.Count}).");
                    return false;
                }
                foreach (var d in dialogue)
                {
                    if (d == null || !d.IsValid()) return false;
                }
            }

            return true;
        }
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            if (interactiveObjects != null)
            {
                if (interactiveObjects.Count > 100) return false;
                foreach (var io in interactiveObjects) if (io == null || !io.IsValid()) return false;
            }
            if (dialogue != null)
            {
                if (dialogue.Count > 200) return false;
                foreach (var d in dialogue) if (d == null || !d.IsValid()) return false;
            }
            return true;
        }
        /// <summary>
        /// 🛡️ Sentinel: Security validation for individual scenarios.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128) return false;
            return true;
        }
        public string scenarioId = null!;
        public string description = null!;
        public List<ObjectInteraction> interactiveObjects = null!;
        public List<Dialogue> dialogue = null!;
    }

    [System.Serializable]
    public class HorizonGameData
    {
        public string sceneId = "";
        public Metadata metadata = null!;
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();
        public List<CharacterProfile> characters = new();
        public List<SceneScenario> scenarios = new();
        public Metadata? metadata;
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();
        [UnityEngine.Tooltip("Unique ID for the scene.")]
        public string sceneId = null!;
        [UnityEngine.Tooltip("Metadata for the scene.")]
        public Milehigh.Data.Metadata metadata = null!;
        [UnityEngine.Tooltip("List of character profiles in this campaign.")]
        public List<CharacterProfile> characters = null!;
        [UnityEngine.Tooltip("List of scenarios in this campaign.")]
        public List<SceneScenario> scenarios = null!;

        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0 || characters.Count > 50) return false;
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100) return false;
            foreach (var c in characters) if (c == null || !c.IsValid()) return false;
            foreach (var s in scenarios) if (s == null || !s.IsValid()) return false;
        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// Validates the deserialized game data for security and integrity.
        /// </summary>
        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0) return false;
            if (scenarios == null || scenarios.Count == 0) return false;
            if (metadata == null) return false;
            if (!metadata.IsValid()) return false;
            if (characters == null || scenarios == null) return false;
            // SECURITY: Enforce global campaign limits to prevent resource exhaustion attacks
            if (sceneId != null && sceneId.Length > 128) return false;
            if (characters != null && characters.Count > 50) return false;
            if (scenarios != null && scenarios.Count > 100) return false;
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] Game data validation failed: sceneId is null, empty or exceeds 128 characters.");
                return false;
            }

            if (metadata == null)
            if (string.IsNullOrEmpty(scenarioId)) return false;
            return true;
        }
    }

    [System.Serializable]
    public class HorizonGameData
    {
        public string sceneId = null!;
        public Metadata metadata = null!;
        public List<CharacterProfile> characters = null!;
        public List<SceneScenario> scenarios = null!;

        public bool IsValid()
        {
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0) return false;
            if (scenarios == null || scenarios.Count == 0) return false;
            if (characters == null || characters.Count == 0)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Metadata is missing.");
                Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of scenarios.");
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is missing.");
            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: sceneId is missing or too long.");
                return false;
            }

            if (metadata == null || !metadata.IsValid())
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                return false;
            }

            if (characters == null || characters.Count == 0)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }
            if (characters.Count > 50)
            {
                UnityEngine.Debug.LogError($"[Security] Game data validation failed: character count {characters.Count} exceeds 50.");
                return false;
            }
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios == null || scenarios.Count == 0)
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
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (scenarios.Count > 100)
            {
                UnityEngine.Debug.LogError($"[Security] Game data validation failed: scenario count {scenarios.Count} exceeds 100.");
                return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] Game data validation failed: Invalid number of scenarios.");
            if (scenarios == null)
            {
                Debug.LogError("[Security] Game data validation failed: Scenarios list is null.");
                return false;
            }
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }

            if (scenarios == null) return false;

            return true;
        }
    }
}