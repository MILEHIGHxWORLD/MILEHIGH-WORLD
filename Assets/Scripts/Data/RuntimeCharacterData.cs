// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

namespace MilehighWorld.Data
{
    [System.Serializable]
    public struct RuntimeCharacterData
    {
        public string Id;
        public string Archetype;
        public string TechAlignment; // "Arcane", "Cybernetic", "Void-Touched", "Hybrid"
        public float HealthPercentage;
        public float BaseTraumaModifier;
        public bool IsVoidCorrupted;
        public bool HasMagenActive; // From Narrative Constraints
    }
}
