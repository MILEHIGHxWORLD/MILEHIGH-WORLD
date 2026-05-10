using System;
using UnityEngine;
using System.IO;
using Milehigh.Data;

namespace Milehigh.Core
{
    [DefaultExecutionOrder(-100)]
    public class CampaignManager : MonoBehaviour
    {
        private static CampaignManager? _instance;
        public static CampaignManager? Instance
        private static CampaignManager _instance;
        public static CampaignManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<CampaignManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                return _instance;
                if (_instance == null) InitializeInstance();
                return _instance!;
            }
        }

        private static void InitializeInstance()
        {
            _instance = FindObjectOfType<CampaignManager>();
            if (_instance == null)
            {
                GameObject go = new GameObject("CampaignManager");
                _instance = go.AddComponent<CampaignManager>();
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
            string filePath = "";

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

                    // 🛡️ Sentinel: Security validation of deserialized data.
                    // UNITY NRT Flow Analysis Pattern: Capture singleton property in local variable
                    var data = currentCampaignData;
                    // 🛡️ Sentinel: Perform validation after deserialization to ensure data integrity.
                    if (data != null && data.IsValid())
                    {
                        currentVoidSaturationLevel = data.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                    // 🛡️ Sentinel: Perform security validation after deserialization to ensure data integrity and prevent DoS.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        // SECURITY: Use generic error message for validation failure
                        Debug.LogError($"Campaign data from {fileName} failed security validation or parsing.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                        currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        Debug.LogError($"Campaign data from {fileName} failed security validation.");
                        // SECURITY: Fail securely and don't use invalid data
                        // SECURITY: Fail securely and don't use invalid data.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data. Mask details to prevent info disclosure.
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data. Consolidate redundant logging.
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: Fail securely and don't use invalid data
                        // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"Failed to parse or validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        Debug.LogError($"Failed to parse or security-validate campaign data from {fileName}.");
                        currentCampaignData = null;
                        // SECURITY: If data fails validation, ensure it's not used by the application and log a safe error message.
                        Debug.LogError($"[Security] Campaign data from {fileName} failed security validation or parsing.");
                        currentCampaignData = null; // Ensure we don't use invalid data
                    }
                }
                catch (Exception ex)
                catch (System.Exception)
                {
                    // SECURITY: Catch exceptions to fail securely and avoid leaking internal stack traces or paths.
                    Debug.LogError($"Failed to load or parse campaign data from {fileName}. Error: {ex.Message}");
                    // SECURITY: Fail securely and avoid leaking internal stack traces or absolute paths.
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces.
                    // SECURITY: Mask runtime exception details and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}");
                    // SECURITY: Removed ex.Message to avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // Consolidate redundant comments and remove ex.Message to prevent path leakage.
                    Debug.LogError($"Error loading campaign data from {fileName}. Parsing failure.");
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}. See logs for details.");
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Do not log ex.Message to avoid leaking absolute file paths or system details.
                    Debug.LogError($"Error loading campaign data from {fileName}.");
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely.
                    // Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the file name, not the absolute path, to prevent information disclosure.
                Debug.LogError($"Campaign master JSON not found: {fileName}");
                currentCampaignData = null;
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            Debug.Log("Void Saturation Level: " + currentVoidSaturationLevel);
        }
    }
}
