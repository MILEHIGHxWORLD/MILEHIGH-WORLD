using UnityEngine;

namespace Milehigh.Core
{
    public class GlobalResonanceManager : MonoBehaviour
    {
        public float resonanceFactor = 1.0f;

        public void UpdateResonance(float state)
        {
            resonanceFactor = state;
            Debug.Log($"Global Resonance: Updated to {resonanceFactor} due to state {state}");
        }

        public float GetIntegrityMultiplier()
        {
            // Implementation logic for resonance-based integrity
            return 1.25f * resonanceFactor;
        }
    }
}
