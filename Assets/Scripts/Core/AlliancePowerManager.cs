using UnityEngine;

namespace Milehigh.Core
{
    public class AlliancePowerManager : MonoBehaviour
    {
        private static AlliancePowerManager? _instance;
        public static AlliancePowerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AlliancePowerManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("AlliancePowerManager");
                        _instance = go.AddComponent<AlliancePowerManager>();
                    }
                }
                return _instance!;
            }
        }

        public float collectivePower = 100f;

        public void ConsumePower(float amount)
        {
            collectivePower = Mathf.Max(0, collectivePower - amount);
            Debug.Log($"Collective Power: {collectivePower}");
        }
    }
}
