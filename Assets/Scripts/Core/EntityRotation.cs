using System.Threading.Tasks;
using UnityEngine;

namespace MilehighWorld.Core
{
    public static class EntityRotation
    {
        public static async Task ApplyPhaseShift(float degrees)
        {
            Debug.Log($"<color=cyan>[EntityRotation]</color> Applying Phase Shift: {degrees} degrees");
            await Task.Yield();
        }
    }
}
