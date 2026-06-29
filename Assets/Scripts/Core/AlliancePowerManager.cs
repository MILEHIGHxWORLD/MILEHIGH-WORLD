// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;

namespace MilehighWorld.Core
{
    public class AlliancePowerManager : MonoBehaviour
    {
        private static AlliancePowerManager? _instance;
        public static AlliancePowerManager Instance => _instance!;

        public float CurrentPowerLevel { get; private set; }

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
            CurrentPowerLevel = level;
            Debug.Log($"Power level set to {level}");
        }

        /// <summary>
        /// Resets the singleton instance. Use only for testing.
        /// </summary>
        public static void ResetInstanceForTesting()
        {
            _instance = null;
        }
    }
}
