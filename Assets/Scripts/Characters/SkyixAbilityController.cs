using UnityEngine;

namespace Milehigh.Characters
{
    public class SkyixAbilityController : CharacterControllerBase
    {
        public override void ExecuteBehavior()
        {
            Debug.Log("Sky.ix: Executing ability behavior.");
        }

        public void ChannelVoidResonance()
        {
            Debug.Log("Sky.ix: Channeling Void Resonance.");
        }
    }
}
