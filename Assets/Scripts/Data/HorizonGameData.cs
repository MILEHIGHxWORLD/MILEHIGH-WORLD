using System;
using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState { Day, Night, Dynamic }

    [System.Serializable]
    public class Metadata
    {
        [UnityEngine.Tooltip("The lighting state for the scene.")]
        public LightingState lighting;
        [UnityEngine.Tooltip("The name of the environment.")]
        public string environment = "";
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
            if (float.IsNaN(voidSaturationLevel) || float.IsInfinity(voidSaturationLevel) || voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                UnityEngine.Debug.LogError($"[Security] Metadata validation failed: voidSaturationLevel {voidSaturationLevel} is invalid or out of range [0.0, 1.0]");
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
        /// 🛡️ Sentinel: Security validation for individual character data.
        /// </summary>
        public bool IsValid()
        {
            // SECURITY: Implement resource exhaustion protection (DoS prevention)
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] CharacterProfile validation failed: Name is missing or too long.");
                return false;
            }
            if (role != null && role.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed for '{name}': Role exceeds 128 characters.");
                return false;
            }
            if (traits != null)
            {
                if (traits.Length > 50)
                {
                    UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed for '{name}': Too many traits ({traits.Length}).");
                    return false;
                }
                foreach (var trait in traits)
                {
                    if (trait != null && trait.Length > 128)
                    {
                        UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed for '{name}': Trait length exceeds 128 characters.");
                        return false;
                    }
                }
            }
            if (behaviorScript != null && behaviorScript.Length > 2048)
            {
                UnityEngine.Debug.LogError($"[Security] CharacterProfile validation failed for '{name}': Behavior script exceeds 2048 characters.");
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
        public float x;
        public float y;
        public float z;

        public UnityEngine.Vector3 GetVectorValue()
        {
            return new UnityEngine.Vector3(x, y, z);
        }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] ObjectInteraction validation failed: objectId is missing or too long.");
                return false;
            }
            if (action != null && action.Length > 128)
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': action too long.");
                return false;
            }

            // SECURITY: Ensure float parameters are valid numbers and not NaN/Infinity
            if (float.IsNaN(floatValue) || float.IsInfinity(floatValue) ||
                float.IsNaN(x) || float.IsInfinity(x) ||
                float.IsNaN(y) || float.IsInfinity(y) ||
                float.IsNaN(z) || float.IsInfinity(z))
            {
                UnityEngine.Debug.LogError($"[Security] ObjectInteraction validation failed for '{objectId}': Numeric parameters contain NaN or Infinity.");
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
            if (speaker != null && speaker.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: speaker name too long.");
                return false;
            }
            if (text != null && text.Length > 2048)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: text too long.");
                return false;
            }
            if (trigger != null && trigger.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] Dialogue validation failed: trigger too long.");
                return false;
            }
            return true;
        }
    }

    [System.Serializable]
    public class SceneScenario
    {
        [UnityEngine.Tooltip("Unique ID for the scenario.")]
        public string scenarioId = null!;
        [UnityEngine.Tooltip("Description of the scenario.")]
        [UnityEngine.TextArea(2, 5)]
        public string description = null!;
        [UnityEngine.Tooltip("List of interactive objects in this scenario.")]
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        [UnityEngine.Tooltip("List of dialogue lines in this scenario.")]
        public List<Dialogue> dialogue = new List<Dialogue>();

        /// <summary>
        /// 🛡️ Sentinel: Security validation for scenario data.
        /// </summary>
        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                UnityEngine.Debug.LogError("[Security] SceneScenario validation failed: scenarioId is missing or too long.");
                return false;
            }
            if (description != null && description.Length > 1024)
            {
                UnityEngine.Debug.LogError($"[Security] SceneScenario '{scenarioId}' validation failed: description exceeds 1024 characters.");
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
                if (dialogue.Count > 50)
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
    }

    [System.Serializable]
    public class HorizonGameData
    {
        [UnityEngine.Tooltip("Unique ID for the scene.")]
        public string sceneId = null!;
        [UnityEngine.Tooltip("Metadata for the scene.")]
        public Metadata metadata = null!;
        [UnityEngine.Tooltip("List of character profiles in this campaign.")]
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        [UnityEngine.Tooltip("List of scenarios in this campaign.")]
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        /// <summary>
        /// 🛡️ Sentinel: Performs integrity and security validation on the entire campaign dataset.
        /// </summary>
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
                UnityEngine.Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            }
            if (scenarios.Count > 50)
            {
                UnityEngine.Debug.LogError($"[Security] Game data validation failed: scenario count {scenarios.Count} exceeds 50.");
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
