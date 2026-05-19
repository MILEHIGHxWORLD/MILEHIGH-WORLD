using System;

namespace MilehighWorld.Systems.Agency
{
    public enum ActionType
    {
        MELEE_ATTACK,
        CAST_ABILITY,
        PERSUADE,
        HACK_TERMINAL,
        PSYCHIC_INTRUSION,
        VOID_STEP
    }

    [Serializable]
    public class NarrativeActionContext
    {
        public ActionType ActionType;
        public string TargetId;
        public bool RequiresVisualValidation;

        // --- NARRATIVE CONSTRAINT PARAMETERS ---
        public string CurrentDimension; // e.g., "ŁĪNC", "AŽŪŘE HEIGĤTS", "BACHIŘØN"
        public bool IsTargetVoidCorrupted;
        public bool HasMagenActive; // Spiritual Shield status
        public float DistanceToNexus; // Distance to Onalym Nexus (affects Void volatility)
    }

    [Serializable]
    public class ActionResolutionRequestPayload
    {
        public string playerId;
        public string actionType;
        public string targetId;
        public string currentDimension;
        public bool isTargetVoidCorrupted;
        public string activeSpiritualShields;
        public float proximityToOnalymNexus;
        public string playerCurrentState;
        public string visualFrame; // Base64 string
    }

    [Serializable]
    public class ActionResolutionResponse
    {
        public string EntityName;
        public string DialogueGenerated;
        public bool WasActionSuccessful;
        public string MechanicalDescription;
        public float VoidVarianceDelta;
        public float MilleniaAlignmentDelta; // Replaces 'RelationshipDelta' - tracks progress toward the Prophecy
    }
}
