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
        public Milehigh.Data.LightingState lighting;
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

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is out of range [0.0, 1.0]");
                return false;
            }
            if (System.String.IsNullOrEmpty(this.environment))
            {
                UnityEngine.Debug.LogError("[Security] Metadata validation failed: environment is missing.");
                return false;
            }
            if (this.environment.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: Environment name length {this.environment.Length} exceeds 128 characters.");
                return false;
            }

            // SECURITY: Ensure voidSaturationLevel is within the expected [0.0, 1.0] range.
            if (this.voidSaturationLevel < 0.0f || this.voidSaturationLevel > 1.0f)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {this.voidSaturationLevel} is out of range [0.0, 1.0]");
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
            if (System.String.IsNullOrEmpty(this.name))
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: Name is missing.");
                return false;
            }
            if (this.name.Length > 64)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Name '{this.name}' exceeds 64 characters.");
                return false;
            }
            if (!string.IsNullOrEmpty(role) && role.Length > 64)
            {
                Debug.LogError("[Security] CharacterProfile validation failed: role is too long.");
            if (!System.String.IsNullOrEmpty(this.role) && this.role.Length > 64)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Role for '{this.name}' exceeds 64 characters.");
                return false;
            }
            if (this.traits != null && this.traits.Length > 20)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Too many traits for '{this.name}'.");
                return false;
            }
            if (!System.String.IsNullOrEmpty(this.behaviorScript) && this.behaviorScript.Length > 2048)
            {
                Debug.LogError("[Security] CharacterProfile validation failed: too many traits.");
                return false;
            }
            if (!string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length > 2048)
            {
                Debug.LogError("[Security] CharacterProfile validation failed: behaviorScript is too long.");
                return false;
            }
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed: Behavior script for '{this.name}' exceeds 2048 characters.");
                return false;
            }
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

        public UnityEngine.Vector3 GetVectorValue()
        {
            return new UnityEngine.Vector3(this.x, this.y, this.z);
        }

        public bool IsValid()
        {
            if (System.String.IsNullOrEmpty(this.objectId))
            {
                UnityEngine.Debug.LogError("[Security] ObjectInteraction validation failed: objectId is missing.");
                return false;
            }
            if (objectId.Length > 128)
            {
                Debug.LogError("[Security] ObjectInteraction validation failed: objectId is too long.");
                return false;
            }
            if (string.IsNullOrEmpty(action))
            {
                Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': action is missing.");
                return false;
            }
            if (action.Length > 128)
            {
                Debug.LogError("[Security] ObjectInteraction validation failed: action is too long.");
            if (this.objectId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] ObjectInteraction validation failed: objectId exceeds 128 characters.");
                return false;
            }
            if (System.String.IsNullOrEmpty(this.action))
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{this.objectId}': action is missing.");
                return false;
            }
            if (this.action.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{this.objectId}': action exceeds 128 characters.");
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
            if (!string.IsNullOrEmpty(speaker) && speaker.Length > 64)
            {
                Debug.LogError("[Security] Dialogue validation failed: speaker name is too long.");
                return false;
            }
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("[Security] Dialogue validation failed: text is missing.");
                return false;
            }
            if (text.Length > 1024)
            {
                Debug.LogError("[Security] Dialogue validation failed: text is too long.");
            if (!System.String.IsNullOrEmpty(this.speaker) && this.speaker.Length > 64)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: speaker name is too long.");
                return false;
            }
            if (System.String.IsNullOrEmpty(this.text))
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: text is missing.");
                return false;
            }
            if (this.text.Length > 1024)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: text exceeds 1024 characters.");
                return false;
            }
            if (!System.String.IsNullOrEmpty(this.trigger) && this.trigger.Length > 128)
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
        public System.Collections.Generic.List<Milehigh.Data.ObjectInteraction> interactiveObjects = null!;
        [UnityEngine.Tooltip("List of dialogue lines in this scenario.")]
        public System.Collections.Generic.List<Milehigh.Data.Dialogue> dialogue = null!;

        public bool IsValid()
        {
            if (System.String.IsNullOrEmpty(this.scenarioId))
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: scenarioId is missing.");
                return false;
            }
            if (this.scenarioId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: scenarioId too long.");
                return false;
            }
            if (!System.String.IsNullOrEmpty(this.name) && this.name.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] SceneScenario '{this.scenarioId}' validation failed: name too long.");
                return false;
            }
            if (scenarioId.Length > 128)
            {
                Debug.LogError("[Security] SceneScenario validation failed: scenarioId is too long.");
                return false;
            }
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                Debug.LogError("[Security] SceneScenario validation failed: name is invalid or too long.");
                return false;
            }

            if (interactiveObjects != null)

            if (this.interactiveObjects != null)
            {
                if (this.interactiveObjects.Count > 50)
                {
                    UnityEngine.Debug.LogError($"[Security] SceneScenario '{this.scenarioId}' validation failed: too many interactive objects ({this.interactiveObjects.Count}).");
                    return false;
                }
                foreach (var interaction in this.interactiveObjects)
                {
                    if (interaction == null || !interaction.IsValid()) return false;
                }
            }

            if (dialogue != null)
            if (this.dialogue != null)
            {
                if (this.dialogue.Count > 100)
                {
                    UnityEngine.Debug.LogError($"[Security] SceneScenario '{this.scenarioId}' validation failed: too many dialogue lines ({this.dialogue.Count}).");
                    return false;
                }
                foreach (var d in this.dialogue)
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
        [UnityEngine.Tooltip("Unique ID for the scene.")]
        public string sceneId = null!;
        [UnityEngine.Tooltip("Metadata for the scene.")]
        public Milehigh.Data.Metadata metadata = null!;
        [UnityEngine.Tooltip("List of character profiles in this campaign.")]
        public System.Collections.Generic.List<Milehigh.Data.CharacterProfile> characters = null!;
        [UnityEngine.Tooltip("List of scenarios in this campaign.")]
        public System.Collections.Generic.List<Milehigh.Data.SceneScenario> scenarios = null!;

        public bool IsValid()
        {
            if (System.String.IsNullOrEmpty(this.sceneId) || this.sceneId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: sceneId is missing or too long.");
                return false;
            }

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

            if (this.metadata == null || !this.metadata.IsValid())
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: Metadata missing or invalid.");
                return false;
            }
            if (scenarios.Count > 100)
            {
                Debug.LogError($"[Security] Game data validation failed: scenario count {scenarios.Count} exceeds 100.");
                return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }

            if (this.characters == null || this.characters.Count == 0)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No character profiles defined.");
                return false;
            }
            if (this.characters.Count > 50)
            {
                UnityEngine.Debug.LogError($"[Security] Game data validation failed: character count {this.characters.Count} exceeds 50.");
                return false;
            }
            foreach (var character in this.characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (this.scenarios == null || this.scenarios.Count == 0)
            {
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (this.scenarios.Count > 100)
            {
                UnityEngine.Debug.LogError($"[Security] Game data validation failed: scenario count {this.scenarios.Count} exceeds 100.");
                return false;
            }
            foreach (var scenario in this.scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }

            return true;
        }
    }
}
