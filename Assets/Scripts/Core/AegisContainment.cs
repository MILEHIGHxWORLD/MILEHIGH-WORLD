using UnityEngine;

namespace Milehigh.Core
{
    public class AegisContainment : MonoBehaviour
    {
        public float containmentFieldStrength = 1.0f;

        public void ModulateContainment(float amount)
        {
            containmentFieldStrength = Mathf.Clamp01(containmentFieldStrength + amount);
            Debug.Log($"Aegis: Containment modulated to {containmentFieldStrength}");
        }
    }
}
