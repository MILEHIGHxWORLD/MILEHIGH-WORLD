using System.Collections.Generic;
using UnityEngine;
using Milehigh.Data;         // Access to CharacterData
using Milehigh.World.Engine; // Access to BicameralBattleEngine

namespace Milehigh.World.Entities
{
    public class Protagonist : MonoBehaviour
    {
        [Header("Identity & Core Data")]
        [Tooltip("The core data container used by the CombatManager")]
        [SerializeField] private CharacterData characterStats;

        [Header("Internal Logic")]
        [SerializeField] private List<string> _abilities = new List<string>();

        // Public accessor for the name property
        public string Name => characterStats != null ? characterStats.characterName : "Unknown";

        /// <summary>
        /// Initializes the protagonist with data and registers them with the world state.
        /// </summary>
        public void InitializeCharacter(CharacterData data, List<string> startingAbilities)
        {
            characterStats = data;
            _abilities = new List<string>(startingAbilities);

            Debug.Log($"[ARCHITECT] {Name} initialized. Ready for Void-Now synchronization.");
        }

        /// <summary>
        /// Example method to apply logic based on the reality state.
        /// Sets the vanguard multiplier based on reality state.
        /// </summary>
        public void UpdateAbilitiesByReality(BicameralBattleEngine.RealityState state)
        {
            if (characterStats == null) return;

            // Resetting or setting based on state to avoid cumulative stacking
            float resonanceMod = (state == BicameralBattleEngine.RealityState.Void) ? 1.5f : 1.0f;

            // Adjusting stats internally for combat logic
            characterStats.vanguardMultiplier = resonanceMod;

            Debug.Log($"{Name} resonance adjusted to {resonanceMod}x for {state} reality.");
        }
    }
}
