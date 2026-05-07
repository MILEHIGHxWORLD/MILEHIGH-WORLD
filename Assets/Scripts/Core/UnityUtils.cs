using UnityEngine;
using System.Collections.Generic;

namespace Milehigh.Core
{
    public static class UnityUtils
    {
        private static readonly Dictionary<float, WaitForSeconds> _waitForSecondsCache = new Dictionary<float, WaitForSeconds>();

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
