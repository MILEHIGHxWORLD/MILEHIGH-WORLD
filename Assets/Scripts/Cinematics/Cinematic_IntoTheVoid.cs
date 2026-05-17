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
    public class Cinematic_IntoTheVoid : UnityEngine.MonoBehaviour
    {
        [UnityEngine.Header("Character References")]
        public UnityEngine.GameObject Skyix_Character = null!;
        public UnityEngine.AudioSource Skyix_VoiceSource = null!;
        public UnityEngine.GameObject Kai_Character = null!;
        public UnityEngine.AudioSource Kai_VoiceSource = null!;
        public UnityEngine.GameObject Delilah_Character = null!;
        public UnityEngine.AudioSource Delilah_VoiceSource = null!;

        // ⚡ Bolt: Cache Animators to avoid expensive GetComponent calls during cinematic execution.
        private UnityEngine.Animator? _skyixAnimator;
        private UnityEngine.Animator? _kaiAnimator;
        private UnityEngine.Animator? _delilahAnimator;

        [UnityEngine.Header("UI Components")]
        public UnityEngine.GameObject DialogueBox = null!;
        public UnityEngine.CanvasGroup DialogueCanvasGroup = null!;
        public TMPro.TextMeshProUGUI SpeakerNameText = null!;
        public TMPro.TextMeshProUGUI DialogueText = null!;
        public TMPro.TextMeshProUGUI SkipHintText = null!;

        [UnityEngine.Header("UX Settings")]
        [UnityEngine.Tooltip("Base delay in seconds between each character being revealed.")]
        public float baseTypingSpeed = 0.03f;
        [UnityEngine.Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
        public float kaiSpeedMultiplier = 3.0f;
        [UnityEngine.Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
        public float skyixSpeedMultiplier = 1.2f;

        private UnityEngine.Coroutine? typingCoroutine;
        private UnityEngine.Coroutine? popScaleCoroutine;
        private float currentTypingSpeed;
        private string currentSpeakerHex = "FFFFFF";
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private bool _isDialogueBoxActive;
        private bool _isSkipHintActive;
        private UnityEngine.Vector3 originalSpeakerScale;

        // BOLT: Cache speaker styles to avoid repeated string conversions and switch evaluations.
        private struct SpeakerStyle
        {
            public UnityEngine.Color color;
            public string hexColor;
            public float speedMultiplier;
            public UnityEngine.AudioSource? voiceSource;
        }
        private readonly System.Collections.Generic.Dictionary<string, SpeakerStyle> _speakerStyles = new System.Collections.Generic.Dictionary<string, SpeakerStyle>();

        // BOLT: Cache for WaitForSeconds to eliminate GC allocations during coroutine execution.
        private static readonly System.Collections.Generic.Dictionary<int, UnityEngine.WaitForSeconds> _waitForSecondsCache = new System.Collections.Generic.Dictionary<int, UnityEngine.WaitForSeconds>();
        private RectTransform _dialogueRect = null!;
        private Vector2 _originalDialoguePos;

        private UnityEngine.WaitForSeconds GetWait(float time)
        {
            int ms = UnityEngine.Mathf.RoundToInt(time * 1000f);
            if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
            {
                wait = new UnityEngine.WaitForSeconds(time);
                _waitForSecondsCache[ms] = wait;
            }
            return wait;
        }

        private void Start()
        {
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null || DialogueCanvasGroup == null)
            {
                UnityEngine.Debug.LogError("Missing UI components required for cinematic. Aborting.");
                return;
            }

            originalSpeakerScale = SpeakerNameText.transform.localScale;
            _dialogueRect = DialogueBox.GetComponent<RectTransform>();
            _originalDialoguePos = _dialogueRect.anchoredPosition;

            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);

            // ⚡ Bolt: Pre-cache animators to eliminate GetComponent allocations during the cinematic sequence.
            if (Skyix_Character != null) _skyixAnimator = Skyix_Character.GetComponent<UnityEngine.Animator>();
            if (Kai_Character != null) _kaiAnimator = Kai_Character.GetComponent<UnityEngine.Animator>();
            if (Delilah_Character != null) _delilahAnimator = Delilah_Character.GetComponent<UnityEngine.Animator>();

            // ⚡ Bolt: Pre-initialize speaker styles to eliminate O(N) lookups and GC allocations in ShowDialogue.
            _speakerStyles["Sky.ix"] = new SpeakerStyle { color = UnityEngine.Color.cyan, hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(UnityEngine.Color.cyan), speedMultiplier = skyixSpeedMultiplier, voiceSource = Skyix_VoiceSource };
            _speakerStyles["Kai"] = new SpeakerStyle { color = new UnityEngine.Color(1f, 0.84f, 0f), hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(new UnityEngine.Color(1f, 0.84f, 0f)), speedMultiplier = kaiSpeedMultiplier, voiceSource = Kai_VoiceSource };
            _speakerStyles["Delilah"] = new SpeakerStyle { color = new UnityEngine.Color(0.6f, 0.1f, 0.9f), hexColor = UnityEngine.ColorUtility.ToHtmlStringRGB(new UnityEngine.Color(0.6f, 0.1f, 0.9f)), speedMultiplier = 1.0f, voiceSource = Delilah_VoiceSource };

            if (SkipHintText == null && DialogueBox != null)
            {
                UnityEngine.Transform hintTransform = DialogueBox.transform.Find("SkipHint");
                if (hintTransform != null) SkipHintText = hintTransform.GetComponent<TMPro.TextMeshProUGUI>();
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

            this.StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            if (UnityEngine.Input.anyKeyDown)
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0f;
                if (_isSkipHintActive)
                {
                    if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
                    _isSkipHintActive = false;
                }
            }

            if (_isDialogueBoxActive && !playerInteracted && !skipRequested)
            {
                idleTimer += UnityEngine.Time.deltaTime;
                if (idleTimer >= 2.0f && !_isSkipHintActive)
                {
                    if (SkipHintText != null)
                    {
                        SkipHintText.gameObject.SetActive(true);
                        _isSkipHintActive = true;
                    }
                }
            }

            if (_isSkipHintActive && SkipHintText != null)
            {
                // ⚡ Bolt: Use CanvasRenderer.SetAlpha to avoid expensive TextMeshPro mesh rebuilds during alpha pulsing.
                float alpha = UnityEngine.Mathf.PingPong(UnityEngine.Time.time * 0.5f, 0.5f) + 0.5f;
                SkipHintText.canvasRenderer.SetAlpha(alpha);
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) this.StopCoroutine(typingCoroutine);

            // UX Enhancement: Reset idle timer and interaction state for each new dialogue line.
            idleTimer = 0f;
            playerInteracted = false;
            if (_isSkipHintActive)
            {
                if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
                _isSkipHintActive = false;
            }

            if (SpeakerNameText.text != speaker)
            {
                if (popScaleCoroutine != null) this.StopCoroutine(popScaleCoroutine);
                popScaleCoroutine = this.StartCoroutine(PopScale(SpeakerNameText.transform, 0.2f, 1.15f));
            }

            SpeakerNameText.text = speaker;

            // ⚡ Bolt: O(1) lookup from pre-cached speaker styles.
            if (_speakerStyles.TryGetValue(speaker, out SpeakerStyle style))
            {
                SpeakerNameText.color = style.color;
                currentSpeakerHex = style.hexColor;
                currentTypingSpeed = baseTypingSpeed * style.speedMultiplier;
                if (style.voiceSource != null) style.voiceSource.Play();
            }
            else
            {
                SpeakerNameText.color = UnityEngine.Color.white;
                currentSpeakerHex = "FFFFFF";
                currentTypingSpeed = baseTypingSpeed;
            }

            skipRequested = false;
            typingCoroutine = this.StartCoroutine(TypeDialogue(message));
        }

        private System.Collections.IEnumerator TypeDialogue(string message)
        {
            // Palette: Pre-append completion cue and use maxVisibleCharacters to ensure layout stability.
            DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            TMPro.TMP_TextInfo textInfo = DialogueText.textInfo;
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
                elapsed += UnityEngine.Time.deltaTime;
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

        private System.Collections.IEnumerator FadeDialogueBox(float targetAlpha, float duration)
        {
            if (targetAlpha > 0)
            {
                DialogueBox.SetActive(true);
                _isDialogueBoxActive = true;
            }
            float startAlpha = DialogueCanvasGroup.alpha;
            UnityEngine.Vector2 startPos = _originalDialoguePos + (targetAlpha > 0 ? new UnityEngine.Vector2(0, -30f) : UnityEngine.Vector2.zero);
            UnityEngine.Vector2 endPos = _originalDialoguePos + (targetAlpha <= 0 ? new UnityEngine.Vector2(0, -30f) : UnityEngine.Vector2.zero);

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += UnityEngine.Time.deltaTime;
                float t = elapsed / duration;
                DialogueCanvasGroup.alpha = UnityEngine.Mathf.Lerp(startAlpha, targetAlpha, t);
                if (_dialogueRect != null) _dialogueRect.anchoredPosition = UnityEngine.Vector2.Lerp(startPos, endPos, t);
                yield return null;
            }
            DialogueCanvasGroup.alpha = targetAlpha;
            if (_dialogueRect != null) _dialogueRect.anchoredPosition = endPos;
            if (targetAlpha <= 0)
            {
                DialogueBox.SetActive(false);
                _isDialogueBoxActive = false;
            }
        }

        private IEnumerator PopScale(UnityEngine.Transform target, float duration, float scaleFactor)
        {
            UnityEngine.Vector3 initialScale = originalSpeakerScale;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += UnityEngine.Time.deltaTime;
                float curve = UnityEngine.Mathf.Sin((elapsed / duration) * UnityEngine.Mathf.PI) * (scaleFactor - 1f);
                target.localScale = initialScale * (1f + curve);
                yield return null;
            }
            target.localScale = initialScale;
            popScaleCoroutine = null;
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            yield return FadeDialogueBox(1.0f, 0.5f);
            yield return GetWait(1.0f);
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
            UnityEngine.Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
            if (typingCoroutine != null) this.StopCoroutine(typingCoroutine);
        }
    }
}
