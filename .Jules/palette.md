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

## 2026-03-25 - [Unified Skip Pattern & Legibility Accessibility]
**Learning:** In Unity dialogue systems, a 'unified skip' experience (where one input skips the typewriter reveal and a second skips the remaining pause) is best achieved by sharing a persistent 'skipRequested' flag between the typewriter loop and a custom 'WaitForSecondsOrSkip' coroutine. Resetting the flag only at the *end* of the wait prevents race conditions. Additionally, ensuring legibility against unpredictable cinematic backgrounds requires a mandatory black outline (outlineWidth = 0.2f) on all dialogue TMP components.
**Action:** Always implement 'WaitForSecondsOrSkip' for cinematic pauses and apply text outlines to ensure accessibility standards are met.

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

## 2026-03-24 - [Dialogue Completion Indicator]
**Learning:** In narrative-driven interfaces, users often struggle to know exactly when a typewriter effect has finished, especially if the last sentence is short. Adding a visual "Continue" indicator (like a '▽' symbol) at the end of the reveal provides a clear affordance that the dialogue sequence is ready for the next interaction.
**Action:** Always append a completion symbol or change the UI state once a text reveal animation concludes to improve usability and reduce user uncertainty.
## 2025-05-20 - [Rich Text Aware Typewriter Reveal]
**Learning:** In Unity TextMeshPro, iterating over string length for typewriter reveals can break when rich text tags (like <color> or <b>) are present, as the tag characters are revealed one-by-one. Using 'TMP_Text.ForceMeshUpdate()' followed by iterating over 'TMP_Text.textInfo.characterCount' ensures only rendered characters are revealed, maintaining both accessibility (screen readers) and visual polish.
**Action:** Always use 'textInfo.characterCount' and 'ForceMeshUpdate' for typewriter effects to ensure compatibility with rich text and accurate character-based pacing.

## 2026-03-26 - [Discoverable Skip Hint for Narrative Immersion]
**Learning:** In narrative-heavy cinematics, a permanent "Skip" prompt can break immersion. Implementing a "discoverable" skip hint that only appears after a short period of user inactivity (e.g., 2 seconds) provides necessary guidance for new or impatient users without cluttering the screen for those fully engaged in the story. This balance preserves the cinematic atmosphere while maintaining accessibility and user agency.
**Action:** Use an 'idleTimer' and 'playerInteracted' flag pattern to trigger contextual UI hints in narrative sequences.
## 2026-03-25 - [Unified Cinematic Skip Pattern]
**Learning:** A "Unified Skip" (skipping both the typewriter reveal and the subsequent wait period with a single input) is best implemented by NOT resetting the 'skipRequested' flag after the typewriter effect ends. Instead, the flag should be consumed and reset only at the end of the final 'WaitForSecondsOrSkip' delay in the dialogue block. This ensures that player intent is fully honored across multiple yielded coroutines.
**Action:** In cinematic sequences, manage the 'skipRequested' lifecycle such that it persists through the reveal and is only cleared after the following pause is handled.
## 2026-03-25 - [Unified Cinematic Skip and Text Legibility Outlines]
**Learning:** For a smooth cinematic experience in Unity, implementing a unified skip mechanic (using a 'WaitForSecondsOrSkip' coroutine) allows players to bypass both the typewriter reveal and the subsequent pause with a single input, making the dialogue feel more responsive. Additionally, applying black outlines to TextMeshProUGUI components (outlineWidth = 0.2f, outlineColor = Color.black) is a critical accessibility pattern to ensure text legibility against dynamic or visually complex backgrounds.
**Action:** Use 'WaitForSecondsOrSkip' for dialogue pauses and always apply text outlines to mission-critical UI elements for better accessibility and polish.

## 2026-03-24 - [Scaling Rhythmic Typewriter and Progression Cues]
**Learning:** Rhythmic punctuation pauses in typewriter effects are most effective when they occur *after* the punctuation character is revealed (checking index `i-1`) and use multipliers (e.g., 15x, 8x) instead of fixed delays. This ensures the cadence remains natural even when base typing speeds vary by character. Additionally, appending a visual completion cue (like '▽') provides essential feedback that a dialogue block is finished and the user can proceed.
**Action:** Always use speed multipliers for rhythmic pauses and include a visual completion character after typewriter reveals to improve readability and interaction clarity.

