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
        [SerializeField] private GameObject skipHint = null!;

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;
        private const float IteratedSanctuary = 0.0777777777f;

        private bool skipRequested = false;
        private float idleTimer = 0f;
        private bool playerInteracted = false;
        private Vector3 originalSpeakerScale;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            if (skipHint != null) skipHint.SetActive(false);

            // Palette: Accessibility - High-contrast outlines for better readability
            if (speakerNameText != null) ApplyHighContrastOutline(speakerNameText);
            if (dialogueText != null) ApplyHighContrastOutline(dialogueText);

            if (speakerNameText != null) originalSpeakerScale = speakerNameText.transform.localScale;

            _ = ExecuteConvergenceSequenceAsync();
        }

        private void ApplyHighContrastOutline(TextMeshProUGUI text)
        {
            text.fontMaterial.EnableKeyword(ShaderUtilities.Keyword_Outline);
            text.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        }

        private void Update()
        {
            // Palette: Universal skip accessibility - Capture any key or click to bypass dialogue pacing
            if (Input.anyKeyDown)
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0f;
                if (skipHint != null) skipHint.SetActive(false);
            }
            else
            {
                // Palette: Idle skip hint discoverability
                idleTimer += Time.deltaTime;
                if (!playerInteracted && idleTimer >= 2.0f && skipHint != null)
                {
                    skipHint.SetActive(true);
                }
            }
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
            await WaitForSecondsOrSkip(0.5f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. We are at 998 shards. Engaging Void Conduit.", 0.03f);
            await WaitForSecondsOrSkip(0.5f);

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
                await WaitForSecondsOrSkip(0.5f);
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
        /// Zero-allocation typewriter effect with rhythmic pacing and character-themed cues.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            // Palette: Reset interaction state for each new line
            skipRequested = false;
            playerInteracted = false;

            string newSpeakerText = $"<color={GetSpeakerColor(speaker)}>[{speaker}]</color>";
            if (speakerNameText.text != newSpeakerText)
            {
                speakerNameText.text = newSpeakerText;
                _ = PopScaleAsync(speakerNameText.transform);
            }

            // Pre-calculate layout with completion cue to avoid jarring shifts
            string colorHex = GetSpeakerColor(speaker);
            dialogueText.text = $"{content} <color={colorHex}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;
            int baseDelayMs = Mathf.RoundToInt(charDelay * 1000);

            for (int i = 1; i <= totalCharacters; i++)
            {
                if (skipRequested)
                {
                    dialogueText.maxVisibleCharacters = totalCharacters;
                    break;
                }

                dialogueText.maxVisibleCharacters = i;

                // Get the character that was just revealed
                char c = dialogueText.textInfo.characterInfo[i - 1].character;
                int currentDelay = baseDelayMs;

                // Rhythmic Pacing Logic: Apply pauses for punctuation to mimic natural speech
                if (c == '.' || c == '?' || c == '!')
                {
                    bool isEllipsis = (i < totalCharacters && dialogueText.textInfo.characterInfo[i].character == '.');
                    bool isEndOfSentence = (i == totalCharacters || char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));

                    if (isEllipsis) currentDelay *= 5;
                    else if (isEndOfSentence) currentDelay *= 15;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    currentDelay *= 8;
                }

                if (skipRequested) break;
                await Task.Delay(currentDelay);
            }
        }

        private async Task PopScaleAsync(Transform target)
        {
            float duration = 0.2f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float scale = 1f + Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.1f;
                target.localScale = originalSpeakerScale * scale;
                await Task.Yield();
            }
            target.localScale = originalSpeakerScale;
        }

        private async Task WaitForSecondsOrSkip(float seconds)
        {
            float elapsed = 0f;
            while (elapsed < seconds && !skipRequested)
            {
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            skipRequested = false; // Palette: Reset skip for the next dialogue beat
        }

        private string GetSpeakerColor(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => "#00FFFF",
                "King Cyrus" => "#FFFF00",
                "Reverie" => "#FF00FF",
                _ => "#FFFFFF"
            };
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
