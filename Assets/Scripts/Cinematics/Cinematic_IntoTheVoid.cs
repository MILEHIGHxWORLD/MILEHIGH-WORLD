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

using System;
using System.Collections;
using System.Collections.Generic;
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

public class Cinematic_IntoTheVoid : MonoBehaviour
{
/// <summary>
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
/// </summary>
public class Cinematic_IntoTheVoid : MonoBehaviour
{
    [Header("Characters")]
    [Header("Character References")]
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;
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
    // ====================================================================
    //
    // CHARACTER ASSET & VOICE REFERENCE BLOCK
    //
    // ====================================================================

    // Protagonist: Sky.ix the The Bionic Goddess
    // Description: A 45-year-old Caucasian cyborg woman with short white hair. She has humanoid features but her face and body have visible cybernetic enhancements that allow her to traverse the Void. She was a brilliant xenolinguist who, along with her family, was part of the research team at the Onalym Nexus.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/characters/skyix.png
    // Ability Script: Ability_Skyix.cs
    /* VOICE PROFILE:
     * Pitch: Mid-Range Mezzo-Shorano
     * Tempo: Steady and Precise (130-140 WPM)
     * Texture & Effects: Clean, Clear, and Articulated. Subtle Digital/Synthetic Filter (low chorus).
     * Projection: Medium-High, Direct
     * Tone & Style: Driven, Loving, Determined. Underlying sorrow/weariness.
     * Keywords: Digital, Bionic, Precise, Loving, Clear Articulation, Subtle Filter.
    */
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;


    // Protagonist: Kai the The Child of Prophecy
    // Description: Sky.ix's child, lost and now found. Holds the key to the Prophecy.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/characters/kai.png
    // Ability Script: Ability_Kai.cs
    /* VOICE PROFILE:
     * Pitch: Gender Neutral/Mid-Range
     * Tempo: Slow and Paused (70-90 WPM)
     * Texture & Effects: Aged, Weathered, and Layered. Subtle Temporal Echo/Layering effect.
     * Projection: Soft, but Infinitely Resonant
     * Tone & Style: Cryptic, Calm, Profound, and Fatalistic. Speaks in metaphor.
     * Keywords: Ancient, Layered, Slow, Resonant, Cryptic, Contemplative.
    */
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;


    // Antagonist: Delilah the The Desolate
    // Description: A corrupted form of Ingris, wielding Voidfire.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/antagonists/delilah.png
    // Ability Script: Ability_Delilah.cs
    /* VOICE PROFILE:
     * Not available.
    */
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
    public TextMeshProUGUI? SkipHintText;

    [Header("UX Settings")]
    [Tooltip("Base delay in seconds between each character being revealed.")]
    public float typingSpeed = 0.03f;
    [Tooltip("Delay in seconds after punctuation (., !, ?).")]
    public float punctuationPause = 0.5f;
    [Tooltip("Delay in seconds after a comma.")]
    public float commaPause = 0.2f;
    [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
    [FormerlySerializedAs("typingSpeed")]
    public float baseTypingSpeed = 0.03f;
    public float kaiSpeedMultiplier = 3.0f;
    public float skyixSpeedMultiplier = 1.2f;

    private Coroutine? typingCoroutine;
    private Coroutine? popScaleCoroutine;
    private Coroutine? scalingCoroutine;
    private Coroutine? speakerPopCoroutine;
    private Coroutine typingCoroutine;
    private Coroutine speakerPopCoroutine;
    private Coroutine popCoroutine;
    private Vector3 originalSpeakerNameScale;
    private Coroutine? typingCoroutine;
    private float currentTypingSpeed;
    private string speakerHexColor;
    private string currentSpeakerHex;
    private string currentSpeakerColorTag;
    private bool skipRequested;
    private Vector3 originalSpeakerScale;
    private bool playerInteracted;
    private Vector3 originalScale;
    private Vector3 originalSpeakerScale;
    private string cachedHexColor = "FFFFFF";
    private string currentSpeakerHex;
    private Vector3 speakerNameOriginalScale;
    private Coroutine popCoroutine;
    private string _currentCompletionCue = null!;
    private string _currentSpeakerHex = null!;

    private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();
    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
    // BOLT: Use int (milliseconds) instead of float for dictionary key to avoid floating-point tolerance cache misses
    private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

    private WaitForSeconds GetWait(float time)
    {
        if (!_waitForSecondsCache.TryGetValue(time, out WaitForSeconds? wait) || wait == null)
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        }

    private IEnumerator PopSpeakerName()
    {
        float duration = 0.25f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;
            float scaleMod = 1f + Mathf.Sin(percent * Mathf.PI) * 0.15f;
            SpeakerNameText.transform.localScale = speakerNameOriginalScale * scaleMod;
            yield return null;
        }
        SpeakerNameText.transform.localScale = speakerNameOriginalScale;
    }

