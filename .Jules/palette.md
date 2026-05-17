## 2025-05-15 - [Optimizing Typewriter Effects for Cinematic Pacing]
**Learning:** When implementing typewriter effects in Unity, using `TextMeshProUGUI.maxVisibleCharacters` is far more performant than manual string concatenation, as it avoids frequent layout rebuilds and memory allocations. Crucially, cinematic dialogue triggers should not always block the main sequence; by not yielding on the typewriter coroutine, we preserve the original pacing and synchronization of animations and sound while still providing a polished visual reveal.
**Action:** Always use `maxVisibleCharacters` for text animation in TMP and carefully consider whether a UX effect should block or run in parallel with sequence timers.
## 2026-03-19 - [Cinematic Typewriter Effect]
**Learning:** Using 'TextMeshProUGUI.maxVisibleCharacters' to reveal text character-by-character provides a smoother, more 'cinematic' feel for dialogue sequences compared to direct text updates. It also prevents layout recalculations that can occur when modifying the text string itself.
**Action:** Apply this 'ShowDialogue' and 'TypeDialogue' pattern for all dialogue-heavy cinematic sequences to ensure consistent pacing and visual polish.
## 2025-03-15 - Initial UX Exploration
**Learning:** This Unity project primarily contains C# scripts for a cinematic sequence. Most UI is handled via TMPro in Cinematic_IntoTheVoid.cs.
**Action:** Focus on improving the dialogue UI or interactions within the cinematic script.
## 2025-03-18 - [Typewriter Effect for Cinematics]
**Learning:** For dialogue-heavy cinematics in Unity, using TextMeshPro's `maxVisibleCharacters` is the most performant and layout-safe way to implement a typewriter effect. It prevents layout shifts and handles rich text tags (like colors or bolding) correctly by revealing the already-calculated characters rather than building the string character by character.
**Action:** Use `maxVisibleCharacters` in a coroutine and always manage coroutine references (`StopCoroutine`) to prevent overlapping text animations when dialogues change quickly.
## 2024-03-20 - [Cinematic Typewriter Effect]
**Learning:** Cinematic dialogue in Unity is more engaging when revealed via a typewriter effect using `TextMeshProUGUI.maxVisibleCharacters`, as it prevents layout rebuilds while maintaining pacing.
**Action:** Use a dedicated `TypeDialogue` coroutine and `ShowDialogue` helper to manage speaker transitions and text reveal animations.
## 2026-03-21 - [Typewriter Effect in Cinematics]
**Learning:** In cinematic dialogue sequences, revealing text character-by-character (typewriter effect) using `TextMeshProUGUI.maxVisibleCharacters` provides a more engaging and readable experience than instant text display. It allows players to follow the narrative pace and prevents them from being overwhelmed by large blocks of text appearing at once. This approach also avoids layout rebuilds during the reveal, ensuring a smooth visual experience.
**Action:** Use the `TypeDialogue` coroutine pattern with a standardized character reveal delay (e.g., 0.03s) for all cinematic dialogue in future mission implementations.
# Palette's Journal - MILEHIGH.WORLD

## 2025-05-15 - Initial UX Audit
**Learning:** Unity UI (TextMeshPro) dialogue sequences often feel static if text appears instantly. A typewriter effect adds a sense of "living" dialogue and helps users pace their reading.
**Action:** Implement a typewriter effect coroutine for the `Cinematic_IntoTheVoid` script.
## 2026-03-22 - [Unified Typewriter Effect in Unity TMP]
**Learning:** Consolidating multiple, often conflicting, dialogue implementations into a single 'ShowDialogue' pattern using 'TextMeshProUGUI.maxVisibleCharacters' significantly improves UI stability and performance. Reusing 'WaitForSeconds' objects via a cache (e.g., GetWait helper) is essential for minimizing GC pressure in character-by-character text reveal loops.
**Action:** Always prefer 'maxVisibleCharacters' over manual string concatenation for text reveal animations and implement a 'GetWait' caching pattern for all high-frequency coroutine yields.

