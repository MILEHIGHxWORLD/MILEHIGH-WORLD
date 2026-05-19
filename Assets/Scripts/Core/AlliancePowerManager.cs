// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;

namespace MilehighWorld.Core
{
    public class AlliancePowerManager : MonoBehaviour
    {
        private static AlliancePowerManager? _instance;
        public static AlliancePowerManager Instance => _instance!;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetPowerLevel(float level)
        {
            Debug.Log($"Power level set to {level}");
        }
    }
}
