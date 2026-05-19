using UnityEngine;

namespace MilehighWorld.World.Engine
{
    public class RealityAnchor : MonoBehaviour
    {
        public string anchorUID;
        public void Stabilize()
        {
            Debug.Log($"Anchor {anchorUID} stabilized at {Time.time}");
        }
    }
}
