// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using MilehighWorld.Core;
using MilehighWorld.Backend;

namespace MilehighWorld.Cinematics
{
    /// <summary>
    /// Manages the asynchronous execution of the "Into the Void" cinematic climax.
    /// Drives HDRP shader manipulation, Base-9 parity alignment, and lexical pacing.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Entity References")]
        [SerializeField] private GameObject skyixPrefab = null!;
        [SerializeField] private GameObject reveriePrefab = null!;
        [SerializeField] private GameObject kingCyrusPrefab = null!;

        [Header("UI & Lexical Systems")]
        [SerializeField] private TextMeshProUGUI speakerNameText = null!;
        [SerializeField] private TextMeshProUGUI dialogueText = null!;
        [SerializeField] private GameObject dialogueCanvas = null!;

        // Palette: Public getters to support existing tests and external UI coordination
        public TextMeshProUGUI SpeakerNameText => speakerNameText;
        public TextMeshProUGUI DialogueText => dialogueText;
        public GameObject DialogueBox => dialogueCanvas;

        [Header("Lexical Pacing Settings")]
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

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;
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

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. We are at 998 shards. Engaging Void Conduit.", 0.03f);

            // 4. Parity Verification via OMEGA.ONE Fulcrum
            LogNarrativeTelemetry("Executing BackendSyncService Call: Validating Parity Resonance...");
            var resolution = await BackendSyncService.Instance.RequestAIResolutionAsync(
                stateHash: 998,
                parityResonance: 0.999f,
                activeReality: "Void",
                zoneId: "LOC_001_LINQ"
            );

            if (resolution.WasActionSuccessful)
            {
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. Severing the loop... now!", 0.03f);
                await ExecuteSaveEveryoneProtocolAsync();
            }
            else
            {
                LogNarrativeTelemetry("WARNING: Parity Lock Failed. Initiating Fallback.");
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
        /// Zero-allocation typewriter effect for dialogue rendering.
        /// Optimized with maxVisibleCharacters, rhythmic pacing, and themed cues.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            Color speakerColor = GetSpeakerColor(speaker);
            string hexColor = ColorUtility.ToHtmlStringRGB(speakerColor);
            float multiplier = GetSpeedMultiplier(speaker);
            float effectiveDelay = (charDelay / multiplier) * 1000;

            speakerNameText.text = $"<color=#{hexColor}>[{speaker}]</color>";

            // Palette: Pre-append completion cue and use maxVisibleCharacters for stable layout
            dialogueText.text = $"{content} <color=#{hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;
            // Subtracting 1 because the last character is our '▽' cue
            int dialogueLength = totalCharacters - 1;

            for (int i = 0; i <= dialogueLength; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= dialogueLength)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float pauseMultiplier = 1f;

                    // Palette: Rhythmic pacing with look-ahead to ignore technical names
                    bool isTechnicalName = (i < dialogueLength && !char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));

                    if (!isTechnicalName)
                    {
                        if (".!?".Contains(c)) pauseMultiplier = 15f;
                        else if (",:".Contains(c)) pauseMultiplier = 8f;
                    }

                    // Ellipsis handling (roughly)
                    if (c == '.' && i > 1 && dialogueText.textInfo.characterInfo[i - 2].character == '.')
                        pauseMultiplier = 5f;

                    await Task.Delay(Mathf.RoundToInt(effectiveDelay * pauseMultiplier));
                }
                else
                {
                    await Task.Delay(Mathf.RoundToInt(effectiveDelay));
                }
            }

            // Reveal the completion cue
            dialogueText.maxVisibleCharacters = totalCharacters;
        }

        // Palette: Speaker-specific theme colors and pacing multipliers
        public Color GetSpeakerColor(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => Color.cyan,
                "Kai" => new Color(1f, 0.84f, 0f), // Gold
                "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
                "King Cyrus" => new Color(1f, 0.27f, 0f), // #FF4500
                "Reverie" => new Color(0.66f, 0.33f, 0.97f), // #A855F7
                _ => Color.white
            };
        }

        public float GetSpeedMultiplier(string speaker)
        {
            return speaker switch
            {
                "Kai" => kaiSpeedMultiplier,
                "Sky.ix" => skyixSpeedMultiplier,
                _ => 1.0f
            };
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
