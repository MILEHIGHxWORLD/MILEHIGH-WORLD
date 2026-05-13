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

## 2024-05-20 - [Robust Dialogue Skip & Unity Serialization]
**Learning:** For responsive skip mechanics in Unity coroutines that include punctuation pauses, polling input in 'Update' to set a persistent flag is more reliable than per-frame 'Input' checks within the loop. Additionally, when renaming serialized UX settings (like 'typingSpeed'), using '[FormerlySerializedAs]' is critical to prevent data loss in the Unity Inspector.
**Action:** Use a 'skipRequested' flag pattern and 'FormerlySerializedAs' for all Unity UI/UX script refactors.
## 2026-03-23 - [Rhythmic Typewriter and Speaker Color-Coding]
**Learning:** Dialogue-heavy cinematics benefit from punctuation-aware typewriter timing (longer pauses after '.', '?', ',', etc.), which mimics the natural cadence of speech and improves readability. Additionally, color-coding speaker names provides an immediate visual cue for character identification, reducing cognitive load during fast-paced sequences.
**Action:** Implement rhythmic pauses in typewriter effects and use consistent character-specific color palettes for speaker UI to enhance immersion and accessibility.

## 2025-05-20 - [Rich Text Aware Typewriter Reveal]
**Learning:** In Unity TextMeshPro, iterating over string length for typewriter reveals can break when rich text tags (like <color> or <b>) are present, as the tag characters are revealed one-by-one. Using 'TMP_Text.ForceMeshUpdate()' followed by iterating over 'TMP_Text.textInfo.characterCount' ensures only rendered characters are revealed, maintaining both accessibility (screen readers) and visual polish.
**Action:** Always use 'textInfo.characterCount' and 'ForceMeshUpdate' for typewriter effects to ensure compatibility with rich text and accurate character-based pacing.

## 2025-05-21 - [Implicit Skip Hints for Better Accessibility]
**Learning:** Players may not always know a cinematic is skippable. An "idle-timer" skip hint—appearing only after a period of no interaction—provides essential guidance without cluttering the UI for experienced players. Programmatically locating these "micro-UX" elements at runtime (e.g., `transform.Find`) reduces setup friction in the Unity Inspector.
**Action:** Implement idle-triggered hints for skippable sequences and use programmatic fallback lookups for non-critical UI feedback elements.

## 2026-03-24 - [Scaling Rhythmic Typewriter and Progression Cues]
**Learning:** Rhythmic punctuation pauses in typewriter effects are most effective when they occur *after* the punctuation character is revealed (checking index `i-1`) and use multipliers (e.g., 15x, 8x) instead of fixed delays. This ensures the cadence remains natural even when base typing speeds vary by character. Additionally, appending a visual completion cue (like '▽') provides essential feedback that a dialogue block is finished and the user can proceed.
**Action:** Always use speed multipliers for rhythmic pauses and include a visual completion character after typewriter reveals to improve readability and interaction clarity.

## 2026-03-25 - [Context-Aware Typewriter Rhythms and Themed Cues]
**Learning:** Fine-tuning typewriter rhythms requires look-ahead logic to distinguish between structural punctuation (sentence ends) and semantic punctuation (mid-word periods in names like 'Sky.ix' or ellipsis dots). Applying a faster 5x delay for ellipsis and ignoring mid-word periods creates a more professional, "human-like" reading pace. Furthermore, color-coding the completion cue ('▽') to match the speaker's theme color strengthens the visual link between the dialogue and the character, enhancing immersion.
**Action:** Implement look-ahead checks for mid-word periods and ellipsis sequences in typewriter effects, and use speaker-themed colors for UI interaction cues.
## 2026-03-25 - [Ellipsis Pacing and Dynamic Completion Cues]
**Learning:** Standardizing typewriter pauses for ellipses (reduced delay) improves dialogue flow and prevents "stuttering." Additionally, dynamically color-coding the completion cue (▽) to match the speaker's theme provides a subtle but high-impact visual delight that reinforces character identification without cluttering the UI.
**Action:** Use 'ColorUtility.ToHtmlStringRGB' to capture speaker colors and implement reduced multipliers for consecutive punctuation marks (ellipses) in typewriter effects.
## 2026-03-25 - [Refined Rhythmic Pacing and Speaker-Matched UI Cues]
**Learning:** Pacing in dialogue-heavy cinematics is significantly improved by distinguishing between sentence endings (long pause), ellipses (medium pause), and mid-word periods (no pause, e.g., 'Sky.ix'). Furthermore, color-coding progress indicators (like the '▽' cue) to match the speaker's theme strengthens the visual association between the narrative content and the character, reducing cognitive load for the player.
**Action:** Implement look-ahead/look-behind logic for punctuation to refine pacing, and use speaker-specific colors for interactive UI cues via TMP rich text tags.

