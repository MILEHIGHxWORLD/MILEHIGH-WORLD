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
                return _instance;
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

                    // 🛡️ Sentinel: Perform security and integrity validation after deserialization.
                    if (currentCampaignData != null && currentCampaignData.IsValid())
                    {
                        if (currentCampaignData.metadata != null)
                        {
                            currentVoidSaturationLevel = currentCampaignData.metadata.voidSaturationLevel;
                        }
                        // SECURITY: Log only the filename to avoid exposing absolute filesystem paths.
                        Debug.Log($"Campaign data loaded and validated from {fileName}");
                    }
                    else
                    {
                        Debug.LogError($"[Security] Campaign data from {fileName} failed validation or is malformed.");
                        currentCampaignData = null; // Prevent use of invalid data.
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
            Debug.Log($"Void Saturation Level: {currentVoidSaturationLevel}");
        }
    }
}
