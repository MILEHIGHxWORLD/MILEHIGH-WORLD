using UnityEngine;

namespace MilehighWorld.Core
{
    public class LatticeSynchronizer : MonoBehaviour
    {
        public static LatticeSynchronizer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public void TriggerParityLock(int level)
        {
            Debug.Log($"<color=cyan>[LatticeSynchronizer]</color> Triggering Parity Lock Level: {level}");
        }

        public void SynchronizeShard(int nodeIndex, float modifier)
        {
            Debug.Log($"<color=cyan>[LatticeSynchronizer]</color> Synchronizing Shard {nodeIndex} with modifier {modifier}");
        }
    }
}
