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
        public float charDelay = 0.05f;
        public float skyixSpeedMultiplier = 1.0f;
        public float kaiSpeedMultiplier = 1.0f;

        [Header("UI References")]
        public TMPro.TextMeshProUGUI speakerNameText = null!;
        public TMPro.TextMeshProUGUI dialogueText = null!;
        public GameObject dialogueBox = null!;
        public GameObject skipHint = null!;

        private bool _skipRequested = false;
        private Vector3 _originalSpeakerScale;

        private void Awake()
        {
            if (speakerNameText != null) _originalSpeakerScale = speakerNameText.transform.localScale;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                _skipRequested = true;
            }
        }

        private async Task StreamDialogueAsync(string speaker, string content, float baseCharDelay)
        {
            if (speakerNameText == null || dialogueText == null || dialogueBox == null) return;

            _skipRequested = false;
            dialogueBox.SetActive(true);
            if (skipHint != null) skipHint.SetActive(false);

            string hexColor = GetSpeakerColorHex(speaker);
            speakerNameText.text = $"<color={hexColor}>[{speaker}]</color>";
            speakerNameText.transform.localScale = _originalSpeakerScale * 1.1f;
            _ = ResetScaleAsync(speakerNameText.transform, 0.2f);

            float speedMult = GetSpeedMultiplier(speaker);
            float actualCharDelay = baseCharDelay / speedMult;

            dialogueText.text = $"{content} <color={hexColor}>▽</color>";
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.ForceMeshUpdate();

            int totalCharacters = dialogueText.textInfo.characterCount;

            for (int i = 1; i <= totalCharacters; i++)
            {
                if (_skipRequested)
                {
                    dialogueText.maxVisibleCharacters = totalCharacters;
                    break;
                }

                dialogueText.maxVisibleCharacters = i;

                char c = dialogueText.textInfo.characterInfo[i - 1].character;
                float delay = actualCharDelay;

                if (c == '.' || c == '!' || c == '?') delay *= 12f;
                else if (c == ',' || c == ':' || c == ';') delay *= 6f;

                await Task.Delay(Mathf.RoundToInt(delay * 1000));
            }

            _skipRequested = false;
            float pauseStart = Time.time;
            while (Time.time - pauseStart < 1.0f && !_skipRequested)
            {
                await Task.Yield();
            }
        }

        private async Task ResetScaleAsync(Transform target, float duration)
        {
            await Task.Delay(Mathf.RoundToInt(duration * 1000));
            if (target != null) target.localScale = _originalSpeakerScale;
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

        public Color GetSpeakerColor(string speaker)
        {
            return speaker switch
            {
                "Sky.ix" => Color.cyan,
                "Kai" => new Color(1f, 0.84f, 0f),
                "Delilah" => new Color(0.6f, 0.1f, 0.9f),
                "King Cyrus" => new Color(1f, 0.27f, 0f),
                "Reverie" => new Color(0.66f, 0.33f, 0.97f),
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
    }
}
