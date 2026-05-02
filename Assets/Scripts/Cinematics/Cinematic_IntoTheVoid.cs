using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

namespace Milehigh.Cinematics
{
    /// <summary>
    /// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        // ====================================================================
        // CHARACTER ASSET & VOICE REFERENCE BLOCK
        // ====================================================================

        public GameObject Skyix_Character = null!;
        public AudioSource Skyix_VoiceSource = null!;

        public GameObject Kai_Character = null!;
        public AudioSource Kai_VoiceSource = null!;

        public GameObject Delilah_Character = null!;
        public AudioSource Delilah_VoiceSource = null!;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;

        [Header("UX Settings")]
        [FormerlySerializedAs("typingSpeed")]
        public float baseTypingSpeed = 0.03f;
        public float kaiSpeedMultiplier = 3.0f;
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private float currentTypingSpeed;
        private bool skipRequested;

        // BOLT: Cache for WaitForSeconds to eliminate GC allocations
        private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

        private WaitForSeconds GetWait(float time)
        {
            if (!_waitForSecondsCache.TryGetValue(time, out var wait))
            {
                wait = new WaitForSeconds(time);
                _waitForSecondsCache[time] = wait;
            }
            return wait;
        }

        private IEnumerator WaitForSecondsOrSkip(float duration)
        {
            float start = Time.time;
            while (Time.time - start < duration && !skipRequested)
            {
                yield return null;
            }
            skipRequested = false;
        }

        void Start()
        {
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }
            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        void Update()
        {
            // BOLT: Single efficient input poll for skipping
            if (Input.anyKeyDown)
            {
                skipRequested = true;
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

            SpeakerNameText.text = speaker;

            float multiplier = 1.0f;
            if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
            else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

            currentTypingSpeed = baseTypingSpeed * multiplier;
            skipRequested = false;

            Color speakerColor;
            switch (speaker)
            {
                case "Sky.ix": speakerColor = Color.cyan; break;
                case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break;
                case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break;
                default: speakerColor = Color.white; break;
            }

            SpeakerNameText.color = speakerColor;
            typingCoroutine = StartCoroutine(TypeDialogue(message));
        }

        private IEnumerator TypeDialogue(string message)
        {
            // UX Enhancement: Color-coded completion cue that matches speaker theme.
            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = 0;

            DialogueText.ForceMeshUpdate();
            // BOLT: Typewriter logic optimized for TextMeshPro character indices
            int totalVisibleCharacters = DialogueText.textInfo.characterCount;

            for (int i = 0; i <= totalVisibleCharacters; i++)
            {
                if (skipRequested)
                {
                    DialogueText.maxVisibleCharacters = totalVisibleCharacters;
                    break;
                }

                DialogueText.maxVisibleCharacters = i;

                if (i < totalVisibleCharacters)
                {
                    float delay = currentTypingSpeed;

                    // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                    if (i > 0)
                    {
                        char c = DialogueText.textInfo.characterInfo[i - 1].character;

                        if (c == '.' || c == '!' || c == '?')
                        {
                            bool isEllipsis = false;
                            if (c == '.')
                            {
                                if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                                if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                            }

                            if (isEllipsis) delay = currentTypingSpeed * 5f;
                            else if (i < totalVisibleCharacters && char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character))
                                delay = currentTypingSpeed * 15f;
                        }
                        else if (c == ',' || c == ';' || c == ':')
                        {
                            delay = currentTypingSpeed * 8f;
                        }
                    }

                    yield return GetWait(delay);
                }
            }

            skipRequested = false;
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
