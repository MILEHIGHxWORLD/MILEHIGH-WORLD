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

        private bool _isStabilized = false;
        private bool _isTyping;
        private bool _skipTypingRequested;
        private bool _skipReadingRequested;
        private bool skipRequested = false;
        private float idleTimer = 0f;
        private bool playerInteracted = false;
        private Vector3 originalSpeakerScale;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            TimelineSimulationEngine.OnTimelineStabilized += () => {
                _isStabilized = true;
                LogNarrativeTelemetry("EVENT: Timeline Stabilized Signal Received.");
            };
            // Palette: Accessibility - Apply high-contrast outlines for better readability
            speakerNameText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
            speakerNameText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            dialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
            dialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            if (speakerNameText != null)
            {
                originalSpeakerScale = speakerNameText.transform.localScale;
                ApplyHighContrastOutline(speakerNameText);
            }

            if (dialogueText != null)
            {
                ApplyHighContrastOutline(dialogueText);
            }

            if (skipHint != null) skipHint.SetActive(false);

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
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                playerInteracted = true;
                idleTimer = 0f;
                if (skipHint != null) skipHint.SetActive(false);

                if (_isTyping)
                {
                    _skipTypingRequested = true; // State 1: Finish typing immediately
                }
                else
                {
                    _skipReadingRequested = true; // State 2: Advance to next line
                }
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

            // 2. Transfinite Data Load: Initialize entities from object pools
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
            await WaitForAdvanceOrSkipAsync(0.5f);
            await WaitForSecondsOrSkip(0.5f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. We are at 998 shards. Engaging Void Conduit.", 0.03f);
            await WaitForAdvanceOrSkipAsync(0.5f);

            double tension = vitisBridge.CalculateSystemTension();
            timelineEngine.EvaluateSystemTension(tension);

            if (_isStabilized && !timelineEngine.IsRealityFractured)
            {
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. System tension within limits. Severing the loop... now!", 0.03f);
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. Severing the loop... now!", 0.03f);
                await WaitForAdvanceOrSkipAsync(0.5f);
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

        private async Task TweenShaderEntropyAsync(float targetIntensity, float duration)
        {
            float startIntensity = hyperrealisticPlatformShader.GetFloat(emissiveIntensityId);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float currentIntensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                hyperrealisticPlatformShader.SetFloat(emissiveIntensityId, currentIntensity);
                await Task.Yield();
            }
        }

        private async Task ExecuteSaveEveryoneProtocolAsync()
        {
            LogNarrativeTelemetry("PROTOCOL_SAVE_EVERYONE Initiated. Physics re-aligning.");

            await TweenAlphaDecayAsync(kingCyrusPrefab.GetComponentInChildren<Renderer>().material, 1.5f);
            kingCyrusPrefab.SetActive(false);

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
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            speakerNameText.text = $"<color=cyan>[{speaker}]</color>";
            dialogueText.text = "";

            for (int i = 0; i < content.Length; i++)
            {
                dialogueText.text += content[i];
        /// Zero-allocation typewriter effect for dialogue rendering with rhythmic pacing and themed cues.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            // Palette: Speaker-specific color coding and themed completion cues
            string speakerColor = speaker switch
            {
                "Sky.ix" => "cyan",
                "King Cyrus" => "yellow",
                "Reverie" => "magenta",
                _ => "white"
            };

            speakerNameText.text = $"<color={speakerColor}>[{speaker}]</color>";

            // Palette: Layout-safe reveal by pre-setting text and using maxVisibleCharacters
            dialogueText.text = $"{content} <color={speakerColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                // Palette: Rhythmic punctuation pacing
                float delayMultiplier = 1f;
                if (i < totalCharacters)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    char next = (i < totalCharacters) ? dialogueText.textInfo.characterInfo[i].character : '\0';

                    if ((c == '.' || c == '!' || c == '?') && (char.IsWhiteSpace(next) || i == totalCharacters - 1))
                        delayMultiplier = 15f; // Sentence end or end of content
                    else if (c == ',' || c == ':' || c == ';')
                        delayMultiplier = 8f;  // Clause pause
                    else if (c == '.' && next == '.')
                        delayMultiplier = 5f;  // Ellipsis dot
                }

                await Task.Delay(Mathf.RoundToInt(charDelay * delayMultiplier * 1000));
        /// Rhythmic typewriter effect for dialogue rendering with speaker-themed cues.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string color = speaker switch
            {
                "Sky.ix" => "cyan",
                "King Cyrus" => "yellow",
                "Reverie" => "magenta",
                _ => "white"
            };

            speakerNameText.text = $"<color={color}>[{speaker}]</color>";

            // Layout stability: set full text (with cue) and reveal characters
            dialogueText.text = $"{content} <color={color}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = dialogueText.textInfo.characterCount;
            int baseDelayMs = Mathf.RoundToInt(charDelay * 1000);

            for (int i = 1; i <= totalVisibleCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i == totalVisibleCharacters) break;

                int currentDelay = baseDelayMs;

                // Rhythmic Pacing: look at the character we just revealed
                var charInfo = dialogueText.textInfo.characterInfo[i - 1];
                char c = charInfo.character;

                if (c == '.' || c == '?' || c == '!')
                {
                    // Look ahead at the next rendered character to distinguish sentence ends from abbreviations
                    char nextChar = dialogueText.textInfo.characterInfo[i].character;
                    if (char.IsWhiteSpace(nextChar))
                        currentDelay *= 15; // Full stop
                }
                else if (c == ',' || c == ':' || c == ';')
                {
                    currentDelay *= 8; // Brief pause
                }

        /// Zero-allocation typewriter effect with rhythmic pacing and character-themed cues.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            _isTyping = true;
            _skipTypingRequested = false;
            _skipReadingRequested = false;

            string colorHex = GetSpeakerColor(speaker);
            string formattedSpeaker = $"<color={colorHex}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform);
            }


            // Palette: Reset skip flag and interaction state for each new dialogue line.
            skipRequested = false;
            playerInteracted = false;
            idleTimer = 0f;

            // Pre-calculate layout with completion cue to avoid jarring shifts
            dialogueText.text = $"{content} <color={colorHex}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;
            int baseDelayMs = Mathf.RoundToInt(charDelay * 1000);

                // Base-9 Frame Parity Alignment: Yield heavily on 9th iterations if needed,
                // but for lexical pacing, we use a scaled delay.
                await Task.Delay(Mathf.RoundToInt(charDelay * 1000));
            for (int i = 1; i <= totalCharacters; i++)
            {
                if (_skipTypingRequested)
                {
                    dialogueText.maxVisibleCharacters = totalCharacters;
                    break;
                }

                dialogueText.maxVisibleCharacters = i;

                // Get the character that was just revealed
                char c = dialogueText.textInfo.characterInfo[i - 1].character;
                int currentDelay = baseDelayMs;

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

                await Task.Delay(currentDelay);
            }

            _isTyping = false;
        }

        private async Task PopScaleAsync(Transform target)
        {
            if (target == null) return;
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

        private async Task WaitForAdvanceOrSkipAsync(float minSeconds)
        {
            float elapsed = 0f;
            // Wait at least minSeconds, or until player requests skip advance
            while (elapsed < minSeconds && !_skipReadingRequested)
            {
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            _skipReadingRequested = false;
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
