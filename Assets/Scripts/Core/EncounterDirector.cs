using UnityEngine;
using System.Collections.Generic;

namespace MilehighWorld.Core
{
    public class EncounterDirector : MonoBehaviour
    {
        public static EncounterDirector Instance;

        private Dictionary<string, CombatantPlaceholder> _allies = new Dictionary<string, CombatantPlaceholder>();
        private Dictionary<string, CombatantPlaceholder> _enemies = new Dictionary<string, CombatantPlaceholder>();

        private void Awake()
        {
            Instance = this;
            // Initialization of placeholders for demo/orchestration purposes
            RegisterPlaceholder("Micah", true);
            RegisterPlaceholder("Sky.ix", true);
            RegisterPlaceholder("Aeron", true);
            RegisterPlaceholder("Cirrus", true);
            RegisterPlaceholder("Reverie", true);
            RegisterPlaceholder("KingCyrus", false);
        }

        private void RegisterPlaceholder(string name, bool isAlly)
        {
            var go = new GameObject(name);
            go.transform.SetParent(this.transform);
            var combatant = go.AddComponent<CombatantPlaceholder>();
            if (isAlly)
            {
                _allies[name] = combatant;
            }
            else
            {
                _enemies[name] = combatant;
            }
        }

        public CombatantPlaceholder GetAlly(string name) => _allies.TryGetValue(name, out var c) ? c : null;
        public CombatantPlaceholder GetEnemy(string name) => _enemies.TryGetValue(name, out var c) ? c : null;

        public void ApplyVoidVariance(float delta)
        {
            Debug.Log($"<color=#ff00ff>[EncounterDirector]</color> Applying Void Variance Delta: {delta}");
        }
    }

    public class CombatantPlaceholder : MonoBehaviour
    {
        public GameObject PrefabReference => this.gameObject;
        public void UseAbility(string ability) => Debug.Log($"{name} uses {ability}");
    }
}