## 2026-03-25 - [Context-Aware Typewriter Rhythm and Themed UI Cues]
**Learning:** Standardizing punctuation pauses in typewriter effects (e.g., 15x for periods) can be disruptive for mid-word periods (like in 'Sky.ix') or ellipses. Implementing a 'look-ahead' check to verify if the next character is whitespace ensures delays only occur at natural sentence breaks. Furthermore, color-coding UI completion cues to match the speaker's unique color palette (captured via 'ColorUtility.ToHtmlStringRGB') creates a stronger visual association and a more cohesive aesthetic experience.
**Action:** Use look-ahead logic for rhythmic pauses and thematic color-coding for supplemental UI indicators to balance pacing and character identity.
## 2026-03-25 - [Pop Scale Animation for Speaker UI]
**Learning:** Adding a subtle 'Pop' scale animation to the speaker name text when it changes provides immediate visual feedback and delight. Using a sine wave with `Time.unscaledDeltaTime` ensures the animation is smooth and independent of game time scale. To prevent "scale drift" during rapid interruptions, it is critical to capture the `initialScale` at the start of the coroutine and explicitly reset it at the end.
**Action:** Implement 'PopScale' animations for UI state changes (like speaker transitions) and always ensure scale state is restored to its baseline after completion.
## 2026-03-25 - [Context-Aware Typewriter Pacing]
**Learning:** Standard punctuation pauses in typewriter effects can be disruptive when they trigger on technical names (e.g., 'Sky.ix') or during ellipses ('...'). Using look-ahead logic to check for following whitespace ensures pauses only occur at actual sentence boundaries. Furthermore, ellipses feel more natural when given a reduced pause (e.g., 5x) compared to full sentence stops (e.g., 15x).
**Action:** Implement look-ahead checks for whitespace and specific ellipsis detection in typewriter loops to maintain rhythmic but fluid dialogue delivery.
## 2026-03-25 - [Refined Rhythmic Pacing and Speaker-Matched UI Cues]
**Learning:** Pacing in dialogue-heavy cinematics is significantly improved by distinguishing between sentence endings (long pause), ellipses (medium pause), and mid-word periods (no pause, e.g., 'Sky.ix'). Furthermore, color-coding progress indicators (like the '▽' cue) to match the speaker's theme strengthens the visual association between the narrative content and the character, reducing cognitive load for the player.
**Action:** Implement look-ahead/look-behind logic for punctuation to refine pacing, and use speaker-specific colors for interactive UI cues via TMP rich text tags.

## 2026-03-26 - [Dynamic Accessibility Outlines and Speaker Transitions]
**Learning:** For accessibility in dark environments, programmatically adding outlines to TextMeshPro components requires using `fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, 0.2f)` to avoid compilation errors on the base `TMP_Text` class. Additionally, a subtle "pop" animation (1.15x scale) on the speaker's name provides a non-verbal cue for turn-taking in dialogue, though the baseline scale must be cached in `Start()` to prevent scale drift if animations overlap.
**Action:** Use `ShaderUtilities` for runtime outline adjustments and always cache original transforms for interruption-safe UI animations.
## 2026-03-26 - [Accessible Text Outlines for Dark Cinematics]
**Learning:** In Unity cinematics with dark or high-contrast backgrounds (like 'The Void'), standard TextMeshPro text can become unreadable. Enabling the 'OUTLINE_ON' keyword and setting a subtle black outline (0.2 width) via 'ShaderUtilities' significantly improves accessibility and readability without cluttering the UI.
**Action:** Apply black text outlines to all dialogue and speaker UI in dark or visually busy cinematic sequences.
## 2026-03-26 - [Skip Hint Micro-UX for Dialogue]
**Learning:** For cinematic dialogue sequences, users may sometimes miss the ability to skip. A subtle skip hint that appears only after a short idle period (e.g., 2 seconds) and disappears once the user has interacted prevents UI clutter for experienced players while providing essential guidance for newcomers.
**Action:** Implement an idle-timer based skip hint with a 'playerInteracted' persistence flag to balance guidance and minimal UI intrusion.
## 2025-05-21 - [Dialogue Skip Hint and Refined Pacing]
**Learning:** In dialogue-heavy Unity cinematics, providing an "idle hint" (e.g., "[Space] Skip") that appears after a short period of inactivity (2s) improves accessibility without cluttering the UI for engaged players. Furthermore, consolidating fragmented typewriter logic into a single, punctuation-aware loop ensures consistent visual pacing and easier maintenance.
**Action:** Implement an 'idleTimer' and 'playerInteracted' flag for optional skip prompts and always consolidate typewriter coroutines to handle smart punctuation delays in a single pass.

