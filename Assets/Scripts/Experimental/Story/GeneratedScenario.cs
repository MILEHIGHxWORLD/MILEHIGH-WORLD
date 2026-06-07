using UnityEngine;
using System.Collections.Generic;

// Define a namespace for the MILEHIGH.WORLD project to prevent naming conflicts
// and organize scripts within the game's context.
namespace MilehighWorld
{
    /// <summary>
    /// Helper class for Ɲōvəmîŋāđ characters (playable protagonists).
    /// This class encapsulates their properties (name, abilities) and actions (using abilities, speaking).
    /// </summary>
    public class NovomindadCharacter
    {
        /// <summary>
        /// The name of the character.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A private list of abilities the character possesses.
        /// Stored as a private list to ensure internal control over modification.
        /// </summary>
        private readonly List<string> _abilities;

        // Constants for rich text colors for character actions and dialogue.
        private const string AbilityColor = "#00FF00"; // Green for actions
        private const string DialogueColor = "cyan";    // Cyan for dialogue
        private const string WarningColor = "yellow";   // Yellow for warnings

        /// <summary>
        /// Constructor to initialize the character with a name and their abilities.
        /// Creates a defensive copy of the abilities list to prevent external modification.
        /// </summary>
        /// <param name="name">The name of the Novomindad character.</param>
        /// <param name="characterAbilities">A list of abilities the character possesses.</param>
        public NovomindadCharacter(string name, List<string> characterAbilities)
        {
            Name = name;
            // Create a new list from the provided collection to prevent external modification
            // of the internal abilities list (defensive copying).
            _abilities = new List<string>(characterAbilities);
        }

        /// <summary>
        /// Simulates a character using an ability and logs it to the console.
        /// Console output uses rich text for better readability, indicating character actions in green.
        /// If the character does not possess the ability, a warning is logged.
        /// </summary>
        /// <param name="ability">The name of the ability to use.</param>
        public void UseAbility(string ability)
        {
            if (_abilities.Contains(ability))
            {
                Debug.Log($"<color={AbilityColor}>[{Name}]: Uses '{ability}'!</color>");
            }
            else
            {
                Debug.LogWarning($"<color={WarningColor}>[{Name}]: Tries to use '{ability}' but does not possess this ability.</color>");
            }
        }

        /// <summary>
        /// Simulates a character speaking and logs their dialogue to the console.
        /// Console output uses rich text for better readability, indicating character dialogue in cyan.
        /// </summary>
        /// <param name="line">The dialogue line spoken by the character.</param>
        public void Speak(string line)
        {
            Debug.Log($"<color={DialogueColor}>[{Name}]: {line}</color>");
        }
    }

    /// <summary>
    /// Helper class for enemy characters (antagonists).
    /// This class encapsulates their properties (name, corruption status) and actions (speaking, reacting).
    /// </summary>
    public class EnemyCharacter
    {
        /// <summary>
        /// The name of the enemy character.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Indicates if the enemy is Void-corrupted.
        /// </summary>
        public bool IsCorrupted { get; private set; }

        // Constants for rich text colors for enemy dialogue and reactions.
        private const string DialogueColor = "red";    // Red for enemy dialogue
        private const string ReactionColor = "orange"; // Orange for enemy reactions/actions

        /// <summary>
        /// Constructor to initialize the enemy character.
        /// </summary>
        /// <param name="name">The name of the enemy.</param>
        /// <param name="isCorrupted">Optional. True if the enemy is Void-corrupted, false otherwise. Defaults to true.</param>
        public EnemyCharacter(string name, bool isCorrupted = true)
        {
            Name = name;
            IsCorrupted = isCorrupted;
        }

        /// <summary>
        /// Simulates an enemy speaking and logs their dialogue to the console.
        /// Console output uses rich text for better readability, indicating enemy dialogue in red.
        /// </summary>
        /// <param name="line">The dialogue line spoken by the enemy.</param>
        public void Speak(string line)
        {
            Debug.Log($"<color={DialogueColor}>[{Name}]: {line}</color>");
        }

        /// <summary>
        /// Simulates an enemy reacting to an event or taking an action, and logs it to the console.
        /// Console output uses rich text for better readability, indicating enemy reactions/actions in orange.
        /// </summary>
        /// <param name="reaction">The reaction or action performed by the enemy.</param>
        public void React(string reaction)
        {
            Debug.Log($"<color={ReactionColor}>[{Name}]: {reaction}</color>");
        }
    }

