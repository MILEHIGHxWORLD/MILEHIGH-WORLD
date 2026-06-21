using UnityEngine;
using System.Collections.Generic;

// Define a namespace for the MILEHIGH.WORLD project to prevent naming conflicts
// and organize scripts within the game's context.
namespace MilehighWorld.Story
{
    /// <summary>
    /// Helper class for Ɲōvəmîŋāđ characters (playable protagonists).
    /// This class encapsulates their properties (name, abilities) and actions (using abilities, speaking).
    /// </summary>
    public class NovomindadCharacter
    {
        public string Name { get; private set; }
        private readonly List<string> _abilities;
        private const string AbilityColor = "#00FF00"; // Green for actions
        private const string DialogueColor = "cyan"; // Cyan for dialogue
        private const string WarningColor = "yellow"; // Yellow for warnings

        public NovomindadCharacter(string name, List<string> characterAbilities)
        {
            Name = name;
            _abilities = new List<string>(characterAbilities);
        }

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
        public string Name { get; private set; }
        public bool IsCorrupted { get; private set; }
        private const string DialogueColor = "red"; // Red for enemy dialogue
        private const string ReactionColor = "orange"; // Orange for enemy reactions/actions

        public EnemyCharacter(string name, bool isCorrupted = true)
        {
            Name = name;
            IsCorrupted = isCorrupted;
        }

        public void Speak(string line)
        {
            Debug.Log($"<color={DialogueColor}>[{Name}]: {line}</color>");
        }

        public void React(string reaction)
        {
            Debug.Log($"<color={ReactionColor}>[{Name}]: {reaction}</color>");
        }
    }

    /// <summary>
    /// GeneratedScenario.cs
    /// This script implements a specific combat scene from MILEHIGH.WORLD.
    /// </summary>
    public class GeneratedScenario : MonoBehaviour
    {
        [Header("Scenario Information")]
        [SerializeField] private string mission = "Prevent King Cyrus from establishing a permanent foothold for the Void Dominion within the opulent floating cities of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§.";
        [SerializeField] private string objective = "Disrupt King Cyrus's ritualistic enthrallment upon the highest golden spire of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§.";
        [SerializeField] private string location = "ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ - The Golden Spire Summit";

        [TextArea(5, 10)]
        [SerializeField] private string initialNarrative = "The gilded spires of ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ shimmered under a perpetually sunlit sky...";

        private readonly Dictionary<string, NovomindadCharacter> _novomindad = new Dictionary<string, NovomindadCharacter>();
        private EnemyCharacter _kingCyrus;

        private const string SceneTitleColor = "#E0BBE4";
        private const string WhiteColor = "white";

        void Start()
        {
            InitializeCombatants();
            SimulateCombatSequence();
        }

        private void InitializeCombatants()
        {
            _novomindad.Add("Sky.ix", new NovomindadCharacter("Sky.ix", new List<string> { "Void Conduit Gauntlet", "Void-infused energy blasts", "Quantum Teleportation", "Void Step" }));
            _novomindad.Add("Anastasia", new NovomindadCharacter("Anastasia", new List<string> { "Oneiric Collapse", "Psychic Tethers" }));
            _novomindad.Add("Zaia", new NovomindadCharacter("Zaia", new List<string> { "Shadow Step", "Justice's Edge" }));
            _novomindad.Add("Aeron", new NovomindadCharacter("Aeron", new List<string> { "Sunder Strikes", "Grizzled Hide" }));
            _novomindad.Add("Reverie", new NovomindadCharacter("Reverie", new List<string> { "Reality Shift", "Arcane Symphony" }));
            _novomindad.Add("Cirrus", new NovomindadCharacter("Cirrus", new List<string> { "Draconic Breath", "Winged Dominion" }));

            _kingCyrus = new EnemyCharacter("King Cyrus", true);

            Debug.Log($"<color={SceneTitleColor}>--- MILEHIGH.WORLD SCENARIO: ÅẒ̌ŪŘẸ ĤĒĪĜĤṬ§ CONFLICT ---</color>");
        }

        private void SimulateCombatSequence()
        {
            _kingCyrus.Speak("Tremble, mortals, as the Age of Millenia crumbles before the might of the Void!");
            _novomindad["Reverie"].Speak("Oops! Did I do that?");
            _novomindad["Reverie"].UseAbility("Arcane Symphony");
            _novomindad["Sky.ix"].UseAbility("Void Step");
            _novomindad["Aeron"].UseAbility("Sunder Strikes");
            _novomindad["Anastasia"].UseAbility("Oneiric Collapse");
            _novomindad["Zaia"].UseAbility("Justice's Edge");
            _novomindad["Cirrus"].UseAbility("Draconic Breath");

            Debug.Log($"<color={SceneTitleColor}>--- BATTLE CONCLUDED ---</color>");
        }
    }
}
