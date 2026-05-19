// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;
using MilehighWorld.World.Core;

namespace MilehighWorld.Core
{
    public class CombatManager : MonoBehaviour
    {
        public static CombatManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public float CalculateVanguardDamage(MilehighWorld.World.Core.CharacterData attacker, float baseDamage)
        {
            float integrityMult = GlobalResonanceManager.Instance != null ? GlobalResonanceManager.Instance.GetIntegrityMultiplier() : 1.0f;
            return (baseDamage * attacker.vanguardMultiplier) * integrityMult;
        }
    }
}
