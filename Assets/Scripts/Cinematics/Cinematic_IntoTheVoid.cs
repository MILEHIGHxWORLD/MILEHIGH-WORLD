// --- UNITY SCENE SETUP --- //
//
// 1. Create an empty GameObject in your scene and name it "SceneController".
//
// 2. Attach this script (`Cinematic_IntoTheVoid.cs`)
//    to the "SceneController" GameObject.
//
// 3. Create or place the character prefabs/GameObjects for "Sky.ix", "Kai", and "Delilah" into the scene.
//
// 4. Ensure each character GameObject has the following components attached:
//    - An Animator component with a configured Animation Controller.
//    - An AudioSource component to be used for their voice lines.
//    - Their respective ability script (e.g., Sky.ix needs `Ability_Skyix.cs`, Kai needs `Ability_Kai.cs`, etc.)
//
// 5. Create the UI for the dialogue system:
//    - Right-click in the Hierarchy -> UI -> Canvas.
//    - Inside the Canvas, create a UI -> Panel. Rename it "DialogueBox". This will be the background.
//    - Inside the "DialogueBox", create two UI -> Text - TextMeshPro objects.
//    - Name the first one "SpeakerNameText" and position it where the speaker's name should appear.
//    - Name the second one "DialogueText" and position it for the main dialogue content.
//    - Initially, set the "DialogueBox" GameObject to be inactive (uncheck the box in the Inspector).
//
// 6. Select the "SceneController" GameObject. In the Inspector, drag and drop the corresponding scene objects
//    into the public fields of this script:
//    - Drag the "Sky.ix" GameObject into the `Skyix_Character` field.
//    - Drag the AudioSource from "Sky.ix" into the `Skyix_VoiceSource` field.
//    - Drag the "Kai" GameObject into the `Kai_Character` field.
//    - Drag the AudioSource from "Kai" into the `Kai_VoiceSource` field.
//    - Drag the "Delilah" GameObject into the `Delilah_Character` field.
//    - Drag the AudioSource from "Delilah" into the `Delilah_VoiceSource` field.
//    - Drag the "DialogueBox" panel into the `Dialogue Box` field.
//    - Drag the "SpeakerNameText" TMP object into the `Speaker Name Text` field.
//    - Drag the "DialogueText" TMP object into the `Dialogue Text` field.
//
// 7. Ensure your project has TextMeshPro imported (Window -> TextMeshPro -> Import TMP Essential Resources).

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

/// <summary>
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ, the very concept of existence is under assault. Delilah, an agent of entropy, has located and harnessed a 'Memory Stream'—a torrent of glitching data containing the metaphysical essence of Sky.ix's recently reunited husband and child. She intends to weaponize this stream, funneling its corrupted energy into a finality engine that will not just kill them, but permanently erase their existence from every timeline and memory. Sky.ix, whose cybernetics offer a fragile anchor in this digital abyss, must race against the unraveling of reality itself, supported by her ally Kai, to sever Delilah's connection before her family becomes nothing more than a corrupted file in the memory of the universe."
/// </summary>
public class Cinematic_IntoTheVoid : MonoBehaviour
{
    // ====================================================================
    //
    // CHARACTER ASSET & VOICE REFERENCE BLOCK
    //
    // ====================================================================

    // Protagonist: Sky.ix the The Bionic Goddess
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;


    // Protagonist: Kai the The Child of Prophecy
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;


    // Antagonist: Delilah the The Desolate
    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;

    [Header("UI Components")]
    public GameObject DialogueBox = null!;
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

    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
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

    void Update()
    {
        // Poll for skip input to ensure responsiveness across all input devices
        if (Input.anyKeyDown)
        {
            skipRequested = true;
        }
    }

    /// <summary>
    /// Yields for the specified duration but returns immediately if a skip is requested.
    /// Resets the skip flag upon completion.
    /// </summary>
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
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    /// <summary>
    /// Updates the speaker name and begins the typewriter effect for the dialogue message.
    /// </summary>
    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        SpeakerNameText.text = speaker;

        // Apply speaker-specific speed multipliers based on voice profiles
        float multiplier = 1.0f;
        if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
        else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;

        // Apply character-specific colors for better speaker identification
        Color speakerColor;
        switch (speaker)
        {
            case "Sky.ix":
                speakerColor = Color.cyan;
                break;
            case "Kai":
                speakerColor = new Color(1f, 0.84f, 0f); // Gold
                break;
            case "Delilah":
                speakerColor = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                break;
            default:
                speakerColor = Color.white;
                break;
        }

        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
    }

    /// <summary>
    /// Unified typewriter effect that handles rich text, rhythmic pauses, and skip logic.
    /// </summary>
    private IEnumerator TypeDialogue(string message)
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        // We append it immediately to ensure the full layout is calculated upfront, preventing "jumping".
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;

        // Ensure TMP is updated to get accurate character info
        DialogueText.ForceMeshUpdate();
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;

        for (int i = 0; i < totalVisibleCharacters; i++)
        {
            // UX Enhancement: Robust skip logic using persistent flag
            if (skipRequested) break;

            DialogueText.maxVisibleCharacters = i + 1;

            float delay = currentTypingSpeed;

            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            // We check the revealed character to pause *after* it has been revealed.
            char c = DialogueText.textInfo.characterInfo[i].character;

            if (c == '.' || c == '!' || c == '?')
            {
                // Smart Punctuation: Look ahead to avoid pauses in middle of words (like Sky.ix)
                bool isMidWord = false;
                if (i + 1 < totalVisibleCharacters)
                {
                    char nextChar = DialogueText.textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isMidWord = true;
                }

                if (!isMidWord)
                {
                    // Refined ellipsis detection: if neighboring characters are also dots, it's an ellipsis
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 0 && DialogueText.textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                        if (i + 1 < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
                    }

                    delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                }
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

            // ⚡ Bolt: Use cached WaitForSeconds to avoid GC allocations in the typewriter loop
            yield return GetWait(delay);
        }

        // Finalize text display
        DialogueText.maxVisibleCharacters = totalVisibleCharacters;

        // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        typingCoroutine = null;
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        // [SCENE SETUP: Disable player controls, position cameras, set initial character states]
        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);

        // --- Dialogue Line 1: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah, who is calmly observing the Memory Stream.]
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        // --- Dialogue Line 2: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix's enraged face.]
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(6.0f);

        // --- Dialogue Line 3: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai, who points towards a glowing conduit pulsating with corrupted energy.]
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(8.0f);

        // --- Dialogue Line 4: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        // [CAMERA: Cut back to a low-angle shot of Delilah, making her appear dominant and unconcerned.]
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 5: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        // [CAMERA: Follow Sky.ix as she turns her body towards the conduit, cybernetics glowing.]
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(4.5f);

        // --- ACTION: Sky.ix dashes towards the conduit ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Dash_Forward");]
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("React_Alarmed");]
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(3.5f);

        // --- Dialogue Line 7: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 8: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);

        // [SCENE CLEANUP: Re-enable player controls, reset cameras, transition to gameplay/boss fight]
        Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
    }
}
