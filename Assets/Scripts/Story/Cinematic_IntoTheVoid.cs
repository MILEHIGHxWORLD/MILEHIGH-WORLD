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

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Properties for Unit Testing
        public TextMeshProUGUI SpeakerNameText { get => speakerNameText; set => speakerNameText = value; }
        public TextMeshProUGUI DialogueText { get => dialogueText; set => dialogueText = value; }
        public GameObject DialogueBox { get => dialogueCanvas; set => dialogueCanvas = value; }

        // Cached Shader Property IDs
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");
        private MaterialPropertyBlock _propBlock = null!;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
        private bool _skipRequested = false;
        private float _lastInteractionTime;

        private void Awake()
        {
            _propBlock = new MaterialPropertyBlock();
        }

        private void Start()
        {
            Time.timeScale = 1.0f;

            if (speakerNameText != null)
            {
                _originalSpeakerScale = speakerNameText.transform.localScale;
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

            _lastInteractionTime = Time.time;
            _ = ExecuteConvergenceSequenceAsync();
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                _skipRequested = true;
                _lastInteractionTime = Time.time;
                if (skipHint != null) skipHint.SetActive(false);
            }
            else if (Time.time - _lastInteractionTime > 2f)
            {
                if (skipHint != null && !skipHint.activeSelf) skipHint.SetActive(true);
            }

            if (skipHint != null && skipHint.activeSelf)
            {
                // Pulse effect for skip hint
                var color = skipHint.GetComponent<TextMeshProUGUI>()?.color ?? Color.white;
                color.a = Mathf.PingPong(Time.time * 0.5f, 0.5f) + 0.5f;
                if (skipHint.TryGetComponent<TextMeshProUGUI>(out var text)) text.color = color;
            }
        }

        private async Task ExecuteConvergenceSequenceAsync()
        {
            LogNarrativeTelemetry("INITIALIZING SCENE: THE OMEN SINGULARITY APEX");

            await TweenShaderEntropyAsync(LinearOmenHexState, 2.0f);

            if (skyixPrefab != null) skyixPrefab.SetActive(true);
            if (reveriePrefab != null) reveriePrefab.SetActive(true);
            if (kingCyrusPrefab != null) kingCyrusPrefab.SetActive(true);

            if (dialogueCanvas != null) dialogueCanvas.SetActive(true);

            await StreamDialogueAsync("King Cyrus", "Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!", baseTypingSpeed);
            await WaitForSecondsOrSkipAsync(0.5f);

            await StreamDialogueAsync("Sky.ix", "Negative. The resonance is peaking. Engaging Void Conduit via Vitis AI Bridge.", baseTypingSpeed);

            for (int i = 0; i < 999; i++) timelineEngine.RegisterSynchronizedShard();

            double tension = vitisBridge.CalculateSystemTension();
            timelineEngine.EvaluateSystemTension(tension);

            if (_isStabilized && !timelineEngine.IsRealityFractured)
            {
                await StreamDialogueAsync("Reverie", "The 999th shard is ours. System tension within limits. Severing the loop... now!", baseTypingSpeed);
                await ExecuteSaveEveryoneProtocolAsync();
            }
            else
            {
                await StreamDialogueAsync("King Cyrus", "Your reality is too brittle for this power!", baseTypingSpeed);
            }

            if (dialogueCanvas != null) dialogueCanvas.SetActive(false);
        }

        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            _skipRequested = false;
            if (skipHint != null) skipHint.SetActive(false);

            string hexColor = GetSpeakerColorHex(speaker);
            speakerNameText.text = $"<color={hexColor}>[{speaker}]</color>";
            _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);

            dialogueText.text = $"{content} <color={hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            float multiplier = GetSpeedMultiplier(speaker);
            float effectiveDelay = charDelay / multiplier;

            int totalVisibleCharacters = dialogueText.textInfo.characterCount;
            // The last character is the '▽' cue
            int dialogueLength = totalVisibleCharacters - 1;

            for (int i = 1; i <= dialogueLength; i++)
            {
                if (_skipRequested) break;

                dialogueText.maxVisibleCharacters = i;

                char c = dialogueText.textInfo.characterInfo[i - 1].character;
                float pauseMultiplier = 1f;

                // Lexical pacing look-ahead to ignore periods in 'Sky.ix'
                bool isTechnicalName = (i < dialogueLength && !char.IsWhiteSpace(dialogueText.textInfo.characterInfo[i].character));

                if (!isTechnicalName)
                {
                    if (".!?".Contains(c)) pauseMultiplier = 15f;
                    else if (",:".Contains(c)) pauseMultiplier = 8f;
                }

                if (c == '.' && i > 1 && dialogueText.textInfo.characterInfo[i - 2].character == '.')
                    pauseMultiplier = 5f;

                await Task.Delay(Mathf.RoundToInt(effectiveDelay * pauseMultiplier * 1000));
            }

            dialogueText.maxVisibleCharacters = totalVisibleCharacters;

            if (!_skipRequested)
            {
                await WaitForSecondsOrSkipAsync(1.0f);
            }
            _skipRequested = false;
        }

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            if (target == null) return;
            float elapsed = 0f;
            Vector3 targetScale = _originalSpeakerScale * scaleFactor;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                target.localScale = Vector3.Lerp(_originalSpeakerScale, targetScale, Mathf.PingPong(elapsed * 2 / duration, 1));
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

        private async Task TweenAlphaDecayAsync(Renderer renderer, float duration)
        {
            if (renderer == null) return;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                renderer.GetPropertyBlock(_propBlock);
                _propBlock.SetFloat(baseColorAlphaId, alpha);
                renderer.SetPropertyBlock(_propBlock);
                await Task.Yield();
            }
        }

        private async Task ExecuteSaveEveryoneProtocolAsync()
        {
            if (kingCyrusPrefab != null)
            {
                Renderer renderer = kingCyrusPrefab.GetComponentInChildren<Renderer>();
                await TweenAlphaDecayAsync(renderer, 1.5f);
                kingCyrusPrefab.SetActive(false);
            }
            await TweenShaderEntropyAsync(TrueMonadBaseline, 1.0f);
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
                "King Cyrus" => new Color(1f, 0.27f, 0f), // #FF4500
                "Reverie" => new Color(0.66f, 0.33f, 0.97f), // #A855F7
                _ => Color.white
            };
        }

        private string GetSpeakerColorHex(string speaker)
        {
            return ColorUtility.ToHtmlStringRGB(GetSpeakerColor(speaker));
        }

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
