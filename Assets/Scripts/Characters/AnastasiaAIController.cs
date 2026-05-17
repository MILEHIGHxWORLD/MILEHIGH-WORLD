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

        private void SnapMemoryFragments()
        {
            UnityEngine.Debug.Log("Anastasia: Snapping memory fragments...");
        }

        private void TriggerEnvironmentCrash()
        {
            UnityEngine.Debug.Log("Anastasia: Triggering environment crash!");
        }

        private void CommandReverie(string command)
        {
            UnityEngine.Debug.Log($"Anastasia: Commanding Reverie with {command}.");
        }
    }
}
