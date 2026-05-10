using UnityEngine;
using System.Collections;
using System;
using Milehigh.Core;
using Milehigh.World.Engine;
using Milehigh.Characters;
using Milehigh.Cinematics;

namespace Milehigh.World.Testing
{
    /// <summary>
    /// An integrated test runner that binds the narrative completion of
    /// "Into the Void" directly into the Bicameral Battle Engine.
    /// </summary>
    public class IntegratedBattleOrchestrator : MonoBehaviour
    {
        [Header("Cinematic Integration")]
        [Tooltip("Reference to the cinematic script controlling the scene.")]
        public Cinematic_IntoTheVoid cinematicSequence = null!;

        [Header("System References")]
        public BicameralBattleEngine battleEngine = null!;

        [Header("Character Ability Controllers")]
        public SkyixAbilityController skyix = null!;
        public KaiAbilityController kai = null!;
        public DelilahAIController delilah = null!;

        [Header("Simulation Settings")]
        public float simulationSpeed = 1.0f;

        private void OnEnable()
        {
            // 1. Subscribe to the Cinematic Completion Event
            if (cinematicSequence != null)
            {
                cinematicSequence.OnCinematicComplete += TransitionToCombat;
            }
            else
            {
                Debug.LogWarning("[ORCHESTRATOR] Cinematic_IntoTheVoid reference missing. Running isolated test.");
                TransitionToCombat();
            }
        }

        private void OnDisable()
        {
            if (cinematicSequence != null)
            {
                cinematicSequence.OnCinematicComplete -= TransitionToCombat;
            }
        }

        private void TransitionToCombat()
        {
            Debug.Log("[ORCHESTRATOR] Transitioning to combat state...");

            if (battleEngine != null)
            {
                battleEngine.currentReality = BicameralBattleEngine.RealityState.Void;
                Debug.Log("[ORCHESTRATOR] Bicameral Engine set to VOID state.");
            }

            // Initialize characters for combat simulation
            if (skyix != null) Debug.Log("[ORCHESTRATOR] Skyix Combat Ready.");
            if (kai != null) Debug.Log("[ORCHESTRATOR] Kai Combat Ready.");
            if (delilah != null) Debug.Log("[ORCHESTRATOR] Delilah Combat Ready.");

            StartCoroutine(CombatSimulationLoop());
        }

        private IEnumerator CombatSimulationLoop()
        {
            yield return new WaitForSeconds(1.0f / simulationSpeed);
            Debug.Log("[ORCHESTRATOR] Combat Simulation Started.");
            // Simulation logic would go here
        }
    }
}
