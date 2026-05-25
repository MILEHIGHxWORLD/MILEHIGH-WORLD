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

        public TextMeshProUGUI SpeakerNameText { get => speakerNameText; set => speakerNameText = value; }
        public TextMeshProUGUI DialogueText { get => dialogueText; set => dialogueText = value; }
        public GameObject DialogueBox { get => dialogueCanvas; set => dialogueCanvas = value; }

        [Header("Typing Settings")]
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
        /// Zero-allocation typewriter effect for dialogue rendering using maxVisibleCharacters.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            Color speakerColor = GetSpeakerColor(speaker);
            string hexColor = ColorUtility.ToHtmlStringRGB(speakerColor);
            speakerNameText.text = $"<color=#{hexColor}>[{speaker}]</color>";

            // Layout-Safe Pattern: Pre-append themed completion cue and set full text at start
            dialogueText.text = content + $" <color=#{hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            float speakerMultiplier = GetSpeedMultiplier(speaker);
            float effectiveDelay = charDelay / speakerMultiplier;

            for (int i = 0; i <= content.Length; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i < content.Length)
                {
                    char c = content[i];
                    float punctuationMultiplier = 1f;

                    // Lexical Pacing: Rhythmic pauses based on punctuation
                    if (c == '.')
                    {
                        // Look-ahead for ellipsis or mid-word period (like Sky.ix)
                        bool isPartOfEllipsis = (i + 1 < content.Length && content[i+1] == '.') || (i > 0 && content[i-1] == '.');
                        bool isEndOfSentence = (i + 1 == content.Length || char.IsWhiteSpace(content[i+1]));

                        if (isPartOfEllipsis) punctuationMultiplier = 5f;
                        else if (isEndOfSentence) punctuationMultiplier = 15f;
                        else punctuationMultiplier = 1f; // Mid-word dot, no extra pause
                    }
                    else if ("!?".Contains(c)) punctuationMultiplier = 15f;
                    else if (",:".Contains(c)) punctuationMultiplier = 8f;

                    await Task.Delay(Mathf.RoundToInt(effectiveDelay * punctuationMultiplier * 1000));
                }
            }

            // Show the completion cue
            dialogueText.maxVisibleCharacters = content.Length + 2;
        }

        public float GetSpeedMultiplier(string speaker)
        {
            if (speaker == "Kai") return kaiSpeedMultiplier;
            if (speaker == "Sky.ix") return skyixSpeedMultiplier;
            return 1.0f;
        }

        public Color GetSpeakerColor(string speaker)
        {
            switch (speaker)
            {
                case "Sky.ix": return Color.cyan;
                case "Kai": return new Color(1f, 0.84f, 0f); // Gold
                case "Delilah": return new Color(0.6f, 0.1f, 0.9f); // Void Purple
                case "King Cyrus": return new Color(1f, 0.27f, 0f); // Orangered-ish
                case "Reverie": return new Color(0.66f, 0.33f, 0.97f); // Lavender/Purple
                default: return Color.white;
            }
        }

        [System.Diagnostics.Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
