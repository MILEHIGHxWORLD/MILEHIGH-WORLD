// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using MilehighWorld.Core;

namespace MilehighWorld.Engine
{
    /// <summary>
    /// Orchestrates the "1000 Fox Parade" entity spawning and movement using the Unity Job System (DOTS).
    /// strictly adheres to Base-9 Parity (999 instances) and asynchronous SOPs.
    /// </summary>
    public class FoxParadeDirector : MonoBehaviour
    {
        [Header("Parade Settings")]
        [SerializeField] private int entityCount = 999; // Base-9 Parity Alignment
        [SerializeField] private float paradeSpeed = 9.0f;

        private NativeArray<float3> _positions;
        private NativeArray<float3> _velocities;
        private JobHandle _jobHandle;

        [BurstCompile]
        struct FoxMovementJob : IJobParallelFor
        {
            public NativeArray<float3> Positions;
            [ReadOnly] public NativeArray<float3> Velocities;
            public float DeltaTime;
            public float Speed;

            public void Execute(int index)
            {
                // Simple linear movement with Base-9 harmonic variation
                float harmonic = math.sin(index * 0.09f);
                Positions[index] += Velocities[index] * Speed * DeltaTime * (1.0f + harmonic * 0.09f);
            }
        }

        private void Start()
        {
            // Initialize Base-9 aligned arrays
            _positions = new NativeArray<float3>(entityCount, Allocator.Persistent);
            _velocities = new NativeArray<float3>(entityCount, Allocator.Persistent);

            for (int i = 0; i < entityCount; i++)
            {
                _positions[i] = new float3(i * 0.09f, 0, 0);
                _velocities[i] = new float3(0, 0, 1.0f);
            }
        }

        private void Update()
        {
            // Schedule the movement job with Base-9 batch parity
            var job = new FoxMovementJob
            {
                Positions = _positions,
                Velocities = _velocities,
                DeltaTime = Time.deltaTime,
                Speed = paradeSpeed
            };

            // Batch size of 81 (9x9) for optimal processor utilization
            _jobHandle = job.Schedule(entityCount, 81);
        }

        private void LateUpdate()
        {
            _jobHandle.Complete();
        }

        private void OnDestroy()
        {
            if (_positions.IsCreated)
            {
                _positions.Dispose();
            }
            if (_velocities.IsCreated)
            {
                _velocities.Dispose();
            }
        }

        /// <summary>
        /// Mathematically validates if the current parade state maintains Base-9 Parity.
        /// </summary>
        public bool ValidateParity()
        {
            return entityCount % 9 == 0;
        }
    }
}
