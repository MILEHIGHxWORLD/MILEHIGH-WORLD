using System;
using System.Collections.Generic;
using UnityEngine;

namespace Milehigh.Data
{
    public enum LightingState { Day, Night, Dynamic }

    [Serializable]
    public class Metadata
    {
        public LightingState lighting;
        public string environment = "";
        public int systemParity;
        public float voidSaturationLevel;

        public bool IsValid()
        {
            if (voidSaturationLevel < 0.0f || voidSaturationLevel > 1.0f)
            {
                Debug.LogError($"[Security] Metadata: voidSaturationLevel {voidSaturationLevel} out of range.");
                return false;
            }

            if (!string.IsNullOrEmpty(environment) && environment.Length > 128)
            {
                Debug.LogError("[Security] Metadata: Environment name too long.");
                return false;
            }

            return true;
        }
    }

    [Serializable]
    public class CharacterProfile
    {
        public string name = "";
        public string role = "";
        public string[] traits = Array.Empty<string>();
        public string behaviorScript = "";

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name) || name.Length > 128)
            {
                Debug.LogError("[Security] CharacterProfile: Name invalid or too long.");
                return false;
            }
            if (role != null && role.Length > 128)
            {
                Debug.LogError("[Security] CharacterProfile: Role too long.");
                return false;
            }
            if (behaviorScript != null && behaviorScript.Length > 2048)
            {
                Debug.LogError("[Security] CharacterProfile: behaviorScript too long.");
                return false;
            }
            if (traits != null)
            {
                if (traits.Length > 20)
                {
                    Debug.LogError("[Security] CharacterProfile: Traits count exceeds limit.");
                    return false;
                }
                foreach (var trait in traits)
                {
                    if (trait != null && trait.Length > 128)
                    {
                        Debug.LogError("[Security] CharacterProfile: Trait string too long.");
                        return false;
                    }
                }
            }
            return true;
        }
    }

    [Serializable]
    public class ObjectInteraction
    {
        public string objectId = "";
        public string action = "";
        public bool isVector;
        public float floatValue;
        public float x;
        public float y;
        public float z;

        public Vector3 GetVectorValue() => new Vector3(x, y, z);

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId) || objectId.Length > 128)
            {
                Debug.LogError("[Security] Interaction: objectId invalid or too long.");
                return false;
            }
            if (action != null && action.Length > 128)
            {
                Debug.LogError("[Security] Interaction: Action too long.");
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker = "";
        public string text = "";
        public string trigger = "";

        public bool IsValid()
        {
            if (speaker != null && speaker.Length > 128)
            {
                Debug.LogError("[Security] Dialogue: Speaker too long.");
                return false;
            }
            if (string.IsNullOrEmpty(text) || text.Length > 2048)
            {
                Debug.LogError("[Security] Dialogue: Text invalid or too long.");
                return false;
            }
            if (trigger != null && trigger.Length > 128)
            {
                Debug.LogError("[Security] Dialogue: Trigger too long.");
                return false;
            }
            return true;
        }
    }

    [Serializable]
    public class SceneScenario
    {
        public string scenarioId = "";
        public string description = "";
        public List<ObjectInteraction> interactiveObjects = new List<ObjectInteraction>();
        public List<Dialogue> dialogue = new List<Dialogue>();

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId) || scenarioId.Length > 128)
            {
                Debug.LogError("[Security] Scenario: scenarioId invalid or too long.");
                return false;
            }
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
    }

    [Serializable]
    public class HorizonGameData
    {
        public string sceneId = "";
        public Metadata metadata = new Metadata();
        public List<CharacterProfile> characters = new List<CharacterProfile>();
        public List<SceneScenario> scenarios = new List<SceneScenario>();

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(sceneId) || sceneId.Length > 128)
            {
                Debug.LogError("[Security] HorizonGameData: sceneId invalid or too long.");
                return false;
            }
            if (metadata == null || !metadata.IsValid()) return false;
            if (characters == null || characters.Count == 0 || characters.Count > 50)
            {
                Debug.LogError("[Security] HorizonGameData: Character count invalid.");
                return false;
            }
            if (scenarios == null || scenarios.Count == 0 || scenarios.Count > 100)
            {
                Debug.LogError("[Security] HorizonGameData: Scenario count invalid.");
                return false;
            }

            if (scenarios == null || scenarios.Count == 0)
            {
                Debug.LogError("[Security] Game data validation failed: No scenarios defined.");
                return false;
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }
            foreach (var scenario in scenarios)
            {
                if (scenario == null || !scenario.IsValid()) return false;
            }

            return true;
        }
    }
}