## 2026-03-24 - [Natural Pacing and Skip Support for Cinematic Dialogue]
**Learning:** A "one-speed-fits-all" typewriter effect can feel robotic. Adding subtle delays after punctuation (periods, question marks, commas) significantly improves the "readability" and natural feel of dialogue. Furthermore, providing a responsive "skip to end" interaction is critical for accessibility and user agency. This is best implemented by polling for input in `Update()` to set a flag, ensuring the skip is caught even during long punctuation pauses.
**Action:** Implement `punctuationPause` and `commaPause` logic and an asynchronous `skipRequested` flag for all cinematic dialogue systems.
## 2024-05-20 - [Robust Dialogue Skip & Unity Serialization]
**Learning:** For responsive skip mechanics in Unity coroutines that include punctuation pauses, polling input in 'Update' to set a persistent flag is more reliable than per-frame 'Input' checks within the loop. Additionally, when renaming serialized UX settings (like 'typingSpeed'), using '[FormerlySerializedAs]' is critical to prevent data loss in the Unity Inspector.
**Action:** Use a 'skipRequested' flag pattern and 'FormerlySerializedAs' for all Unity UI/UX script refactors.
## 2026-03-23 - [Rhythmic Typewriter and Speaker Color-Coding]
**Learning:** Dialogue-heavy cinematics benefit from punctuation-aware typewriter timing (longer pauses after '.', '?', ',', etc.), which mimics the natural cadence of speech and improves readability. Additionally, color-coding speaker names provides an immediate visual cue for character identification, reducing cognitive load during fast-paced sequences.
**Action:** Implement rhythmic pauses in typewriter effects and use consistent character-specific color palettes for speaker UI to enhance immersion and accessibility.

## 2025-05-20 - [Rich Text Aware Typewriter Reveal]
**Learning:** In Unity TextMeshPro, iterating over string length for typewriter reveals can break when rich text tags (like <color> or <b>) are present, as the tag characters are revealed one-by-one. Using 'TMP_Text.ForceMeshUpdate()' followed by iterating over 'TMP_Text.textInfo.characterCount' ensures only rendered characters are revealed, maintaining both accessibility (screen readers) and visual polish.
**Action:** Always use 'textInfo.characterCount' and 'ForceMeshUpdate' for typewriter effects to ensure compatibility with rich text and accurate character-based pacing.

## 2025-11-23 - [Carry-over Skip Intent in Dialogue Sequences]
**Learning:** To create a truly responsive dialogue skip mechanic, avoid resetting the 'skipRequested' flag at the end of the typewriter reveal. Instead, allow the flag to persist into the subsequent 'WaitForSecondsOrSkip' pause. This enables a single user input to both instantly reveal text and skip the following wait period, aligning with common player expectations for high-speed dialogue navigation.
**Action:** Move skip flag resets to the end of the post-dialogue pause coroutine rather than the end of the text reveal coroutine.

## 2026-03-24 - [Scaling Rhythmic Typewriter and Progression Cues]
**Learning:** Rhythmic punctuation pauses in typewriter effects are most effective when they occur *after* the punctuation character is revealed (checking index `i-1`) and use multipliers (e.g., 15x, 8x) instead of fixed delays. This ensures the cadence remains natural even when base typing speeds vary by character. Additionally, appending a visual completion character after typewriter reveals to improve readability and interaction clarity.
**Action:** Always use speed multipliers for rhythmic pauses and include a visual completion character after typewriter reveals to improve readability and interaction clarity.

