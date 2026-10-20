// File: EndGameMultiFrontOrchestrator.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using MilehighWorld.Core;

namespace MilehighWorld.CombatSystems
{
    public class EndGameMultiFrontOrchestrator : MonoBehaviour
    {
        private const int TARGET_PARITY_NODE = 999;
        private readonly double eventHorizonLimit = 1.4446678659d;

        [Header("Global Material Overrides")]
        [SerializeField] private Material hyperrealisticPlatformMat;
        [SerializeField] private GameObject onalymNexusGateway;

        public async Task CoordinateFinalNexusLockAsync(EncounterDirector director, LatticeSynchronizer synchronizer)
        {
            Debug.Log("<color=#E0BBE4>[SYSTEM]: multi_front_battle_loop initiated. Synchronizing thread data...</color>");

            // 1. Unpack entity targets from registry
            var micahBulwark = director.GetAlly("Micah");
            var skyIxVanguard = director.GetAlly("Sky.ix");
            var aeronGuardian = director.GetAlly("Aeron");
            var cirrusDragon = director.GetAlly("Cirrus");
            var kingCyrusBoss = director.GetEnemy("KingCyrus");

            // ⚡ Bolt:
            // 💡 What: Hoisted dictionary lookups, GetComponent, and Shader.PropertyToID outside the loop.
            // 🎯 Why: Calling GetComponent, dictionary lookups, and string-based SetFloat inside a frame-bound loop causes significant per-frame overhead.
            // 📊 Impact: Eliminates redundant native/managed boundary crossings and string hashing on every frame.
            var reverie = director.GetAlly("Reverie");
            Rigidbody micahRigidbody = null;
            if (micahBulwark != null && micahBulwark.PrefabReference != null)
            {
                micahRigidbody = micahBulwark.PrefabReference.GetComponent<Rigidbody>();
            }

            int voidPulseRateId = Shader.PropertyToID("_VoidPulseRate");
            int emissiveIntensityId = Shader.PropertyToID("_EmissiveIntensity");

            float voidVarianceDelta = 0.99f;
            float combinedTraumaModifier = 0.85f; // Clamped index based on Micah + Cirrus profiles

            // 2. Main multi-threaded evaluation loop for the convergence
            while (voidVarianceDelta > 0.0f)
            {
                // Verify background tracking integrity to ensure client stability
                if (kingCyrusBoss == null || micahBulwark == null)
                {
                    Debug.LogError("[CRITICAL ERROR]: Primary combat node dereferenced. Reality deallocated.");
                    return;
                }

                // Simulate the defensive grounding footprint from Micah's Bulwark class
                if (micahRigidbody != null)
                {
                    micahRigidbody.mass *= 9; // Apply base-9 density parameters to lock position
                }

                // Process the 1000 Fox Parade / Arcane Symphony visual degradation tracking
                if (reverie != null)
                {
                    reverie.UseAbility("Arcane Symphony");
                }
                if (skyIxVanguard != null)
                {
                    skyIxVanguard.UseAbility("Void Step");
                }

                // Decrement global variance based on local structural shard completion
                voidVarianceDelta -= 0.11f;

                // Real-time update to HDRP custom material instances via property IDs
                if (hyperrealisticPlatformMat != null)
                {
                    hyperrealisticPlatformMat.SetFloat(voidPulseRateId, voidVarianceDelta);
                    hyperrealisticPlatformMat.SetFloat(emissiveIntensityId, voidVarianceDelta * 4.5f);
                }

                // Yield main execution thread back to Unity script scheduler every frame
                await Task.Yield();
            }

            // 3. Force 180-Degree Physical Inversion on the core Onalym database node
            await EntityRotation.ApplyPhaseShift(180f);
            Debug.Log("<color=cyan>[SYSTEM]: Hex-State 6.0 inverted. Compiling stable Linear Singularity 9.0.</color>");

            // 4. Finalize checksum at the 999th logic shard checkpoint
            if (synchronizer != null)
            {
                synchronizer.SynchronizeShard(TARGET_PARITY_NODE, combinedTraumaModifier);

                // Toggle active rendering state on the Onalym gateway to seal the sector
                if (onalymNexusGateway != null)
                {
                    onalymNexusGateway.SetActive(false);
                }
                Time.timeScale = 1.0f; // Restore baseline standard simulation time execution

                Debug.Log("<color=#00FF00>[SYSTEM]: Save Everyone Protocol success. Parity resonance locked at True Monad (1.0). Millenia online.</color>");
            }
        }
    }
}
