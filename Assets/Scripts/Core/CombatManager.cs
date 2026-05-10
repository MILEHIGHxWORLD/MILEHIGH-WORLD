using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Core
{
    public class CombatManager : MonoBehaviour
    {
        private static CombatManager _instance;
        public static CombatManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CombatManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CombatManager");
                        _instance = go.AddComponent<CombatManager>();
                    }
                }
                return _instance;
            }
        }

        [SerializeField] private GameObject beamFXPrefab;

        private void Awake()
        {
            // Error 4 Solution: Add singleton initialization in Awake()
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public float CalculateVanguardDamage(CharacterData data)
        {
            // Error 2 Solution: Base damage calculation
            float baseDamage = 25f;
            if (data != null && data.traits != null)
            {
                baseDamage += data.traits.Length * 5f;
            }
            return baseDamage * GlobalResonanceManager.Instance.GetIntegrityMultiplier() * (data != null ? data.vanguardMultiplier : 1.0f);
        }

        public void FireLogicParity()
        {
            // Error 11 Solution: Add Instantiate() call for visual effects
            if (beamFXPrefab != null)
            {
                Instantiate(beamFXPrefab, transform.position, Quaternion.identity);
                Debug.Log("Logic Parity Beam fired.");
            }
        }

        public void TriggerEnemyGlitch(GameObject target)
        {
            // Error 14 Solution: Add null check before using renderer
            if (target == null) return;

            Renderer targetRenderer = target.GetComponent<Renderer>();
            if (targetRenderer != null)
            {
                // Apply visual glitch logic here
                Debug.Log($"Triggering enemy glitch on {target.name}");
            }
        }
    }
}
