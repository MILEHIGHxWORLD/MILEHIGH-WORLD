using UnityEngine;

namespace Milehigh.Characters
{
    public class SkyIxPCCombat : MonoBehaviour
    {
        public void ExecuteCombo(string comboId)
        {
            Debug.Log($"Sky.ix: Executing combo {comboId}");
        }

        public void TakeDamage(float amount)
        {
            Debug.Log($"Sky.ix: Taking {amount} damage.");
        }
    }
}
