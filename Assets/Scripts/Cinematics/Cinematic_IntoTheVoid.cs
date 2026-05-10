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
using UnityEngine;
using TMPro;
using System.Collections;
using ⁹System.Collections.Generic;

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
//    - Create a third TMP object named "SkipHint", set its text to "[Space] Skip", and place it in a corner.
//    - Initially, set the "DialogueBox" GameObject to be inactive.
//
// 6. Select the "SceneController" GameObject. In the Inspector, drag and drop the corresponding scene objects
//    into the public fields of this script.
//
// 7. Ensure your project has TextMeshPro imported.
// 2. Attach this script (Cinematic_IntoTheVoid.cs) to it.
// 3. Configure character prefabs (Sky.ix, Kai, Delilah) with Animators and AudioSources.
// 4. Setup UI: Canvas -> DialogueBox (Panel) -> SpeakerNameText (TMP), DialogueText (TMP), SkipHint (TMP).
// 5. Drag and drop references into the Inspector.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;

namespace Milehigh.Cinematics
{
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;

        [Header("Settings")]
        [FormerlySerializedAs("typingSpeed")]
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

        private void Update()
        {
            // Cinematic skip logic: space, return, or left mouse button
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                skipRequested = true;
            }
        }

        private void Start()
        {
            // Automated component lookup if unassigned in Inspector
            if (DialogueBox == null) DialogueBox = transform.Find("DialogueBox")?.gameObject!;
            if (SpeakerNameText == null) SpeakerNameText = GetComponentInChildren<TextMeshProUGUI>();
            if (DialogueText == null) DialogueText = transform.Find("DialogueText")?.GetComponent<TextMeshProUGUI>()!;

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        public void ShowDialogue(string speaker, string message)
        {
            if (SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic");
                return;
            }

            SpeakerNameText.text = speaker;
            // Theme colors for characters
            Color speakerColor = speaker switch
            {
                "Delilah" => new Color(0.8f, 0.2f, 0.2f), // Crimson
                "Sky.ix" => new Color(0.2f, 0.6f, 0.9f),  // Cyan
                "Kai" => new Color(0.9f, 0.8f, 0.2f),     // Amber
                _ => Color.white
            };
            SpeakerNameText.color = speakerColor;

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
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
            int totalVisibleCharacters = textInfo.characterCount;

            for (int i = 0; i < totalVisibleCharacters; i++)
            {
                if (skipRequested) break;

                DialogueText.maxVisibleCharacters = i + 1;

                char c = textInfo.characterInfo[i].character;
                float delay = currentTypingSpeed;

                // Rhythmic typewriter effect: longer pauses for punctuation to mimic natural speech
                if (c == '.' || c == '!' || c == '?')
                {
                    // Refined ellipsis detection: if neighboring characters are also dots, it's an ellipsis
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 0 && textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                        if (i < totalVisibleCharacters - 1 && textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
                    }

                    if (isEllipsis)
                        delay = currentTypingSpeed * 3f;
                    // Special case: mid-word period (e.g., Sky.ix) should have no extra delay
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

            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2; // Show the completion cue
            typingCoroutine = null;
            // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        }

        private IEnumerator WaitForSecondsOrSkip(float seconds)
        {
            float elapsed = 0;
            while (elapsed < seconds && !skipRequested)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            skipRequested = false; // Reset skip flag after the full pause (or skip) is completed
        }

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            if (DialogueBox == null) yield break;

            DialogueBox.SetActive(true);
            yield return WaitForSecondsOrSkip(1.0f);

            // --- Dialogue Line 1: Delilah ---
            ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
            yield return WaitForSecondsOrSkip(7.5f);

            // --- Dialogue Line 2: Sky.ix ---
            ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
            yield return WaitForSecondsOrSkip(6.0f);

            // --- Dialogue Line 3: Kai ---
            ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
            yield return WaitForSecondsOrSkip(8.0f);

            // --- Dialogue Line 4: Delilah ---
            ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
            yield return WaitForSecondsOrSkip(7.0f);

namespace Milehigh.Cinematics
{
            // --- Dialogue Line 5: Sky.ix ---
            ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
            yield return WaitForSecondsOrSkip(4.5f);

            // --- ACTION: Sky.ix dashes towards the conduit ---
            yield return WaitForSecondsOrSkip(2.0f);

            // --- Dialogue Line 6: Kai ---
            ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
            yield return WaitForSecondsOrSkip(3.5f);

            // --- Dialogue Line 7: Delilah ---
            ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
            yield return WaitForSecondsOrSkip(5.5f);

            // --- Dialogue Line 8: Sky.ix ---
            ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
            yield return WaitForSecondsOrSkip(7.5f);

            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            SpeakerNameText.text = "";
            DialogueText.text = "";
            DialogueBox.SetActive(false);

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
using Milehigh.Core;

namespace Milehigh.Cinematics
{
/// <summary>
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of THE VOID..."
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of ŤĤÊ VØĪĐ..."
/// Controls the cinematic sequence for "Deep within the anti-reality of ŤĤÊ VØĪĐ".
/// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of THE VOID, the very concept of existence is under assault. Delilah, an agent of entropy, has located and harnessed a 'Memory Stream'--a torrent of glitching data containing the metaphysical essence of Sky.ix's recently reunited husband and child. She intends to weaponize this stream, funneling its corrupted energy into a finality engine that will not just kill them, but permanently erase their existence from every timeline and memory. Sky.ix, whose cybernetics offer a fragile anchor in this digital abyss, must race against the unraveling of reality itself, supported by her ally Kai, to sever Delilah's connection before her family becomes nothing more than a corrupted file in the memory of the universe."
/// </summary>
public class Cinematic_IntoTheVoid : MonoBehaviour
namespace Milehigh.Cinematics
{
    [Header("Characters")]
    [Header("Character Assets")]

    [Header("Character References")]
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;
    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;
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
    // Protagonist: Sky.ix the The Bionic Goddess
    // Description: A 45-year-old Caucasian cyborg woman with short white hair. She has humanoid features but her face and body have visible cybernetic enhancements that allow her to traverse the Void.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/characters/skyix.png
    /* VOICE PROFILE: Pitch: Mid-Range Mezzo-Shorano, Tempo: Steady and Precise (130-140 WPM), Tone: Driven, Loving, Determined. Articulated. */
    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;

    public GameObject Skyix_Character = null!;
    public AudioSource Skyix_VoiceSource = null!;

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
    public GameObject Skyix_Character;
    public AudioSource Skyix_VoiceSource;

    // Protagonist: Kai the The Child of Prophecy
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;

    // Description: Sky.ix's child, lost and now found. Holds the key to the Prophecy.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/characters/kai.png
    /* VOICE PROFILE: Pitch: Gender Neutral/Mid-Range, Tempo: Slow and Paused (70-90 WPM), Tone: Cryptic, Calm, Profound, Fatalistic. */
    public GameObject Kai_Character = null!;
    public AudioSource Kai_VoiceSource = null!;

    // Ability Script: Ability_Kai.cs
    /* VOICE PROFILE:
     * Pitch: Gender Neutral/Mid-Range
     * Tempo: Slow and Paused (70-90 WPM)
     * Texture & Effects: Aged, Weathered, and Layered. Subtle Temporal Echo/Layering effect.
     * Projection: Soft, but Infinitely Resonant
     * Tone & Style: Cryptic, Calm, Profound, and Fatalistic. Speaks in metaphor.
     * Keywords: Ancient, Layered, Slow, Resonant, Cryptic, Contemplative.
    */
    public GameObject Kai_Character;
    public AudioSource Kai_VoiceSource;

    // Antagonist: Delilah the The Desolate
    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;
    // Description: A corrupted form of Ingris, wielding Voidfire.
    // Image URL: https://storage.googleapis.com/aistudio-e-i-internal-proctoring-prod.appspot.com/public-assets/antagonists/delilah.png
    public GameObject Delilah_Character = null!;
    public AudioSource Delilah_VoiceSource = null!;
    // Ability Script: Ability_Delilah.cs
    /* VOICE PROFILE:
     * Not available.
    */
    public GameObject Delilah_Character;
    public AudioSource Delilah_VoiceSource;


    [Header("UI Components")]
    public GameObject DialogueBox = null!;
    public CanvasGroup? DialogueCanvasGroup;
    public TextMeshProUGUI SpeakerNameText = null!;
    public TextMeshProUGUI DialogueText = null!;
    public TextMeshProUGUI SkipHintText = null!;
    [Tooltip("Optional: If not assigned, will try to get from DialogueBox.")]
    public CanvasGroup? DialogueCanvasGroup;
    public TextMeshProUGUI SkipHint = null!;
    public TextMeshProUGUI? SkipHint;
    public TextMeshProUGUI SkipHint = null!;
    public GameObject DialogueBox;
    public TextMeshProUGUI SpeakerNameText;
    public TextMeshProUGUI DialogueText;

    [Header("UX Settings")]
    [Tooltip("Base delay in seconds between each character being revealed.")]
    [FormerlySerializedAs("typingSpeed")]
    public float baseTypingSpeed = 0.03f;
    public float kaiSpeedMultiplier = 3.0f;
    public float skyixSpeedMultiplier = 1.2f;
    [Tooltip("Duration of dialogue box fade transitions.")]
    public float fadeDuration = 0.5f;
    public float idleHintThreshold = 2.0f;

    public TextMeshProUGUI SkipHint = null!;

    private Coroutine typingCoroutine;
    private Coroutine? typingCoroutine;
    private CanvasGroup? dialogueCanvasGroup;
    private Coroutine typingCoroutine;
    private Coroutine popCoroutine;
    private Coroutine namePopCoroutine;
    private string lastSpeaker;
    private float currentTypingSpeed;
    private string currentSpeakerHex;
    private bool skipRequested;
    private Vector3 _originalSpeakerScale;
    private Coroutine popCoroutine;
    private float idleTimer;
    private bool playerInteracted;

    // Cache for WaitForSeconds to eliminate GC allocations
    // BOLT: Shared cache for WaitForSeconds to eliminate GC allocations
    private void Update()
    {
        // UX Enhancement: Standardized skip logic for both keyboard and mouse
    // Cache for WaitForSeconds to eliminate GC allocations
    // Cache for WaitForSeconds to eliminate GC allocations during coroutine execution
    // ⚡ Bolt: Keyed by integer milliseconds to prevent cache misses from floating-point inaccuracies
    private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

    private WaitForSeconds GetWait(float time)
    /// <summary>
    /// This script controls the cinematic sequence for the mission: "Deep within the anti-reality of THE VOID, the very concept of existence is under assault. Delilah, an agent of entropy, has located and harnessed a 'Memory Stream'--a torrent of glitching data containing the metaphysical essence of Sky.ix's recently reunited husband and child. She intends to weaponize this stream, funneling its corrupted energy into a finality engine that will not just kill them, but permanently erase their existence from every timeline and memory. Sky.ix, whose cybernetics offer a fragile anchor in this digital abyss, must race against the unraveling of reality itself, supported by her ally Kai, to sever Delilah's connection before her family becomes nothing more than a corrupted file in the memory of the universe."
    /// </summary>
    public class Cinematic_IntoTheVoid : MonoBehaviour
    {
        // ⚡ Bolt: Convert time to milliseconds for reliable dictionary lookups, eliminating per-frame GC allocations from cache misses
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        int ms = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[ms] = wait;
        int timeMs = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeMs, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeMs] = wait;
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        // ⚡ Bolt: Use int key (milliseconds) to prevent dictionary cache misses from floating-point tolerance variations
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        // ⚡ Bolt: Use integer key (milliseconds) to prevent float imprecision cache misses
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        // Add an Action Delegate at the top of the class
        public event Action? OnCinematicComplete;

        // ====================================================================
        // CHARACTER ASSET & VOICE REFERENCE BLOCK
        // ====================================================================

        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        int key = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(key, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[key] = wait;
        int timeKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeKey] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int ms = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[ms] = wait;
        int msKey = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(msKey, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[msKey] = wait;
        int timeMs = Mathf.RoundToInt(time * 1000f);
        if (!_waitForSecondsCache.TryGetValue(timeMs, out var wait))
        {
            wait = new WaitForSeconds(time);
            _waitForSecondsCache[timeMs] = wait;
        [Header("Characters")]
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
        public GameObject Delilah_Character = null!;
        public AudioSource Delilah_VoiceSource = null!;

        [Header("UI Components")]
        public GameObject DialogueBox = null!;
        public CanvasGroup? DialogueCanvasGroup;
        public TextMeshProUGUI SpeakerNameText = null!;
        public TextMeshProUGUI DialogueText = null!;
        public TextMeshProUGUI? SkipHint;

        [Header("UX Settings")]
        [FormerlySerializedAs("typingSpeed")]
        [Tooltip("Base delay in seconds between each character being revealed.")]
        public float baseTypingSpeed = 0.03f;
        [Tooltip("Delay multiplier for Kai (Slow/Paused tempo).")]
        public float kaiSpeedMultiplier = 3.0f;
        [Tooltip("Delay multiplier for Skyix (Steady/Precise tempo).")]
        public float skyixSpeedMultiplier = 1.2f;

        private Coroutine? typingCoroutine;
        private Coroutine? namePopCoroutine;
        private string lastSpeaker = "";
        private float currentTypingSpeed;
        private string currentSpeakerHex = "#FFFFFF";
        private bool skipRequested;
        private float idleTimer;
        private bool playerInteracted;
        private Vector3 originalSpeakerScale;

        private void Awake()
        private static readonly Dictionary<int, WaitForSeconds> _waitForSecondsCache = new Dictionary<int, WaitForSeconds>();

        private WaitForSeconds GetWait(float time)
        {
            int ms = Mathf.RoundToInt(time * 1000f);
            if (!_waitForSecondsCache.TryGetValue(ms, out var wait))
            {
                wait = new WaitForSeconds(time);
                _waitForSecondsCache[ms] = wait;
            }
            return wait;
        }
        return wait!;
    }

    void Start()
    {
        // Poll for skip input to ensure responsiveness (anyKeyDown covers mouse/keyboard/gamepad)
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic.");
            return;
        }

        // Accessibility: Add text outlines for better readability in dark environments
        if (DialogueText.fontMaterial != null)
        {
            DialogueText.fontMaterial.EnableKeyword("OUTLINE_ON");
            DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        }
        if (SpeakerNameText.fontMaterial != null)
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown)
        {
            skipRequested = true;
            playerInteracted = true;
            idleTimer = 0f;
            if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
        }

        // UX: Display skip hint if player is idle during dialogue
        if (typingCoroutine != null && !playerInteracted)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2.0f && SkipHintText != null)
            {
                SkipHintText.gameObject.SetActive(true);
            }
        }
        if (Input.anyKeyDown || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        // Poll for skip input to ensure responsiveness across keyboard and mouse
        if (Input.anyKeyDown)
        {
            skipRequested = true;
            idleTimer = 0f;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }
        else if (DialogueBox != null && DialogueBox.activeInHierarchy)
        {
            idleTimer += Time.deltaTime;
            // Palette UX: Show skip hint after 2 seconds of inactivity
            if (idleTimer > 2f && !playerInteracted && SkipHint != null)
            {
                SkipHint.gameObject.SetActive(true);
            }
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting to prevent errors.");
            return;
        }
    }

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
    private IEnumerator WaitForSecondsOrSkip(float duration)

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
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
        // Ensure UI components are assigned before property access to prevent NullReferenceException.
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            SpeakerNameText.fontMaterial.EnableKeyword("OUTLINE_ON");
            SpeakerNameText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            SpeakerNameText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        }

        // Programmatically find SkipHint if not assigned
        if (SkipHintText == null && DialogueBox != null)
        {
            SkipHintText = DialogueBox.transform.Find("SkipHint")?.GetComponent<TextMeshProUGUI>()!;
        }

        if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    void Update()
    {
        if (Input.anyKeyDown)
        {
            skipRequested = true;
        }
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
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
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

        // Single location for skip input to ensure responsiveness and accessibility
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
        // Palette UX: Programmatically find SkipHint if not assigned
        if (SkipHint == null)
        {
            var hintObj = DialogueBox.transform.Find("SkipHint");
            if (hintObj != null) SkipHint = hintObj.GetComponent<TextMeshProUGUI>();
        }

    /// <summary>
    /// Updates the speaker name and begins the typewriter effect for the dialogue message.
    /// </summary>
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Initialize UI state
        if (DialogueCanvasGroup != null) DialogueCanvasGroup.alpha = 0;
        DialogueBox.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    public void ShowDialogue(string speaker, string message)
    void Awake()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        if (SpeakerNameText.text != speaker)
        {
            SpeakerNameText.text = speaker;
            if (popCoroutine != null) StopCoroutine(popCoroutine);
            popCoroutine = StartCoroutine(PopEffect(SpeakerNameText.transform));
        }
        SpeakerNameText.text = speaker;
        skipRequested = false;
        private Coroutine? typingCoroutine;
        private float currentTypingSpeed;
        private bool skipRequested;

        // 🎨 Palette: Speaker Name Pop animation for better visual cue
        if (popCoroutine != null) StopCoroutine(popCoroutine);
        popCoroutine = StartCoroutine(PopScale());

        // Apply speaker-specific speed multipliers based on voice profiles
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

        currentTypingSpeed = baseTypingSpeed * multiplier;
        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
    }

    private IEnumerator PopEffect(Transform target)
    {
        // Reset scale first to prevent accumulation if interrupted
        target.localScale = Vector3.one;
        Vector3 initialScale = Vector3.one;
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;
            // Use a Sin wave for a "pop" effect (1.0 -> 1.2 -> 1.0)
            float curve = Mathf.Sin(percent * Mathf.PI);
            target.localScale = initialScale + Vector3.one * (curve * 0.2f);
            yield return null;
        }

        target.localScale = initialScale;
        popCoroutine = null;
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

    private IEnumerator TypeDialogue(string message)
    {
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        // Reset interaction timer when new dialogue starts
        idleTimer = 0f;

        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        // We append it immediately to ensure the full layout is calculated upfront, preventing "jumping".
        string h9997978989979779799966768⁷exColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        int totalVisibleCharacters = DialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            if (skipRequested)
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

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

                // If skip is requested, we break the loop, so no need for WaitForSecondsOrSkip here
                yield return GetWait(delay);
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
        float multiplier = 1.0f;
        if (speaker == "Kai") multiplier = kaiSpeedMultiplier;
        else if (speaker == "Sky.ix") multiplier = skyixSpeedMultiplier;

        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;
        idleTimer = 0f; // Reset idle timer for new dialogue

        // Apply character-specific colors for better speaker identification

        Color speakerColor;
        switch (speaker)
        {
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break; // Gold
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break; // Void Purple
            default: speakerColor = Color.white; break;
        }

        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
        if (DialogueBox != null)
        {
            dialogueCanvasGroup = DialogueBox.GetComponent<CanvasGroup>();
            if (dialogueCanvasGroup == null) dialogueCanvasGroup = DialogueBox.AddComponent<CanvasGroup>();
            dialogueCanvasGroup.alpha = 0f;
        }
    }

    void Start()
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        DialogueText.maxVisibleCharacters = 0;

        DialogueText.maxVisibleCharacters = 0;

        // Ensure TMP is updated to get accurate character info
        DialogueText.ForceMeshUpdate();
        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalVisibleCharacters = textInfo.characterCount;

        for (int i = 0; i < totalVisibleCharacters; i++)
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        // 🛡️ Sentinel: Defensive programming to prevent NullReferenceException
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting.");
            return;
        }

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            // Check for sentence endings
            bool isEndOfSentence = (c == '.' || c == '!' || c == '?');
            if (isEndOfSentence)
            {
                // Refined ellipsis detection
                bool isEllipsis = false;
                if (c == '.')
                {
                    if (i > 0 && textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                    if (i < totalVisibleCharacters - 1 && textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
                }

                // Mid-word period detection (e.g. Sky.ix)
                bool isMidWord = false;
                if (c == '.' && i < totalVisibleCharacters - 1 && !char.IsWhiteSpace(textInfo.characterInfo[i + 1].character))
                {
                    isMidWord = true;
                }

                if (isEllipsis) delay = currentTypingSpeed * 5f;
                else if (isMidWord) delay = currentTypingSpeed;
                else delay = currentTypingSpeed * 15f;
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

            yield return GetWait(delay);
    void Start()
    {
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        {
            Debug.LogError("Missing UI components required for cinematic. Aborting.");
            return;
        }

        // Palette: Ensure skip hint is hidden by default
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

    void Awake()
    {
        // Force update to ensure layout is ready
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalCharacters = textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            if (skipRequested)
            {
                DialogueText.maxVisibleCharacters = totalCharacters;
                break;
            }
        if (DialogueCanvasGroup == null && DialogueBox != null)
        {
            DialogueCanvasGroup = DialogueBox.GetComponent<CanvasGroup>();
        }
        // Poll for any input to set skip flag and reset idle timer
        if (Input.anyKeyDown)
        {
            skipRequested = true;
            idleTimer = 0;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }
        else
        {
            idleTimer += Time.deltaTime;
            // Palette: Show skip hint if player is idle during dialogue
            if (idleTimer >= idleHintThreshold && !playerInteracted && DialogueBox.activeInHierarchy && SkipHint != null)
            {
                float delay = currentTypingSpeed;
                char c = DialogueText.textInfo.characterInfo[i].character;

                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = true;
                    if (i + 1 < totalVisibleCharacters)
                    {
                        char nextChar = DialogueText.textInfo.characterInfo[i + 1].character;
                        if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                    }

                    if (isEndOfSentence) delay *= 15f;
                    else delay *= 5f; // Ellipsis or mid-word
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay *= 8f;
                }
                SkipHint.gameObject.SetActive(true);
            }
        }
    }

        if (Input.anyKeyDown) skipRequested = true;
    }

    private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
    {
        if (dialogueCanvasGroup == null) yield break;
        float startAlpha = dialogueCanvasGroup.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        if (Input.anyKeyDown || (Input.GetMouseButtonDown(0)))
        {
            skipRequested = true;
            idleTimer = 0;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }
        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2.0f && !playerInteracted && SkipHint != null && DialogueBox != null && DialogueBox.activeInHierarchy)
            {
        // Poll for skip input to ensure responsiveness
        // 🎨 Palette: Prefer specific keys for skip to avoid accidental triggers
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
    private IEnumerator WaitForSecondsOrSkip(float time)
    {
        // UX: Support all input types (keyboard, mouse, gamepad) for cinematic skipping
        if (Input.anyKeyDown) skipRequested = true;
        // Poll for skip input to ensure responsiveness
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
            playerInteracted = true;
            idleTimer = 0;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }
    }

        else
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= 2f && !playerInteracted && SkipHint != null)
            {
        if (Input.anyKeyDown)
        {
            skipRequested = true;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
        }

        // UX Enhancement: Show "[Space] Skip" hint after 2 seconds of idleness during dialogue.
        // This makes the skip feature discoverable without cluttering the UI for experienced players.
        if (DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
        {
            if (SpeakerNameText != null)
                originalSpeakerScale = SpeakerNameText.transform.localScale;
        }
    }


    }

    /// <summary>
    /// Yields for the specified duration but returns immediately if a skip is requested.
    /// Resets the skip flag upon completion.
    /// </summary>
    private IEnumerator WaitForSecondsOrSkip(float duration)
        float startTime = Time.time;
        while (Time.time - startTime < time && !skipRequested)
        {
            elapsed += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        dialogueCanvasGroup.alpha = targetAlpha;
    }

        DialogueText.maxVisibleCharacters = totalVisibleCharacters;
    private IEnumerator WaitForSecondsOrSkip(float duration)
    {
        float start = Time.time;
        while (Time.time - start < duration && !skipRequested) yield return null;
        skipRequested = false;
        typingCoroutine = null;
    }

    private void Start()
    {
    private IEnumerator FadeDialogueBox(float targetAlpha, float duration)
    {
        if (DialogueCanvasGroup == null)
        {
            DialogueBox.SetActive(targetAlpha > 0);
            yield break;
        }

        if (targetAlpha > 0) DialogueBox.SetActive(true);

            if (i < totalCharacters)
            {
                float delay = currentTypingSpeed;
                char c = textInfo.characterInfo[i].character;

                // Smart Punctuation: Look ahead to avoid pauses in middle of words
                bool isEndOfSentence = true;
                if (i + 1 < totalCharacters)
                {
                    char nextChar = textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (c == '.' || c == '!' || c == '?')
                {
                    if (i + 1 < totalCharacters && textInfo.characterInfo[i + 1].character == '.')
                        delay = currentTypingSpeed * 5f; // Ellipsis
                    else if (isEndOfSentence)
                        delay = currentTypingSpeed * 15f;
        float startAlpha = DialogueCanvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        // Note: skipRequested is NOT reset here to allow skipping both the reveal and the subsequent pause
    }


        DialogueCanvasGroup.alpha = targetAlpha;
        if (targetAlpha <= 0) DialogueBox.SetActive(false);
    private IEnumerator PopScale(Transform target)
    {
        Vector3 originalScale = target.localScale;
        float elapsed = 0f;
        float duration = 0.15f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1f + Mathf.Sin((elapsed / duration) * Mathf.PI) * 0.1f;
            target.localScale = originalScale * scale;
            yield return null;
        }
        target.localScale = originalScale;
    }

    void Start()
    {
        //  Sentinel: Security enhancement - Defensive programming
        // Ensure UI components are assigned to prevent NullReferenceException and potential stack trace leakage.
        // 🛡️ Sentinel: Security enhancement - Defensive programming
        if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
        private void Update()
        {
            Debug.LogError("Missing UI components! Cinematic aborted.");
            return;
        }

        // Initialize UI state
        if (DialogueCanvasGroup != null) DialogueCanvasGroup.alpha = 0;
        DialogueBox.SetActive(false);
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        if (SkipHint == null)
        {
            // Fallback: try to find it in children if not assigned
            SkipHint = DialogueBox.GetComponentInChildren<TextMeshProUGUI>(true);
            if (SkipHint == SpeakerNameText || SkipHint == DialogueText) SkipHint = null!;
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }

        SpeakerNameText.text = speaker;
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
    /// <summary>
    /// Updates the speaker name and begins the typewriter effect for the dialogue message.
    /// </summary>
    void Update()
    {
        // Polling for space or left-click for skip functionality
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            skipRequested = true;
        }
    }

    private IEnumerator FadeDialogueBox(float targetAlpha)
    {
        if (DialogueCanvasGroup == null)
        {
            DialogueBox.SetActive(targetAlpha > 0);
            yield break;
        }

        if (targetAlpha > 0) DialogueBox.SetActive(true);

        float startAlpha = DialogueCanvasGroup.alpha;
        float elapsed = 0;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            DialogueCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        DialogueCanvasGroup.alpha = targetAlpha;
        if (targetAlpha <= 0) DialogueBox.SetActive(false);
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
        // Reset idle timer and hide hint on any interaction
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            idleTimer = 0;
            playerInteracted = true;
            if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            skipRequested = true;
        }
        else if (DialogueBox.activeInHierarchy && !playerInteracted)
        {
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleHintThreshold && SkipHint != null)
            {
                SkipHint.gameObject.SetActive(true);
            }
        }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.anyKeyDown)
            {
                skipRequested = true;
            }
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Palette UX: Improve text contrast in the dark "Void" environment via outlines.
        DialogueText.fontMaterial.EnableKeyword("OUTLINE_ON");
        DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
        DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);

        // Programmatic fallback for SkipHint to ensure UX feature works even if not assigned in inspector.
        if (SkipHint == null)
        {
            SkipHint = DialogueBox.transform.Find("SkipHint")?.GetComponent<TextMeshProUGUI>()!;
        }

        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }
        // Palette UX: Apply subtle outlines to dialogue text for improved accessibility and legibility.
        SpeakerNameText.outlineWidth = 0.2f;
        SpeakerNameText.outlineColor = Color.black;
        DialogueText.outlineWidth = 0.2f;
        DialogueText.outlineColor = Color.black;
        // PALETTE: Capture original scale for animations
        _originalSpeakerScale = SpeakerNameText.transform.localScale;

    public void ShowDialogue(string speaker, string message)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        StartCoroutine(Cinematic_IntoTheVoid_Sequence());
    }
        private void Start()
        {
            // 🛡️ Sentinel: Security enhancement - Defensive programming
            if (DialogueBox == null || SpeakerNameText == null || DialogueText == null)
            {
                Debug.LogError("Missing UI components required for cinematic.");
                return;
            }

            if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Reset interaction/idle state for new dialogue line
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

            // Palette UX: Improve text contrast in the dark "Void" environment via outlines.
            DialogueText.fontMaterial.EnableKeyword("OUTLINE_ON");
            DialogueText.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color.black);
            DialogueText.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f);
        // Apply speaker-specific speed multipliers and colors
        float multiplier = 1.0f;
        Color speakerColor = Color.white;

        // Reset idle timer and hint state for each new dialogue line
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

            StartCoroutine(Cinematic_IntoTheVoid_Sequence());
        }

        private void Update()
        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;

        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f), // Gold
            "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
            _ => Color.white
        };
        SpeakerNameText.color = speakerColor;

        typingCoroutine = StartCoroutine(TypeDialogue(message, speakerColor));

        // Apply character-specific colors for better speaker identification
        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f), // Gold
            "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
            _ => Color.white
        };
        Color speakerColor;
        switch (speaker)
        idleTimer = 0;
        playerInteracted = false;
        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // Apply character-specific colors for better speaker identification
        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f), // Gold
            "Delilah" => new Color(0.6f, 0.1f, 0.9f), // Void Purple
            _ => Color.white
        };

        SpeakerNameText.color = speakerColor;
        typingCoroutine = StartCoroutine(TypeDialogue(message));
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break; // Gold
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break; // Void Purple
            default: speakerColor = Color.white; break;
        }
        }
        SpeakerNameText.color = speakerColor;
        public void ShowDialogue(string speaker, string message)
        {
            case "Sky.ix": speakerColor = Color.cyan; break;
            case "Kai": speakerColor = new Color(1f, 0.84f, 0f); break;
            case "Delilah": speakerColor = new Color(0.6f, 0.1f, 0.9f); break;
            default: speakerColor = Color.white; break;
            case "Sky.ix":
                return Color.cyan;
            case "Kai":
                return new Color(1f, 0.84f, 0f); // Gold
            case "Delilah":
                return new Color(0.6f, 0.1f, 0.9f); // Void Purple
            default:
                return Color.white;
        }
    }

    private IEnumerator TypeDialogue(string message)
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;

                multiplier = skyixSpeedMultiplier;
                speakerColor = Color.cyan;
                break;
            case "Kai":
                multiplier = kaiSpeedMultiplier;
                speakerColor = new Color(1f, 0.84f, 0f); // Gold
                SpeakerNameText.color = Color.cyan;
                currentSpeakerHex = "#00FFFF";
                break;
            case "Kai":
                SpeakerNameText.color = new Color(1f, 0.84f, 0f); // Gold
                currentSpeakerHex = "#FFD700";
                break;
            case "Delilah":
                SpeakerNameText.color = new Color(0.6f, 0.1f, 0.9f); // Void Purple
                currentSpeakerHex = "#991AE6";
                break;
            default:
                SpeakerNameText.color = Color.white;
                currentSpeakerHex = "#FFFFFF";
                break;
        }
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            if (popCoroutine != null) StopCoroutine(popCoroutine);

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
        Color speakerColor = speaker switch
        {
            "Sky.ix" => Color.cyan,
            "Kai" => new Color(1f, 0.84f, 0f),
            "Delilah" => new Color(0.6f, 0.1f, 0.9f),
            _ => Color.white
        };
        SpeakerNameText.color = speakerColor;
        currentTypingSpeed = baseTypingSpeed * multiplier;
        skipRequested = false;

        typingCoroutine = StartCoroutine(TypeDialogue(message, speakerColor));
    }

    private IEnumerator TypeDialogue(string message, Color themeColor)
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        int totalCharacters = DialogueText.textInfo.characterCount;
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        currentSpeakerHex = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        typingCoroutine = StartCoroutine(TypeDialogue(message));
            SpeakerNameText.text = speaker;
            popCoroutine = StartCoroutine(PopScale(SpeakerNameText.transform));

    private IEnumerator PopEffect(Transform target, Vector3 baseScale)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float percent = elapsed / duration;
            // Subtle pop using a sine wave: scales up to 1.1x and back down
            float curve = Mathf.Sin(percent * Mathf.PI);
            target.localScale = baseScale * (1f + 0.1f * curve);
            yield return null;
        }
        target.localScale = baseScale;
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
        popCoroutine = null;
    }
    private IEnumerator PopScale(Transform target, float duration, float multiplier)
    {
        Vector3 initialScale = target.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            // Use a sine wave for a smooth 'pop' effect (scaling up and back down)
            float scaleFactor = 1f + Mathf.Sin(progress * Mathf.PI) * (multiplier - 1f);
            target.localScale = initialScale * scaleFactor;
            yield return null;
        }

        target.localScale = initialScale;
        namePopCoroutine = null;
    }

    private IEnumerator TypeDialogue(string message, Color color)
    {
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        // BOLT: Efficient typewriter reveal using maxVisibleCharacters
        DialogueText.text = message;
        DialogueText.maxVisibleCharacters = 0;
        string hexColor = ColorUtility.ToHtmlStringRGB(color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = 0;

        // Ensure TMP is updated to get accurate character info
        DialogueText.ForceMeshUpdate();

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();

        DialogueText.maxVisibleCharacters = 0;

        // Ensure TMP is updated to get accurate character info
        DialogueText.ForceMeshUpdate();

        TMP_TextInfo textInfo = DialogueText.textInfo;
        int totalCharacters = textInfo.characterCount;
        int messageCharacters = totalCharacters - 1; // Exclude completion cue from reveal loop

        for (int i = 0; i < messageCharacters; i++)
        for (int i = 0; i <= totalCharacters; i++)
        {
            if (skipRequested)
            {
                DialogueText.maxVisibleCharacters = totalCharacters;
                break;
            if (skipRequested) break;

            DialogueText.maxVisibleCharacters = i + 1;
            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            // We pause AFTER the character has been revealed.
            if (c == '.' || c == '!' || c == '?')
            {
                // Smart Punctuation: Look ahead to avoid pauses in mid-word (like Sky.ix).
                // We also treat the end of the message (before the cue) as an end of sentence.
                bool isEndOfSentence = true;
                if (i + 1 < messageCharacters)
                {
                    char nextChar = textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (isEndOfSentence)
                {
                    bool isEllipsis = false;
                    if (c == '.')
                    {
                        if (i > 0 && textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                        if (i + 1 < messageCharacters && textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;
                    }
                    delay = currentTypingSpeed * (isEllipsis ? 5f : 15f);
                }
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

    private IEnumerator TypeDialogue(string message)
    {
        // UX Enhancement: Color-coded completion cue that matches speaker theme.
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = $"{message} <color=#{hexColor}>▽</color>";

        DialogueText.maxVisibleCharacters = 0;
        DialogueText.ForceMeshUpdate();
        // Final reveal including the completion cue
        DialogueText.maxVisibleCharacters = totalCharacters;
        skipRequested = false; // Reset skip intent after the line is fully revealed
        int totalCharacters = DialogueText.textInfo.characterCount;
        // The cue ▽ is usually the last character
        int textOnlyCount = totalCharacters - 1;

        for (int i = 0; i <= textOnlyCount; i++)
        {
            if (skipRequested) break;
            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            // ⚡ Bolt: Use centralized UnityUtils.GetWait to avoid GC allocations
            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            if (c == '.' || c == '!' || c == '?')
            {
                // Smart Punctuation: Look ahead to avoid pauses in middle of words (like Sky.ix)
                bool isEndOfSentence = true;
                if (i + 1 < totalCharacters)
                {
                    char nextChar = textInfo.characterInfo[i + 1].character;
                    if (!char.IsWhiteSpace(nextChar)) isEndOfSentence = false;
                }

                if (isEndOfSentence) delay *= 15f;
                else delay *= 5f; // Faster pause for mid-word periods (e.g. Sky.ix)
        int totalVisibleCharacters = DialogueText.textInfo.characterCount;
        // Calculation: characterCount includes the completion cue '▽'.
        // We only want to apply rhythmic pacing to the message itself.
        int messageVisibleCount = totalVisibleCharacters - 2; // Subtracting ' ▽'

        for (int i = 0; i < totalCharacters; i++)
        {
            if (skipRequested)
            // Poll for skip input to ensure responsiveness across multiple accessible inputs
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                DialogueText.maxVisibleCharacters = totalCharacters;
                break;
            }

            // Reveal character at index i
            DialogueText.maxVisibleCharacters = i + 1;

            float delay = currentTypingSpeed;
            char c = DialogueText.textInfo.characterInfo[i].character;

            // UX Enhancement: Rhythmic punctuation pauses for natural reading.
            // These occur AFTER the character has been revealed for a more natural cadence.
            if (c == '.' || c == '!' || c == '?')
            {
                // Refined ellipsis detection: if neighboring characters are also dots, it's an ellipsis
                bool isEllipsis = (i + 1 < totalCharacters && DialogueText.textInfo.characterInfo[i + 1].character == '.') ||
                                    (i > 0 && DialogueText.textInfo.characterInfo[i - 1].character == '.');

                bool isEndOfSentence = (i + 1 >= totalCharacters) || char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i + 1].character);

                if (isEllipsis) delay = currentTypingSpeed * 5f;
                else if (isEndOfSentence) delay = currentTypingSpeed * 15f;
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay = currentTypingSpeed * 8f;
            }

            yield return GetWait(delay);
        }

        // Add the completion cue
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.ForceMeshUpdate();
        DialogueText.maxVisibleCharacters = DialogueText.textInfo.characterCount;

        int totalCharacters = DialogueText.textInfo.characterCount;

        for (int i = 0; i <= totalCharacters; i++)
        {
            if (skipRequested) break;
                skipRequested = true;
                playerInteracted = true;
                idleTimer = 0;
                if (SkipHint != null) SkipHint.gameObject.SetActive(false);
            }

            // UX Enhancement: Show skip hint after idleness
            if (DialogueBox != null && DialogueBox.activeInHierarchy && !playerInteracted && !skipRequested)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= 2.0f && SkipHint != null && !SkipHint.gameObject.activeSelf)
            DialogueText.maxVisibleCharacters = i + 1;

            // Pause AFTER the character is revealed for natural reading rhythm
            if (i > 0 && i <= totalVisibleCharacters)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
            if (i < totalCharacters)
            if (i > 0 && i < totalVisibleCharacters)
            {
                char c = DialogueText.textInfo.characterInfo[i - 1].character;
            if (i < totalCharacters - 1) // Don't delay after the last character (cue)
            if (i > 0 && i <= textOnlyCount)
            if (i > 0 && i <= totalVisibleCharacters)
            {
                char c = textInfo.characterInfo[i].character;
                float delay = currentTypingSpeed;
                char c = DialogueText.textInfo.characterInfo[i].character;

                // Rhythmic punctuation pauses for natural reading
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEllipsis = false;
                    if (i > 0 && DialogueText.textInfo.characterInfo[i - 1].character == '.') isEllipsis = true;
                    if (i < totalVisibleCharacters - 1 && DialogueText.textInfo.characterInfo[i + 1].character == '.') isEllipsis = true;

                    if (isEllipsis) delay = currentTypingSpeed * 5f;
                    else if (i == totalVisibleCharacters - 1 || char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i + 1].character))
                        delay = currentTypingSpeed * 15f;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;

                // Rhythmic punctuation pauses
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEllipsis = (i > 0 && DialogueText.textInfo.characterInfo[i-1].character == '.') ||
                                     (i < totalVisibleCharacters - 1 && DialogueText.textInfo.characterInfo[i+1].character == '.');

                    bool isEndOfSentence = (i == totalVisibleCharacters - 1) ||
                                          (i < totalVisibleCharacters - 1 && char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i+1].character));

                    if (isEllipsis) delay = currentTypingSpeed * 5f;
                    else if (isEndOfSentence) delay = currentTypingSpeed * 15f;
                    // Mid-word periods (Sky.ix) stay at base speed
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                char c = textInfo.characterInfo[i].character;

                // UX Enhancement: Rhythmic punctuation pauses for natural reading
                if (c == '.' || c == '!' || c == '?')
                {
                    bool isEndOfSentence = (i >= totalVisibleCharacters) || char.IsWhiteSpace(DialogueText.textInfo.characterInfo[i].character);
                    bool isEllipsis = (i > 1 && DialogueText.textInfo.characterInfo[i - 2].character == '.') || (i < totalVisibleCharacters && DialogueText.textInfo.characterInfo[i].character == '.');

                    if (isEllipsis) delay = currentTypingSpeed * 5f;
                    else if (c == '.' && !isEndOfSentence) delay = currentTypingSpeed; // e.g. Sky.ix
                    else delay = currentTypingSpeed * 15f;
                }
                else if (c == ',' || c == ';' || c == ':')
                {
                    delay = currentTypingSpeed * 8f;
                }
            }

            // Append color-coded completion indicator
            DialogueText.text = message + $" <color={currentSpeakerHex}>▽</color>";
            DialogueText.maxVisibleCharacters = totalVisibleCharacters + 2;

            typingCoroutine = null;
        }

        DialogueText.maxVisibleCharacters = totalVisibleCharacters;
        skipRequested = false;
        DialogueText.maxVisibleCharacters = totalCharacters;

        // UX Enhancement: Colored completion cue
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters += 2;

        // Reset interaction states after reveal
        skipRequested = false;
        idleTimer = 0f;
        if (SkipHintText != null) SkipHintText.gameObject.SetActive(false);
        // skipRequested is NOT reset here to allow the subsequent pause to be skipped
        // UX Enhancement: Visual progression cue
        string hexColor = ColorUtility.ToHtmlStringRGB(SpeakerNameText.color);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = totalCharacters + 2;

        DialogueText.ForceMeshUpdate();
        DialogueText.maxVisibleCharacters = DialogueText.textInfo.characterCount;

        private IEnumerator PopScale(Transform target, float duration, float multiplier)
        {
            Vector3 initialScale = originalSpeakerScale;
            float elapsed = 0f;
                // ⚡ Bolt: Use global yield cache to eliminate GC allocations
                yield return Milehigh.Core.UnityUtils.GetWait(delay);
                if (isEllipsis) delay *= 5f;
                else if (isEndOfSentence) delay *= 15f;
            }
            else if (c == ',' || c == ';' || c == ':')
            {
                delay *= 8f;
            }
            else if (totalVisibleCharacters > 0)
            {
                // Palette UX: Final pause after last punctuation for natural pacing before completion symbol.
                char lastC = DialogueText.textInfo.characterInfo[totalVisibleCharacters - 1].character;
                if (lastC == '.' || lastC == '!' || lastC == '?' || lastC == ',' || lastC == ';' || lastC == ':')
                {
                    yield return GetWait(currentTypingSpeed * 10f);
                }
            }
            else if (i == 0) yield return GetWait(currentTypingSpeed);

            DialogueText.maxVisibleCharacters = i + 1;
            char c = textInfo.characterInfo[i].character;
            float delay = currentTypingSpeed;

            if (c == '.' || c == '!' || c == '?')
                delay = currentTypingSpeed * 15f;
            else if (c == ',')
                delay = currentTypingSpeed * 8f;
            yield return UnityUtils.GetWait(delay);
        }

        // Note: skipRequested is NOT reset here to allow the subsequent WaitForSecondsOrSkip to also be skipped.
        skipRequested = false;
        typingCoroutine = null;
    }


    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        // [SCENE SETUP: Disable player controls, position cameras, set initial character states]
        DialogueBox.SetActive(true);
        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        string hexColor = ColorUtility.ToHtmlStringRGB(themeColor);
        DialogueText.text = message + $" <color=#{hexColor}>▽</color>";
        DialogueText.maxVisibleCharacters = totalVisibleCharacters + 1;

        DialogueText.maxVisibleCharacters = totalCharacters + 2;
        skipRequested = false;
        typingCoroutine = null;
    }
        skipRequested = false;
        DialogueText.maxVisibleCharacters = totalCharacters;
        skipRequested = false;
        typingCoroutine = null;
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
        // Note: skipRequested is NOT reset here to allow skipping the subsequent pause.
        // UX Enhancement: Visual progression cue indicating text reveal is complete.
        // Color-coded to the current speaker for a subtle touch of delight.
        DialogueText.text = $"{message} <color=#{currentSpeakerHex}>▽</color>";

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
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float progress = elapsed / duration;
        m7        float scaleFactor = 1f + Mathf.Sin(progress * Mathf.PI) * (multiplier - 1f);
                target.localScale = initialScale * scaleFactor;
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
        yield return StartCoroutine(FadeDialogueBox(1.0f));
            target.localScale = initialScale;
            namePopCoroutine = null;
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

        private IEnumerator Cinematic_IntoTheVoid_Sequence()
        {
            yield return FadeDialogueBox(1.0f, 0.5f);
            yield return WaitForSecondsOrSkip(1.0f);

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return StartCoroutine(FadeDialogueBox(1f, 0.5f));
        yield return FadeDialogueBox(1f, 0.5f);
        yield return WaitForSecondsOrSkip(1.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 1: Delilah ---
        yield return WaitForSecondsOrSkip(1.5f);
        yield return FadeDialogueBox(1.0f, 0.5f);
        yield return WaitForSecondsOrSkip(1.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. Hit the resonant conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is built on pure nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you.");
        yield return WaitForSecondsOrSkip(5.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Sky.ix", "I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(3.5f);

    private IEnumerator Cinematic_IntoTheVoid_Sequence()
    {
        DialogueBox.SetActive(true);
        yield return GetWait(1.0f);

        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        yield return WaitForSecondsOrSkip(7.5f);

        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        yield return WaitForSecondsOrSkip(6.0f);

        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        yield return WaitForSecondsOrSkip(8.0f);

        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        yield return WaitForSecondsOrSkip(7.0f);

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

        yield return WaitForSecondsOrSkip(2.0f); // Dash sequence

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 1: Delilah ---
        yield return WaitForSecondsOrSkip(1.5f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Channeling_Idle");]
        // [CAMERA: Slow dolly zoom towards Delilah, who is calmly observing the Memory Stream.]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Can you feel them, Sky.ix? Fading. Every laugh, every touch, every promise... becoming meaningless noise. It's a mercy, really. Attachments are just flaws in the code.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        // --- Dialogue Line 2: Sky.ix ---
        yield return WaitForSecondsOrSkip(0.5f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("React_Furious");]
        // [CAMERA: Quick cut to a tight close-up on Sky.ix's enraged face.]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Sky.ix", "Those 'flaws' are everything that matters! You're not cleansing anything, you're just a vandal smashing something beautiful you could never understand.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(6.0f);

        // --- Dialogue Line 3: Kai ---
        yield return WaitForSecondsOrSkip(0.7f);
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("Point_Urgent");]
        // [CAMERA: Pan to Kai, who points towards a glowing conduit pulsating with corrupted energy.]
        yield return WaitForSecondsOrSkip(0.7f);
        ShowDialogue("Kai", "Sky, don't let her distract you. Her channeling is creating a feedback loop. It's unstable, but it's shielded. I need you to hit the third resonant frequency conduit... now!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(8.0f);

        // --- Dialogue Line 4: Delilah ---
        yield return WaitForSecondsOrSkip(1.2f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Smirk_Dismissive");]
        // [CAMERA: Cut back to a low-angle shot of Delilah, making her appear dominant and unconcerned.]
        yield return WaitForSecondsOrSkip(1.2f);
        ShowDialogue("Delilah", "The little drifter thinks it's found a backdoor. How quaint. This power is not built on code you can hack. It is built on pure, unadulterated nothingness.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.0f);

        // --- Dialogue Line 5: Sky.ix ---
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        yield return WaitForSecondsOrSkip(4.5f);

        // ACTION: Sky.ix dashes towards the conduit
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        yield return WaitForSecondsOrSkip(0.5f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Action_Ready");]
        // [CAMERA: Follow Sky.ix as she turns her body towards the conduit, cybernetics glowing.]
        yield return WaitForSecondsOrSkip(0.8f);
        ShowDialogue("Sky.ix", "Then I'll just have to break it with something real. Kai, I see it! I'm going in!");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(4.5f);
        yield return WaitForSecondsOrSkip(4.5f);

        yield return WaitForSecondsOrSkip(2.0f);

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);


        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);


        // --- Dialogue Line 6: Kai ---

        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        yield return WaitForSecondsOrSkip(3.5f);

        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        yield return WaitForSecondsOrSkip(5.5f);


        // --- ACTION: Sky.ix dashes towards the conduit ---
        // [ANIMATION: Dash_Forward] [VFX: Glitchy dash particle trail] [CAMERA: Fast dolly track] [SFX: Cybernetic dash whoosh]
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        yield return WaitForSecondsOrSkip(0.5f);

        // --- Dialogue Line 6: Kai ---
        yield return WaitForSecondsOrSkip(0.5f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Dash_Forward");]
        // [VFX: Play glitchy dash particle trail from Sky.ix's starting position to the conduit.]
        // [CAMERA: Fast dolly track, following Sky.ix's movement. Add motion blur.]
        // [SFX: Play sound of cybernetic dash and energy whoosh.]
        yield return WaitForSecondsOrSkip(2.0f);

        // --- Dialogue Line 6: Kai ---
        // [ANIMATION: Kai_Character.GetComponent<Animator>().SetTrigger("React_Alarmed");]
        // [CAMERA: Cut to Kai, holographic energy spike warning.]
        yield return WaitForSecondsOrSkip(0.5f);
        ShowDialogue("Kai", "The energy spike is massive! Your shields won't hold for long!");
        // Kai_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(3.5f);

        // --- Dialogue Line 7: Delilah ---
        // [ANIMATION: Taunt_OpenArms] [CAMERA: Wide shot showing Sky.ix nearing objective]
        yield return WaitForSecondsOrSkip(1.5f);
        // [ANIMATION: Delilah_Character.GetComponent<Animator>().SetTrigger("Taunt_OpenArms");]
        // [CAMERA: Wide shot showing Sky.ix nearing the objective, with Delilah in the background, arms spread in a mocking invitation.]
        yield return WaitForSecondsOrSkip(1.5f);
        ShowDialogue("Delilah", "Come then. Offer your existence to the glitch. Join your precious family in the great deletion.");
        // Delilah_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(5.5f);

        // --- Dialogue Line 8: Sky.ix ---
        // [ANIMATION: Determined_Resolve] [CAMERA: Extreme close-up on Sky.ix's eyes]
        yield return WaitForSecondsOrSkip(1.0f);
        // [ANIMATION: Skyix_Character.GetComponent<Animator>().SetTrigger("Determined_Resolve");]
        // [CAMERA: Extreme close-up on Sky.ix's eyes, reflecting the corrupted energy, but her expression is resolute.]
        yield return WaitForSecondsOrSkip(1.0f);
        ShowDialogue("Sky.ix", "My family is my anchor. They are the reason I can walk through this hell and not become a monster like you. And I am bringing them home.");
        // Skyix_VoiceSource.Play();
        yield return WaitForSecondsOrSkip(7.5f);

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        yield return StartCoroutine(FadeDialogueBox(0.0f));
        yield return StartCoroutine(FadeDialogueBox(0f, 0.5f));
        yield return FadeDialogueBox(0f, 0.5f);

        Debug.Log("Cinematic Sequence Complete.");
        SpeakerNameText.text = "";
        DialogueText.text = "";
        DialogueBox.SetActive(false);
        Debug.Log("Cinematic Sequence Complete.");
        DialogueBox.SetActive(false);        if (SkipHint != null) SkipHint.gameObject.SetActive(false);

        // [SCENE CLEANUP: Re-enable player controls, reset cameras, transition to boss fight]
        // Example: PlayerInput.Instance.EnableControls();
        // Example: BossFightController.StartFight();
        Debug.Log("Cinematic Sequence Complete.");
        Debug.Log("Cinematic Sequence Complete.");
        yield return FadeDialogueBox(0.0f, 0.5f);

        Debug.Log("Cinematic Sequence Complete.");
        // [SCENE CLEANUP: Re-enable player controls, reset cameras, transition to gameplay/boss fight]
        // Example: PlayerInput.Instance.DisableControls();
        // Example: CinematicCamera.SetActive(false);
        // Example: BossFightController.StartFight();
        Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of THE VOID...]");
        Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
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

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");
            Debug.Log("Cinematic Sequence Complete.");
            yield return FadeDialogueBox(0.0f, 0.5f);

            Debug.Log("Cinematic Sequence Complete: [Deep within the anti-reality of ŤĤÊ VØĪĐ...]");

            // Invoke the event so the Battle Orchestrator knows to begin combat
            OnCinematicComplete?.Invoke();
        }
    }
}
