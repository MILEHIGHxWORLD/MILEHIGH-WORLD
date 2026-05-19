using UnityEngine;

namespace Milehigh.World.Engine
{
    public class BicameralBattleEngine : MonoBehaviour
    {
        public enum RealityState { Void, Now }
        public RealityState currentReality = RealityState.Now;

        public void ToggleReality()
        {
            currentReality = (currentReality == RealityState.Now) ? RealityState.Void : RealityState.Now;
            // Update GlobalResonance based on state
        }
    }
}
