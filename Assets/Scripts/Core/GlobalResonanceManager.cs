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
        }

        public float GetIntegrityMultiplier()
        {
            // Placeholder logic for IX-Node stabilization levels
            return 1.25f;
        }
    }
}