    /// <summary>
    /// GeneratedScenario.cs
    /// This script implements a specific combat scene from MILEHIGH.WORLD.
    /// It acts as a CombatManager to orchestrate events, dialogue, and character actions
    /// in a predefined, sequential narrative flow.
    /// </summary>
    public class GeneratedScenario : MonoBehaviour
    {
        // --- SCENE METADATA ---
        // These fields provide context for the scenario and are visible in the Unity Inspector.
        [Header("Scenario Information")]
        [Tooltip("The overarching mission for this scenario.")]
        [SerializeField] private string mission = "Prevent King Cyrus from establishing a permanent foothold for the Void Dominion within the opulent floating cities of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ and thereby initiating the 'Age of Millenia' under his tyrannical rule.";

        [Tooltip("The specific objective to be completed in this scenario.")]
        [SerializeField] private string objective = "Disrupt King Cyrus's ritualistic enthrallment upon the highest golden spire of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§, forcing his retreat and safeguarding the Onalym Nexus from immediate Void corruption.";

        [Tooltip("The location where the scenario takes place.")]
        [SerializeField] private string location = "ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ - The Golden Spire Summit";

        [TextArea(5, 10)]
        [Tooltip("The initial narrative setting the scene before combat begins.")]
        [SerializeField] private string initialNarrative = "The gilded spires of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ shimmered under a perpetually sunlit sky, now marred by a growing digital distortion emanating from the highest peak. King Cyrus, a towering figure wreathed in dark energy, stood poised to claim the spire's apex as his throne, his presence causing visible glitches in the surrounding anti-gravity platforms. His voice, amplified by the Void, boomed across the city, threatening its very existence.";

        [TextArea(5, 10)]
        [Tooltip("Additional lore and context related to this specific scenario.")]
        [SerializeField] private string loreDeepDive = "This scenario in ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ perfectly embodies the 'Neo-Arcane Fractured Realism' aesthetic, blending the pristine architecture and advanced anti-gravity tech with the organic chaos of Reverie's Arcane Symphony and the digital entropy of the Void. King Cyrus's attempt to enthrone himself here signifies the Void Dominion's strategy to corrupt key, powerful locations to usher in the 'Final Judgment' instead of the promised 'Millenia'. The fracturing platforms directly visualize the Void as a 'digital abyss' that corrupts and erases data/identity, even in the physical environment. The deployment of multiple Ɲōvəmîŋāđ highlights their diverse abilities and cooperative potential. Reverie's trickster nature and 'Reality Shift' are crucial for environmental control, setting the stage for Sky.ix's precise 'Void Step' and energy blasts. Anastasia's remote 'Oneiric Collapse' demonstrates her unique ability to project power, even while physically distant, showcasing the multi-dimensional nature of MILEHIGH.WORLD. Zaia's 'Justice's Edge' against corrupted targets is lore-consistent, positioning her as a direct counter to Void forces. Aeron's noble charge and aerial prowess are characteristic of his warrior ethos, while Cirrus's conflicted participation, especially his grumbling acceptance of fighting his own father, is a pivotal narrative element, emphasizing the personal stakes and the overarching struggle within the Ɲōvəmîŋāđ to unite against the Void, even when faced with deeply personal conflict. This interaction further emphasizes the critical choice between Millenia and Final Judgment.";

        // --- GAME ENTITIES ---
        /// <summary>
        /// A dictionary to store the playable protagonists (Ɲōvəmîŋāđ) by their names for easy access.
        /// Marked as `readonly` since its reference won't change after initialization.
        /// </summary>
        private readonly Dictionary<string, NovomindadCharacter> _novomindad = new Dictionary<string, NovomindadCharacter>();

        /// <summary>
        /// The main antagonist for this scene.
        /// </summary>
        private EnemyCharacter _kingCyrus;

        // Constants for general console logging colors.
        private const string SceneTitleColor = "#E0BBE4"; // Light purple for titles
        private const string WhiteColor = "white";        // White for general narrative

        /// <summary>
        /// Called when the script instance is being loaded.
        /// This method serves as the entry point for the scenario,
        /// initializing all combatants and then simulating the combat sequence.
        /// </summary>
        void Start()
        {
            InitializeCombatants();
            SimulateCombatSequence();
        }

