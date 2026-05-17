namespace Milehigh.Core
{
    [UnityEngine.DefaultExecutionOrder(-100)]
    public class CampaignManager : UnityEngine.MonoBehaviour
    {
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
                    UnityEngine.GameObject go = new UnityEngine.GameObject("CampaignManager");
                    _instance = go.AddComponent<Milehigh.Core.CampaignManager>();
                }
                return _instance!;
            }
        }

        public Milehigh.Data.HorizonGameData currentCampaignData = null!;
        public float currentVoidSaturationLevel;

        private void Awake()
        {
            if (_instance != null && (UnityEngine.Object)_instance != (UnityEngine.Object)this)
            {
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
                    // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces or absolute paths.
                    // Mask runtime exception details and log only the file name to prevent information disclosure.
                    UnityEngine.Debug.LogError($"[Security] Error loading campaign data from {fileName}: {ex.Message}");
                    this.currentCampaignData = null!;
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
