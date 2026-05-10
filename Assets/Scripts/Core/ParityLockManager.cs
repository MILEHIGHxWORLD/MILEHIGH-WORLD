using UnityEngine;

namespace Milehigh.Core
{
    public class ParityLockManager : MonoBehaviour
    {
        public bool isParityLocked = false;

        public void EngageParityLock()
        {
            isParityLocked = true;
            Debug.Log("Parity Lock engaged.");
        }

        public void ReleaseParityLock()
        {
            isParityLocked = false;
            Debug.Log("Parity Lock released.");
        }
    }
}