## 2026-03-26 - [Idle Skip Hint & Input Consolidation]
**Learning:** In narrative sequences, users may not always know they can skip dialogue. An idle timer (e.g., 2 seconds) that reveals a "Skip" hint only when the user is inactive provides guidance without cluttering the UI for experienced players. Additionally, consolidating fragmented input and coroutine logic into a single source of truth prevents race conditions and ensures that 'Input.anyKeyDown' captures all forms of interaction for maximum accessibility.
**Action:** Use an idle timer with a 'playerInteracted' flag to show hints dynamically and always unify redundant 'Update' methods to maintain a clean input state.
## 2026-03-26 - [Skip Hint Discoverability and User Agency]
**Learning:** In cinematic-heavy games, discoverability of skip mechanics is crucial for user agency, especially for returning players or those with cognitive load preferences. A "soft" hint that only appears after a short period of user inactivity (e.g., 2 seconds) provides guidance without cluttering the UI for engaged first-time viewers. Programmatically discovering UI components like hints allows for robust UX enhancements that don't strictly depend on manual Inspector setup.
**Action:** Implement idle-timer-based UI hints for secondary interactions to balance clean aesthetics with accessible discoverability.
## 2026-03-26 - [Universal Skip Accessibility and Smart Pacing]
**Learning:** Consolidating skip logic into a single 'Input.anyKeyDown' check in 'Update' is the most robust way to ensure accessibility across keyboard, mouse, and gamepads in Unity. Furthermore, rhythmic typewriter pacing is most natural when it employs 'smart' look-ahead logic to distinguish between end-of-sentence punctuation and mid-word characters (like 'Sky.ix') to avoid unintended delays.
**Action:** Always prefer 'anyKeyDown' for global interaction skips and implement look-ahead checks for rhythmic punctuation pauses.
## 2026-03-26 - [Dynamic Speaker Feedback and Visual Flash Prevention]
**Learning:** To provide subtle but effective feedback for speaker changes, a brief scaling animation ("PopScale") on the speaker's name text enhances presence without distraction. To ensure this remains stable over many interactions, caching the 'originalScale' in 'Start()' is necessary to prevent accumulation errors (drift) from interrupted animations. Additionally, to avoid a one-frame 'visual flash' of full text before a typewriter reveal starts, 'maxVisibleCharacters' must be set to 0 immediately after the text property is updated and before 'ForceMeshUpdate()' is called.
**Action:** Use cached baseline values for UI animations to prevent drift, and ensure 'maxVisibleCharacters' is zeroed before mesh updates in typewriter implementations.
## 2026-03-27 - [Unified Typewriter and Skip Logic Consolidation]
**Learning:** Overlapping and redundant implementation of typewriter effects and input polling leads to unpredictable UI behavior and syntax errors. Consolidating these into a single, clean 'TypeDialogue' coroutine and a unified 'Update' handler significantly improves reliability and maintainability.
**Action:** Always perform a structural audit of cinematic scripts to eliminate duplicate members and logic blocks before layering micro-UX improvements.
## 2025-05-21 - [Discoverable Skip Mechanics with Idle Hints]
**Learning:** For cinematic sequences, a "Skip" hint that only appears after a period of user inactivity (e.g., 2 seconds) provides a clean UI for experienced players while ensuring accessibility and discoverability for new ones. Using 'Input.anyKeyDown' for skip interactions ensures the mechanic is responsive to any deliberate user input (keyboard or mouse), making the interface feel more intuitive.
**Action:** Implement idle-timer-based UX hints for non-obvious interactions and prefer 'anyKeyDown' for global sequence skips.
## 2026-03-25 - [Dynamic Dialogue Cohesion and Responsive Skipping]
**Learning:** Micro-UX polish in dialogue systems is best achieved by unifying visual cues (like matching the completion icon color to the speaker name) and ensuring interaction responsiveness. A persistent 'skipRequested' flag that carries through both the typewriter reveal and subsequent pauses provides a much more fluid experience for fast readers.
**Action:** Always implement skippable pauses in cinematic sequences and use 'ColorUtility' to maintain visual consistency across dynamic UI elements.
## 2026-03-25 - [Layout-Safe Cinematic Dialogue Reveal]
**Learning:** Appending visual elements (like a completion cue '▽') to the end of a typewriter-revealed string can cause jarring layout shifts if the new character forces a line break or word wrap. By setting the final text (including the cue) at the beginning and using 'maxVisibleCharacters' to reveal it, the layout is pre-calculated and remains stable throughout the animation.
**Action:** Pre-append completion cues and use 'maxVisibleCharacters' to ensure visual stability in all Unity text animations.
## 2026-04-22 - [Rhythmic Typewriter and Themed Completion Cue]
**Learning:** Enhancing the typewriter effect with look-ahead logic for punctuation (like ellipses and mid-word periods) creates a much more natural reading cadence. Furthermore, theme-coloring the completion cue and setting the full text at the start of the reveal prevents jarring layout shifts and provides subtle visual delight.
**Action:** Use look-ahead logic in typewriter loops and ensure all dialogue progression cues are themed and layout-stable.

