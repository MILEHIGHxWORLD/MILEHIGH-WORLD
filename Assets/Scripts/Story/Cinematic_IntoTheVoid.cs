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
        private MaterialPropertyBlock? _alphaPropBlock;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
        private bool _skipRequested = false;
        private float _lastInteractionTime;
        private GameObject? _foundSkipHint;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            _alphaPropBlock = new MaterialPropertyBlock();

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

            TimelineSimulationEngine.OnTimelineStabilized += () => {
                _isStabilized = true;
                LogNarrativeTelemetry("EVENT: Timeline Stabilized Signal Received.");
            };

            // Palette: Search for SkipHint UI element within the dialogue canvas
            if (dialogueCanvas != null)
            {
                _foundSkipHint = dialogueCanvas.transform.Find("SkipHint")?.gameObject;
                if (_foundSkipHint != null) _foundSkipHint.SetActive(false);
            }
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

                if (_foundSkipHint != null && _foundSkipHint.activeSelf) _foundSkipHint.SetActive(false);
                if (skipHint != null && skipHint.activeSelf) skipHint.SetActive(false);
            }
            else
            {
                // Palette: Show the skip hint only after 2 seconds of inactivity to maintain immersion.
                if (Time.time - _lastInteractionTime >= 2.0f)
                {
                    if (_foundSkipHint != null && !_foundSkipHint.activeSelf) _foundSkipHint.SetActive(true);
                    if (skipHint != null && !skipHint.activeSelf) skipHint.SetActive(true);
                }
            }
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX - SECTOR 09-09-09");

            // 1. Force the local coordinate space into a Linear Omen (6.0) Hex-State
            await TweenShaderEntropyAsync(LinearOmenHexState, 2.0f);

            // 2. Transfinite Data Load: Initialize entities from object pools
            if (skyixPrefab != null)
            {
                skyixPrefab.SetActive(true);
            }
            if (reveriePrefab != null)
            {
                reveriePrefab.SetActive(true);
            }
            if (kingCyrusPrefab != null)
            {
                kingCyrusPrefab.SetActive(true);
            }

            // 3. Asynchronous Lexical Pacing
            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(true);
            }

            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", baseTypingSpeed);
            await WaitForSecondsOrSkipAsync(0.5f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. Engaging Void Conduit via Vitis AI Bridge.", baseTypingSpeed);

            // 4. Parity Verification via Vitis AI and Timeline Engine
            LogNarrativeTelemetry("Executing Vitis AI Bridge Analysis: Calculating System Tension...");

            // Register final shards to reach parity
            if (timelineEngine != null)
            {
                for (int i = 0; i < 999; i++)
                {
                    timelineEngine.RegisterSynchronizedShard();
                }
            }

            double tension = vitisBridge != null ? vitisBridge.CalculateSystemTension() : 0.0;
            if (timelineEngine != null)
            {
                timelineEngine.EvaluateSystemTension(tension);
            }

            bool isRealityFractured = timelineEngine != null && timelineEngine.IsRealityFractured;

            if (_isStabilized && !isRealityFractured)
            {
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. System tension within limits. Severing the loop... now!", baseTypingSpeed);
                await ExecuteSaveEveryoneProtocolAsync();
            }
            else
            {
                string reason = isRealityFractured ? "Structural Reality Fracture" : "Parity Synchronization Failure";
                LogNarrativeTelemetry($"WARNING: Convergence Failed. Reason: {reason}");
                await StreamDialogueAsync("King Cyrus", "Your reality is too brittle for this power!", baseTypingSpeed);
            }

            if (dialogueCanvas != null)
            {
                dialogueCanvas.SetActive(false);
            }
        }

        private async Task TweenShaderEntropyAsync(float targetIntensity, float duration)
        {
            if (hyperrealisticPlatformShader == null)
            {
                return;
            }

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
                kingCyrusPrefab.SetActive(false); // Return to pool
            }

            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);
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

                // ⚡ Bolt: Use MaterialPropertyBlock to prevent material cloning on the heap and GC allocations,
                // preserving draw call batching (GPU instancing/SRP batcher).
                renderer.GetPropertyBlock(_alphaPropBlock);
                _alphaPropBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_alphaPropBlock);

                await Task.Yield();
            }
        }

        private string GetSpeakerColorHex(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => "#00FFFF",
                "King Cyrus" => "#FFFF00",
                "Reverie" => "#FF00FF",
                "Kai" => "#FFD700",
                "Delilah" => "#9933FF",
                _ => "#FFFFFF"
            };
        }

        public Color GetSpeakerColor(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => Color.cyan,
                "King Cyrus" => Color.yellow,
                "Reverie" => Color.magenta,
                "Kai" => new Color(1f, 0.84f, 0f), // Gold
                "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
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

        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            // Reset skip state and idle timer for the new segment
            _skipRequested = false;
            _lastInteractionTime = Time.time;
            if (_foundSkipHint != null) _foundSkipHint.SetActive(false);
            if (skipHint != null) skipHint.SetActive(false);

            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
            }

            // Pre-append completion cue to avoid layout jumps / word-wrap shifts
            dialogueText.text = $"{content} <color={hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;
            // Subtracting 1 to exclude the ▽ cue from typing delay pacing
            int dialogueLength = totalCharacters - 1;

            float speedMultiplier = GetSpeedMultiplier(speaker);
            float effectiveDelay = charDelay / speedMultiplier;

            for (int i = 0; i <= dialogueLength; i++)
            {
                if (_skipRequested)
                {
                    break;
                }

                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= dialogueLength)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float pauseMultiplier = 1.0f;

                    // Look-ahead to avoid pausing on abbreviations/technical names (no trailing whitespace)
                    bool isTechnicalName = i < dialogueLength && !char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character);

                    if (!isTechnicalName)
                    {
                        if (".!?".Contains(c))
                        {
                            pauseMultiplier = 15.0f;
                        }
                        else if (",:;".Contains(c))
                        {
                            pauseMultiplier = 8.0f;
                        }
                    }

                    // Ellipsis handling
                    if (c == '.' && i > 1 && dialogueText.textInfo.characterInfo[i - 2].character == '.')
                    {
                        pauseMultiplier = 5.0f;
                    }

                    await Task.Delay(Mathf.RoundToInt(effectiveDelay * pauseMultiplier * 1000f));
                }
                else
                {
                    await Task.Delay(Mathf.RoundToInt(effectiveDelay * 1000f));
                }
            }

            // Complete reveal (including ▽ cue)
            dialogueText.maxVisibleCharacters = totalCharacters;

            // Carry skip intent to the reading pause
            if (!_skipRequested)
            {
                await WaitForSecondsOrSkipAsync(1.0f);
            }
        }

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            if (target == null) return;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;
                float scale = 1.0f + Mathf.Sin(progress * Mathf.PI) * (scaleFactor - 1.0f);
                target.localScale = _originalSpeakerScale * scale;
                await Task.Yield();
            }
            target.localScale = _originalSpeakerScale;
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

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