## 2026-04-16 - [Persistent Skip Intent in Cinematic Sequences]
**Learning:** In dialogue-heavy cinematics, users expect a single "Skip" action to bypass both the text reveal and the subsequent post-dialogue pause. Carrying the 'skipRequested' flag through both the typewriter coroutine and a custom 'WaitForSecondsOrSkip' coroutine creates a much more responsive and intuitive feel than requiring multiple clicks per line.
**Action:** Implement 'WaitForSecondsOrSkip' and ensure 'skipRequested' flags are not prematurely reset between typing and pausing phases.
## 2026-03-25 - [Responsive Cinematic Skip & Speaker Pop Animation]
**Learning:** In cinematic dialogue sequences, user experience is significantly enhanced by making "skip" actions feel snappy and persistent. By allowing a single skip intent to carry over from the typewriter reveal to the subsequent line pause, we reduce user frustration during replay or fast reading. Additionally, a subtle (200ms) Sin-wave scale "pop" on the speaker's name provides a clear, delightful visual anchor for character transitions.
**Action:** Use a persistent `skipRequested` flag that is only reset at the end of a skippable wait cycle, and implement lightweight scale animations for UI state changes.
## 2026-03-25 - [Rhythmic Typewriter and Speaker-Themed Feedback]
**Learning:** Cinematic dialogue feels significantly more natural when the typewriter effect respects the rhythm of speech by distinguishing between sentence endings, mid-word punctuation (like "Sky.ix"), and ellipses. Furthermore, color-coding visual feedback cues (like the completion symbol '▽') to the speaker's theme provides a subtle but effective way to maintain character identity throughout the dialogue reveal.
**Action:** Implement rhythmic look-ahead logic in typewriter effects to handle edge-case punctuation and use speaker-specific hex colors for UI completion indicators to improve visual cohesion.
## 2026-03-25 - [Layout-Safe Cinematic Dialogue Reveal]
**Learning:** Appending visual elements (like a completion cue '▽') to the end of a typewriter-revealed string can cause jarring layout shifts if the new character forces a line break or word wrap. By setting the final text (including the cue) at the beginning and using 'maxVisibleCharacters' to reveal it, the layout is pre-calculated and remains stable throughout the animation.
**Action:** Pre-append completion cues and use 'maxVisibleCharacters' to ensure visual stability in all Unity text animations.
## 2026-04-17 - [Advanced Rhythmic Typewriter & Context-Aware Pauses]
**Learning:** Typewriter effects can be further refined by distinguishing between sentence-ending punctuation and mid-word periods (e.g., abbreviations or technical names like 'Sky.ix') using look-ahead whitespace checks. Additionally, specific handling for ellipses (...) with reduced multipliers prevents the dialogue from feeling stagnant, while color-coding the completion cue to match the speaker's theme provides a subtle but effective visual anchor for the player.
**Action:** Implement look-ahead logic for punctuation and context-specific multipliers (15x for ends, 8x for clauses, 5x for ellipses) in dialogue systems to ensure natural pacing.
## 2026-03-25 - [Context-Aware Typewriter Rhythms and Themed Visual Cues]
**Learning:** Simple punctuation pauses can feel unnatural if triggered by technical names (e.g., "Sky.ix") or in the middle of ellipsis ("..."). A look-ahead check for trailing whitespace ensures pauses only occur at true sentence ends. Furthermore, dynamically coloring visual progression cues (like "▽") to match the current speaker's theme color provides a subtle but effective micro-reinforcement of the speaker's identity, enhancing immersion without cluttering the UI.
**Action:** Use look-ahead logic for rhythmic punctuation pauses to avoid false positives and leverage current UI state (like name color) to theme interactive or progression indicators.

