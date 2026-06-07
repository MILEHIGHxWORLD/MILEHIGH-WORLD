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
            else
            {
                Destroy(gameObject);
            }
        }

        public void TriggerParityLock(int level)
        {
            Debug.Log($"[LatticeSynchronizer] Parity Lock Triggered at Level: {level}");
        }

        public void SynchronizeShard(int nodeId, float traumaModifier)
        {
            Debug.Log($"[LatticeSynchronizer] Synchronizing Shard {nodeId} with Trauma Modifier: {traumaModifier}");
        }
    }
}
