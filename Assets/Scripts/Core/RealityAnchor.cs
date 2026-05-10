using UnityEngine;

namespace Milehigh.World.Engine
{
    public class RealityAnchor : MonoBehaviour
    {
        [SerializeField] public string anchorUID;
        public float anchorStability = 1.0f;

        public void Stabilize()
        {
            Debug.Log($"Anchor {anchorUID} stabilized at {Time.time}");
        }

        public void StabilizeReality(float amount)
        {
            anchorStability = Mathf.Clamp01(anchorStability + amount);
            Debug.Log($"Reality Anchor: Stability at {anchorStability}");
        }
    }
}
