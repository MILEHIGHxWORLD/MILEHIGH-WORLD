using UnityEngine;
using System.Threading.Tasks;

namespace MilehighWorld.Simulation
{
    public static class EntityRotation
    {
        public static async Task ApplyPhaseShift(float degrees)
        {
            Debug.Log($"[EntityRotation] Applying Phase Shift of {degrees} degrees.");
            await Task.Yield();
        }
    }
}
