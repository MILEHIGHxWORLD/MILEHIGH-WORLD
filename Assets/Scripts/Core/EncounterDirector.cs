using UnityEngine;

namespace MilehighWorld.Core
{
    public class EncounterDirector : MonoBehaviour
    {
        public static EncounterDirector Instance;
        private void Awake() => Instance = this;

        public void ApplyVoidVariance(float delta)
        {
            Debug.Log($"<color=#ff00ff>[EncounterDirector]</color> Applying Void Variance Delta: {delta}");
            // Future logic to interact with GlobalResonanceManager
        }
    }
}