        /// <summary>
        /// Sets up the Ɲōvəmîŋāđ characters and the enemy King Cyrus with their respective abilities.
        /// Also logs initial scene information to the console for context.
        /// </summary>
        private void InitializeCombatants()
        {
            // Initialize Ɲōvəmîŋāđ characters with their signature abilities as provided in the lore.
            _novomindad.Add("Sky.ix", new NovomindadCharacter("Sky.ix", new List<string> { "Void Conduit Gauntlet", "Void-infused energy blasts", "Quantum Teleportation", "Void Step" }));
            _novomindad.Add("Anastasia", new NovomindadCharacter("Anastasia", new List<string> { "Oneiric Collapse", "Psychic Tethers", "Healing via psychic tethers" }));
            _novomindad.Add("Zaia", new NovomindadCharacter("Zaia", new List<string> { "Shadow Step", "Justice's Edge" }));
            _novomindad.Add("Aeron", new NovomindadCharacter("Aeron", new List<string> { "Sunder Strikes", "Grizzled Hide", "Inspiring Roars" }));
            _novomindad.Add("Reverie", new NovomindadCharacter("Reverie", new List<string> { "Reality Shift", "Arcane Symphony", "Glitching Terrain" }));
            _novomindad.Add("Cirrus", new NovomindadCharacter("Cirrus", new List<string> { "Draconic Breath", "Winged Dominion", "Primal Fury" }));

            // Initialize the main antagonist, King Cyrus, noting his corrupted status.
            _kingCyrus = new EnemyCharacter("King Cyrus", true);

            // Log initial scene information to the console for context.
            Debug.Log($"<color={SceneTitleColor}>--- MILEHIGH.WORLD SCENARIO: ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ CONFLICT ---</color>");
            Debug.Log($"<color={WhiteColor}><b>Mission:</b> {mission}</color>");
            Debug.Log($"<color={WhiteColor}><b>Objective:</b> {objective}</color>");
            Debug.Log($"<color={WhiteColor}><b>Location:</b> {location}</color>");
            Debug.Log($"<color={WhiteColor}><b>Initial Narrative:</b>\n{initialNarrative}</color>");
            Debug.Log($"<color={SceneTitleColor}>--- BATTLE COMMENCING ---</color>");
        }

