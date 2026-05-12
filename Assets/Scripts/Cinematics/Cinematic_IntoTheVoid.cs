using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("Character References")]
        public GameObject Skyix_Character = null!;
        public AudioSource Skyix_VoiceSource = null!;
        public GameObject Kai_Character = null!;
        public AudioSource Kai_VoiceSource = null!;
        public GameObject Delilah_Character = null!;
        public AudioSource Delilah_VoiceSource = null!;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public CanvasGroup DialogueCanvasGroup = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public TextMeshProUGUI? SkipHintText;
        public TextMeshProUGUI SkipHint = null!;

        [Header("UX Settings")]
        public float baseTypingSpeed = 0.03f;
        public float kaiSpeedMultiplier = 3.0f;
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private Coroutine? popCoroutine;
        private float currentTypingSpeed;
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private Vector3 originalSpeakerScale;

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

        void Start()
        {
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }
            originalSpeakerScale = SpeakerNameText.transform.localScale;
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        void Update()
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0;
                if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            }
            else if (DialogueBox != null && DialogueBox.activeInHierarchy)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && !playerInteracted && SkipHint != null)
                {
                    SkipHint.gameObject.SetActive(true);
                }
            }
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

        private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
        {
            if (DialogueCanvasGroup == null)
            {
                DialogueBox.SetActive(targetAlpha > 0);
                yield break;
            }
            if (targetAlpha > 0) DialogueBox.SetActive(true);
            float startAlpha = DialogueCanvasGroup.alpha;
            float elapsed = 0;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                yield return null;
            }
            DialogueCanvasGroup.alpha = targetAlpha;
            if (targetAlpha <= 0) DialogueBox.SetActive(false);
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (popCoroutine != null) StopCoroutine(popCoroutine);

            SpeakerNameText.text = speaker;
            popCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform, 0.2f, 1.15f));

            float multiplier = 1.0f;
            if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
            else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

            currentTypingSpeed = baseTypingSpeed * multiplier;
            skipRequested = false;

            Color speakerColor = speaker switch
            {
                "Sky.ix" => Color.cyan,
                "Kai" => new Color(1f, 0.84f, 0f),
                "Delilah" => new Color(0.6f, 0.1f, 0.9f),
                _ => Color.white
            };
            SpeakerNameText.color = speakerColor;
            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = DialogueText.textInfo.characterCount;
            int messageLength = totalVisibleCharacters - 2;

            for (int i = 0; i <= messageLength; i++)
            {
                if (skipRequested) break;
                DialogueText.maxVisibleCharacters = i;
                if (i > 0 && i <= messageLength)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                    float delay = currentTypingSpeed;
                    if (c == '.' || c == '!' || c == '?') delay *= 15f;
                    else if (c == ',' || c == ';' || c == ':') delay *= 8f;
                    yield return GetWait(delay);
                }
            }
            DialogueText.maxVisibleCharacters = totalVisibleCharacters;
            typingCoroutine = null;
        }

        private IEnumerator PopScale(Transform target, float duration, float scaleFactor)
        {
            Vector3 initialScale = Vector3.one;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float curve = Mathf.Sin((elapsed / duration) * Mathf.PI) * (scaleFactor - 1f);
                target.localScale = initialScale * (1f + curve);
                yield return null;
            }
            target.localScale = initialScale;
            popCoroutine = null;
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            yield return FadeDialogueBox(1.0f, 0.5f);
            yield return WaitForSecondsOrSkip(1.0f);

            ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
            yield return WaitForSecondsOrSkip(7.5f);

            ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
            yield return WaitForSecondsOrSkip(6.0f);

            ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
            yield return WaitForSecondsOrSkip(8.0f);

            ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
            yield return WaitForSecondsOrSkip(7.0f);

            ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
            yield return WaitForSecondsOrSkip(4.5f);

            yield return WaitForSecondsOrSkip(2.0f);

            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return WaitForSecondsOrSkip(3.5f);

            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return WaitForSecondsOrSkip(5.5f);

            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return WaitForSecondsOrSkip(7.5f);

            yield return FadeDialogueBox(0f, 0.5f);
            Debug.Log("Cinematic Sequence Complete.");
        }
    }
}
