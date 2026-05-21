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

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Palette: Speaker Colors & UI Cues
        private const string ColorCyrus = "#FF4500";
        private const string ColorSkyix = "#00FFFF";
        private const string ColorReverie = "#A855F7";
        private const string CompletionCue = "▽";

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;
        private const float IteratedSanctuary = 0.0777777777f;

        private bool _skipRequested;

        private void Update() { if (UnityEngine.Input.anyKeyDown) _skipRequested = true; }

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            // Palette: Apply high-contrast outlines for better accessibility/readability
            ApplyHighContrastOutline(speakerNameText);
            ApplyHighContrastOutline(dialogueText);

            _ = ExecuteConvergenceSequenceAsync();
        }

        private void ApplyHighContrastOutline(TextMeshProUGUI text)
        {
            if (text == null) return;
            text.fontMaterial.EnableKeyword("OUTLINE_ON");
            text.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.25f);
        private async Task WaitForSecondsOrSkip(float s)
        {
            float e = 0;
            while (e < s && !_skipRequested) { e += Time.deltaTime; await Task.Yield(); }
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
            await WaitForSecondsOrSkip(1.0f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. We are at 998 shards. Engaging Void Conduit.", 0.03f);
            await WaitForSecondsOrSkip(1.0f);

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
                await WaitForSecondsOrSkip(1.0f);
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
            await TweenAlphaDecayAsync(kingCyrusPrefab.GetComponentInChildren<Renderer>(), 1.5f);
            kingCyrusPrefab.SetActive(false); // Return to pool

            // Clamp environmental delta changes instantly upon loop completion
            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);

            LogNarrativeTelemetry("Omen Singularity Severed. Verse Stabilized.");
        }

        private static MaterialPropertyBlock? _alphaDecayPropertyBlock;

        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;
            if (_alphaDecayPropertyBlock == null) _alphaDecayPropertyBlock = new MaterialPropertyBlock();

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

                renderer.GetPropertyBlock(_alphaDecayPropertyBlock);
                _alphaDecayPropertyBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_alphaDecayPropertyBlock);

                await Task.Yield();
            }
        }

        /// <summary>
        /// Zero-allocation, rhythmic typewriter effect for dialogue rendering.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            // Palette: Speaker-specific color coding for better character identification
            string speakerColor = speaker switch
            {
                "King Cyrus" => "#FF4500", // OrangeRed
                "Sky.ix" => "#00FFFF",     // Cyan
                "Reverie" => "#A855F7",    // Purple
                _ => "#FFFFFF"
            };

            speakerNameText.text = $"<color={speakerColor}>[{speaker}]</color>";

            // Palette: Layout stability - set full text first and use maxVisibleCharacters
            // Pre-append a themed completion cue for interaction clarity
            dialogueText.text = $"{content} <color={speakerColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int visibleChars = content.Length;
            for (int i = 0; i < visibleChars; i++)
            {
                dialogueText.maxVisibleCharacters = i + 1;

                // Palette: Rhythmic punctuation pauses to mimic natural speech cadence
                char c = content[i];
                float delayFactor = 1.0f;

                if (c == '.' || c == '!' || c == '?') delayFactor = 15f;
                else if (c == ',' || c == ':') delayFactor = 8f;

                await Task.Delay(Mathf.RoundToInt(charDelay * 1000 * delayFactor));
            }

            // Reveal the completion cue
            dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
        /// Rhythmic typewriter effect with layout stability and speaker-themed cues.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string speakerColor = speaker switch
            {
                "Sky.ix" => "cyan",
                "King Cyrus" => "yellow",
                "Reverie" => "magenta",
                _ => "white"
            };

            speakerNameText.text = $"<color={speakerColor}>[{speaker}]</color>";

            // Layout stability: Set full text (with cue) to pre-calculate wrapping
            dialogueText.text = $"{content} <color={speakerColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisible = dialogueText.textInfo.characterCount;

            for (int i = 0; i <= totalVisible; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i < totalVisible)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float multiplier = 1f;

                    // Rhythmic Pacing: Sentence ends (15x), clauses (8x), ellipses (5x)
                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEllipsis = i < totalVisible && dialogueText.textInfo.characterInfo[i].character == '.';
                        multiplier = isEllipsis ? 5f : 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
        /// Zero-allocation typewriter effect for dialogue rendering with rhythmic pacing.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            _skipRequested = false;
            string color = speaker == "Sky.ix" ? "cyan" : speaker == "King Cyrus" ? "yellow" : "magenta";
            speakerNameText.text = $"<color={color}>[{speaker}]</color>";
            dialogueText.text = $"{content} <color={color}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            for (int i = 0; i <= dialogueText.textInfo.characterCount; i++)
            {
                if (_skipRequested) break;
                dialogueText.maxVisibleCharacters = i;
                float m = 1f;
                if (i > 0 && i < dialogueText.textInfo.characterCount) {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    m = (c == '.' || c == '?' || c == '!') ? 15f : (c == ',' || c == ':' || c == ';') ? 8f : 1f;
                }
                await Task.Delay(Mathf.RoundToInt(charDelay * 1000 * m));
            }
            dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
        /// Zero-allocation typewriter effect for dialogue rendering with rhythmic pacing and layout stability.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string speakerColor = speaker switch
            {
                "King Cyrus" => ColorCyrus,
                "Sky.ix" => ColorSkyix,
                "Reverie" => ColorReverie,
                _ => "#FFFFFF"
            };

            speakerNameText.text = $"<color={speakerColor}>[{speaker}]</color>";

            // Palette: Set full text (including cue) at start to stabilize layout, then reveal via maxVisibleCharacters.
            dialogueText.text = $"{content} <color={speakerColor}>{CompletionCue}</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();
        /// Zero-allocation typewriter effect for dialogue rendering using maxVisibleCharacters.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            speakerNameText.text = $"<color=cyan>[{speaker}]</color>";
            dialogueText.text = content;
            dialogueText.maxVisibleCharacters = 0;

            // ⚡ Bolt: Eliminate O(N^2) string allocations and mesh rebuilds by controlling visibility.
            for (int i = 1; i <= content.Length; i++)

            // ⚡ Bolt: Zero-allocation typewriter effect.
            dialogueText.text = content;
            dialogueText.maxVisibleCharacters = 0;

            for (int i = 1; i <= content.Length; i++)
            {
                dialogueText.maxVisibleCharacters = i;
            dialogueText.text = content;
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = content.Length;
            for (int i = 0; i <= totalCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

            int totalCharacters = dialogueText.textInfo.characterCount;
            // Subtract 1 for the completion cue
            int messageLength = totalCharacters - 1;

            for (int i = 0; i <= messageLength; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= messageLength)
                {
                    // Palette: Use textInfo.characterInfo to correctly identify the revealed character even with rich text tags.
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float multiplier = 1f;

                    // Palette: Rhythmic pacing - check for sentence ends and clauses.
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Lookahead for Sky.ix or ellipsis - check next visible character if available
                        bool isMidWord = (i < messageLength && !char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));
                        if (!isMidWord) multiplier = 15f;
                    }
                    else if (c == ',' || c == ':')
                    {
                        multiplier = 8f;
                    }

                    await Task.Delay(Mathf.RoundToInt(charDelay * 1000 * multiplier));
                    await Task.Delay(Mathf.RoundToInt(charDelay * multiplier * 1000));
                }
                else
                {
                    await Task.Delay(Mathf.RoundToInt(charDelay * 1000));
                }
            }

            // Reveal the completion cue
            dialogueText.maxVisibleCharacters = totalCharacters;
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
