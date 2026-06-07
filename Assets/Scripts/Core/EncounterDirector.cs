using UnityEngine;

namespace MilehighWorld.Core
{
    public class EncounterDirector : MonoBehaviour
    {
        public static EncounterDirector Instance;
        private void Awake() => Instance = this;

        public void ApplyVoidVariance(float delta)
        {
            Debug.Log($"<color=#ff00ff>[EncounterDirector]</color> Applying Void Variance Delta: {delta}");
            // Future logic to interact with GlobalResonanceManager
        }

        // Extension methods to support EndGameMultiFrontOrchestrator
        public ICombatant GetAlly(string name)
        {
            Debug.Log($"[EncounterDirector] Getting ally: {name}");
            return null; // Implementation deferred
        }

        public ICombatant GetEnemy(string name)
        {
            Debug.Log($"[EncounterDirector] Getting enemy: {name}");
            return null; // Implementation deferred
        }
    }

    public interface ICombatant
    {
        GameObject PrefabReference { get; }
        void UseAbility(string ability);
    }
}
