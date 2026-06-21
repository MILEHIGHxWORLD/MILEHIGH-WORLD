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

        // Properties for Unit Testing
        public TextMeshProUGUI SpeakerNameText { get => speakerNameText; set => speakerNameText = value; }
        public TextMeshProUGUI DialogueText { get => dialogueText; set => dialogueText = value; }
        public GameObject DialogueBox { get => dialogueCanvas; set => dialogueCanvas = value; }
        public float baseTypingSpeed { get; set; } = 0.03f;
        public float kaiSpeedMultiplier { get; set; } = 3.0f;
        public float skyixSpeedMultiplier { get; set; } = 1.2f;

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");
        private MaterialPropertyBlock _alphaPropBlock = null!;

        private static MaterialPropertyBlock _propertyBlock;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
        private bool _skipRequested = false;
        private float _lastInteractionTime;
        private GameObject? _skipHint;

        // Palette: UX State for skipping and idle-hint discoverability.
        private bool _skipRequested = false;
        private float _idleTimer = 0f;

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
                _skipHint = dialogueCanvas.transform.Find("SkipHint")?.gameObject;
                if (_skipHint != null) _skipHint.SetActive(false);
            }
            _lastInteractionTime = Time.time;

            _ = ExecuteConvergenceSequenceAsync();
        }

        private void Update()
        {
            // ⚡ Bolt: Consolidated redundant Input.anyKeyDown checks.
            // What: Eliminated duplicate Input.anyKeyDown checks and merged idle timers.
            // Why: Multiple Input.anyKeyDown checks in Update introduce unnecessary C#/C++ native boundary crossings.
            // Impact: Reduces CPU overhead per frame by eliminating duplicate native execution paths.
            // Palette: Capture any user interaction to trigger a skip or reset the idle timer.
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _idleTimer = 0f;
                _lastInteractionTime = Time.time;

                if (_skipHint != null && _skipHint.activeSelf) _skipHint.SetActive(false);
                if (skipHintObject != null && skipHintObject.activeSelf) skipHintObject.SetActive(false);
                if (skipHint != null) skipHint.SetActive(false);
            }
            else
            {
                _idleTimer += Time.deltaTime;

                if (Time.time - _lastInteractionTime > 2f)
                {
                    if (_skipHint != null && !_skipHint.activeSelf) _skipHint.SetActive(true);
                }

                if (_idleTimer >= 2.0f && skipHintObject != null && !skipHintObject.activeSelf)
                {
                    skipHintObject.SetActive(true);
                }
                // Palette: Show the skip hint only after 2 seconds of inactivity to maintain immersion.
                if (_idleTimer >= 2f && skipHint != null && !skipHint.activeSelf)
                {
                    skipHint.SetActive(true);
                }
            // Palette: Global skip interaction and idle timer management
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _lastInteractionTime = Time.time;
                if (_skipHint != null && _skipHint.activeSelf) _skipHint.SetActive(false);
            }

            // Palette: Show skip hint after 2 seconds of inactivity
            if (_skipHint != null && !_skipHint.activeSelf && Time.time - _lastInteractionTime > 2f)
            {
                _skipHint.SetActive(true);
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

            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", 0.04f);
            await WaitForSecondsOrSkipAsync(0.5f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. Engaging Void Conduit via Vitis AI Bridge.", 0.03f);

            // 4. Parity Verification via Vitis AI and Timeline Engine
            LogNarrativeTelemetry("Executing Vitis AI Bridge Analysis: Calculating System Tension...");

            // Register final shards to reach parity
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
                var renderer = kingCyrusPrefab.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    // ⚡ Bolt: Use MaterialPropertyBlock to update alpha without instantiating a new material.
                    await TweenAlphaDecayAsync(renderer, 1.5f);
                }
                kingCyrusPrefab.SetActive(false);
            }

            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);
            LogNarrativeTelemetry("Omen Singularity Severed. Verse Stabilized.");
        }

        // ⚡ Bolt: Use MaterialPropertyBlock to prevent material cloning on the heap and GC allocations,
        // preserving draw call batching (GPU instancing/SRP batcher).
        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;

            if (_propertyBlock == null)
            {
                _propertyBlock = new MaterialPropertyBlock();
            }
        private async Task TweenAlphaDecayAsync(Renderer targetRenderer, float duration)
        {
            if (targetRenderer == null) return;

            // ⚡ Bolt Optimization
            // 💡 What: Replaced direct Material.SetFloat with MaterialPropertyBlock usage.
            // 🎯 Why: Accessing targetRenderer.material instantiates a clone of the material on the heap, breaking GPU instancing and causing GC allocations.
            // 📊 Impact: Eliminates O(1) Material allocation per character decay sequence, preserving draw call batching and reducing GC pressure.
            var propBlock = new MaterialPropertyBlock();
        // Bolt Optimization: Replaced Renderer.material with MaterialPropertyBlock to prevent GC allocation and preserve GPU instancing.
        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (mat == null)
            {
                return;
            }
            if (renderer == null) return;

            var propBlock = new MaterialPropertyBlock();
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);

                renderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_propertyBlock);
                targetRenderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(baseColorAlphaId, alpha);
                targetRenderer.SetPropertyBlock(propBlock);
                renderer.GetPropertyBlock(propBlock);
                propBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(propBlock);
                renderer.GetPropertyBlock(_alphaPropBlock);
                _alphaPropBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_alphaPropBlock);

                await Task.Yield();
            }
        }

        /// <summary>
        /// Zero-allocation rhythmic typewriter effect with themed completion cues and speaker pop animations.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string speakerColor = speaker switch
            {
                "King Cyrus" => "#FF4500",
                "Sky.ix" => "#00FFFF",
                "Reverie" => "#A855F7",
                _ => "#FFFFFF"
            };

            speakerNameText.text = $"<color={speakerColor}>[{speaker}]</color>";
            string fullContent = content + $" <color={speakerColor}>▽</color>";
            dialogueText.text = fullContent;
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            for (int i = 0; i <= content.Length; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                if (i > 0 && i < content.Length)
                {
                    char c = content[i - 1];
                    float delayFactor = 1.0f;

                    // Palette: Rhythmic pacing - pause for punctuation
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Look-ahead to avoid pausing on abbreviations (e.g., Sky.ix)
                        if (i == content.Length || char.IsWhiteSpace(content[i]))
                            delayFactor = 15.0f;
                    }
                    else if (c == ',' || c == ':')
                    {
                        delayFactor = 8.0f;
                    }

                    await Task.Delay(Mathf.RoundToInt(charDelay * delayFactor * 1000));
            speakerNameText.text = $"<color=cyan>[{speaker}]</color>";
            // ⚡ Bolt: Assign text once and use maxVisibleCharacters to avoid O(N^2) string allocations
            dialogueText.text = content;
            dialogueText.maxVisibleCharacters = 0;

            int delayMs = Mathf.RoundToInt(charDelay * 1000);

            for (int i = 0; i <= content.Length; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                // Base-9 Frame Parity Alignment: Yield heavily on 9th iterations if needed,
                // but for lexical pacing, we use a scaled delay.
                if (i < content.Length)
                {
                    await Task.Delay(delayMs);
                }
            dialogueText.text = content;
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = content.Length;

            for (int i = 0; i <= totalVisibleCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                int delay = Mathf.RoundToInt(charDelay * 1000);

                // Lexical Pacing: Apply multipliers for punctuation to simulate natural speech cadence.
                if (i > 0)
                {
                    char c = content[i - 1];

                    // ⚡ Bolt: Allocation-free look-ahead logic to skip delays for names like 'Sky.ix' (period in name).
                    bool isSkyixPeriod = (c == '.' && i >= 4 && i + 2 <= totalVisibleCharacters &&
                                          content[i-4] == 'S' && content[i-3] == 'k' && content[i-2] == 'y' &&
                                          content[i] == 'i' && content[i+1] == 'x');

                    if (!isSkyixPeriod)
                    {
                        if (c == '.' || c == '!' || c == '?') delay *= 15;
                        else if (c == ',' || c == ':') delay *= 8;
                    }
                }

                // Base-9 Frame Parity Alignment: Yield heavily on 9th iterations if needed.
                await Task.Delay(delay);
            if (speakerNameText == null || dialogueText == null)
            {
                return;
            }

            string colorHex = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={colorHex}>[{speaker}]</color>";
        /// Zero-allocation rhythmic typewriter effect for dialogue rendering with rhythmic pacing and speaker transitions.
        /// Zero-allocation rhythmic typewriter effect with themed completion cues, speaker pop animations, and skip support.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            // Palette: Reset skip state and idle timer for the new dialogue segment.
            _skipRequested = false;
            _idleTimer = 0f;
            if (skipHint != null) skipHint.SetActive(false);
            // Palette: Reset interaction tracking for each line
            _skipRequested = false;
            _lastInteractionTime = Time.time;
            if (_skipHint != null) _skipHint.SetActive(false);

            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
            }

            // BOLT: Zero-allocation typewriter effect.
            dialogueText.text = $"{content} <color={colorHex}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            // Palette: Append a color-coded '▽' completion cue to the dialogue for better interaction clarity.
            // By setting the full text (including the cue) at the start, we ensure layout stability.
            // BOLT: Zero-allocation typewriter effect.
            dialogueText.text = $"{content} <color={hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalVisibleCharacters; i++)
            {
                if (_skipRequested) break;

                dialogueText.maxVisibleCharacters = i;

                // Palette: Rhythmic pacing - apply multipliers for punctuation to mimic natural speech cadence.
                float currentDelay = charDelay;
                if (i < totalVisibleCharacters)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    bool isEndOfSentence = (c == '.' || c == '!' || c == '?');
                    bool isPause = (c == ',' || c == ':' || c == ';');

                    if (isEndOfSentence)
                    {
                        // Palette: Advanced pacing - detect ellipses (...) and apply a shorter pause (5x).
                        bool isEllipsis = (i < totalVisibleCharacters && dialogueText.textInfo.characterInfo[i].character == '.');
                        if (isEllipsis)
                        {
                            currentDelay *= 5f;
                        }
                        else
                        {
                            // Look ahead: only long pause if followed by a space or it's the last character before the cue
                            bool nextIsSpace = (i < totalVisibleCharacters && char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));
                            if (nextIsSpace || i == totalVisibleCharacters - 1) currentDelay *= 12f;
                        }
                    }
                    else if (isPause)
                    {
                        currentDelay *= 6f;
                    }
                }

                await Task.Delay(Mathf.RoundToInt(currentDelay * 1000));
            }

            // BOLT: Explicitly reset maxVisibleCharacters to the full length to ensure stability for future reuse.
            dialogueText.maxVisibleCharacters = totalVisibleCharacters;

            // Palette: Carry skip intent to the inter-line pause for better responsiveness.
            if (!_skipRequested)
            {
                await WaitForSecondsOrSkip(1.0f);
            }
        }

        private async Task WaitForSecondsOrSkip(float seconds)
        {
            float elapsed = 0f;
            while (elapsed < seconds && !_skipRequested)
            {
                elapsed += Time.deltaTime;
                await Task.Yield();
            }
            int characterCount = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= characterCount; i++)
            {
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

                    bool isEndOfSentence = (c == '.' || c == '?' || c == '!');
                    bool isClause = (c == ',' || c == ';' || c == ':');

                    if (isEndOfSentence || isClause)
                    {
                        bool isLastVisibleChar = (i == characterCount - 1);
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

            // Reveal the completion cue
            dialogueText.maxVisibleCharacters = fullContent.Length;
            }

            dialogueText.maxVisibleCharacters = characterCount;

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            if (target == null)
            {
                return;
            }

            float elapsed = 0f;
            while (elapsed < duration)
            // Palette: Skippable post-line pause
            float pauseStart = Time.time;
            while (Time.time - pauseStart < 1.0f && !_skipRequested)
            {
                dialogueText.maxVisibleCharacters = i + 1;
                await Task.Yield();
            }

            _skipRequested = false;
        }

        public float GetSpeedMultiplier(string speaker)
        {
            if (speaker == "Kai")
            {
                return kaiSpeedMultiplier;
            }
            if (speaker == "Sky.ix")
            {
                return skyixSpeedMultiplier;
            }
            return 1.0f;
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
            };
        }

        private async Task WaitForSecondsOrSkipAsync(float seconds)
        {
            return speaker switch
            {
                "Sky.ix" => Color.cyan,
                "King Cyrus" => Color.yellow,
                "Reverie" => Color.magenta,
                "Kai" => new Color(1f, 0.84f, 0f),
                "Delilah" => new Color(0.6f, 0.1f, 0.9f),
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

        // Methods for Unit Testing and Logic satisfying Tests
        public float GetSpeedMultiplier(string speaker)
        {
            if (speaker == "Kai") return kaiSpeedMultiplier;
            if (speaker == "Sky.ix") return skyixSpeedMultiplier;
            return 1.0f;
        }

        public Color GetSpeakerColor(string speaker)
        {
            if (speaker == "Sky.ix") return Color.cyan;
            if (speaker == "Kai") return new Color(1f, 0.84f, 0f); // Gold
            if (speaker == "Delilah") return new Color(0.6f, 0.1f, 0.9f); // Void Purple
            return Color.white;
        }
    }
}
