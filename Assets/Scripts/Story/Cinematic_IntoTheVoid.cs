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

        // Properties for Unit Testing and external UI coordination
        public TextMeshProUGUI SpeakerNameText { get => speakerNameText; set => speakerNameText = value; }
        public TextMeshProUGUI DialogueText { get => dialogueText; set => dialogueText = value; }
        public GameObject DialogueBox { get => dialogueCanvas; set => dialogueCanvas = value; }

        [Header("Environmental Shaders")]
        [SerializeField] private Material hyperrealisticPlatformShader = null!;

        // Cached Shader Property IDs for zero-allocation performance
        private readonly int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");
        private readonly int baseColorAlphaId = Shader.PropertyToID("_BaseColor_Alpha");
        private MaterialPropertyBlock _alphaPropBlock = null!;

        // Mathematical Constants
        private const float TrueMonadBaseline = 1.0f;
        private const float LinearOmenHexState = 6.0f;

        private bool _isStabilized = false;
        private Vector3 _originalSpeakerScale;
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
            }
        }

        private void Update()
        {
            // Palette: Idle-hint discoverability logic.
            if (Input.anyKeyDown)
            {
                _idleTimer = 0f;
                if (skipHint != null) skipHint.SetActive(false);
            }
            else
            {
                _idleTimer += Time.deltaTime;
                if (_idleTimer > 5f && skipHint != null) skipHint.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                _skipRequested = true;
            }
        }

        /// <summary>
        /// Zero-allocation rhythmic typewriter effect with themed completion cues and skip support.
        /// </summary>
        private async Task StreamDialogueAsync(string speaker, string content, float charDelay)
        {
            if (speakerNameText == null || dialogueText == null) return;

            _skipRequested = false;
            if (skipHint != null) skipHint.SetActive(false);

            string hexColor = GetSpeakerColorHex(speaker);
            string formattedSpeaker = $"<color={hexColor}>[{speaker}]</color>";

            if (speakerNameText.text != formattedSpeaker)
            {
                speakerNameText.text = formattedSpeaker;
                _ = PopScaleAsync(speakerNameText.transform, 0.2f, 1.1f);
            }

            // Layout stability: setting full text including the completion cue at the start.
            dialogueText.text = $"{content} <color={hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalVisibleCharacters; i++)
            {
                if (_skipRequested) break;

                dialogueText.maxVisibleCharacters = i;

                // Lexical pacing multipliers.
                float currentDelay = charDelay;
                if (i < totalVisibleCharacters)
                {
                    char c = dialogueText.textInfo.characterInfo[i - 1].character;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEllipsis = (i < totalVisibleCharacters && dialogueText.textInfo.characterInfo[i].character == '.');
                        currentDelay *= isEllipsis ? 5f : 12f;
                    }
                    else if (c == ',' || c == ':' || c == ';')
                    {
                        currentDelay *= 6f;
                    }
                }

                await Task.Delay(Mathf.RoundToInt(currentDelay * 1000));
            }

            dialogueText.maxVisibleCharacters = totalVisibleCharacters;

            if (!_skipRequested)
            {
                await WaitForSecondsOrSkipAsync(1.0f);
            }
            _skipRequested = false;
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

        private async Task PopScaleAsync(Transform target, float duration, float scaleFactor)
        {
            if (target == null) return;

            float elapsed = 0f;
            Vector3 targetScale = _originalSpeakerScale * scaleFactor;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                target.localScale = Vector3.Lerp(_originalSpeakerScale, targetScale, Mathf.PingPong(elapsed * (2f / duration), 1f));
                await Task.Yield();
            }
            target.localScale = _originalSpeakerScale;
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
            return speaker switch
            {
                "Sky.ix" => "#00FFFF",
                "King Cyrus" => "#FF4500",
                "Reverie" => "#A855F7",
                "Kai" => "#FFD700",
                "Delilah" => "#9919E6",
                _ => "#FFFFFF"
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

        [Conditional("ENABLE_NARRATIVE_LOGS")]
        private void LogNarrativeTelemetry(string message)
        {
            UnityEngine.Debug.Log($"<color=#E0BBE4>[CINEMATIC_ORCHESTRATOR]: {message}</color>");
        }
    }
}