## 2026-03-26 - [Discoverable Dialogue Skip via Idle Hints]
**Learning:** In cinematic dialogue systems, users may not always know they can skip text. Implementing an "idle hint" that appears after a short period of inactivity (e.g., 2s) provides discoverability without cluttering the UI for experienced players. This is best paired with a `playerInteracted` flag to ensure the hint only appears once per line until the player provides input.
**Action:** Use an `idleTimer` and `playerInteracted` flag to show contextual skip hints during dialogue sequences, resetting state on every new dialogue block.
## 2026-03-26 - [Contextual Skip Hints for Narrative Engagement]
**Learning:** Forcing players to discover skip mechanics through trial-and-error can lead to frustration, but permanent "Skip" buttons can break immersion. A contextual "Skip Hint" that only appears after a short period of user inactivity (e.g., 2 seconds) during dialogue provides a balance between assistance and cinematic immersion.
**Action:** Implement an `idleTimer` that triggers a subtle skip hint after a period of inactivity, and ensure it resets immediately upon any user interaction or dialogue transition.
## 2026-03-26 - [Smart Punctuation and Smooth Transitions]
**Learning:** Cinematic dialogue is greatly enhanced by 'smart punctuation' in typewriter effects, which uses look-ahead logic to distinguish between sentence terminators and mid-word periods (e.g., in names like 'Sky.ix'), avoiding immersion-breaking pauses. Additionally, using 'CanvasGroup' for alpha-based transitions provides a more professional feel than binary 'SetActive' calls and allows for easily adjustable transition speeds.
**Action:** Implement look-ahead checks for punctuation in typewriter loops and prefer 'CanvasGroup' fading for UI element transitions.

