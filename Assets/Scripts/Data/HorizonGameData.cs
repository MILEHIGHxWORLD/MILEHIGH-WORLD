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
        public string environment;
        public int systemParity;
        public float voidSaturationLevel;

        public bool IsValid() => !string.IsNullOrEmpty(environment) && environment.Length <= 128 &&
                                 voidSaturationLevel >= 0f && voidSaturationLevel <= 1f;
    }

    [Serializable]
    public class CharacterProfile
    {
        public string name, role, behaviorScript;
        public string[] traits;

        public bool IsValid() => !string.IsNullOrEmpty(name) && name.Length <= 64 &&
                                 !string.IsNullOrEmpty(role) && role.Length <= 64 &&
                                 !string.IsNullOrEmpty(behaviorScript) && behaviorScript.Length <= 64 &&
                                 (traits == null || traits.Length <= 10);
    }

    [Serializable]
    public class ObjectInteraction
    {
        public string objectId, action;
        public bool isVector;
        public float floatValue, x, y, z;
        public Vector3 GetVectorValue() => new Vector3(x, y, z);
        public bool IsValid() => !string.IsNullOrEmpty(objectId) && objectId.Length <= 128 &&
                                 !string.IsNullOrEmpty(action) && action.Length <= 128;
    }

    [Serializable]
    public class Dialogue
    {
        public string speaker, text, trigger;
        public bool IsValid() => !string.IsNullOrEmpty(speaker) && speaker.Length <= 64 &&
                                 !string.IsNullOrEmpty(text) && text.Length <= 1024;
    }

    [Serializable]
    public class SceneScenario
    {
        public string scenarioId, description;
        public List<ObjectInteraction> interactiveObjects;
        public List<Dialogue> dialogue;

        public bool IsValid() => !string.IsNullOrEmpty(scenarioId) && scenarioId.Length <= 128 &&
                                 (interactiveObjects == null || (interactiveObjects.Count <= 50 && !interactiveObjects.Exists(o => !o.IsValid()))) &&
                                 (dialogue == null || (dialogue.Count <= 50 && !dialogue.Exists(d => !d.IsValid())));
    }

    [Serializable]
    public class HorizonGameData
    {
        public string sceneId;
        public Metadata metadata;
        public List<CharacterProfile> characters;
        public List<SceneScenario> scenarios;

        public bool IsValid() => metadata != null && metadata.IsValid() &&
                                 characters != null && characters.Count > 0 && characters.Count <= 50 && !characters.Exists(c => !c.IsValid()) &&
                                 (scenarios == null || (scenarios.Count <= 100 && !scenarios.Exists(s => !s.IsValid())));
    }
}