## 2026-03-25 - [Speaker Transitions and Responsive Dialogue Skipping]
**Learning:** For dialogue-heavy sequences, adding a subtle scale animation ("pop") to the speaker's name provides a clear visual cue for speaker transitions, reducing cognitive load. Additionally, a "fast skip" mechanic that carries the skip request from the typewriter reveal into the subsequent dialogue pause significantly improves the user's sense of control and pace. To ensure UI stability, any scaling animations must use a cached baseline scale to prevent incremental drift if the animation is interrupted.
**Action:** Implement 'PopScale' for speaker changes and use a persistent skip flag that is only reset after the dialogue beat pause completes.
## 2026-04-17 - [Advanced Rhythmic Typewriter & Context-Aware Pauses]
**Learning:** Typewriter effects can be further refined by distinguishing between sentence-ending punctuation and mid-word periods (e.g., abbreviations or technical names like 'Sky.ix') using look-ahead whitespace checks. Additionally, specific handling for ellipses (...) with reduced multipliers prevents the dialogue from feeling stagnant, while color-coding the completion cue to match the speaker's theme provides a subtle but effective visual anchor for the player.
**Action:** Implement look-ahead logic for punctuation and context-specific multipliers (15x for ends, 8x for clauses, 5x for ellipses) in dialogue systems to ensure natural pacing.

## 2026-05-22 - [Inclusive Interaction Hints for Accessibility]
**Learning:** Providing inclusive interaction hints, such as "[Any Key/Click] Skip" instead of specific keys like "[Space] Skip", ensures that users across different input methods (keyboard, mouse, gamepad, or assistive technology) understand that any deliberate action will trigger the skip. This transparency improves accessibility and reduces frustration for users who might not be using a standard keyboard.
**Action:** Always use inclusive language for interaction hints when the underlying logic supports multiple input types (e.g., 'Input.anyKeyDown').