## 2026-04-18 - [Terminal Orientation and Command Feedback]
**Learning:** In terminal-style UIs, users can feel "lost" if destructive commands like 'clear' leave the screen entirely blank. Re-issuing a welcome or orientation message after clearing the display provides immediate re-orientation. Additionally, echoing user commands in a distinct, muted color (#888888) provides clear interaction history and differentiates user input from system responses.
**Action:** Always provide an orientation prompt after display resets and implement colored command echoing for interactive terminal components.

## 2026-03-26 - [Advanced Punctuation Clusters in Typewriter Effects]
**Learning:** Punctuation clusters (e.g., "!!?" or "...") can cause stuttering if every character triggers a full sentence-end pause. Implementing a look-ahead to identify the end of a cluster ensures the long pause only occurs once the full punctuation mark is revealed, maintaining a professional and intentional rhythm. Additionally, calling 'ForceMeshUpdate' before calculating final 'maxVisibleCharacters' is crucial when using rich text in TMPro to ensure the underlying 'textInfo' accurately reflects the rendered characters.
**Action:** Use cluster-aware look-ahead logic for rhythmic pauses and always force a mesh update before concluding character-based animations in TextMeshPro.
## 2026-03-25 - [Responsive Cinematic Skip & Narrative Juice]
**Learning:** In cinematic dialogue sequences, a "skip" action should ideally bypass both the typewriter reveal *and* the subsequent pause between dialogue lines. Decoupling the skip flag from the typewriter logic and using a dedicated 'WaitForSecondsOrSkip' helper ensures a much more responsive feel for players who wish to proceed quickly. Additionally, a subtle scale "pop" animation (using a sine wave) on the speaker's name whenever it changes provides a clear, high-quality visual cue for character transitions without cluttering the screen.
**Action:** Use the 'WaitForSecondsOrSkip' pattern for all skippable cinematic sequences and implement scale-based 'pop' effects for significant UI transitions.
## 2026-03-25 - [Refined Rhythmic Pacing and Speaker-Matched UI Cues]
**Learning:** Pacing in dialogue-heavy cinematics is significantly improved by distinguishing between sentence endings (long pause), ellipses (medium pause), and mid-word periods (no pause, e.g., 'Sky.ix'). Furthermore, color-coding progress indicators (like the '▽' cue) to match the speaker's theme strengthens the visual association between the narrative content and the character, reducing cognitive load for the player.
**Action:** Implement look-ahead/look-behind logic for punctuation to refine pacing, and use speaker-specific colors for interactive UI cues via TMP rich text tags.

## 2026-03-26 - [Dynamic Speaker Feedback and Visual Flash Prevention]
**Learning:** To provide subtle but effective feedback for speaker changes, a brief scaling animation ("PopScale") on the speaker's name text enhances presence without distraction. To ensure this remains stable over many interactions, caching the 'originalScale' in 'Start()' is necessary to prevent accumulation errors (drift) from interrupted animations. Additionally, to avoid a one-frame 'visual flash' of full text before a typewriter reveal starts, 'maxVisibleCharacters' must be set to 0 immediately after the text property is updated and before 'ForceMeshUpdate()' is called.
**Action:** Use cached baseline values for UI animations to prevent drift, and ensure 'maxVisibleCharacters' is zeroed before mesh updates in typewriter implementations.
## 2026-03-26 - [Unified Rhythmic Typewriter and Smart Punctuation]
**Learning:** Consolidating redundant dialogue logic into a single, robust typewriter coroutine prevents visual glitches and ensures consistent UX. Smart punctuation (detecting ellipses vs. end-of-sentence vs. mid-word periods) significantly improves the natural "breath" of digital dialogue, making it more readable and engaging.
**Action:** Use look-ahead and look-behind logic in text reveal loops to apply context-sensitive delays, and always unify duplicate input/update logic to ensure responsiveness.
## 2026-03-25 - [Contextual Punctuation and Theme-Linked Cues]
**Learning:** Generic punctuation-based pauses can feel jerky when encountering mid-word periods (e.g., in character names like 'Sky.ix') or ellipses. Implementing contextual checks (e.g., next-character whitespace) prevents unnatural delays. Furthermore, linking the visual progression cue ('▽') to the speaker's specific color theme creates a more cohesive and delightful UX without adding interface clutter.
**Action:** Always implement look-ahead/look-behind checks for punctuation pauses to handle abbreviations and ellipses, and leverage existing theme tokens (like speaker colors) for micro-interactions and visual cues.

## 2024-05-21 - [Responsive Terminal Interaction Loop]
**Learning:** Terminal interfaces in Unity (TMPro) feel significantly more responsive when they handle the full interaction lifecycle: auto-focusing on enable, clearing and refocusing after submission, and providing multi-modal feedback (rich text color + physical shake animation) for errors.
**Action:** Always implement 'ActivateInputField' in 'OnEnable' and after command processing, and use 'localPosition' coroutines for non-blocking haptic-style visual feedback.

## 2024-06-20 - [Rich Text Aware Terminal Typewriter]
**Learning:** In terminal-style UIs using TextMeshPro, raw string length is unreliable for typewriter reveals because it includes hidden Rich Text tags (e.g. <color>). Using `ForceMeshUpdate()` and `textInfo.characterCount` ensures the typewriter pacing is accurate and only reveals visible glyphs, maintaining the intended aesthetic without glitching over markup.
**Action:** Always use `textInfo.characterCount` for typewriter effects involving Rich Text to ensure accurate character-by-character reveal.

## 2024-06-21 - [Terminal Affordance and Feedback]
**Learning:** Terminal interfaces in Unity (TMPro) feel more intuitive when they provide clear affordances (placeholders) and immediate feedback (command echoing). Echoing the user's input in a muted color (#888888) creates a clear distinction between user actions and system responses while maintaining a readable interaction history.
**Action:** Always include placeholder text for input fields and implement command echoing in terminal-style UIs to improve user orientation and feedback loops.
