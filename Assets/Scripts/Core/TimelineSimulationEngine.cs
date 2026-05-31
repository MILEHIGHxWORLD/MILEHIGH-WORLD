// ==========================================
// Copyright 2026 MILEHIGH-WORLD LLC.
// All Rights Reserved.
// ==========================================

using System;
using UnityEngine;

namespace Milehigh.World.CoreLogic
{
    /// <summary>
    /// Core simulation engine managing timeline synchronization and structural integrity.
    /// </summary>
    public class TimelineSimulationEngine : MonoBehaviour
    {
        public static event Action OnTimelineStabilized;

        [Header("System State Tracking")]
        [SerializeField] private int currentSynchronizedShards = 0;

        public bool IsLoopSevered { get; private set; } = false;
        public bool IsRealityFractured { get; private set; } = false;

        /// <summary>
        /// Registers a synchronized shard into the core chronos engine.
        /// </summary>
        public void RegisterSynchronizedShard()
        {
            if (IsLoopSevered) return;

            currentSynchronizedShards++;
            EvaluateTimelineState();
        }

        /// <summary>
        /// Evaluates current structural tension against baseline thresholds.
        /// </summary>
        public void EvaluateSystemTension(double calculatedTension)
        {
            if (calculatedTension > RealityConstants.AbsoluteTensionBase)
            {
                IsRealityFractured = true;
                BreakGeometryStability();
            }
        }

        private void EvaluateTimelineState()
        {
            if (currentSynchronizedShards >= RealityConstants.MaxShardParity)
            {
                IsLoopSevered = true;
                OnTimelineStabilized?.Invoke();
                SecureIXNodeStabilization();
            }
        }

        private void SecureIXNodeStabilization()
        {
            Debug.Log("SUCCESS: 999 Shard Parity reached. Local IX-Node stabilized. Chronos loop severed.");
        }

        private void BreakGeometryStability()
        {
            Debug.LogError("CRITICAL FAILURE: AbsoluteTensionBase exceeded. Structural fracture detected.");
        }
    }
}
