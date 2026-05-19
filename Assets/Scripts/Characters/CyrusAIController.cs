// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Characters
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
            UnityEngine.Debug.Log($"Cyrus: Invading {target}!");
            UnleashVoidShockwave();
            OverwriteCoreLogic();
        }

        private void UnleashVoidShockwave()
        {
            UnityEngine.Debug.Log("Cyrus: Unleashing Void Shockwave!");
        }

        private void OverwriteCoreLogic()
        {
            UnityEngine.Debug.Log("Cyrus: Overwriting Core Logic...");
        }
    }
}
