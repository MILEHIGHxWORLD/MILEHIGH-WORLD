using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class AeronAIController : CharacterControllerBase
    {
        public float auraRadius = 10f;

        public override void ExecuteBehavior()
        {
        }

        public void AerialCombat(bool isFlappingArms, bool thrustersActive, float distanceToSolarBarrier)
        {
            if (isFlappingArms || thrustersActive)
            {
                MaintainAltitude();
            }
            else if (distanceToSolarBarrier > auraRadius)
            {
                ApplyDefenseDebuff();
            }
        }

        private void MaintainAltitude()
        {
            Debug.Log("Aeron: Maintaining altitude.");
        }

        private void ApplyDefenseDebuff()
        {
            Debug.Log("Aeron: Applying defense debuff due to distance from Solar Barrier.");
        }
    }
}
