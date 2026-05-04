using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Milehigh.Cinematics
{
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public CanvasGroup CinematicOverlay = null!;

        [Header("Settings")]
        public float DefaultTypingSpeed = 0.05f;
        private float currentTypingSpeed;
        private Coroutine? typingCoroutine;
        private bool skipRequested = false;

        private Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

        private WaitForSeconds GetWait(float seconds)
        {
            if (!_waitCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[seconds] = wait;
            }
            return wait;
        }

        private void Start()
        {
            currentTypingSpeed = DefaultTypingSpeed;
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }

        private IEnumerator WaitForSecondsOrSkip(float seconds)
        {
            float elapsed = 0;
            while (elapsed < seconds && !skipRequested)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            skipRequested = false;
        }

        public void ShowDialogue(string speaker, string text)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = speaker;
            typingCoroutine = StartCoroutine(TypeDialogue(text));
        }

        private IEnumerator TypeDialogue(string message)
        {
            DialogueText.text = message;
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = DialogueText.textInfo.characterCount;
            skipRequested = false;

            for (int i = 1; i <= totalVisibleCharacters; i++)
            {
                if (skipRequested)
                {
                    DialogueText.maxVisibleCharacters = totalVisibleCharacters;
                    break;
                }

                DialogueText.maxVisibleCharacters = i;

                float delay = currentTypingSpeed;
                char c = DialogueText.textInfo.characterInfo[i - 1].character;

                // Rhythmic punctuation pauses for natural reading.
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i < totalVisibleCharacters)
                    {
                        char nextChar = DialogueText.textInfo.characterInfo[i].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence)
                    {
                        // Check for ellipsis
                        bool isEllipsis = false;
                        if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                        if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;

                        delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                    }
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                }

                yield return GetWait(delay);
            }

            // Visual progression cue indicating text reveal is complete.
            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

            typingCoroutine = null;
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
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

            // ACTION: Sky.ix dashes towards the conduit
            yield return WaitForSecondsOrSkip(2.0f);

            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return WaitForSecondsOrSkip(3.5f);

            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return WaitForSecondsOrSkip(5.5f);

            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return WaitForSecondsOrSkip(7.5f);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = "";
            DialogueText.text = "";
            DialogueBox.SetActive(false);

            Debug.Log("Cinematic Sequence Complete.");
        }
    }
}
