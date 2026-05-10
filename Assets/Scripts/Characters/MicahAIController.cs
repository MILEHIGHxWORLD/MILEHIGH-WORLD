using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    // Error 8: Wrapped in Milehigh.Characters namespace
    public class MicahController : CharacterControllerBase
    {
        public float gauntletExtrusion = 0f;

        public override void ExecuteBehavior()
        {
            // Update logic here
            Debug.Log("Micah: Executing AI behavior.");
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
