using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class AnastasiaAIController : CharacterControllerBase
    {
        public float parityFailsThreshold = 5f;

        public override void ExecuteBehavior()
        {
            // ProcessDreamscape("stable", 0);
        }

        public void ProcessDreamscape(string geometryState, int parityFails)
        {
            if (geometryState == "brittle" || geometryState == "gray")
            {
                SnapMemoryFragments();
                if (parityFails > parityFailsThreshold)
                {
                    TriggerEnvironmentCrash();
                }
            }
            CommandReverie("toggle_form");
        }

        protected virtual void SnapMemoryFragments()
        {
            Debug.Log("Anastasia: Snapping memory fragments...");
        }

        protected virtual void TriggerEnvironmentCrash()
        {
            Debug.Log("Anastasia: Triggering environment crash!");
        }

        protected virtual void CommandReverie(string command)
        {
            Debug.Log($"Anastasia: Commanding Reverie with {command}.");
        }
    }
}