    private IEnumerator PopEffect(RectTransform rect)
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.anyKeyDown)
        // Poll for skip input to ensure responsiveness across all input types
        if (Input.anyKeyDown)
        // Poll for skip input to ensure responsiveness across keyboard and mouse
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        if (rect == null) yield break;

        Vector3 initialScale = Vector3.one;
        rect.localScale = initialScale;

        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;
            float curve = Mathf.Sin(percent * Mathf.PI);
            rect.localScale = initialScale + Vector3.one * (curve * 0.1f);
            yield return null;
        }

        rect.localScale = initialScale;
    }

    private IEnumerator WaitForSecondsOrSkip(float seconds)
    {
        float timer = 0f;
        while (timer < seconds && !skipRequested)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        skipRequested = false;
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

    private IEnumerator PopEffect(Transform target)
    {
        target.localScale = Vector3.one;
        float duration = 0.2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1f + Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.15f;
            target.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    /// <summary>
    /// Yields for the specified duration but returns immediately if a skip is requested.
    /// Resets the skip flag upon completion to ensure it doesn't bleed into the next reveal.
        target.localScale = Vector3.one;
    }

    /// <summary>
    /// Wait for a specified duration, but allow the user to skip the wait by pressing any key.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float timer = 0f;
        while (timer < duration && !skipRequested)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        skipRequested = false;
    }

    /// <summary>
    /// Adds a "pop" scale animation to a UI element for visual juice.
    /// </summary>
    private IEnumerator PopScale(RectTransform target, float duration, float scaleFactor)
    {
        if (target == null) yield break;

        target.localScale = Vector3.one;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            // Use a sine wave for a smooth pop out and back in
            float scale = 1f + Mathf.Sin(percent * Mathf.PI) * scaleFactor;
            target.localScale = new Vector3(scale, scale, 1f);
            yield return null;
        }
        target.localScale = Vector3.one;
    }

    private IEnumerator WaitForSecondsOrSkip(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds && !skipRequested)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Waits for a specified duration, but returns immediately if a skip is requested.
    /// </summary>
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

    void Start()
    private void Start()
    {
        // Cache original scale to prevent scale drift during interrupted animations
        if (SpeakerNameText != null) originalSpeakerNameScale = SpeakerNameText.transform.localScale;

        // 🛡️ Sentinel: Security enhancement - Defensive programming
        // Ensure UI components are assigned to prevent NullReferenceException and potential stack trace leakage.
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }

        originalSpeakerScale = SpeakerNameText.transform.localScale;

        // Palette: Find SkipHint programmatically if not assigned
        if (SkipHintText == null && DialogueBox != null)
        {
            Transform hintTransform = DialogueBox.transform.Find("SkipHint");
            if (hintTransform != null) SkipHintText = hintTransform.GetComponent<TextMeshProUGUI>();
        }

        if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);

        // UX Enhancement: Cache the original scale of the speaker text to prevent animation drift
        originalScale = SpeakerNameText.transform.localScale;

        originalSpeakerScale = SpeakerNameText.transform.localScale;

        // 🎨 Palette: Accessibility - Text outline for better contrast in dark scenes
        SpeakerNameText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        SpeakerNameText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    private void Update()
    {
        if (Input.anyKeyDown) skipRequested = true;
    }

        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown)
    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float start = Time.unscaledTime;
        while (Time.unscaledTime - start < duration && !skipRequested)
        {
            yield return null;
        }
    }

        skipRequested = false;
    }

    /// <summary>
    /// UX Enhancement: A skippable wait that allows users to 'fast-forward' through dialogue beats.
    /// </summary>
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

    /// <summary>
    /// UX Enhancement: A subtle 'pop' animation for UI elements to draw focus during state changes.
    /// </summary>
    private IEnumerator PopScale(Transform target, float duration = 0.2f, float scaleFactor = 1.15f)
    {
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        Vector3 initialScale = target.localScale;
        Vector3 targetScale = initialScale * scaleFactor;

        float elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            Debug.LogError("Missing UI components required for cinematic.");
            return;
        }
        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

            target.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < duration / 2f)
        {
            target.localScale = Vector3.Lerp(targetScale, initialScale, elapsed / (duration / 2f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = initialScale;
        popCoroutine = null;
    }

    void Start()
    void Update()
    {
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
            playerInteracted = true;
            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
        }
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0)) skipRequested = true;
        if (Input.anyKeyDown) skipRequested = true;
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown)
        // ⚡ Bolt: Precise skip detection for refined UX.
        // We focus on primary confirmation keys to prevent accidental skips from accidental key presses.
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        private void Update()
        {
            // UX Enhancement: Allow skipping dialogue reveal with Space or Left Click.
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }

        speakerNameOriginalScale = SpeakerNameText.transform.localScale;

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    /// <summary>
    /// Yields for the specified duration but returns immediately if a skip is requested.
    /// Resets the skip flag upon completion.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float start = Time.time;
        while (Time.time - start < duration && !skipRequested)

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

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    public void ShowDialogue(string speaker, string message)
    {
        skipRequested = false;
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        if (speakerPopCoroutine != null) StopCoroutine(speakerPopCoroutine);

        // 🎨 Palette: PopScale animation for speaker name changes
        if (SpeakerNameText.text != speaker)
        {
            if (scalingCoroutine != null) StopCoroutine(scalingCoroutine);
            scalingCoroutine = StartCoroutine(PopScaleSpeaker());
        }

        // UI Juice: Apply a pop animation to the speaker name when it changes
        if (SpeakerNameText.text != speaker)
        {
            StartCoroutine(PopScale(SpeakerNameText.rectTransform, 0.2f, 0.15f));
        }

        // UX Enhancement: Trigger a subtle "Pop" animation when the speaker changes
        if (SpeakerNameText.text != speaker)
        {
            if (popScaleCoroutine != null) StopCoroutine(popScaleCoroutine);
            popScaleCoroutine = StartCoroutine(PopScaleEffect());
        }

        bool speakerChanged = SpeakerNameText.text != speaker;
        SpeakerNameText.text = speaker;
        speakerPopCoroutine = StartCoroutine(PopEffect());
        currentTypingSpeed = baseTypingSpeed * GetSpeedMultiplier(speaker);
        skipRequested = false;
        SpeakerNameText.color = GetSpeakerColor(speaker);

        typingCoroutine = StartCoroutine(TypeDialogue(message));
    }

    internal float GetSpeedMultiplier(string speaker)
    {
        if (speaker == "Kai") return kaiSpeedMultiplier;
        if (speaker == "Sky.ix") return skyixSpeedMultiplier;
        return 1.0f;
    }

    internal Color GetSpeakerColor(string speaker)
    {
        speakerPopCoroutine = StartCoroutine(PopEffect(SpeakerNameText.rectTransform));
        if (popCoroutine != null) StopCoroutine(popCoroutine);

        if (speakerChanged) StartCoroutine(PopEffect(SpeakerNameText.transform));

        // Apply speaker-specific speed multipliers based on voice profiles
        SpeakerNameText.text = speaker;
        popCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform));
        popCoroutine = StartCoroutine(PopSpeakerName());
        public void ShowDialogue(string speaker, string message)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        // Apply speaker-specific speed multipliers
        float multiplier = 1.0f;
        if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
        else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;
            SpeakerNameText.text = speaker;

        currentTypingSpeed = typingSpeed * multiplier;
        skipRequested = false;

        Color speakerColor = Color.white;
        if (speaker == "Sky.ix") speakerColor = Color.cyan;
        else if (speaker == "Kai") speakerColor = new Color(1f, 0.84f, 0f);
        else if (speaker == "Delilah") speakerColor = new Color(0.6f, 0.1f, 0.9f);


        Color speakerColor = Color.white;
        // Apply character-specific colors
        // Apply character-specific colors for better speaker identification
        Color speakerColor;
        switch (speaker)
        {
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break; // Gold
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break; // Void Purple
            default: speakerColor = Color.white; break;
        // Apply character-specific colors for better speaker identification
        switch (speaker)
        {
            case "Sky.ix":
                return Color.cyan;
            case "Kai":
                return new Color(1f, 0.84f, 0f); // Gold
            case "Delilah":
                return new Color(0.6f, 0.1f, 0.9f); // Void Purple
            default:
                return Color.white;
        }
        SpeakerNameText.color = speakerColor;
        // BOLT: Cache hex color to avoid per-character allocation in the reveal loop
        cachedHexColor = ColorUtility.ToHtmlStringRGB(speakerColor);
        speakerHexColor = ColorUtility.ToHtmlStringRGB(speakerColor);

                SpeakerNameText.color = Color.cyan;
                currentSpeakerColorTag = "<color=#00FFFF>";
                break;
            case "Kai":
                SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                currentSpeakerColorTag = "<color=#FFD700>";
                break;
            case "Delilah":
                SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                currentSpeakerColorTag = "<color=#9919E6>";
                break;
            default:
                SpeakerNameText.color = Color.white;
                currentSpeakerColorTag = "<color=#FFFFFF>";
                break;
        }

        currentSpeakerHex = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
        StartCoroutine(PopScale(SpeakerNameText.rectTransform));
    }

    private IEnumerator PopScaleSpeaker()
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;

        DialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalCharacters = textInfo.characterCount;
        float duration = 0.2f;
        float elapsed = 0f;
        float targetScale = 1.15f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Simple bounce-out curve
            float s = 1f + Mathf.Sin(t * Mathf.PI) * (targetScale - 1f);
            SpeakerNameText.transform.localScale = originalSpeakerScale * s;
            yield return null;
        }

            DialogueText.maxVisibleCharacters = i + 1;
            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;
        SpeakerNameText.transform.localScale = originalSpeakerScale;
        scalingCoroutine = null;
    }

    private IEnumerator PopScaleEffect()
    {
        float elapsed = 0f;
        float duration = 0.2f;
        float targetScale = 1.15f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            // Simple ease-out curve using Sin
            float curve = Mathf.Sin(percent * Mathf.PI);
            SpeakerNameText.transform.localScale = originalScale * (1f + (targetScale - 1f) * curve);
            yield return null;
        }

        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        DialogueText.text = message + " ▽";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

        typingCoroutine = null;
        SpeakerNameText.transform.localScale = originalScale;
        popScaleCoroutine = null;
    }

    private IEnumerator PopEffect()
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 popScale = originalSpeakerScale * 1.2f;

        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            // Simple sin wave for the pop
            float curve = Mathf.Sin(t * Mathf.PI);
            SpeakerNameText.transform.localScale = Vector3.Lerp(originalSpeakerScale, popScale, curve);
            yield return null;
        }

        SpeakerNameText.transform.localScale = originalSpeakerScale;
        speakerPopCoroutine = null;
    }

    private IEnumerator TypeDialogue(string message)
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        DialogueText.maxVisibleCharacters = 0;

        // Ensure TMP is updated to get accurate character info

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalVisibleCharacters = textInfo.characterCount;
    /// <summary>
    /// Unified typewriter effect that handles rich text, rhythmic pauses, and skip logic.
    /// </summary>
    private IEnumerator TypeDialogue(string message)
    {
        // UX Enhancement: Color-coded completion cue ('▽') that matches the current speaker.
        // We append it immediately to ensure the full layout is calculated upfront, preventing layout shifts.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        // The message length excluding the cue (▽ is 1 char)
        int messageChars = totalCharacters - 1;

        float startTime = Time.time;
        bool hintShown = false;

        for (int i = 0; i < totalCharacters; i++)
        for (int i = 0; i <= messageChars; i++)
        {
            // UX Enhancement: Robust skip logic using persistent flag
            if (skipRequested) break;

            DialogueText.maxVisibleCharacters = i;

            // Palette: Show skip hint if player is idle for 2 seconds
            if (!playerInteracted && !hintShown && Time.time - startTime > 2f)
            {
                if (SkipHintText != null)
                {
                    SkipHintText.text = "[Space] Skip";
                    SkipHintText.gameObject.SetActive(true);
                    hintShown = true;
                }
            }

            char c = textInfo.characterInfo[i].character;
            float delay = typingSpeed;
            if (i < messageChars)
            {
                char c = textInfo.characterInfo[i].character;
                float delay = currentTypingSpeed;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                // Delay occurs after character reveal for natural rhythm.
                if (c == '.' || c == '!' || c == '?')
                {
                    // Refined ellipsis detection: if neighboring characters are also dots, it's an ellipsis
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 0 && textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                        if (i < messageChars - 1 && textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        DialogueText.text = $"{message} <color=#{cachedHexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        // The cue character is at the end. We reveal it only after the main message is typed.
        int mainMessageLength = totalVisibleCharacters - 1;

        for (int i = 0; i < mainMessageLength; i++)
        {
            if (skipRequested) break;

        for (int i = 1; i <= totalVisibleCharacters; i++)
        {
            if (skipRequested)
            {
                DialogueText.maxVisibleCharacters = totalVisibleCharacters;
                break;
            }

            char c = DialogueText.textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;
            DialogueText.maxVisibleCharacters = i;
            float delay = currentTypingSpeed;
            char c = DialogueText.textInfo.characterInfo[i - 1].character;

            // Rhythmic punctuation pauses to mimic natural speech cadence
            if (c == '.' || c == '!' || c == '?')
                delay = currentTypingSpeed * 15f;
            else if (c == ',')
                delay = currentTypingSpeed * 8f;
            {
                float delay = currentTypingSpeed;
                char c = textInfo.characterInfo[i].character;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalVisibleCharacters)
                    {
                        char nextChar = textInfo.characterInfo[i + 1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                // Look ahead to distinguish sentence endings from mid-word periods (e.g., Sky.ix)
                bool isEndOfSentence = true;
                if (i + 1 < mainMessageLength)
                {
                    char nextChar = DialogueText.textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (isEndOfSentence)
                {
                    // Refined ellipsis detection
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 0 && DialogueText.textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                        if (i + 1 < mainMessageLength && DialogueText.textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
            {
                bool isEndOfSentence = true;
                if (i < totalVisibleCharacters)
                {
                    char nextChar = DialogueText.textInfo.characterInfo[i].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (isEndOfSentence)
                {
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                        if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
                    }
                    delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                }
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

            yield return GetWait(delay);
        }

        DialogueText.maxVisibleCharacters = totalCharacters + 2;
        skipRequested = false;
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;
        skipRequested = false;
        // UX Enhancement: Reveal with completion cue already appended to prevent layout shifts.
        // The cue is color-coded to match the speaker for a subtle touch of polish.
        DialogueText.text = $"{message} <color=#{speakerHexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        int totalCharacters = DialogueText.textInfo.characterCount;

        int totalVisibleCharacters = DialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        for (int i = 0; i <= message.Length; i++)
        for (int i = 0; i <= totalCharacters; i++)
        {
            if (skipRequested)
            {
                DialogueText.maxVisibleCharacters = totalCharacters;
                break;
            }

            DialogueText.maxVisibleCharacters = i;

            if (i > 0 && i < totalCharacters)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
                float delay = currentTypingSpeed;

                // UX Enhancement: Rhythmic punctuation pauses
                if (c == '.' || c == '!' || c == '?') delay += 0.4f;
                else if (c == ',' || c == ';' || c == ':') delay += 0.2f;
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                     (i < totalCharacters && DialogueText.textInfo.characterInfo[i].character == '.');
                    delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                }
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        // We pre-calculate it once here to avoid string allocations and hex conversion in the loop or final update.
        _currentCompletionCue = $" <color=#{_currentSpeakerHex}>▽</color>";
        DialogueText.text = message + _currentCompletionCue;

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        int totalCharacters = DialogueText.textInfo.characterCount;

        // Ensure TMP is updated to get accurate character info
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        int totalVisibleCharacters = DialogueText.textInfo.characterCount;

        for (int i = 1; i <= totalVisibleCharacters; i++)
        {
            if (skipRequested)

        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalVisibleCharacters = textInfo.characterCount;

        // BOLT: Optimized single loop for typewriter effect.
        // We use GetWait(float) to ensure zero-allocation yields.
        for (int i = 0; i < totalVisibleCharacters; i++)
        {
            if (skipRequested) break;

            // Reveal character at index i
            DialogueText.maxVisibleCharacters = i + 1;
            char c = DialogueText.textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            // Rhythmic punctuation pauses
            if (c == '.' || c == '!' || c == '?')
            {
                // Check for ellipsis
                bool isEllipsis = (c == '.' && ((i > 0 && DialogueText.textInfo.characterInfo[i - 1].character == '.') || (i < totalCharacters - 1 && DialogueText.textInfo.characterInfo[i + 1].character == '.')));

                // Check for mid-word punctuation (e.g. Sky.ix)
                bool isMidWord = (c == '.' && i < totalCharacters - 1 && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i + 1].character));

                if (isMidWord) delay = currentTypingSpeed;
                else delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            // Base reveal delay
            yield return GetWait(currentTypingSpeed);

            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            // Note: Delay occurs *after* character reveal for natural rhythm.
            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            if (c == '.' || c == '!' || c == '?')
            {
                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                // Note: Delay occurs *after* character reveal for natural rhythm.
                delay += 0.4f;
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay += 0.2f;
            }

            yield return GetWait(delay);
        }

        DialogueText.maxVisibleCharacters = totalCharacters;
        skipRequested = false;
            float extraDelay = 0f;

            if (c == '.' || c == '!' || c == '?')
            {
                // Smart Punctuation: Check for ellipsis or mid-word periods (e.g., Sky.ix)
                bool isEndOfSentence = true;
                if (i + 1 < totalVisibleCharacters)
                {
                    char nextChar = textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (isEndOfSentence)
                {
                    extraDelay = currentTypingSpeed * 14f;
                }
                else if (c == '.' && i + 1 < totalVisibleCharacters && textInfo.characterInfo[i + 1].character == '.')
                {
                    extraDelay = currentTypingSpeed * 4f; // Faster dot-dot-dot
                }
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                extraDelay = currentTypingSpeed * 7f;
            }

            DialogueText.maxVisibleCharacters = i;
            float delay = currentTypingSpeed;

            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            // We check the revealed character to pause after it appears.
            char c = DialogueText.textInfo.characterInfo[i - 1].character;

            if (c == '.' || c == '!' || c == '?')
            {
                // Smart Punctuation: Detect ellipsis or end of sentence
                bool isEndOfSentence = true;
                if (i < totalVisibleCharacters)
                {
                    char nextChar = DialogueText.textInfo.characterInfo[i].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                // UX Enhancement: Rhythmic punctuation pauses for natural reading.
                // Includes look-ahead to avoid pauses in names like "Sky.ix", handles clusters (!!?), and ellipsis dots.
                if (i > 0)
                {
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // Rhythmic Look-ahead: Detect ellipses (multiple dots) and mid-word periods (like Sky.ix)
                        bool isEllipsis = (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') ||
                                          (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.');

                        bool isMidWord = i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);

                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isMidWord) delay = currentTypingSpeed; // No extra pause for mid-word punctuation
                        else delay = currentTypingSpeed * 15f;

                    // Advanced Rhythmic Pacing: Natural speech patterns via punctuation pauses.
                    if (c == '.' || c == '!' || c == '?')
                    {
                        delay = currentTypingSpeed * 15f;

                        // Look-ahead logic: Avoid long pauses for mid-word periods (e.g., Sky.ix)
                        if (i < totalVisibleCharacters)
                        {
                            char nextChar = DialogueText.textInfo.characterInfo[i].character;
                            if (!char.IsWhiteSpace(nextChar)) delay = currentTypingSpeed;
                        }
                    }
                    else if (c == ',' || c == ';' || c == ':') delay = currentTypingSpeed * 8f;
                    // Detect ellipsis by checking previous characters
                    else if (c == '.' && i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') delay = currentTypingSpeed * 5f;
                    bool isEllipsis = i > 1 && c == '.' && DialogueText.textInfo.characterInfo[i - 2].character == '.';

                    if (isEllipsis)
                    {
                        delay = currentTypingSpeed * 5f;
                    if (isEndOfSentence)
                    {
                        bool isEllipsis = (i + 1 < totalVisibleCharacters && textInfo.characterInfo[i + 1].character == '.') ||
                                         (i > 0 && textInfo.characterInfo[i - 1].character == '.');
                        delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                    }
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                    delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                }
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

            yield return GetWait(delay);
        }

        // Finalize reveal
        DialogueText.maxVisibleCharacters = totalVisibleCharacters;
        skipRequested = false;
                    if (isEllipsis)
                    {
                        delay = currentTypingSpeed * 5f;
                    }
                    else if (c == '.' || c == '!' || c == '?')
                    {
                        // Check for ellipsis (consecutive dots)
                        bool isEllipsis = false;
                        if (c == '.')
                        // Look-ahead to avoid pausing on mid-word periods (e.g., Sky.ix)
                        bool isEndOfSentence = (i >= totalVisibleCharacters) || Char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);
                        if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                    }
                    else
                    {
                        // Smart Punctuation: Look ahead to avoid pauses in middle of words (like Sky.ix)
                        bool isEndOfSentence = true;
                        if (i < messageChars - 1)
                        {
                            char nextChar = textInfo.characterInfo[i + 1].character;
                            if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                        }

                        if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                    }
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                        delay = currentTypingSpeed * 8f;
                    }
                    if (c == '.' || c == '!' || c == '?')
                    {
                        bool isEndOfCluster = true;
                        if (i < totalVisibleCharacters)
                        {
                            char nextC = DialogueText.textInfo.characterInfo[i].character;
                            if (nextC == '.' || nextC == '!' || nextC == '?') isEndOfCluster = false;
                        }

                        if (isEllipsis)
                            delay = currentTypingSpeed * 5f;
                        else if (i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character))
                            delay = currentTypingSpeed; // Mid-word period (Sky.ix)
                        else
                            delay = currentTypingSpeed * 15f;
                    }
                    else if (c == ',' || c == ';' || c == ':')
                    {
                        delay = currentTypingSpeed * 8f;
                    }
                        if (isEndOfCluster)
                        {
                            bool isEndOfSentence = true;
                            if (i < totalVisibleCharacters)
                            {
                                char nextC = DialogueText.textInfo.characterInfo[i].character;
                                if (!char.IsWhiteSpace(nextC)) isEndOfSentence = false;
                            }

                            if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                        }
                        else if (c == '.')
                        {
                            // Mid-cluster dot (ellipsis)
                            delay = currentTypingSpeed * 5f;
                        }
                    }
                    else if (c == ',' || c == ';' || c == ':' || c == '—' || c == '-')
                    {
                        delay = currentTypingSpeed * 8f;
                    }
                if (isEndOfSentence)
                {
                    // Check for ellipsis (neighboring dots)
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') isEllipsis = true;
                        if (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.') isEllipsis = true;
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

                float timer = 0f;
                while (timer < delay && !skipRequested)
                {
                    timer += Time.unscaledDeltaTime;
                    yield return null;
                }
                // BOLT: Zero-allocation yield via shared cache
        // Finalize text display
        DialogueText.maxVisibleCharacters = totalVisibleCharacters;
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

            yield return GetWait(delay);
        }

        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2; // Show the completion cue
        skipRequested = false;
            if (extraDelay > 0 && !skipRequested)
            {
                yield return GetWait(extraDelay);
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
                    char c = DialogueText.textInfo.characterInfo[i - 1].character;
                    if (c == '.' || c == '!' || c == '?')
                    {
                        // PALETTE: Check for ellipsis or mid-word periods to maintain natural flow.
                        bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') ||
                                        (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                        bool isMidWord = (i < totalVisibleCharacters && !char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character));

                // ⚡ Bolt: Use cached WaitForSeconds to avoid GC allocations in the typewriter loop
                        if (isEllipsis) delay = currentTypingSpeed * 5f;
                        else if (isMidWord) delay = currentTypingSpeed; // No pause for mid-word abbreviations
                        else delay = currentTypingSpeed * 15f;
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
                yield return GetWait(delay);
            }
            else if (i == totalVisibleCharacters && totalVisibleCharacters > 0)
            {
                // Final pause for punctuation endings before the completion cue appears
                char lastChar = DialogueText.textInfo.characterInfo[totalVisibleCharacters - 1].character;
                if (lastChar == '.' || lastChar == '!' || lastChar == '?') yield return GetWait(currentTypingSpeed * 15f);
            }
        }

        DialogueText.maxVisibleCharacters = totalCharacters;
        // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;
        skipRequested = false;
        // UX Enhancement: Visual progression cue color-coded to the speaker's theme.
        DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";
        skipRequested = false;
        DialogueText.text = message + " ▽";
            }
            else
            {
                yield return GetWait(currentTypingSpeed);
            }

            yield return GetWait(delay);
        }

        // UX Enhancement: Final pause after punctuation before showing the completion cue.
        if (!skipRequested && totalVisibleCharacters > 0)
        {
            char lastChar = DialogueText.textInfo.characterInfo[totalVisibleCharacters - 1].character;
            if (lastChar == '.' || lastChar == '!' || lastChar == '?')
            {
                yield return GetWait(currentTypingSpeed * 15f);
            }
        }

        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        // Color-coded to match the speaker's theme for better visual cohesion.
        DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";
        DialogueText.maxVisibleCharacters = totalCharacters + 2; // Show cue
        // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = totalCharacters + 2;

            string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
            DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 1;

            typingCoroutine = null;
        }

        // UX Enhancement: Visual progression cue themed to the speaker.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.ForceMeshUpdate();
        DialogueText.maxVisibleCharacters = DialogueText.textInfo.characterCount;
        }

        // Ensure all characters (including completion cue) are visible
        DialogueText.maxVisibleCharacters = textInfo.characterCount;

        // Ensure the visual cue is shown at the end by updating the text one last time with the cached cue.
        DialogueText.text = message + _currentCompletionCue;
        DialogueText.maxVisibleCharacters = totalCharacters + 2;

        typingCoroutine = null;
        yield break;
        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        // PALETTE: Color-coded cue to match the speaker's theme for a delightful touch.
        DialogueText.text = message + $" {currentSpeakerColorTag}▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

        // We do NOT reset skipRequested here. We want the skip intent to carry
        // over into the post-dialogue pause in Cinematic_IntoTheVoid_Sequence.
        skipRequested = false;
        typingCoroutine = null!;
        // Note: skipRequested is NOT reset here to allow 'fast skip' to carry over to the subsequent pause.
        typingCoroutine = null;
            DialogueText.maxVisibleCharacters = totalCharacters + 2; // Include the cue
            skipRequested = false;
            typingCoroutine = null;
        }

    /// <summary>
    /// Performs a subtle scale-up and scale-down animation to provide visual feedback.
    /// </summary>
    private IEnumerator PopScale(RectTransform target)
    {
        if (target == null) yield break;

        Vector3 originalScale = target.localScale;
        float duration = 0.15f;
        float elapsed = 0f;

        // Scale up
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.localScale = originalScale * Mathf.Lerp(1f, 1.15f, t);
            yield return null;
        }

        // Scale down
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / duration;
            target.localScale = originalScale * Mathf.Lerp(1.15f, 1f, t);
            yield return null;
        }

        target.localScale = originalScale;
    }

    /// <summary>
    /// Yields until the specified time has passed or the user requests a skip.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float time)
    {
        float startTime = Time.time;
        while (Time.time - startTime < time && !skipRequested)
        {
            yield return null;
        }
        skipRequested = false;
    }

    private IEnumerator PopScale(Transform target)
    {
        Vector3 initialScale = originalSpeakerNameScale;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;
            float scale = 1f + Mathf.Sin(percent * Mathf.PI) * 0.15f;
            target.localScale = initialScale * scale;
            yield return null;
        }

        target.localScale = initialScale;
        popCoroutine = null;
    }

    private IEnumerator WaitForSecondsOrSkip(float seconds)
    {
        float elapsed = 0f;
        while (elapsed < seconds && !skipRequested)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        skipRequested = false;
    }
    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);

        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);

        // [SCENE SETUP: Disable player controls, position cameras, set initial character states]
        DialogueBox.SetActive(true);
        yield return WaitForSecondsOrSkip(1.0f);

        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return GetWait(7.5f);

        yield return GetWait(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return GetWait(6.0f);

        yield return GetWait(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return GetWait(8.0f);

        yield return GetWait(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return GetWait(7.0f);

        yield return GetWait(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return GetWait(4.5f);

        yield return GetWait(2.0f);

        yield return GetWait(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return GetWait(3.5f);

        yield return GetWait(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return GetWait(5.5f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return GetWait(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return GetWait(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return GetWait(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return GetWait(7.0f);

        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return GetWait(4.5f);

        yield return GetWait(2.0f);

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return GetWait(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return GetWait(5.5f);

        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return GetWait(7.5f);
        yield return GetWait(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return GetWait(7.5f);
        // --- Dialogue Line 1: Delilah ---
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah, who is calmly observing the Memory Stream.]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);
        yield return WaitForSecondsOrSkip(7.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));

        // --- Dialogue Line 2: Sky.ix ---
        yield return WaitForSecondsOrSkip(0.5f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix's enraged face.]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(6.0f);
        yield return WaitForSecondsOrSkip(6.0f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(6.0f));

        // --- Dialogue Line 3: Kai ---
        yield return WaitForSecondsOrSkip(0.7f);
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai, who points towards a glowing conduit pulsating with corrupted energy.]
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(8.0f);
        yield return WaitForSecondsOrSkip(8.0f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(8.0f));

        // --- Dialogue Line 4: Delilah ---
        yield return WaitForSecondsOrSkip(1.2f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        // [CAMERA: Cut back to a low-angle shot of Delilah, making her appear dominant and unconcerned.]
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.0f);
        yield return WaitForSecondsOrSkip(7.0f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);
        yield return StartCoroutine(WaitForSecondsOrSkip(7.0f));

        // --- Dialogue Line 5: Sky.ix ---
        yield return WaitForSecondsOrSkip(0.8f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        // [CAMERA: Follow Sky.ix as she turns her body towards the conduit, cybernetics glowing.]
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(4.5f);
        yield return WaitForSecondsOrSkip(4.5f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(4.5f));

        yield return WaitForSecondsOrSkip(2.0f);

        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        yield return WaitForSecondsOrSkip(0.5f);
        // --- ACTION: Sky.ix dashes towards the conduit ---
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Dash_Forward");]
        yield return WaitForSecondsOrSkip(2.0f);
        // [VFX: Play glitchy dash particle trail from Sky.ix's starting position to the conduit.]
        // [CAMERA: Fast dolly track, following Sky.ix's movement. Add motion blur.]
        // [SFX: Play sound of cybernetic dash and energy whoosh.]
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("React_Alarmed");]
        // [CAMERA: Cut to Kai, a holographic display in front of them shows a massive energy spike warning.]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(3.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(3.5f));

        // --- Dialogue Line 7: Delilah ---
        yield return WaitForSecondsOrSkip(1.5f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        // [CAMERA: Wide shot showing Sky.ix nearing the objective, with Delilah in the background, arms spread in a mocking invitation.]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(5.5f);
        yield return WaitForSecondsOrSkip(5.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(5.5f));

        // --- Dialogue Line 8: Sky.ix ---
        yield return WaitForSecondsOrSkip(1.0f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        // [CAMERA: Extreme close-up on Sky.ix's eyes, reflecting the corrupted energy, but her expression is resolute.]
        yield return WaitForSecondsOrSkip(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);
        yield return WaitForSecondsOrSkip(7.5f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        yield return WaitForSecondsOrSkip(7.5f);
        yield return StartCoroutine(WaitForSecondsOrSkip(7.5f));

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);
        Debug.Log("Cinematic Sequence Complete.");

        Debug.Log("Cinematic Sequence Complete.");
        // [SCENE CLEANUP: Re-enable player controls, reset cameras, transition to gameplay/boss fight]
        // Example: PlayerInput.Instance.DisableControls();
        // Example: CinematicCamera.SetActive(false);
        // Example: BossFightController.StartFight();
        skipRequested = false;
        Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
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
