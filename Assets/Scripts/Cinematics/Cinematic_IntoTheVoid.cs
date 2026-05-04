using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// Manages the cinematic sequence "Into the Void", handling dialogue, camera transitions, and character animations.
    /// This script uses a typewriter effect for dialogue reveal with rhythmic punctuation pauses.
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public CanvasGroup CinematicOverlay = null!;

        [Header("Settings")]
        public float typingSpeed = 0.05f;
        public float punctuationPause = 0.4f;
        public float commaPause = 0.2f;

        private bool skipRequested = false;
        private Coroutine? typingCoroutine;
        private Dictionary<float, WaitForSeconds> waitCache = new Dictionary<float, WaitForSeconds>();

        private void Start()
        {
            if (DialogueBox == null) DialogueBox = transform.Find("DialogueBox")?.gameObject!;
            if (SpeakerNameText == null) SpeakerNameText = GetComponentInChildren<TextMeshProUGUI>();
            if (CinematicOverlay == null) CinematicOverlay = GetComponent<CanvasGroup>();

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        {
            // UX Enhancement: Allow skipping dialogue reveal with Space or Left Click.
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }

        private WaitForSeconds GetWait(float seconds)
        {
            if (!waitCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                waitCache[seconds] = wait;
            }
            return wait;
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

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            SpeakerNameText.text = speaker;

            // Set speaker theme color
            Color speakerColor = speaker switch
            {
                "Delilah" => new Color(0.4f, 0.8f, 0.2f), // Sickly green
                "Sky.ix" => new Color(0.2f, 0.6f, 1.0f), // Cyber blue
                "Kai" => new Color(1.0f, 0.8f, 0.2f),   // Warning yellow
                _ => Color.white
            };
            SpeakerNameText.color = speakerColor;

            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            // UX Enhancement: Color-coded completion cue that matches speaker theme.
            // We append it immediately to ensure the full layout is calculated upfront, preventing "jumping".
            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

            DialogueText.maxVisibleCharacters = 0;

            // Ensure TMP is updated to get accurate character info
            DialogueText.ForceMeshUpdate();
            TMP_TextInfo textInfo = DialogueText.textInfo;
            int totalCharacters = textInfo.characterCount;

            for (int i = 0; i < totalCharacters; i++)
            {
                if (skipRequested) break;

                DialogueText.maxVisibleCharacters = i + 1;

                char c = textInfo.characterInfo[i].character;
                float delay = typingSpeed;

                // UX Pattern: Rhythmic punctuation pauses for natural reading.
                if (c == '.' || c == '!' || c == '?')
                {
                    // Look ahead for ellipsis
                    if (i + 1 < totalCharacters && textInfo.characterInfo[i + 1].character == '.')
                    {
                        delay = typingSpeed * 2f;
                    }
                    else
                    {
                        delay = punctuationPause;
                    }
                }
                else if (c == ',')
                {
                    delay = commaPause;
                }

                yield return GetWait(delay);
            }

            DialogueText.maxVisibleCharacters = totalCharacters + 2; // Include the cue
            skipRequested = false;
            typingCoroutine = null;
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            DialogueBox.SetActive(true);
            yield return WaitForSecondsOrSkip(1.0f);

            // --- Dialogue Line 1: Delilah ---
            yield return WaitForSecondsOrSkip(1.5f);
            ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
            yield return WaitForSecondsOrSkip(7.5f);

            // --- Dialogue Line 2: Sky.ix ---
            yield return WaitForSecondsOrSkip(0.5f);
            ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
            yield return WaitForSecondsOrSkip(6.0f);

            // --- Dialogue Line 3: Kai ---
            yield return WaitForSecondsOrSkip(0.7f);
            ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
            yield return WaitForSecondsOrSkip(8.0f);

            // --- Dialogue Line 4: Delilah ---
            yield return WaitForSecondsOrSkip(1.2f);
            ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
            yield return WaitForSecondsOrSkip(7.0f);

            // --- Dialogue Line 5: Sky.ix ---
            yield return WaitForSecondsOrSkip(0.8f);
            ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
            yield return WaitForSecondsOrSkip(4.5f);

            // --- ACTION: Sky.ix dashes towards the conduit ---
            yield return WaitForSecondsOrSkip(2.0f);

            // --- Dialogue Line 6: Kai ---
            yield return WaitForSecondsOrSkip(0.5f);
            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return WaitForSecondsOrSkip(3.5f);

            // --- Dialogue Line 7: Delilah ---
            yield return WaitForSecondsOrSkip(1.5f);
            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return WaitForSecondsOrSkip(5.5f);

            // --- Dialogue Line 8: Sky.ix ---
            yield return WaitForSecondsOrSkip(1.0f);
            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return WaitForSecondsOrSkip(7.5f);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = "";
            DialogueText.text = "";
            DialogueBox.SetActive(false);

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
        }
    }
}
