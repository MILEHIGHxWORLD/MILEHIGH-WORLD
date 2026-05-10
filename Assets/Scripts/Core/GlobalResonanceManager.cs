using UnityEngine;
using Milehigh.World.Engine;

namespace Milehigh.Core
{
    public class GlobalResonanceManager : MonoBehaviour
    {
        private static GlobalResonanceManager _instance;
        public static GlobalResonanceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GlobalResonanceManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("GlobalResonanceManager");
                        _instance = go.AddComponent<GlobalResonanceManager>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private float resonanceFactor = 1.0f;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void UpdateGlobalResonance(BicameralBattleEngine.RealityState state)
        {
            switch (state)
            {
                case BicameralBattleEngine.RealityState.Now:
                    resonanceFactor = 1.0f;
                    break;
                case BicameralBattleEngine.RealityState.Void:
                    resonanceFactor = 0.5f;
                    break;
            }
            Debug.Log($"Global Resonance: Updated to {resonanceFactor} due to state {state}");
        }

        public float GetIntegrityMultiplier()
        {
            // Implementation logic for resonance-based integrity
            return 1.25f * resonanceFactor;
        }
    }
}
