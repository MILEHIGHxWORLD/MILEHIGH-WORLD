using System;
using UnityEngine;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    [UnityEngine.DefaultExecutionOrder(-100)]
    public class CampaignManager : UnityEngine.MonoBehaviour
    {
        private static CampaignManager? _instance;

        public static CampaignManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = UnityEngine.Object.FindAnyObjectByType<CampaignManager>();
                    if (_instance == null)
                    {
                        UnityEngine.GameObject go = new UnityEngine.GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                return _instance;
            }
        }

        public HorizonGameData? currentCampaignData;
        public float currentVoidSaturationLevel;

        // 🛡️ Sentinel: Cache deviceUniqueIdentifier to avoid expensive native boundary crossing
        private static string? _cachedDeviceIdentifier;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                UnityEngine.Object.Destroy(this.gameObject);
                return;
            }
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            LoadCampaignData();
        }

        private void LoadCampaignData()
        {
            string fileName = "campaign_master.json";
            string filePath;

#if UNITY_EDITOR
            filePath = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Scripts/Data", fileName);
#else
            filePath = System.IO.Path.Combine(UnityEngine.Application.streamingAssetsPath, fileName);
#endif

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    string json = System.IO.File.ReadAllText(filePath);
                    HorizonGameData? data = UnityEngine.JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Perform security and integrity validation after deserialization.
                    if (data != null && data.IsValid())
                    {
                        currentCampaignData = data;
                        if (data.metadata != null)
                        {
                            currentVoidSaturationLevel = data.metadata.voidSaturationLevel;
                        }
                        // SECURITY: Log only the filename to avoid exposing absolute filesystem paths.
                        UnityEngine.Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely by nullifying corrupted data and logging the error without leaking internal paths.
                        UnityEngine.Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null;
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                    // Log only the file name and exception type to prevent information disclosure of internal paths.
                    UnityEngine.Debug.LogError($"[Security] Error loading campaign data from {fileName}: ({ex.GetType().Name})");
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                UnityEngine.Debug.LogError($"Campaign master JSON not found: {fileName}");
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = UnityEngine.Mathf.Clamp01(currentVoidSaturationLevel + amount);
            UnityEngine.Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
            this.SaveSecureData("VoidSaturation", this.currentVoidSaturationLevel.ToString());
        }

        public void SaveSecureData(string key, string data)
        {
            if (System.String.IsNullOrEmpty(key) || System.String.IsNullOrEmpty(data)) return;

            string obfuscated = this.ProcessXOR(data);
            UnityEngine.PlayerPrefs.SetString($"SECURE_{key}", obfuscated);
            UnityEngine.PlayerPrefs.Save();
        }

        public string LoadSecureData(string key)
        {
            string obfuscated = UnityEngine.PlayerPrefs.GetString($"SECURE_{key}", "");
            if (System.String.IsNullOrEmpty(obfuscated)) return "";

            return this.ProcessXOR(obfuscated);
        }

        private string ProcessXOR(string textToProcess)
        {
            if (string.IsNullOrEmpty(_cachedDeviceIdentifier))
            {
                _cachedDeviceIdentifier = UnityEngine.SystemInfo.deviceUniqueIdentifier;
                if (string.IsNullOrEmpty(_cachedDeviceIdentifier) || _cachedDeviceIdentifier == "n/a")
                {
                    _cachedDeviceIdentifier = "MILEHIGH_FALLBACK_STABLE_SALT_2025";
                }
            }

            string salt = _cachedDeviceIdentifier;
            char[] output = new char[textToProcess.Length];

            for (int i = 0; i < textToProcess.Length; i++)
            {
                output[i] = (char)(textToProcess[i] ^ salt[i % salt.Length]);
            }

            return new string(output);
        }
    }
}
