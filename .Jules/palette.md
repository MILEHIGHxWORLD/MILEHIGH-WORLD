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
