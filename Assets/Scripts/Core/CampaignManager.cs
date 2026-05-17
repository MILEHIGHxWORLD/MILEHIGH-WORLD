using System;
using UnityEngine;
using System;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    [UnityEngine.DefaultExecutionOrder(-100)]
    public class CampaignManager : UnityEngine.MonoBehaviour
    {
        private static CampaignManager _instance = null!;
        private static CampaignManager? _instance;
        public static CampaignManager Instance
        private static Milehigh.Core.CampaignManager? _instance;
        public static Milehigh.Core.CampaignManager Instance
        {
            get
            {
                // BOLT: O(1) access in the common case after initialization
                if (_instance != null) return _instance;

                _instance = UnityEngine.Object.FindObjectOfType<Milehigh.Core.CampaignManager>();
                if (_instance == null)
                {
                    _instance = UnityEngine.Object.FindObjectOfType<CampaignManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                    UnityEngine.GameObject go = new UnityEngine.GameObject("CampaignManager");
                    _instance = go.AddComponent<Milehigh.Core.CampaignManager>();
                }
                return _instance!;
            }
        }

        public HorizonGameData currentCampaignData = null!;
        public HorizonGameData? currentCampaignData;
        public Milehigh.Data.HorizonGameData currentCampaignData = null!;
        public float currentVoidSaturationLevel;

        private void Awake()
        {
            if (_instance != null && (UnityEngine.Object)_instance != (UnityEngine.Object)this)
            {
                UnityEngine.Object.Destroy(gameObject);
                return;
            }
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            LoadCampaignData();
                UnityEngine.Object.Destroy(this.gameObject);
                return;
            }
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this.LoadCampaignData();
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
                    string json = File.ReadAllText(filePath);
                    // NRT Pattern: Explicitly mark deserialized object as nullable
                    HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Perform security and integrity validation after deserialization.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        if (currentCampaignData.metadata != null)
                        {
                            currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        }
                        // SECURITY: Log only the filename to avoid exposing absolute filesystem paths.
                    // 🛡️ Sentinel: Perform validation after deserialization to ensure data integrity.
                    // NRT Pattern: Use local variable 'data' for consistent flow analysis.
                    if (data != null && data.IsValid())
                    {
                        currentCampaignData = data;
                        currentVoidSaturationLevel = data.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                    // 🛡️ Sentinel: Security validation of deserialized data.
                    // SECURITY: Perform validation after deserialization to ensure data integrity
                    if (currentCampaignData == null)
                    {
                        Debug.LogError($"Failed to parse campaign data from {fileName}.");
                    if (currentCampaignData != null)
                    {
                        if (currentCampaignData.IsValid())
                        {
                            currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                            // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                            Debug.Log($"Campaign data loaded and validated from {fileName}");
                        }
                        else
                        {
                            Debug.LogError($"Campaign data from {fileName} failed security validation.");
                            currentCampaignData = null;
                        }
                    }
                    else
                    {
                        Debug.LogError($"Failed to parse campaign data from {fileName}.");
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata!.voidSaturationLevel;
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else if (!currentCampaignData.IsValid())
                    {
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        currentCampaignData = null;
                    }
                    else
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                        UnityEngine.Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        UnityEngine.Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null; // Prevent use of invalid data.
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        Debug.LogError($"Campaign data from {fileName} failed security validation or parsing.");
                        // SECURITY: Fail securely and don't use invalid data. Mask runtime exception details and avoid leaking absolute paths in logs.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"Campaign data from {fileName} failed security validation or parsing.");
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation.");
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null!; // Ensure we don't use invalid data
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data
                        // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        // 🛡️ Sentinel: Failed validation means we cannot trust the campaign data.
                        // SECURITY: Log the validation failure without exposing internal paths
                        Debug.LogError($"Campaign data from {fileName} failed security validation or is malformed.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    string json = System.IO.File.ReadAllText(filePath);
                    this.currentCampaignData = UnityEngine.JsonUtility.FromJson<Milehigh.Data.HorizonGameData>(json);

                    // 🛡️ Sentinel: Security validation of deserialized data.
                    // SECURITY: Perform validation after deserialization to ensure data integrity and fail securely.
                    if (this.currentCampaignData != null && this.currentCampaignData.IsValid())
                    {
                        this.currentVoidSaturationLevel = this.currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                        UnityEngine.Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely by nullifying corrupted data and logging the error without leaking internal paths.
                        UnityEngine.Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        this.currentCampaignData = null!;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null; // Ensure we don't use partially loaded or invalid data
                    // SECURITY: Fail securely by catching exceptions and masking sensitive details (e.g., stack traces).
                    Debug.LogError($"[Security] Critical error during campaign data load from {fileName}: {ex.Message}");
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}. Error: {ex.Message}");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    // SECURITY: Catch exceptions to fail securely and avoid leaking internal stack traces or paths.
                    Debug.LogError($"Failed to load or parse ca
                                  mpaign data from {fileName}. Error: {ex.Message}");
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}");
                    currentCampaignData = null;
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces or absolute paths.
                    // Mask runtime exception details and log only the file name and exception type to prevent information disclosure of internal paths.
                    UnityEngine.Debug.LogError($"[Security] Error loading campaign data from {fileName}: ({ex.GetType().Name})");
                    this.currentCampaignData = null!;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                UnityEngine.Debug.LogError($"Campaign master JSON not found: {fileName}");
                // SECURITY: Log only the filename to prevent information disclosure.
                Debug.LogError($"Campaign master JSON not found: {fileName}");
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                UnityEngine.Debug.LogError($"Campaign master JSON not found: {fileName}");
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            UnityEngine.Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
            this.currentVoidSaturationLevel = UnityEngine.Mathf.Clamp01(this.currentVoidSaturationLevel + amount);
            UnityEngine.Debug.Log($"Void Saturation Level: {this.currentVoidSaturationLevel}");
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

        // 🛡️ Sentinel: Cache deviceUniqueIdentifier to avoid expensive native boundary crossing
        private static string? _cachedDeviceIdentifier;

        private string ProcessXOR(string textToProcess)
        {
            if (string.IsNullOrEmpty(_cachedDeviceIdentifier))
            {
                _cachedDeviceIdentifier = UnityEngine.SystemInfo.deviceUniqueIdentifier ?? "MILEHIGH_FALLBACK_SALT";
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
