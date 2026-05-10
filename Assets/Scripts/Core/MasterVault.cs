using UnityEngine;
using System.Collections.Generic;

namespace Milehigh.Core
{
    public class MasterVault : MonoBehaviour
    {
        private Dictionary<string, string> vaultData = new Dictionary<string, string>();

        public void StoreData(string key, string value)
        {
            vaultData[key] = value;
            Debug.Log($"Master Vault: Stored {key}");
        }

        public string RetrieveData(string key)
        {
            if (vaultData.TryGetValue(key, out string value))
            {
                return value;
            }
            return null;
        }
    }
}
