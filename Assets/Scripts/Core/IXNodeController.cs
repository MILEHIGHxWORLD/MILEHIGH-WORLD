using System;
using UnityEngine;
using MilehighWorld.Core;

namespace MilehighWorld.Core
{
    /// <summary>
    /// Handles the stabilization loop of an IX-Node, preventing Void incursion in localized space.
    /// </summary>
    public class IXNodeController : MonoBehaviour
    {
        [Header("Node Configuration")]
        [SerializeField] private string nodeId = "IX-001";
        [SerializeField] private float stabilizationMax = 100f;
        [SerializeField] private float decayRatePerSecond = 2.5f;
        [SerializeField] private float chargeRatePerSecond = 15f;

        [Header("Runtime State")]
        [SerializeField, Range(0, 100)] private float currentStability = 100f;
        private bool isBeingCharged = false;

        public event Action<float> OnStabilityChanged;
        public event Action OnNodeCollapsed;

        private bool hasCollapsed = false;

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRealityShiftCompleted += HandleRealityShift;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRealityShiftCompleted -= HandleRealityShift;
            }
        }

        private void Update()
        {
            if (hasCollapsed) return;

            ProcessStabilizationLoop();
        }

        private void ProcessStabilizationLoop()
        {
            float previousStability = currentStability;

            if (isBeingCharged)
            {
                currentStability += chargeRatePerSecond * Time.deltaTime;
            }
            else if (GameManager.Instance != null && GameManager.Instance.CurrentReality == GameManager.RealityState.TheVoid)
            {
                // Nodes decay rapidly when exposed to The Void without active charging
                currentStability -= decayRatePerSecond * Time.deltaTime;
            }

            currentStability = Mathf.Clamp(currentStability, 0f, stabilizationMax);

            if (Mathf.Abs(currentStability - previousStability) > 0.01f)
            {
                OnStabilityChanged?.Invoke(currentStability / stabilizationMax);
            }

            if (currentStability <= 0f)
            {
                TriggerCollapse();
            }
        }

        /// <summary>
        /// External interface for Player/NPC interactions to charge the node.
        /// </summary>
        public void SetChargingState(bool charging)
        {
            isBeingCharged = charging;

            // Lore integration: Reverie's commentary on node interaction
            if (charging && currentStability < 50f)
            {
                Debug.Log($"[Reverie_Dialogue_Trigger] \"You really think this ancient junk will hold it back? Fine, but hurry up.\"");
            }
        }

        private void TriggerCollapse()
        {
            hasCollapsed = true;
            OnNodeCollapsed?.Invoke();
            Debug.LogError($"[IXNodeController] CRITICAL: {nodeId} has collapsed. Void incursion imminent in this sector.");

            // TODO: Trigger localized Void environment swap
        }

        private void HandleRealityShift(GameManager.RealityState newState)
        {
            // Node behavior can change depending on global reality states
            if (newState == GameManager.RealityState.TheNow && hasCollapsed)
            {
                // Potential mechanic: Collapsed nodes reset when returning to The Now?
                // Left intentionally open for architectural decision.
            }
        }
    }
}
