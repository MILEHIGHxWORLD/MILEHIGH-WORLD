using UnityEngine;
using System.Collections.Generic;

namespace Milehigh.Core
{
    public class CombatFramework : MonoBehaviour
    {
        public void RegisterCombatant(GameObject combatant)
        {
            Debug.Log($"Combat Framework: Registered {combatant.name}");
        }

        public void ProcessCombatQueue()
        {
            Debug.Log("Combat Framework: Processing combat queue.");
        }
    }
}
