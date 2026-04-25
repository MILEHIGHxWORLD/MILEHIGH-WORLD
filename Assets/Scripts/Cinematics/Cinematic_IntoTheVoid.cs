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
    /* VOICE PROFILE:
     * Pitch: Mid-Range Mezzo-Shorano
     * Tempo: Steady and Precise (130-140 WPM)
     * Tone & Style: Driven, Loving, Determined. Underlying sorrow/weariness.
    */
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;

    // Protagonist: Kai the The Child of Prophecy
    /* VOICE PROFILE:
     * Pitch: Gender Neutral/Mid-Range
     * Tempo: Slow and Paused (70-90 WPM)
     * Tone & Style: Cryptic, Calm, Profound, and Fatalistic. Speaks in metaphor.
    */
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
    private Vector3 _originalSpeakerScale;
    private Coroutine? _popCoroutine;

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
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        _originalSpeakerScale = SpeakerNameText.transform.localScale;

        // 🎨 Palette: Improve text readability with outlines for the dark Void environment
        ApplyOutline(SpeakerNameText);
        ApplyOutline(DialogueText);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    private void ApplyOutline(TextMeshProUGUI textComponent)
    {
        if (textComponent == null) return;
        Material mat = textComponent.fontMaterial;
        mat.EnableKeyword(ShaderUtilities.Keyword_Outline);
        mat.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        mat.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
    }

    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
        }
    }

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        SpeakerNameText.text = speaker;

        // 🎨 Palette: Speaker Name Pop animation for better visual cue
        if (_popCoroutine != null) StopCoroutine(_popCoroutine);
        _popCoroutine = StartCoroutine(PopScale());

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

    private IEnumerator PopScale()
    {
        float duration = 0.2f;
        float halfDuration = duration / 2f;
        float elapsed = 0f;
        Vector3 targetScale = _originalSpeakerScale * 1.15f;

        while (elapsed < halfDuration)
        {
            SpeakerNameText.transform.localScale = Vector3.Lerp(_originalSpeakerScale, targetScale, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            SpeakerNameText.transform.localScale = Vector3.Lerp(targetScale, _originalSpeakerScale, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SpeakerNameText.transform.localScale = _originalSpeakerScale;
        _popCoroutine = null;
    }

    private IEnumerator TypeDialogue(string message)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

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

            // Pause AFTER the character is revealed for natural reading rhythm
            if (i > 0 && i <= totalVisibleCharacters)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
                float delay = currentTypingSpeed;

                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = (i >= totalVisibleCharacters) || char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);
                    bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') || (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                    if (isEllipsis) delay = currentTypingSpeed * 5f;
                    else if (c == '.' && !isEndOfSentence) delay = currentTypingSpeed;
                    else delay = currentTypingSpeed * 15f;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                }

                yield return GetWait(delay);
            }
        }

        DialogueText.maxVisibleCharacters = totalVisibleCharacters;
        skipRequested = false;
        typingCoroutine = null;
    }

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        // [SCENE SETUP: Disable player controls, position cameras]
        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);

        // --- Dialogue Line 1: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);

        // --- Dialogue Line 2: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        // --- Dialogue Line 3: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai pointing towards a conduit]
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        // --- Dialogue Line 4: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 5: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        // --- ACTION: Sky.ix dashes towards the conduit ---
        // [SFX: Play cybernetic dash sound]
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("React_Alarmed");]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        // --- Dialogue Line 7: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 8: Sky.ix ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        yield return WaitForSecondsOrSkip(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);

        // [SCENE CLEANUP: Re-enable player controls, transition to boss fight]
        // Example: PlayerInput.Instance.EnableControls();
        Debug.Log("Cinematic Sequence Complete.");
    }
}
