using UnityEngine;

namespace Milehigh.Core
{
    public class AlliancePowerManager : MonoBehaviour
    {
        private static AlliancePowerManager _instance;
        public static AlliancePowerManager Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public void SetPowerLevel(float level)
        {
            Debug.Log($"Power level set to {level}");
        }
    }
}
