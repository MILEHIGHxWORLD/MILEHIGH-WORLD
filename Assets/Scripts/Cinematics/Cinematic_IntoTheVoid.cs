using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

/// <summary>
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
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

    void Start()
    {
        // 🛡️ Sentinel: Defensive programming - Ensure UI components are assigned
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting.");
            return;
        }

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Update()
    {
        if (Input.anyKeyDown) skipRequested = true;
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

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        SpeakerNameText.text = speaker;

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
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        // The cue is the last character '▽' (and potentially rich text tags),
        // we reveal characters up to the cue.
        int messageLength = message.Length;

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
                char c = DialogueText.textInfo.characterInfo[i].character;

                // Rhythmic punctuation pauses
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalVisibleCharacters)
                    {
                        char nextChar = DialogueText.textInfo.characterInfo[i+1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence) delay *= 15f;
                    else delay *= 5f; // Ellipsis or mid-word period
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay *= 8f;
                }

                yield return GetWait(delay);
            }
        }

        // skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        typingCoroutine = null;
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. Hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        yield return WaitForSecondsOrSkip(2.0f);

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you.");
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);

        Debug.Log("Cinematic Sequence Complete.");
    }
}
