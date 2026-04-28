using UnityEngine;
using TMPro;
using System.Collections;

namespace Milehigh.Cinematics
{
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public float defaultTypingSpeed = 0.05f;

        private bool skipRequested = false;
        private Coroutine? typingCoroutine;

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
            if (Input.anyKeyDown)
            {
                skipRequested = true;
            }
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeDialogue(speaker, message));
        }

        private IEnumerator WaitForSecondsOrSkip(float seconds)
        {
            float timer = 0;
            while (timer < seconds && !skipRequested)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            skipRequested = false;
        }

        private IEnumerator TypeDialogue(string speaker, string message)
        {
            skipRequested = false;
            SpeakerNameText.text = speaker;
            DialogueText.text = message;
            DialogueText.maxVisibleCharacters = 0;
            DialogueText.ForceMeshUpdate();

            int totalVisibleCharacters = message.Length;
            for (int i = 0; i <= totalVisibleCharacters; i++)
            {
                if (skipRequested)
                {
                    DialogueText.maxVisibleCharacters = totalVisibleCharacters;
                    break;
                }

                DialogueText.maxVisibleCharacters = i;

                if (i > 0 && i <= totalVisibleCharacters)
                {
                    char c = message[i - 1];
                    float delay = defaultTypingSpeed;

                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEllipsis = false;
                        if (c == '.')
                        {
                            if (i > 1 && message[i - 2] == '.') isEllipsis = true;
                            if (i < totalVisibleCharacters && message[i] == '.') isEllipsis = true;
                        }

                        if (isEllipsis) delay *= 5f;
                        else if (i < totalVisibleCharacters && !char.IsWhiteSpace(message[i])) delay = defaultTypingSpeed;
                        else delay *= 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay *= 8f;
                    }

                    float timer = 0;
                    while (timer < delay && !skipRequested)
                    {
                        timer += Time.deltaTime;
                        yield return null;
                    }
                }
                else
                {
                    yield return null;
                }
            }

            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 1;

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

            DialogueBox.SetActive(false);
            Debug.Log("Cinematic Sequence Complete");
        }
    }
}
