using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class CyrusAIController : CharacterControllerBase
    {
        public override void ExecuteBehavior()
        {
            ExecuteInvasion();
        }

        public void ExecuteInvasion()
        {
            string target = "Onalym Nexus";
            Debug.Log($"Cyrus: Invading {target}!");
            UnleashVoidShockwave();
            OverwriteCoreLogic();
        }

        private void UnleashVoidShockwave()
        {
            Debug.Log("Cyrus: Unleashing Void Shockwave!");
        }

        private void OverwriteCoreLogic()
        {
            Debug.Log("Cyrus: Overwriting Core Logic...");
        }
    }
}
