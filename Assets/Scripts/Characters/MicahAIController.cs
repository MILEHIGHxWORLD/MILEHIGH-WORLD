using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class MicahAIController : CharacterControllerBase
    {
        public float gauntletExtrusion = 0f;

        public override void ExecuteBehavior()
        {
            // Update logic here
        }

        public void UpdateBehavior(float incomingForce, string attackType)
        {
            if (attackType == "rigid")
            {
                RedirectKineticForce(incomingForce);
            }
        }

        private void RedirectKineticForce(float force)
        {
            Debug.Log($"Micah: Redirecting {force} kinetic force.");
        }
    }
}
