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
                    _instance = FindFirstObjectByType<CampaignManager>();
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
                    }
                }
                catch (System.Exception)
                {
                    // SECURITY: Mask runtime exception stack traces and avoid leaking absolute paths in logs
                    Debug.LogError($"Error loading campaign data from {fileName}.");
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
        }
    }
}
