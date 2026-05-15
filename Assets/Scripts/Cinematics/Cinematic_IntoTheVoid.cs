using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// Manages the cinematic sequence "Into the Void", handling dialogue, character animations, audio, and rhythmic text reveal.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Character References")]
        public GameObject Skyix_Character = null!;
        public AudioSource Skyix_VoiceSource = null!;
        public GameObject Kai_Character = null!;
        public AudioSource Kai_VoiceSource = null!;
        public GameObject Delilah_Character = null!;
        public AudioSource Delilah_VoiceSource = null!;

        // ⚡ Bolt: Cache Animators to avoid expensive GetComponent calls during cinematic execution.
        private Animator? _skyixAnimator;
        private Animator? _kaiAnimator;
        private Animator? _delilahAnimator;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public CanvasGroup DialogueCanvasGroup = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public TextMeshProUGUI SkipHintText = null!;

        [Header("UX Settings")]
        [Tooltip("Base delay in seconds between each character being revealed.")]
        public float baseTypingSpeed = 0.03f;
        [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
        public float kaiSpeedMultiplier = 3.0f;
        [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private Coroutine? popScaleCoroutine;
        private float currentTypingSpeed;
        private string currentSpeakerHex = "FFFFFF";
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private Vector3 originalSpeakerScale;

        // BOLT: Cache for WaitForSeconds to eliminate GC allocations during coroutine execution.
        private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

        private WaitForSeconds GetWait(float time)
        {
            int ms = Mathf.RoundToInt(time * 1000f);
            if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
            {
                wait = new WaitForSeconds(time);
                _waitForSecondsCache[ms] = wait;
            }
            return wait;
        }

        private void Start()
        {
            // 🛡️ Sentinel: Defensive programming - Ensure UI components are assigned.
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null || DialogueCanvasGroup == null)
            {
                Debug.LogError("Missing UI components required for cinematic. Aborting.");
                return;
            }

            originalSpeakerScale = SpeakerNameText.transform.localScale;

            // ⚡ Bolt: Pre-cache animators to eliminate GetComponent allocations during the cinematic sequence.
            if (Skyix_Character != null) _skyixAnimator = Skyix_Character.GetComponent<Animator>();
            if (Kai_Character != null) _kaiAnimator = Kai_Character.GetComponent<Animator>();
            if (Delilah_Character != null) _delilahAnimator = Delilah_Character.GetComponent<Animator>();

            // Palette: Programmatically locate SkipHint if not assigned.
            if (SkipHintText == null && DialogueBox != null)
            {
                Transform hintTransform = DialogueBox.transform.Find("SkipHint");
                if (hintTransform != null) SkipHintText = hintTransform.GetComponent<TextMeshProUGUI>();
            }

            if (SkipHintText != null)
            {
                SkipHintText.text = "[Any Key/Click] Skip";
                SkipHintText.gameObject.SetActive(false);
            }

            // Palette: Accessibility - Consolidated text outline for better contrast in dark scenes.
            foreach (var text in new[] { SpeakerNameText, DialogueText, SkipHintText })
            {
                if (text != null && text.fontMaterial != null)
                {
                    text.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.25f);
                    text.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
                }
            }

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            // ⚡ Bolt: Precise skip detection for refined UX.
            if (Input.anyKeyDown)
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0f;
                if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
            }

            // UX Enhancement: Show skip hint after 2 seconds of idleness.
            if (DialogueBox != null && DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && SkipHintText != null)
                {
                    SkipHintText.gameObject.SetActive(true);
                }
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            // UX Enhancement: Reset interaction state for each new dialogue line.
            idleTimer = 0f;
            playerInteracted = false;
            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);

            // UX Enhancement: Trigger a subtle "Pop" animation when the speaker changes.
            if (SpeakerNameText.text != speaker)
            {
                if (popScaleCoroutine != null) StopCoroutine(popScaleCoroutine);
                popScaleCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform, 0.2f, 1.15f));
            }

            SpeakerNameText.text = speaker;

            // Apply speaker-specific speed multipliers and colors.
            float multiplier = 1.0f;
            Color speakerColor = speaker switch
            {
                "Sky.ix" => Color.cyan,
                "Kai" => new Color(1f, 0.84f, 0f), // Gold
                "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
                _ => Color.white
            };

            if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
            else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

            SpeakerNameText.color = speakerColor;
            currentSpeakerHex = ColorUtility.ToHtmlStringRGB(speakerColor);
            currentTypingSpeed = baseTypingSpeed * multiplier;

            // Palette: Reset skip request only at the start of a new dialogue line.
            skipRequested = false;

            // Audio: Play the character's voice line if assigned.
            AudioSource? voiceSource = speaker switch
            {
                "Sky.ix" => Skyix_VoiceSource,
                "Kai" => Kai_Character?.GetComponent<AudioSource>(), // Fallback attempt
                "Delilah" => Delilah_VoiceSource,
                _ => null
            };
            if (speaker == "Kai") voiceSource = Kai_VoiceSource; // Ensure Kai is handled correctly

            if (voiceSource != null) voiceSource.Play();

            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            // Palette: Pre-append completion cue and use maxVisibleCharacters to ensure layout stability.
            DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            TMP_TextInfo textInfo = DialogueText.textInfo;
            int totalCharacters = textInfo.characterCount;
            int mainMessageLength = totalCharacters - 1; // Exclude the completion cue

            for (int i = 0; i <= mainMessageLength; i++)
            {
                if (skipRequested) break;

                DialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= mainMessageLength)
                {
                    char c = textInfo.characterInfo[i - 1].character;
                    float delay = currentTypingSpeed;

                    // Rhythmic pacing
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Check for mid-word periods (like Sky.ix) using look-ahead
                        bool isEndOfSentence = true;
                        if (i < mainMessageLength)
                        {
                            char nextChar = textInfo.characterInfo[i].character;
                            if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                        }

                        if (isEndOfSentence)
                        {
                            bool isEllipsis = (i > 1 && textInfo.characterInfo[i - 2].character == '.') ||
                                             (i < mainMessageLength && textInfo.characterInfo[i].character == '.');
                            delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                        }
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }

                    yield return GetWait(delay);
                }
            }

            DialogueText.maxVisibleCharacters = totalCharacters;
            // Palette: Carry-over skip - skipRequested is NOT reset here to allow skipping the reading pause too.
            typingCoroutine = null;
        }

        private IEnumerator WaitForSecondsOrSkip(float duration)
        {
            float elapsed = 0f;
            while (elapsed < duration && !skipRequested)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            skipRequested = false;
        }

        private IEnumerator PlayDialogueLine(string speaker, string message, float readingPause)
        {
            ShowDialogue(speaker, message);
            while (typingCoroutine != null) yield return null;
            yield return WaitForSecondsOrSkip(readingPause);
        }

        private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
        {
            if (targetAlpha > 0) DialogueBox.SetActive(true);
            float startAlpha = DialogueCanvasGroup.alpha;
            RectTransform rect = (RectTransform)DialogueBox.transform;
            Vector2 startPos = rect.anchoredPosition;
            Vector2 targetPos = new Vector2(startPos.x, targetAlpha > 0 ? 0f : -30f);
            if (targetAlpha > 0) startPos.y = -30f;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
                yield return null;
            }
            DialogueCanvasGroup.alpha = targetAlpha;
            rect.anchoredPosition = targetPos;
            if (targetAlpha <= 0) DialogueBox.SetActive(false);
        }

        private IEnumerator PopScale(Transform target, float duration, float scaleFactor)
        {
            Vector3 initialScale = originalSpeakerScale;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float curve = Mathf.Sin((elapsed / duration) * Mathf.PI) * (scaleFactor - 1f);
                target.localScale = initialScale * (1f + curve);
                yield return null;
            }
            target.localScale = initialScale;
            popScaleCoroutine = null;
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            yield return FadeDialogueBox(1.0f, 0.5f);
            yield return WaitForSecondsOrSkip(1.0f);

            // Line 1: Delilah
            if (_delilahAnimator != null) _delilahAnimator.SetTrigger("Channeling_Idle");
            yield return PlayDialogueLine("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.", 2.5f);

            // Line 2: Sky.ix
            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("React_Furious");
            yield return PlayDialogueLine("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.", 1.5f);

            // Line 3: Kai
            if (_kaiAnimator != null) _kaiAnimator.SetTrigger("Point_Urgent");
            yield return PlayDialogueLine("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!", 2.0f);

            // Line 4: Delilah
            if (_delilahAnimator != null) _delilahAnimator.SetTrigger("Smirk_Dismissive");
            yield return PlayDialogueLine("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.", 2.0f);

            // Line 5: Sky.ix
            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("Action_Ready");
            yield return PlayDialogueLine("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!", 1.0f);

            // ACTION: Sky.ix dashes
            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("Dash_Forward");
            yield return WaitForSecondsOrSkip(2.0f);

            // Line 6: Kai
            if (_kaiAnimator != null) _kaiAnimator.SetTrigger("React_Alarmed");
            yield return PlayDialogueLine("Kai", "The energy spike is massive! Your shields won't hold for long!", 1.0f);

            // Line 7: Delilah
            if (_delilahAnimator != null) _delilahAnimator.SetTrigger("Taunt_OpenArms");
            yield return PlayDialogueLine("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.", 1.5f);

            // Line 8: Sky.ix
            if (_skyixAnimator != null) _skyixAnimator.SetTrigger("Determined_Resolve");
            yield return PlayDialogueLine("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.", 3.0f);

            yield return FadeDialogueBox(0f, 0.5f);
            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        }
    }
}
