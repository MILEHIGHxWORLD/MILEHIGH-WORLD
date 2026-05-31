// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using MilehighWorld.Core;
using MilehighWorld.Backend;
using Milehigh.World.CoreLogic;

namespace MilehighWorld.Cinematics
{
    /// <summary>
    /// Manages the asynchronous execution of the "Into the Void" cinematic climax.
    /// Drives HDRP shader manipulation, Base-9 parity alignment, and lexical pacing.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Core Engine References")]
        [SerializeField] private TimelineSimulationEngine timelineEngine = null!;
        [SerializeField] private VitisAIBridge vitisBridge = null!;

        [Header("Entity References")]
        [SerializeField] private GameObject skyixPrefab = null!;
        [SerializeField] private GameObject reveriePrefab = null!;
        [SerializeField] private GameObject kingCyrusPrefab = null!;

        [Header("UI & Lexical Systems")]
        [SerializeField] private TextMeshProUGUI speakerNameText = null!;
        [SerializeField] private TextMeshProUGUI dialogueText = null!;
        [SerializeField] private GameObject dialogueCanvas = null!;

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;
        private const float IteratedSanctuary = 0.0777777777f;

        private bool _isStabilized = false;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            TimelineSimulationEngine.OnTimelineStabilized += () => {
                _isStabilized = true;
                LogNarrativeTelemetry("EVENT: Timeline Stabilized Signal Received.");
            };

            _ = ExecuteConvergenceSequenceAsync();
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX - SECTOR 09-09-09");

            // 1. Force the local coordinate space into a Linear Omen (6.0) Hex-State
            await TweenShaderEntropyAsync(LinearOmenHexState, 2.0f);

            // 2. Transfinite Data Load: Initialize entities from object pools (Disable vs Destroy SOP)
            skyixPrefab.SetActive(true);
            reveriePrefab.SetActive(true);
            kingCyrusPrefab.SetActive(true);

            // 3. Asynchronous Lexical Pacing
            dialogueCanvas.SetActive(true);
            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", 0.04f);
            await Task.Delay(500);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. Engaging Void Conduit via Vitis AI Bridge.", 0.03f);

            // 4. Parity Verification via Vitis AI and Timeline Engine
            LogNarrativeTelemetry("Executing Vitis AI Bridge Analysis: Calculating System Tension...");

            // Register final shards to reach parity
            for (int i = 0; i < 999; i++) timelineEngine.RegisterSynchronizedShard();

            double tension = vitisBridge.CalculateSystemTension();
            timelineEngine.EvaluateSystemTension(tension);

            if (_isStabilized && !timelineEngine.IsRealityFractured)
            {
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. System tension within limits. Severing the loop... now!", 0.03f);
                await ExecuteSaveEveryoneProtocolAsync();
            }
            else
            {
                string reason = timelineEngine.IsRealityFractured ? "Structural Reality Fracture" : "Parity Synchronization Failure";
                LogNarrativeTelemetry($"WARNING: Convergence Failed. Reason: {reason}");
                await StreamDialogueAsync("King Cyrus", "Your reality is too brittle for this power!", 0.04f);
            }

            dialogueCanvas.SetActive(false);
        }

        /// <summary>
        /// Mathematically tweens the HDRP shader's emissive intensity to simulate Void corruption.
        /// </summary>
        private async Task TweenShaderEntropyAsync(float targetIntensity, float duration)
        {
            float startIntensity = hyperrealisticPlatformShader.GetFloat(emissiveIntensityId);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                hyperrealisticPlatformShader.SetFloat(emissiveIntensityId, currentIntensity);

                // Processor Choking Prevention: Yield execution
                await Task.Yield();
            }
        }

        /// <summary>
        /// Executes the final visual and logical reset to the True Monad (1.0).
        /// </summary>
        private async Task ExecuteSaveEveryoneProtocolAsync()
        {
            LogNarrativeTelemetry("PROTOCOL_SAVE_EVERYONE Initiated. Physics re-aligning.");

            // Fade out Cyrus using Object Pooling SOP (Alpha Decay)
            await TweenAlphaDecayAsync(kingCyrusPrefab.GetComponentInChildren<Renderer>().material, 1.5f);
            kingCyrusPrefab.SetActive(false); // Return to pool

            // Clamp environmental delta changes instantly upon loop completion
            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);

            LogNarrativeTelemetry("Omen Singularity Severed. Verse Stabilized.");
        }

        private async Task TweenAlphaDecayAsync(Material mat, float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                mat.SetFloat(baseColorAlphaId, alpha);
                await Task.Yield();
            }
        }

        /// <summary>
        /// Zero-allocation typewriter effect for dialogue rendering with rhythmic pacing.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            speakerNameText.text = $"<color=cyan>[{speaker}]</color>";

            // BOLT: Zero-allocation typewriter effect.
            // Assign the full text once and use maxVisibleCharacters to reveal it.
            dialogueText.text = content;
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                // Palette: Rhythmic Pacing - delays based on character context for a more natural feel.
                char c = dialogueText.textInfo.characterInfo[i - 1].character;
                int delayMs = Mathf.RoundToInt(charDelay * 1000);

                if (c == '.' || c == '?' || c == '!')
                {
                    // Look-ahead to avoid pausing on abbreviations or mid-sentence periods.
                    bool isEndOfSentence = (i == totalCharacters) ||
                        (i < totalCharacters && char.IsWhiteSpace(dialogueText.text[dialogueText.textInfo.characterInfo[i].index]));

                    if (isEndOfSentence) delayMs *= 12; // 12x delay for sentence ends.
                }
                else if (c == ',' || c == ':' || c == ';')
                {
                    delayMs *= 6; // 6x delay for clauses.
                }

                await Task.Delay(delayMs);
            }

            // BOLT: Explicitly reset maxVisibleCharacters to the full length to ensure stability for future reuse.
            dialogueText.maxVisibleCharacters = totalCharacters;
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
