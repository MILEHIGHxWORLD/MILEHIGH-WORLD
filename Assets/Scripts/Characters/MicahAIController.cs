// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Characters
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
            UnityEngine.Debug.Log($"Micah: Redirecting {force} kinetic force.");
        }
    }
}
