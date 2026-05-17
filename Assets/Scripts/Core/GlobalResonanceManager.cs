using UnityEngine;

namespace Milehigh.World.Core
{
    public class GlobalResonanceManager : MonoBehaviour
    {
        public static GlobalResonanceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); }
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
            // Placeholder logic for IX-Node stabilization levels
            return 1.25f;
            // Implementation logic for resonance-based integrity
            return 1.25f * resonanceFactor;
        }
    }
}
