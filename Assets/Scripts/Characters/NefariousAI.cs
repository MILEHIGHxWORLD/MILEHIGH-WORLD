// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MilehighWorld.Characters
{
    public class NefariousAI : MonoBehaviour
    {
        private Renderer _renderer = null!;
        private static readonly int GlitchIntensityId = Shader.PropertyToID("_GlitchIntensity");
        private static MaterialPropertyBlock _propertyBlock = null!;

        // ⚡ Bolt: Cache for WaitForSeconds using millisecond keys to prevent floating-point precision issues
        // and eliminate redundant GC allocations during coroutine execution.
        private static readonly Dictionary<int, WaitForSeconds> _waitCache = new Dictionary<int, WaitForSeconds>();

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private WaitForSeconds GetWait(float seconds)
        {
            int ms = Mathf.RoundToInt(seconds * 1000f);
            if (!_waitCache.TryGetValue(ms, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[ms] = wait;
            }
            return wait;
        }

        public void TriggerEnemyGlitch()
        {
            // ⚡ Bolt: Use cached renderer to avoid expensive GetComponent calls.
            if (_renderer == null) return;

            // ⚡ Bolt: Use MaterialPropertyBlock to prevent material instantiation and preserve draw call batching.
            if (_propertyBlock == null) _propertyBlock = new MaterialPropertyBlock();

            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(GlitchIntensityId, 1.0f);
            _renderer.SetPropertyBlock(_propertyBlock);

            // Reset after a brief moment
            StartCoroutine(ResetGlitch());
        }

        private IEnumerator ResetGlitch()
        {
            // ⚡ Bolt: Zero-allocation yield via shared cache.
            yield return GetWait(0.2f);

            if (_renderer != null)
            {
                if (_propertyBlock == null) _propertyBlock = new MaterialPropertyBlock();

                _renderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetFloat(GlitchIntensityId, 0f);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }
    }
}
