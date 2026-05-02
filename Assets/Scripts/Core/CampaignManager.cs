using UnityEngine;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    public class CampaignManager : MonoBehaviour
    {
        private static CampaignManager? _instance;
        public static CampaignManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CampaignManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                return _instance!;
            }
        }

        public HorizonGameData? currentCampaignData;
        public float currentVoidSaturationLevel;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCampaignData();
        }

        private void LoadCampaignData()
        {
            string fileName = "campaign_master.json";
            string filePath;

#if UNITY_EDITOR
            filePath = Path.Combine(Application.dataPath, "Scripts/Data", fileName);
#else
            filePath = Path.Combine(Application.streamingAssetsPath, fileName);
#endif

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    currentCampaignData = JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Perform validation after deserialization to ensure data integrity.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                Debug.LogError($"Campaign master JSON not found: {fileName}");
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
            SaveSecureData("VoidSaturation", currentVoidSaturationLevel.ToString());
        }

        /// <summary>
        /// 🛡️ Sentinel: Saves persistent data with basic XOR obfuscation to demonstrate
        /// client-side hardening for SOC 2 CC6.6 (Encryption of Data at Rest).
        /// Note: For production audits, use an industry-standard cryptographic library (e.g., AES-256).
        /// </summary>
        public void SaveSecureData(string key, string data)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(data)) return;

            string obfuscated = ProcessXOR(data);
            PlayerPrefs.SetString($"SECURE_{key}", obfuscated);
            PlayerPrefs.Save();
        }

        public string LoadSecureData(string key)
        {
            string obfuscated = PlayerPrefs.GetString($"SECURE_{key}", "");
            if (string.IsNullOrEmpty(obfuscated)) return "";

            return ProcessXOR(obfuscated);
        }

        private string ProcessXOR(string textToProcess)
        {
            // 🛡️ Sentinel: Deriving a key from device/app properties rather than hardcoding a string secret.
            string salt = SystemInfo.deviceUniqueIdentifier ?? "MILEHIGH_FALLBACK_SALT";
            char[] output = new char[textToProcess.Length];

            for (int i = 0; i < textToProcess.Length; i++)
            {
                output[i] = (char)(textToProcess[i] ^ salt[i % salt.Length]);
            }

            return new string(output);
        }
    }
}
