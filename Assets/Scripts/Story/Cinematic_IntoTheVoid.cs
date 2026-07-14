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
        [SerializeField] private GameObject skipHint = null!;

        [Header("Lexical Tuning")]
        public float baseTypingSpeed = 0.03f;
        public float kaiSpeedMultiplier = 3.0f;
        public float skyixSpeedMultiplier = 1.2f;

        // Properties for Unit Testing
        public TextMeshProUGUI SpeakerNameText { get => speakerNameText; set => speakerNameText = value; }
        public TextMeshProUGUI DialogueText { get => dialogueText; set => dialogueText = value; }
        public GameObject DialogueBox { get => dialogueCanvas; set => dialogueCanvas = value; }

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");
        private MaterialPropertyBlock _alphaPropBlock = null!;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
        private bool _skipRequested = false;
        private float _lastInteractionTime;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            if (_alphaPropBlock == null) _alphaPropBlock = new MaterialPropertyBlock();

            if (speakerNameText != null)
            {
                _originalSpeakerScale = speakerNameText.transform.localScale;
                // Palette: Accessibility - Apply high-contrast black outlines to ensure readability.
                speakerNameText.outlineWidth = 0.2f;
                speakerNameText.outlineColor = Color.black;
            }

            if (dialogueText != null)
            {
                dialogueText.outlineWidth = 0.2f;
                dialogueText.outlineColor = Color.black;
            }

            if (skipHint != null) skipHint.SetActive(false);
            else if (dialogueCanvas != null)
            {
                // Fallback: Programmatic search for SkipHint if not assigned in Inspector
                var foundHint = dialogueCanvas.transform.Find("SkipHint")?.gameObject;
                if (foundHint != null) skipHint = foundHint;
            }

            TimelineSimulationEngine.OnTimelineStabilized += () => {
                _isStabilized = true;
                LogNarrativeTelemetry("EVENT: Timeline Stabilized Signal Received.");
            };

            _lastInteractionTime = Time.time;
            _ = ExecuteConvergenceSequenceAsync();
        }

        private void Update()
        {
            // Palette: Capture any user interaction to trigger a skip or reset the idle timer.
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _lastInteractionTime = Time.time;

                if (skipHint != null && skipHint.activeSelf) skipHint.SetActive(false);
            }
            else
            {
                // Palette: Show the skip hint only after 2 seconds of inactivity to maintain immersion.
                if (skipHint != null && !skipHint.activeSelf && Time.time - _lastInteractionTime > 2f)
                {
                    skipHint.SetActive(true);
                }
            }
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX - SECTOR 09-09-09");

            // 1. Force the local coordinate space into a Linear Omen (6.0) Hex-State
            await TweenShaderEntropyAsync(6.0f, 2.0f);

            // 2. Transfinite Data Load: Initialize entities from object pools
            if (skyixPrefab != null) skyixPrefab.SetActive(true);
            if (reveriePrefab != null) reveriePrefab.SetActive(true);
            if (kingCyrusPrefab != null) kingCyrusPrefab.SetActive(true);

            // 3. Asynchronous Lexical Pacing
            if (dialogueCanvas != null) dialogueCanvas.SetActive(true);

            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", 0.04f);
            await WaitForSecondsOrSkipAsync(0.5f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. Engaging Void Conduit via Vitis AI Bridge.", 0.03f);

            // 4. Parity Verification via Vitis AI and Timeline Engine
            LogNarrativeTelemetry("Executing Vitis AI Bridge Analysis: Calculating System Tension...");

            for (int i = 0; i < 999; i++)
            {
                timelineEngine.RegisterSynchronizedShard();
            }

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

            if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
        }

        private async Task TweenShaderEntropyAsync(float targetIntensity, float duration)
        {
            if (hyperrealisticPlatformShader == null) return;

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

            if (kingCyrusPrefab != null)
            {
                Renderer cyrusRenderer = kingCyrusPrefab.GetComponentInChildren<Renderer>();
                if (cyrusRenderer != null)
                {
                    await TweenAlphaDecayAsync(cyrusRenderer, 1.5f);
                }
                kingCyrusPrefab.SetActive(false);
            }

            await TweenShaderEntropyAsync(1.0f, 1.0f);
            LogNarrativeTelemetry("Omen Singularity Severed. Verse Stabilized.");
        }

        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;
            if (_alphaPropBlock == null) _alphaPropBlock = new MaterialPropertyBlock();

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

                renderer.GetPropertyBlock(_alphaPropBlock);
                _alphaPropBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_alphaPropBlock);

                await Task.Yield();
            }
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

        public Color GetSpeakerColor(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => Color.cyan,
                "Kai" => new Color(1f, 0.84f, 0f), // Gold
                "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
                "King Cyrus" => new Color(1f, 0.27f, 0f), // OrangeRed
                "Reverie" => new Color(0.66f, 0.33f, 0.97f), // MediumPurple
                _ => Color.white
            };
        }

        private string GetSpeakerColorHex(string speaker)
        {
            return ColorUtility.ToHtmlStringRGB(GetSpeakerColor(speaker));
        }

        /// <summary>
        /// Zero-allocation rhythmic typewriter effect with themed completion cues, speaker pop animations, and skip support.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            _skipRequested = false;
            _lastInteractionTime = Time.time;
            if (skipHint != null) skipHint.SetActive(false);

            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color=#{hexColor}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
            }

            // BOLT: Zero-allocation typewriter effect.
            // Palette: Pre-append completion cue for layout stability.
            dialogueText.text = $"{content} <color=#{hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;
            int dialogueLength = totalCharacters - 1;

            float multiplier = GetSpeedMultiplier(speaker);
            float effectiveDelay = (charDelay / multiplier);

            for (int i = 1; i <= dialogueLength; i++)
            {
                if (_skipRequested) break;

                dialogueText.maxVisibleCharacters = i;

                char c = dialogueText.textInfo.characterInfo[i - 1].character;
                float pauseMultiplier = 1f;

                // Palette: Rhythmic pacing with look-ahead to ignore technical names
                bool isTechnicalName = (i < dialogueLength && !char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));

                if (!isTechnicalName)
                {
                    if (".!?".Contains(c)) pauseMultiplier = 15f;
                    else if (",:".Contains(c)) pauseMultiplier = 8f;
                }

                // Ellipsis handling
                if (c == '.' && i > 1 && dialogueText.textInfo.characterInfo[i - 2].character == '.')
                    pauseMultiplier = 5f;

                await Task.Delay(Mathf.RoundToInt(effectiveDelay * pauseMultiplier * 1000));
            }

            dialogueText.maxVisibleCharacters = totalCharacters;

            if (!_skipRequested)
            {
                await WaitForSecondsOrSkipAsync(1.0f);
            }
        }

        private async Task WaitForSecondsOrSkipAsync(float seconds)
        {
            float elapsed = 0f;
            while (elapsed < seconds && !_skipRequested)
            {
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            _skipRequested = false;
        }

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            if (target == null) return;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float ratio = elapsed / duration;
                float s = 1f + Mathf.Sin(ratio * Mathf.PI) * (scaleFactor - 1f);
                target.localScale = _originalSpeakerScale * s;
                await Task.Yield();
            }
            target.localScale = _originalSpeakerScale;
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
