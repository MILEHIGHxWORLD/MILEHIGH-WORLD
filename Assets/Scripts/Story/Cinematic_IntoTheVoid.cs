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
        public TextMeshProUGUI SpeakerNameText { get => speakerNameText; set => speakerNameText = value; }
        [SerializeField] private TextMeshProUGUI dialogueText = null!;
        public TextMeshProUGUI DialogueText { get => dialogueText; set => dialogueText = value; }
        [SerializeField] private GameObject dialogueCanvas = null!;
        public GameObject DialogueBox { get => dialogueCanvas; set => dialogueCanvas = value; }

        [Header("Lexical Tuning")]
        public float baseTypingSpeed = 0.03f;
        public float kaiSpeedMultiplier = 3.0f;
        public float skyixSpeedMultiplier = 1.2f;

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
        private Vector3 _originalSpeakerScale;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            if (speakerNameText != null)
            {
                _originalSpeakerScale = speakerNameText.transform.localScale;
            }
            // Palette: Accessibility - Apply high-contrast black outlines to ensure readability.
            speakerNameText.outlineWidth = 0.2f;
            speakerNameText.outlineColor = Color.black;
            dialogueText.outlineWidth = 0.2f;
            dialogueText.outlineColor = Color.black;

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
        /// Zero-allocation rhythmic typewriter effect for dialogue rendering with rhythmic pacing and speaker transitions.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

            // Palette: Trigger a subtle scale pulse if the speaker changes
            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                if (speakerNameText.transform != null)
                {
                    _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
                }
            }

            // BOLT: Zero-allocation typewriter effect.
            // Assign the full text once (including completion cue) and use maxVisibleCharacters to reveal it.
            dialogueText.text = $"{content} <color={hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int characterCount = dialogueText.textInfo.characterCount;

            for (int i = 0; i <= characterCount; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i < characterCount)
                {
                    // Palette: Correctly use textInfo for punctuation detection to handle rich text tags properly.
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float multiplier = 1f;

                    // Palette: Rhythmic punctuation pauses with look-ahead to handle mid-word periods (e.g., Sky.ix)
                    bool isEndOfSentence = (c == '.' || c == '?' || c == '!');
                    bool isClause = (c == ',' || c == ';' || c == ':');

                    if (isEndOfSentence || isClause)
                    {
                        // Look-ahead using characterInfo to handle potential trailing whitespace.
                        bool isLastVisibleChar = (i == characterCount - 1); // Last character before the cue '▽'
                        bool isFollowedBySpace = (!isLastVisibleChar && char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));

                        if (isLastVisibleChar || isFollowedBySpace)
                        {
                            multiplier = isEndOfSentence ? 12f : 6f;
                        }
                    }

                    await Task.Delay(Mathf.RoundToInt(charDelay * multiplier * 1000));
                }
                else
                {
                    await Task.Delay(Mathf.RoundToInt(charDelay * 1000));
                }
            }

            // BOLT: Explicitly reset maxVisibleCharacters to the full length to ensure stability for future reuse.
            dialogueText.maxVisibleCharacters = characterCount;
        }

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float sin = Mathf.Sin(t * Mathf.PI);
                target.localScale = _originalSpeakerScale * (1f + (sin * (scaleFactor - 1f)));
                await Task.Yield();
            }
            target.localScale = _originalSpeakerScale;
        }

        private string GetSpeakerColorHex(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => "#00FFFF",      // Cyan
                "King Cyrus" => "#FFFF00",  // Yellow
                "Reverie" => "#FF00FF",     // Magenta
                "Kai" => "#FFD700",         // Gold
                "Delilah" => "#9933FF",     // Void Purple
                _ => "#FFFFFF"              // Default White
            };
        }

        public float GetSpeedMultiplier(string speaker)
        {
            if (speaker == "Kai") return kaiSpeedMultiplier;
            if (speaker == "Sky.ix") return skyixSpeedMultiplier;
            return 1.0f;
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