        /// <summary>
        /// Simulates the sequence of combat events and dialogue as described in the scenario.
        /// This method acts as the "CombatManager" for this specific scene,
        /// sequentially triggering character actions and logging narrative beats.
        /// </summary>
        private void SimulateCombatSequence()
        {
            // --- EVENT 1: King Cyrus's Opening Threat ---
            // King Cyrus begins his ritual with a declaration.
            _kingCyrus.Speak("Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!");
            Debug.Log("<i>King Cyrus prepares his ritual upon the Golden Spire, his dark energy distorting the opulent surroundings.</i>");

            // --- EVENT 2: Reverie's Environmental Disruption ---
            // Reverie initiates the attack by using her trickster abilities to destabilize King Cyrus's position.
            _novomindad["Reverie"].Speak("Oops! Did I do that? Looks like your fancy throne is having a bad day, Cyrus!");
            _novomindad["Reverie"].UseAbility("Arcane Symphony");
            Debug.Log("<i>The pristine white and gold anti-gravity platforms around Cyrus's perch flicker violently, their energy conduits spasming with digital static. The elegant anti-gravity technology of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ becomes a deathtrap, shifting and collapsing with fractured realism, a direct manifestation of the Void's influence mixed with Reverie's 'Reality Shift'.</i>");

            // --- EVENT 3: Sky.ix's Precise Assault ---
            // Sky.ix coordinates with Reverie, using her advanced cybernetics and Void powers to attack.
            _novomindad["Sky.ix"].Speak("Maintain focus, Reverie! Keep those platforms unstable. I'm finding the optimal phase vector.");
            _novomindad["Sky.ix"].UseAbility("Void Step");
            Debug.Log("<i>Sky.ix becomes a blur of quantum light, executing precise 'Void Steps' between vanishing platforms, her cybernetic enhancements glowing.</i>");
            _novomindad["Sky.ix"].UseAbility("Void-infused energy blasts");
            Debug.Log("<i>She charges her Void Conduit Gauntlet, unleashing torrents of Void-infused energy blasts at King Cyrus, forcing him to momentarily abandon his focus on the ritual.</i>");

            // --- EVENT 4: Aeron's Noble Charge ---
            // Aeron performs an aerial dive, showcasing his warrior prowess.
            _novomindad["Aeron"].Speak("Your dominion ends here, fiend! For The Verse!");
            _novomindad["Aeron"].UseAbility("Sunder Strikes");
            Debug.Log("<i>Aeron, the winged lion warrior, soars through the treacherous sky, his powerful wings creating gusts that further destabilize Cyrus's position, diving towards the King with claws extended.</i>");

            // --- EVENT 5: Anastasia's Psychic Control ---
            // Anastasia attempts to bind King Cyrus with dream logic, demonstrating her unique power projection.
            _novomindad["Anastasia"].Speak("(Voice resonating with ethereal calm) Your reality begins to fray, Cyrus. Allow me to assist it along.");
            _novomindad["Anastasia"].UseAbility("Oneiric Collapse");
            Debug.Log("<i>Anastasia, her physical form distant in the Dreamscape, projects her will, attempting to snare Cyrus in 'Oneiric Collapse', threads of psychic energy coiling around his limbs, causing his movements to become sluggish and disjointed, as if caught in a dream's illogic.</i>");
            _kingCyrus.React("snarled, his eyes glowing with dark power, momentarily breaking free from Anastasia's psychic grip."); // King Cyrus's reaction to Anastasia's CC.

            // --- EVENT 6: Zaia's Tactical Strike ---
            // Zaia leverages the ongoing disruption to strike a critical blow against the corrupted King.
            _novomindad["Zaia"].Speak("Justice will find its mark, even in fractured reality.");
            _novomindad["Zaia"].UseAbility("Shadow Step");
            Debug.Log("<i>From the shadows between crystalline structures, Zaia 'Shadow Stepped' onto a precarious ledge, 'Justice's Edge' glowing with a righteous azure light.</i>");
            _novomindad["Zaia"].UseAbility("Justice's Edge");
            Debug.Log("<i>She moves with deadly precision, targeting weaknesses exposed by Anastasia's and Reverie's disruptions, her strikes carrying bonus damage against the corrupted essence of Cyrus.</i>");

            // --- EVENT 7: Cirrus's Conflicted Contribution ---
            // Cirrus reluctantly aids the Ɲōvəmîŋāđ against his own father, highlighting his internal conflict.
            _novomindad["Cirrus"].Speak("(Grumbling, unleashing fire) Father, you leave me no choice… This is not the way.");
            _novomindad["Cirrus"].UseAbility("Draconic Breath");
            Debug.Log("<i>Even as his father roared in defiance, Cirrus, the conflicted Dragon King, reluctantly joins the fray, unleashing a concentrated 'Draconic Breath' – a stream of pure elemental fire – towards Cyrus, adding a primal force to the multifaceted assault.</i>");

            // --- EVENT 8: King Cyrus's Defiance and Sky.ix's Final Push ---
            // The combined assault severely disrupts King Cyrus, leading to a final exchange.
            Debug.Log("<i>The golden spire, though resilient, begins to show scorch marks and digital decay, a testament to the combined might of the Ɲōvəmîŋāđ. King Cyrus's ritual is severely disrupted.</i>");
            _kingCyrus.Speak("You fools! This realm will be but a jewel in the Void's endless crown!");
            _novomindad["Sky.ix"].Speak("Negative. Your reign ends with this spire. Engaging Void Conduit at maximum output.");
            Debug.Log("<i>The collective assault forces King Cyrus to retreat, his ritual incomplete and his immediate designs on ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ thwarted. The immediate threat to ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ is averted.</i>");

            // --- SCENE CONCLUSION ---
            // Final logs summarizing the outcome and providing the lore deep dive.
            Debug.Log($"<color={SceneTitleColor}>--- BATTLE CONCLUDED ---</color>");
            Debug.Log($"<color={WhiteColor}><b>Outcome:</b> King Cyrus's ritual was disrupted, preventing a permanent Void foothold in ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§. Objective achieved.</color>");
            Debug.Log($"<color={WhiteColor}><b>Lore Deep Dive:</b>\n{loreDeepDive}</color>");
            Debug.Log($"<color={SceneTitleColor}>--- SCENARIO END ---</color>");
        }
    }
}