## 2026-03-26 - [Discoverable Skip Hint for Narrative UX]
**Learning:** In narrative-heavy sequences, users may not always know that dialogue can be skipped. Implementing a "lazy" skip hint that only appears after a period of idle time (e.g., 2 seconds) provides guidance without cluttering the UI for experienced players. This preserves immersion while ensuring accessibility for new or distracted users.
**Action:** Use an `idleTimer` and `playerInteracted` flag to show skip prompts only when the user appears stuck or inactive.
## 2026-03-26 - [Discoverable Dialogue Skip Hint]
**Learning:** Implementing a "discoverable" skip hint that only appears after a short period of inactivity (e.g., 2 seconds) during dialogue provides essential guidance for new or struggling users without cluttering the UI or breaking immersion for experienced players. This "just-in-time" assistance balances accessibility with cinematic aesthetics.
**Action:** Use an 'idleTimer' and 'playerInteracted' flag pattern in cinematic scripts to show skip prompts only when needed.
## 2026-03-25 - [Robust Cinematic UI Animations]
**Learning:** Scale-based "pop" effects for speaker identification in Unity should use 'Time.unscaledDeltaTime' to ensure consistent behavior regardless of game pauses or time-scale shifts during cinematics. Capturing the initial 'localScale' in 'Start' is critical to ensure compatibility with dynamic layouts and prevent permanent scale drift when stopping and starting overlapping animations.
**Action:** Always use 'Time.unscaledDeltaTime' for UI juice effects and capture base transform states before starting lerp-based coroutines.
## 2026-04-06 - [Context-Aware Punctuation Pauses]
**Learning:** Standard punctuation-aware typewriter pauses can be disruptive if they trigger on mid-word periods (e.g., in character names like 'Sky.ix') or feel too slow during ellipses. Look-ahead logic (checking the next character for whitespace) and specialized multipliers for consecutive dots (e.g., 5x) ensure the rhythm remains natural and uninterrupted in specialized narrative contexts.
**Action:** Implement look-ahead checks for non-whitespace characters after periods and specialized ellipsis detection in rhythmic typewriter effects to maintain conversational flow.
## 2026-03-25 - [Context-Aware Punctuation Pacing and Thematic Feedback]
**Learning:** Standard punctuation pauses can break immersion if they trigger on mid-word periods (e.g., in character names like 'Sky.ix') or feel too slow during ellipses ('...'). Look-ahead logic (checking if the next character is non-whitespace) and look-behind logic (detecting consecutive dots) allow for more natural "rhythmic" typing. Furthermore, color-coding completion cues (e.g., '▽') using speaker-specific theme colors strengthens the visual association between the dialogue and the character.
**Action:** Implement look-ahead/behind logic for typewriter punctuation to avoid inappropriate pauses and always theme visual feedback elements to match character identities.
## 2026-03-25 - [Intelligent Typewriter Rhythm and Color-Coded Cues]
**Learning:** Rhythmic punctuation pauses in typewriter effects are most effective when they handle ellipses and mid-word periods intelligently to maintain natural cadence and readability. Color-coding the dialogue completion cue to match the speaker's theme provides a subtle but consistent visual branding.
**Action:** Implement look-ahead and look-behind logic for punctuation characters in typewriter effects to distinguish between sentence ends, ellipses, and mid-word punctuation. Use character-specific color tags for visual UI cues.
## 2026-03-25 - [Unified Cinematic Skipping and Legibility Outlines]
**Learning:** In dialogue-heavy Unity cinematics, players often find it frustrating when they can skip the text reveal but remain 'locked' in the subsequent pause before the next line. Implementing a unified skip pattern where the 'skipRequested' flag persists through a custom 'WaitForSecondsOrSkip' coroutine allows for a much more responsive experience. Additionally, applying black outlines to TextMeshProUGUI components (outlineWidth = 0.2f) is a low-cost, high-impact accessibility win for ensuring legibility against varied or dark backgrounds.
**Action:** Always implement skippable pauses in cinematics to match the typewriter skip, and default to using TMP outlines for all critical dialogue UI.
## 2026-03-25 - [Context-Aware Typewriter Logic and Thematic UI]
**Learning:** Enhancing typewriter effects with context-aware logic—such as look-ahead checks to ignore pauses for mid-word periods (e.g., "Sky.ix") and rhythmic delays for ellipses—significantly improves narrative immersion. Furthermore, applying character-specific color coding to UI elements like the completion character ("▽") provides intuitive thematic cues that reinforce speaker identity without cluttering the interface.
**Action:** Implement look-ahead logic to handle edge-case punctuation and use speaker-themed rich text for UI feedback symbols.
## 2026-03-25 - [Refined Typewriter Rhythm and Character-Coded Cues]
**Learning:** Typewriter effects can feel jerky if mid-word periods (e.g., "Sky.ix") trigger full sentence-end pauses; verifying if the next character is non-whitespace before applying a delay preserves natural cadence. Additionally, color-coding progression cues (like "▽") using speaker-specific rich text tags provides a thematic "finish line" for users that reinforces character identity without cluttering the UI.
**Action:** Use look-ahead logic to distinguish sentence endings from mid-word punctuation and utilize rich-text-tagged symbols for character-specific completion feedback.
## 2026-03-25 - [Interactive Cinematic Sequences and Visual Feedback]
**Learning:** Implementing interactive cinematic sequences by replacing fixed 'GetWait' delays with skippable coroutines ('WaitForSecondsOrSkip') empowers players to control the narrative pace, significantly improving UX. Furthermore, adding subtle scale-based animations (like a 'Pop' effect) to UI elements when content changes (e.g., speaker names) provides essential visual feedback, making the interface feel more responsive and "alive."
**Action:** Always prefer skippable delays for cinematic pauses and use subtle scale animations for UI state transitions to enhance responsiveness.
## 2026-03-25 - [Rhythmic Typewriter Refinement and Thematic Cues]
**Learning:** Standard punctuation-aware typewriter effects often fail on edge cases like mid-word periods (e.g., "Sky.ix") or ellipses ("..."), leading to jarring pauses. By implementing look-ahead logic (checking for whitespace after a period) and neighbor-dot detection for ellipses, the pacing remains natural. Additionally, color-coding visual progression cues (like '▽') using speaker-specific hex colors reinforces character identity and provides a more cohesive "thematic" feel to the UI.
**Action:** Always implement look-ahead/behind checks for punctuation pauses and ensure completion cues are visually integrated with the speaker's established color palette.
## 2026-03-25 - [Unified Cinematic Skipping]
**Learning:** In interactive cinematics, players expect a single input to advance the scene meaningfully. By centralizing the 'skipRequested' flag lifecycle within a dedicated 'WaitForSecondsOrSkip' coroutine, we ensure that skipping a typewriter reveal naturally flows into skipping the subsequent pause, creating a more responsive and respectful UX.
**Action:** Implement 'WaitForSecondsOrSkip' for all cinematic delays and only reset the skip flag at the end of the pause, not the reveal.
## 2026-03-25 - [Contextual Punctuation and Theme-Linked Cues]
**Learning:** Generic punctuation-based pauses can feel jerky when encountering mid-word periods (e.g., in character names like 'Sky.ix') or ellipses. Implementing contextual checks (e.g., next-character whitespace) prevents unnatural delays. Furthermore, linking the visual progression cue ('▽') to the speaker's specific color theme creates a more cohesive and delightful UX without adding interface clutter.
**Action:** Always implement look-ahead/look-behind checks for punctuation pauses to handle abbreviations and ellipses, and leverage existing theme tokens (like speaker colors) for micro-interactions and visual cues.
## 2026-03-25 - [Unified Skipping for Cinematic Dialogue]
**Learning:** In narrative-heavy games, players often read faster than the typewriter effect reveals text. Implementing a "Unified Skip" pattern—where a single input skips both the typewriter reveal AND the subsequent fixed cinematic pause—significantly improves game flow and player agency. This is achieved by maintaining the `skipRequested` flag across both coroutines and only resetting it after the final delay in a dialogue block.
**Action:** Use a persistent `skipRequested` flag and skippable delay coroutines (`WaitForSecondsOrSkip`) to allow players to bypass both text reveal and cinematic pauses with a single interaction.
## 2026-03-25 - [Granular Typewriter Punctuation Handling]
**Learning:** Standard typewriter punctuation pauses (e.g., 15x delay for periods) can feel "broken" or stuttery when encountering ellipsis (...) or mid-word periods (e.g., "Sky.ix"). Implementing a look-ahead/look-behind check to detect these patterns allows for smoother, more natural-feeling text reveal without sacrificing the rhythmic impact of actual sentence ends.
**Action:** Always implement context-aware punctuation checks in typewriter effects: use reduced multipliers for ellipsis dots and zero extra delay for mid-word abbreviations or technical names.
## 2026-03-25 - [Unified Dialogue Skip UX]
**Learning:** In narrative-heavy Unity cinematics, players appreciate a responsive skipping system that respects their reading speed. Implementing a `WaitForSecondsOrSkip` pattern—where a single input skips both the typewriter reveal and the subsequent cinematic pause—creates a much smoother experience. Key to this is managing the `skipRequested` flag lifecycle such that it only resets *after* the final pause in a dialogue beat.
**Action:** Use `WaitForSecondsOrSkip` for all cinematic pauses and defer `skipRequested` resets until the end of the wait block to support unified skipping.
## 2024-03-28 - [Unified Skip and Visual Feedback for Cinematics]
**Learning:** In dialogue-heavy cinematics, user frustration is often caused by unskippable "cinematic pauses" that occur after the text has finished revealing. By implementing a unified skip flag that isn't reset until *after* the post-dialogue delay, users can skip both the typewriter reveal and the subsequent pause with a single input. Additionally, adding subtle visual feedback like a 'Pop' scale animation on the speaker name when it changes helps anchor the user's attention and makes the UI transition feel more intentional and polished.
**Action:** Use a 'WaitForSecondsOrSkip' pattern for all cinematic delays and always provide a subtle visual cue (e.g., scale pop, color shift) when switching speakers to improve interaction clarity and "juice".
## 2026-03-25 - [Context-Aware Punctuation Pacing]
**Learning:** Standard 15x punctuation delays in typewriter effects can feel broken when applied to mid-word periods (e.g., "Sky.ix") or ellipsis dots, which require shorter, rhythmic pauses. Verifying if a character is part of a cluster (for ellipsis) or followed by a non-whitespace character (for names/technical terms) allows the system to maintain conversational flow without technical interruptions.
**Action:** Implement look-ahead and look-behind checks for periods to distinguish between sentence ends, ellipsis dots, and mid-word abbreviations to ensure fluid dialogue pacing.
## 2026-03-25 - [Context-Aware Punctuation Pauses and Themed Feedback]
**Learning:** Standard punctuation pauses (e.g., 15x delay) can feel disruptive when applied to ellipses (...) or mid-word periods in specialized names (e.g., 'Sky.ix'). Refining these pauses to 5x for ellipsis and 0x for mid-word periods maintains narrative flow. Furthermore, color-coding completion cues (▽) to match speaker themes (using rich text tags) reinforces character identity and provides a polished, cohesive UI experience.
**Action:** Use look-ahead logic to distinguish punctuation context (ellipsis vs. end-of-sentence vs. mid-word) and apply speaker-themed rich text to all interaction cues.
## 2026-03-25 - [Accessibility and Interaction in Cinematics]
**Learning:** When adding skippable waits (`WaitForSecondsOrSkip`) to Unity cinematics, the `skipRequested` flag should be managed carefully to avoid race conditions. Resetting it only at the *start* of a new dialogue block (rather than after each skip) allows a single user input to skip both the typing animation and the following cinematic pause. For accessibility, always ensure high contrast by adding black outlines to TMP text elements in scenes with dynamic or unpredictable backgrounds.
**Action:** Use a centralized skip flag lifecycle and mandatory TMP outlines (0.2 width) for improved cinematic accessibility and control.
