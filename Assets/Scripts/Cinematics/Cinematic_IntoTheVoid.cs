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
    // ====================================================================
    // CHARACTER ASSET & VOICE REFERENCE BLOCK
    // ====================================================================

    [Header("Character References")]
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;

    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;

    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;

    [Header("UI Components")]
    public GameObject DialogueBox = null!;
    public CanvasGroup? DialogueCanvasGroup;
    public TextMeshProUGUI SpeakerNameText = null!;
    public TextMeshProUGUI DialogueText = null!;

    [Header("UX Settings")]
    [FormerlySerializedAs("typingSpeed")]
    [Tooltip("Base delay in seconds between each character being revealed.")]
    public float baseTypingSpeed = 0.03f;
    [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
    public float kaiSpeedMultiplier = 3.0f;
    [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
    public float skyixSpeedMultiplier = 1.2f;

    private Coroutine? typingCoroutine;
    private float currentTypingSpeed;
    private bool skipRequested;

    // Cache for WaitForSeconds to eliminate GC allocations
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

    void Awake()
    {
        // Automated component lookup for CanvasGroup if not assigned
        if (DialogueCanvasGroup == null && DialogueBox != null)
        {
            DialogueCanvasGroup = DialogueBox.GetComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        // Initialize UI state
        if (DialogueCanvasGroup != null) DialogueCanvasGroup.alpha = 0;
        DialogueBox.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Update()
    {
        // Poll for skip input to ensure responsiveness
        // 🎨 Palette: Prefer specific keys for skip to avoid accidental triggers
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            skipRequested = true;
        }
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

        SpeakerNameText.text = speaker;

        // Apply speaker-specific speed multipliers and colors
        float multiplier = 1.0f;
        Color speakerColor = Color.white;

        switch (speaker)
        {
            case "Sky.ix":
                multiplier = skyixSpeedMultiplier;
                speakerColor = Color.cyan;
                break;
            case "Kai":
                multiplier = kaiSpeedMultiplier;
                speakerColor = new Color(1f, 0.84f, 0f); // Gold
                break;
            case "Delilah":
                speakerColor = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                break;
        }

        SpeakerNameText.color = speakerColor;
        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;

        typingCoroutine = StartCoroutine(TypeDialogue(message, speakerColor));
    }

    private IEnumerator TypeDialogue(string message, Color themeColor)
    {
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

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

                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                // Note: Delay occurs AFTER character reveal for natural rhythm.
                if (i > 0)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;

                    // Smart Punctuation: Look ahead to avoid pauses in middle of words (like Sky.ix)
                    bool isEndOfSentence = true;
                    if (i < totalVisibleCharacters)
                    {
                        char nextChar = DialogueText.textInfo.characterInfo[i].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Handle ellipsis or end of sentence
                        bool isEllipsis = false;
                        if (c == '.')
                        {
                            if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                            if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                        }

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }
                }

                yield return GetWait(delay);
            }
        }

        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        string hexColor = ColorUtility.ToHtmlStringRGB(themeColor);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 1;

        skipRequested = false;
        typingCoroutine = null;
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        yield return FadeDialogueBox(1f, 0.5f);
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

        // ACTION: Sky.ix dashes towards the conduit
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
        yield return FadeDialogueBox(0f, 0.5f);

        Debug.Log("Cinematic Sequence Complete.");
    }
}
