using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Milehigh.Narrative
{
    public class ReverieDialogueSync : MonoBehaviour
    {
        [Header("Reverie's Data Links")]
        [SerializeField] private AudioSource _reverieVoiceBox;
        [SerializeField] private AudioClip _reverieAcknowledgeClip;
        [SerializeField] private Animator _reverieAnimator;

        // BOLT: Cache yield instructions to prevent GC allocations in frequently triggered routines
        private static readonly Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

        private WaitForSeconds GetWait(float seconds)
        {
            if (!_waitCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[seconds] = wait;
            }
            return wait;
        }

        private void Awake()
        {
            // SENTINEL: Security & Robustness - Ensure all required components are assigned to prevent NullReferenceExceptions
            if (_reverieVoiceBox == null) Debug.LogError("[REVERIE_SYNC] Missing AudioSource reference.");
            if (_reverieAcknowledgeClip == null) Debug.LogError("[REVERIE_SYNC] Missing AudioClip reference.");
            if (_reverieAnimator == null) Debug.LogError("[REVERIE_SYNC] Missing Animator reference.");
        }

        public void OnHologramCritical(string sourceId)
        {
            StartCoroutine(ReverieResponseRoutine(sourceId));
        }

        private IEnumerator ReverieResponseRoutine(string sourceId)
        {
            // Wait for Kai to finish his line ("You're walking too loudly...")
            // BOLT: Use cached wait instruction
            yield return GetWait(3.0f);

            // Reverie reacts physically to the hologram
            if (_reverieAnimator != null)
            {
                _reverieAnimator.SetTrigger("React_To_Hologram");
            }

            // Play Reverie's response audio
            if (_reverieVoiceBox != null && _reverieAcknowledgeClip != null)
            {
                _reverieVoiceBox.PlayOneShot(_reverieAcknowledgeClip);
            }

            // PALETTE: Enhanced logging for FULCRUM dashboard with source traceability
            Debug.Log($"<color=#a855f7>[REVERIE_NODE]</color> I see it, Kai. Source {sourceId} is bleeding. Adjusting local gravity parameters to compensate.");
        }
    }
}
