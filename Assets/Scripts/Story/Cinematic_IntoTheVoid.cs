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
        private MaterialPropertyBlock _propertyBlock;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

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

            _ = ExecuteConvergenceSequenceAsync();
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
                var renderer = kingCyrusPrefab.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
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

            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propBlock);
            _propertyBlock ??= new MaterialPropertyBlock();

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
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

        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            Color speakerColor = GetSpeakerColor(speaker);
            string colorHex = "#" + ColorUtility.ToHtmlStringRGB(speakerColor);
            string formattedSpeaker = $"<color={colorHex}>[{speaker}]</color>";
        /// <summary>
        /// Zero-allocation rhythmic typewriter effect with themed completion cues and speaker pop animations.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string colorHex = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={colorHex}>[{speaker}]</color>";
        /// Zero-allocation rhythmic typewriter effect for dialogue rendering with rhythmic pacing and speaker transitions.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
            }

            dialogueText.text = $"{content} <color={colorHex}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalVisibleCharacters; i++)
            {
                dialogueText.maxVisibleCharacters = i;

                float currentDelay = charDelay;
                if (i < totalVisibleCharacters)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    bool isEndOfSentence = (c == '.' || c == '!' || c == '?');
                    bool isPause = (c == ',' || c == ':' || c == ';');

                    if (isEndOfSentence)
                    {
                        bool nextIsSpace = (i < totalVisibleCharacters && dialogueText.textInfo.characterInfo[i].character == ' ');
                        if (nextIsSpace || i == totalVisibleCharacters - 1) currentDelay *= 12f;
                    }
                    else if (isPause)
                    {
                        currentDelay *= 6f;
                    }
                }

                await Task.Delay(Mathf.RoundToInt(currentDelay * 1000));
            }

            dialogueText.maxVisibleCharacters = totalVisibleCharacters;
                if (speakerNameText.transform != null)
                {
                    _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
                }
            }

            // Palette: Append a color-coded '▽' completion cue to the dialogue for better interaction clarity.
            // By setting the full text (including the cue) at the start, we ensure layout stability.
            dialogueText.text = $"{content} <color={colorHex}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalVisibleCharacters; i++)
            {
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
                        // Look ahead: only long pause if followed by a space or it's the last character before the cue
                        bool nextIsSpace = (i < totalVisibleCharacters && char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));
                        if (nextIsSpace || i == totalVisibleCharacters - 1) currentDelay *= 12f;
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
                "Sky.ix" => "#00FFFF",
                "King Cyrus" => "#FFFF00",
                "Reverie" => "#FF00FF",
                "Kai" => "#FFD700",
                "Delilah" => "#9932CC",
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

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            if (target == null) return;

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
                case "King Cyrus": return Color.yellow;
                case "Reverie": return Color.magenta;
                case "Kai": return new Color(1f, 0.84f, 0f); // Gold
                case "Delilah": return new Color(0.6f, 0.1f, 0.9f); // Void Purple
                default: return Color.white;
            }
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
