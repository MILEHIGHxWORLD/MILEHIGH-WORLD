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

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(environment)) return false;
            if (environment.Length > 128) return false;
            if (voidSaturationLevel < 0.0f) return false;
            if (voidSaturationLevel > 1.0f) return false;
            return true;
        }
    }

    [Serializable]
    public class CharacterProfile
    {
        public string name;
        public string role;
        public string behaviorScript;
        public string[] traits;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(name)) return false;
            if (name.Length > 64) return false;
            if (string.IsNullOrEmpty(role)) return false;
            if (role.Length > 64) return false;
            if (string.IsNullOrEmpty(behaviorScript)) return false;
            if (behaviorScript.Length > 64) return false;
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

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(objectId)) return false;
            if (objectId.Length > 128) return false;
            if (string.IsNullOrEmpty(action)) return false;
            if (action.Length > 128) return false;
            return true;
        }
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker;
        public string text;
        public string trigger;

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(speaker)) return false;
            if (speaker.Length > 64) return false;
            if (string.IsNullOrEmpty(text)) return false;
            if (text.Length > 1024) return false;
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

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(scenarioId)) return false;
            if (scenarioId.Length > 128) return false;

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

        public bool IsValid()
        {
            if (metadata == null) return false;
            if (!metadata.IsValid()) return false;

            if (characters == null) return false;
            if (characters.Count == 0) return false;
            if (characters.Count > 50) return false;
            foreach (var character in characters)
            {
                if (character == null || !character.IsValid()) return false;
            }

            if (scenarios != null)
            {
                if (scenarios.Count > 100) return false;
                foreach (var scenario in scenarios)
                {
                    if (scenario == null || !scenario.IsValid()) return false;
                }
            }

            return true;
        }
    }
}
