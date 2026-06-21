using UnityEngine;

namespace MilehighWorld.Core
{
    public static class MathUtils
    {
        public static float CalculateBase9Tetration(int index, float factor)
        {
            // Base-9 Parity Logic: Placeholder for tetration-inspired variance
            return Mathf.Pow(9, factor * (index % 9)) * 0.01f;
        }
    }
}
