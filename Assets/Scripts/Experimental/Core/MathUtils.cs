using UnityEngine;

namespace MilehighWorld.Core
{
    public static class MathUtils
    {
        public static float CalculateBase9Tetration(int i, float factor)
        {
            // Implementation aligned with Base-9 structural rules
            float value = Mathf.Pow(9, (i % 9) / 9f);
            return value * factor;
        }
    }
}
