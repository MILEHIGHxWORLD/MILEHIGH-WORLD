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

        [Header("Typing Settings")]
        public float baseTypingSpeed = 0.04f;
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
        private static MaterialPropertyBlock _propertyBlock = null!;
        private static MaterialPropertyBlock _sharedPropertyBlock = null!;
        private MaterialPropertyBlock _alphaPropBlock = null!;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
        private bool _skipRequested = false;
        private float _lastInteractionTime;
        private GameObject? _skipHint;
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
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _idleTimer = 0f;
                _lastInteractionTime = Time.time;

                if (_skipHint != null && _skipHint.activeSelf) _skipHint.SetActive(false);
                if (skipHint != null && skipHint.activeSelf) skipHint.SetActive(false);
            }
            else
            {
                _idleTimer += Time.deltaTime;

                if (Time.time - _lastInteractionTime > 2f || _idleTimer >= 2.0f)
                {
                    if (_skipHint != null && !_skipHint.activeSelf) _skipHint.SetActive(true);
                    if (skipHint != null && !skipHint.activeSelf) skipHint.SetActive(true);
                }
            }
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX - SECTOR 09-09-09");

            await TweenShaderEntropyAsync(LinearOmenHexState, 2.0f);

            if (skyixPrefab != null) skyixPrefab.SetActive(true);
            if (reveriePrefab != null) reveriePrefab.SetActive(true);
            if (kingCyrusPrefab != null) kingCyrusPrefab.SetActive(true);

            if (dialogueCanvas != null) dialogueCanvas.SetActive(true);

            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", 0.04f);
            await WaitForSecondsOrSkipAsync(0.5f);
            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. Engaging Void Conduit via Vitis AI Bridge.", 0.03f);

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
                float intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                hyperrealisticPlatformShader.SetFloat(emissiveIntensityId, intensity);
                await Task.Yield();
            }
        }

        private async Task ExecuteSaveEveryoneProtocolAsync()
        {
            LogNarrativeTelemetry("Executing Save-Everyone Protocol: Synchronizing Shards...");
            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);
        }

        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;
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

        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;
            string hexColor = GetSpeakerColorHex(speaker);
            speakerNameText.text = $"<color={hexColor}>[{speaker}]</color>";
            dialogueText.text = content;
            dialogueText.ForceMeshUpdate();
            int characterCount = dialogueText.textInfo.characterCount;
            dialogueText.maxVisibleCharacters = 0;
            _skipRequested = false;
            float multiplier = GetSpeedMultiplier(speaker);
            for (int i = 0; i < characterCount; i++)
            {
                if (_skipRequested) { dialogueText.maxVisibleCharacters = characterCount; break; }
                dialogueText.maxVisibleCharacters = i + 1;
                char c = dialogueText.textInfo.characterInfo[i].character;
                float delay = charDelay * multiplier;
                if (c == '.' || c == '?' || c == '!') delay *= 15f;
                else if (c == ',') delay *= 8f;
                await Task.Delay(Mathf.RoundToInt(delay * 1000));
            }
            _skipRequested = false;
            float pauseStart = Time.time;
            while (Time.time - pauseStart < 1.0f && !_skipRequested) await Task.Yield();
            _skipRequested = false;
        }

        private string GetSpeakerColorHex(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => "#00FFFF", "King Cyrus" => "#FFFF00", "Reverie" => "#FF00FF",
                "Kai" => "#FFD700", "Delilah" => "#9933FF", _ => "#FFFFFF"
            };
        }

        private async Task WaitForSecondsOrSkipAsync(float seconds)
        {
            float elapsed = 0f;
            while (elapsed < seconds && !_skipRequested) { elapsed += Time.deltaTime; await Task.Yield(); }
            _skipRequested = false;
        }

        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }

        public float GetSpeedMultiplier(string speaker)
        {
            if (speaker == "Kai") return kaiSpeedMultiplier;
            if (speaker == "Sky.ix") return skyixSpeedMultiplier;
            return 1.0f;
        }

        public Color GetSpeakerColor(string speaker)
        {
            if (speaker == "Sky.ix") return Color.cyan;
            if (speaker == "Kai") return new Color(1f, 0.84f, 0f);
            if (speaker == "Delilah") return new Color(0.6f, 0.1f, 0.9f);
            return Color.white;
        }
    }
}
