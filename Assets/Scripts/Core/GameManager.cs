using System;
using UnityEngine;

namespace MilehighWorld.Core
{
    /// <summary>
    /// Core simulation manager for 'Into the Void'.
    /// Dictates global reality states and persistent game data.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public enum RealityState
        {
            TheNow,
            TheVoid,
            Transitional
        }

        [Header("Simulation State")]
        [SerializeField] private RealityState currentReality = RealityState.TheNow;

        // Event delegates for loosely-coupled system architecture
        public event Action<RealityState> OnRealityShiftInitiated;
        public event Action<RealityState> OnRealityShiftCompleted;

        public RealityState CurrentReality => currentReality;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeCoreSystems();
        }

        private void InitializeCoreSystems()
        {
            // TODO: Initialize Data Persistence, Object Pools, and Input mapping here.
            Debug.Log("[GameManager] Core systems initialized. Current anchor: The Now.");
        }

        /// <summary>
        /// Triggers a global reality shift. Listened to by VFX, Audio, and IX-Nodes.
        /// </summary>
        public void InitiateRealityShift(RealityState targetReality)
        {
            if (currentReality == targetReality || currentReality == RealityState.Transitional)
            {
                Debug.LogWarning("[GameManager] Shift aborted: Already in or transitioning to target state.");
                return;
            }

            currentReality = RealityState.Transitional;
            OnRealityShiftInitiated?.Invoke(targetReality);

            // TODO: Hook into Timeline/Coroutine for actual transition duration.
            // Simulating immediate completion for prototype phase.
            CompleteRealityShift(targetReality);
        }

        private void CompleteRealityShift(RealityState newReality)
        {
            currentReality = newReality;
            OnRealityShiftCompleted?.Invoke(currentReality);
            Debug.Log($"[GameManager] Reality shift complete. Welcome to {currentReality}.");
        }
    }
}
