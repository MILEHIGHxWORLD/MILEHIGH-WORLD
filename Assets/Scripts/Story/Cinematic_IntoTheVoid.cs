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
        private static MaterialPropertyBlock _propertyBlock;
        private MaterialPropertyBlock _propertyBlock;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
        private bool _skipRequested = false;
        private float _lastInteractionTime;
        private GameObject? _skipHint;

        // Palette: UX state for skip mechanics and idle hints
        private bool _skipRequested = false;
        private float _idleTimer = 0f;
        [SerializeField] private GameObject? skipHintObject;

        private void Start()
        {
            // Lock timeScale for deterministic cinematic pacing
            Time.timeScale = 1.0f;

            if (speakerNameText != null)
            {
                _originalSpeakerScale = speakerNameText.transform.localScale;
            }

            // Palette: Accessibility - Apply high-contrast black outlines to ensure readability.
            if (speakerNameText != null)
            {
                speakerNameText.outlineWidth = 0.2f;
                speakerNameText.outlineColor = Color.black;
            }
            if (dialogueText != null)
            {
                dialogueText.outlineWidth = 0.2f;
                dialogueText.outlineColor = Color.black;
            }

            TimelineSimulationEngine.OnTimelineStabilized += () => {
                _isStabilized = true;
                LogNarrativeTelemetry("EVENT: Timeline Stabilized Signal Received.");
            };

            // Palette: Search for SkipHint UI element within the dialogue canvas
            if (dialogueCanvas != null)
            {
                _skipHint = dialogueCanvas.transform.Find("SkipHint")?.gameObject;
                if (_skipHint != null) _skipHint.SetActive(false);
            }
            _lastInteractionTime = Time.time;

            _ = ExecuteConvergenceSequenceAsync();
        }

        private void Update()
        {
            // Palette: Global skip interaction and idle timer management
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _lastInteractionTime = Time.time;
                if (_skipHint != null) _skipHint.SetActive(false);
            }

            // Palette: Show skip hint after 2 seconds of inactivity
            if (_skipHint != null && !_skipHint.activeSelf && Time.time - _lastInteractionTime > 2f)
            {
                _skipHint.SetActive(true);
            // Palette: Detect any interaction to trigger skip or reset idle hint timer.
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _idleTimer = 0f;

                if (skipHintObject != null && skipHintObject.activeSelf)
                {
                    skipHintObject.SetActive(false);
                }
            }
            else
            {
                _idleTimer += Time.deltaTime;

                // Palette: Show skip hint after 2 seconds of inactivity to improve discoverability.
                if (_idleTimer >= 2.0f && skipHintObject != null && !skipHintObject.activeSelf)
                {
                    skipHintObject.SetActive(true);
                }
            }
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX - SECTOR 09-09-09");

            // 1. Force the local coordinate space into a Linear Omen (6.0) Hex-State
            await TweenShaderEntropyAsync(LinearOmenHexState, 2.0f);

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

        private static MaterialPropertyBlock _propertyBlock = new MaterialPropertyBlock();

        private async Task ExecuteSaveEveryoneProtocolAsync()
        {
            LogNarrativeTelemetry("PROTOCOL_SAVE_EVERYONE Initiated. Physics re-aligning.");

            if (kingCyrusPrefab != null)
            {
                var renderer = kingCyrusPrefab.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    // ⚡ Bolt: Use MaterialPropertyBlock instead of renderer.material to prevent GC allocations from material cloning and maintain SRP/GPU instancing batching.
                    // ⚡ Bolt: Use MaterialPropertyBlock to update alpha without instantiating a new material.
                    // This preserves draw call batching (SRP/GPU instancing) and eliminates GC allocations.
                    await TweenAlphaDecayAsync(renderer, 1.5f);
                }
                kingCyrusPrefab.SetActive(false);
            }

            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);
            LogNarrativeTelemetry("Omen Singularity Severed. Verse Stabilized.");
        }

        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;

            // ⚡ Bolt: Used MaterialPropertyBlock instead of Renderer.material to prevent material instantiation, GC allocations, and breaking draw call batching.
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        private async Task TweenAlphaDecayAsync(Renderer targetRenderer, float duration)
        {
            if (targetRenderer == null) return;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                renderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(propBlock);
                await Task.Yield();
            }
        }

        /// <summary>
        /// Zero-allocation rhythmic typewriter effect with themed completion cues, speaker pop animations, and skip support.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            // Palette: Reset interaction tracking for each line
            _skipRequested = false;
            _lastInteractionTime = Time.time;

            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
            }

            // Palette: Append a color-coded '▽' completion cue to the dialogue for better interaction clarity.
            // BOLT: Replaced .material with MaterialPropertyBlock to prevent runtime material instantiation and preserve draw call batching.
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;

            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
        // BOLT: Use MaterialPropertyBlock to prevent material cloning on the heap, eliminating GC allocations and preserving draw call batching.
        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;
        private async Task TweenAlphaDecayAsync(Renderer targetRenderer, float duration)
        {
            if (targetRenderer == null) return;

            // ⚡ Bolt: Replaced Renderer.material with MaterialPropertyBlock to prevent GC allocations and preserve GPU instancing.
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;

            if (_propertyBlock == null)
            {
                _propertyBlock = new MaterialPropertyBlock();
            }
        {
            if (renderer == null) return;
        private MaterialPropertyBlock _alphaPropBlock;

        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;

            // ⚡ Bolt: Cached MaterialPropertyBlock to prevent GC allocations and preserve GPU instancing.
            if (_alphaPropBlock == null)
            {
                _alphaPropBlock = new MaterialPropertyBlock();
            }

            renderer.GetPropertyBlock(_alphaPropBlock);
            // ⚡ Bolt: Use MaterialPropertyBlock to prevent material instantiation and preserve draw call batching.
            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            _propertyBlock ??= new MaterialPropertyBlock();

            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                propertyBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(propertyBlock);

                renderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(propBlock);

                targetRenderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(baseColorAlphaId, alpha);
                targetRenderer.SetPropertyBlock(propBlock);
                // ⚡ Bolt: Use MaterialPropertyBlock instead of Renderer.material to prevent material instantiation on the heap and preserve draw call batching (SRP/GPU instancing).
                if (_propertyBlock == null) _propertyBlock = new MaterialPropertyBlock();
                _alphaPropBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_alphaPropBlock);
                propertyBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(propertyBlock);
                propBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(propBlock);

                // ⚡ Bolt: Use MaterialPropertyBlock instead of Renderer.material.
                // What: Replaced direct material access with PropertyBlock.
                // Why: Accessing Renderer.material instantiates a material clone on the heap.
                // Impact: Prevents GC allocations per frame during the tween and preserves draw call batching (SRP/GPU instancing).
                renderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_propertyBlock);

                await Task.Yield();
            }
        }

        /// <summary>
        /// Zero-allocation rhythmic typewriter effect for dialogue rendering with rhythmic pacing and speaker transitions.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            // Palette: Reset skip request and idle timer at the start of each dialogue segment.
            _skipRequested = false;
            _idleTimer = 0f;

            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

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

            for (int i = 1; i <= characterCount; i++)
            {
                // Palette: Immediate reveal if skip is requested
                // Palette: Support instant skip to the end of the reveal.
                if (_skipRequested)
                {
                    dialogueText.maxVisibleCharacters = characterCount;
                    break;
                }

                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i < characterCount)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    float multiplier = 1f;

                    // Palette: Rhythmic punctuation pauses with look-ahead to handle mid-word periods (e.g., Sky.ix)
                    bool isEndOfSentence = (c == '.' || c == '?' || c == '!');
                    bool isClause = (c == ',' || c == ';' || c == ':');

                    if (isEndOfSentence || isClause)
                    {
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

            dialogueText.maxVisibleCharacters = characterCount;

            // Palette: Brief pause for reading, also skippable
            float pauseStart = Time.time;
            while (Time.time - pauseStart < 1.0f && !_skipRequested)
            {
                await Task.Yield();
            }

            _skipRequested = false; // Reset for next line
        }

        public Color GetSpeakerColor(string speaker)
        {
            return speaker switch
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
                "Sky.ix" => "#00FFFF",
                "King Cyrus" => "#FFFF00",
                "Reverie" => "#FF00FF",
                "Kai" => "#FFD700",
                "Delilah" => "#9933FF",
                _ => "#FFFFFF"
                "Sky.ix" => Color.cyan,
                "King Cyrus" => Color.yellow,
                "Reverie" => Color.magenta,
                "Kai" => new Color(1f, 0.84f, 0f), // Gold
                "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
                _ => Color.white
            };
        }

        /// <summary>
        /// Waits for the specified duration or until a skip is requested.
        /// </summary>
        private async Task WaitForSecondsOrSkipAsync(float seconds)
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
