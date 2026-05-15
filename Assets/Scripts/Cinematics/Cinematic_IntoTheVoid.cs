using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using Milehigh.Core;

namespace Milehigh.Cinematics
{
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        public delegate void CinematicComplete();
        public event CinematicComplete? OnCinematicComplete;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;

        [Header("Settings")]
        public float currentTypingSpeed = 0.05f;
        public float punctuationPause = 0.5f;
        public float commaPause = 0.2f;

        private Coroutine? typingCoroutine;
        private bool skipRequested;
        private readonly Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

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
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (SpeakerNameText == null || DialogueText == null) return;

            SpeakerNameText.text = speaker;
            Color speakerColor = speaker switch
            {
                "Delilah" => new Color(0.8f, 0.2f, 0.2f),
                "Sky.ix" => new Color(0.2f, 0.6f, 0.9f),
                "Kai" => new Color(0.9f, 0.8f, 0.2f),
                _ => Color.white
            };
            SpeakerNameText.color = speakerColor;

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            TMP_TextInfo textInfo = DialogueText.textInfo;
            int totalVisibleCharacters = textInfo.characterCount;

            for (int i = 0; i < totalVisibleCharacters; i++)
            {
                if (skipRequested) break;

                DialogueText.maxVisibleCharacters = i + 1;
                char c = textInfo.characterInfo[i].character;
                float delay = currentTypingSpeed;

                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 0 && textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                        if (i < totalVisibleCharacters - 1 && textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
                    }

                    if (isEllipsis)
                        delay = currentTypingSpeed * 3f;
                    else if (c == '.' && i < totalVisibleCharacters - 1 && !char.IsWhiteSpace(textInfo.characterInfo[i + 1].character))
                        delay = currentTypingSpeed;
                    else
                        delay = punctuationPause;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = commaPause;
                }

                yield return GetWait(delay);
            }

            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;
            typingCoroutine = null;
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

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            if (DialogueBox == null) yield break;

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

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of THE VOID...]");
            OnCinematicComplete?.Invoke();
        }
    }
}
