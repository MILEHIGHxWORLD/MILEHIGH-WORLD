using UnityEngine;
using System.Collections.Generic;

namespace Milehigh.Core
{
    /// <summary>
    /// Centralized utility for Unity-specific optimizations and helpers.
    /// </summary>
    public static class UnityUtils
    {
        private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

        /// <summary>
        /// Returns a cached WaitForSeconds object to eliminate GC allocations.
        /// </summary>
        public static WaitForSeconds GetWait(float seconds)
        {
            if (!_waitForSecondsCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitForSecondsCache[seconds] = wait;
            }
            return wait;
        }
    }
}
