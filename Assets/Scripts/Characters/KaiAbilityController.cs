using UnityEngine;

namespace Milehigh.Characters
{
    public class KaiAbilityController : CharacterControllerBase
    {
        public override void ExecuteBehavior()
        {
            Debug.Log("Kai: Executing prophetic behavior.");
        }

        public void ForeseeResonanceConduit()
        {
            Debug.Log("Kai: Foreseeing resonance conduit.");
        }
    }
}
