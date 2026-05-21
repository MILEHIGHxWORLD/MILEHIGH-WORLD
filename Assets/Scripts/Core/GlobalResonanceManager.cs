// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;

namespace MilehighWorld.Core
{
    public class GlobalResonanceManager : MonoBehaviour
    {
        public static GlobalResonanceManager Instance;
        public float resonanceFactor = 0.5f;
        public float CurrentVoidVariance => resonanceFactor;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public float GetIntegrityMultiplier()
        {
            float integrity = 1.0f - CurrentVoidVariance;
            return Mathf.Clamp01(integrity);
        }

        public void RedirectVoidData(byte[] rawData)
        {
            if (System.BitConverter.IsLittleEndian)
            {
                System.Array.Reverse(rawData);
            }
            ProcessData(rawData);
        }

        public void UpdateResonance(float state)
        {
            resonanceFactor = state;
            Debug.Log($"Global Resonance: Updated to {resonanceFactor} due to state {state}");
        }

        private void ProcessData(byte[] data)
        {
            Debug.Log($"[Resonance]: Processing {data.Length} bytes of Void Data.");
        }
    }
}
