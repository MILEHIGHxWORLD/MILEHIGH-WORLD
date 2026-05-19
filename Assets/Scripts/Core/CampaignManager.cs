using UnityEngine;
using System;
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
                    _instance = UnityEngine.Object.FindObjectOfType<CampaignManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CampaignManager");
                        _instance = go.AddComponent<CampaignManager>();
                    }
                }
                return _instance;
            }
        }

        public HorizonGameData? currentCampaignData;
        public float currentVoidSaturationLevel;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                UnityEngine.Object.Destroy(gameObject);
                return;
            }
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
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
                    HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

                    // 🛡️ Sentinel: Fix Security Bypass.
                    // Validate newly loaded data BEFORE assigning to currentCampaignData.
                    if (data != null && data.IsValid())
                    {
                        currentCampaignData = data;
                        if (currentCampaignData.metadata != null)
                        {
                            currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        }
                        // SECURITY: Log only the filename to avoid exposing absolute filesystem paths.
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        // SECURITY: Fail securely and don't use invalid data
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null;
                    }
                }
                catch (System.Exception ex)
                {
                    // SECURITY: Fail securely by catching exceptions and masking sensitive details (e.g., stack traces).
                    Debug.LogError($"[Security] Critical error during campaign data load from {fileName}: {ex.Message}");
                    currentCampaignData = null;
                }
            }
            else
            {
                // SECURITY: Log only the filename to prevent information disclosure.
                Debug.LogError($"Campaign master JSON not found: {fileName}");
            }
        }

        public void IncreaseVoidSaturation(float amount)
        {
            currentVoidSaturationLevel = Mathf.Clamp01(currentVoidSaturationLevel + amount);
            UnityEngine.Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
        }
    }
}
