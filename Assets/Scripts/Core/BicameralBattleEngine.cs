using UnityEngine;
using Milehigh.Data;
using Milehigh.Core;

namespace Milehigh.World.Engine
{
    public class BicameralBattleEngine : MonoBehaviour
    {
        public enum RealityState { Void, Now }

        [SerializeField] public RealityState currentReality = RealityState.Now;

        public bool isDualProcessingActive = false;

        public void ToggleReality()
        {
            currentReality = (currentReality == RealityState.Now) ? RealityState.Void : RealityState.Now;
            // Update GlobalResonance based on state
            GlobalResonanceManager.Instance.UpdateGlobalResonance(currentReality);
        }

        public void ProcessDualIntent(string protagonistIntent, string antagonistIntent)
        {
            isDualProcessingActive = true;
            Debug.Log($"Bicameral Engine: Processing {protagonistIntent} vs {antagonistIntent}");

            // Phase 2 Logic: Resolve conflicts between protagonist and antagonist
            bool success = Random.value > 0.3f;
            if (success)
            {
                Debug.Log("Bicameral Engine: Protagonist intent succeeded.");
            }
            else
            {
                Debug.Log("Bicameral Engine: Antagonist intent prevailed.");
            }
        }

        public void FinalizeTurn()
        {
            isDualProcessingActive = false;
            Debug.Log("Bicameral Engine: Turn finalized.");
        }
    }
}
