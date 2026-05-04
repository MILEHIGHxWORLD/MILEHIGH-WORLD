using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using Milehigh.Data;

namespace Milehigh.Editor
{
    public class CharacterFactory : EditorWindow
    {
        [MenuItem("Milehigh/Import Campaign Data")]
        public static void ImportCampaignData()
        {
            string path = "Assets/Scripts/Data/campaign_master.json";
            if (!File.Exists(path))
            {
                Debug.LogError("Campaign master JSON not found at " + path);
                return;
            }

            HorizonGameData data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
            }
            catch (System.Exception)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                return;
            }

            string folderPath = "Assets/Data/Characters";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Data"))
                {
                    AssetDatabase.CreateFolder("Assets", "Data");
                }
                AssetDatabase.CreateFolder("Assets/Data", "Characters");
            }

            if (data.characters != null)
            {
                foreach (var charProfile in data.characters)
                {
                    if (charProfile == null) continue;

                    CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                    asset.characterName = charProfile.name;
                    asset.role = charProfile.role;
                    asset.traits = charProfile.traits;
                    asset.behaviorScript = charProfile.behaviorScript;

                    string safeFileName = GetSafeFileName(charProfile.name);
                    string assetPath = $"{folderPath}/{safeFileName}.asset";

                    AssetDatabase.CreateAsset(asset, assetPath);
                    // SECURITY: Log relative asset path to avoid absolute path disclosure.
                    Debug.Log($"Created character asset: {assetPath}");
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
        /// </summary>
        private static string GetSafeFileName(string baseName)
        {
            if (string.IsNullOrWhiteSpace(baseName))
            {
                return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            // 🛡️ Sentinel: Use a whitelist-based approach to replace all non-alphanumeric characters.
            // This prevents path traversal (../), hidden files (.name), and invalid OS characters.
            string safeName = Regex.Replace(baseName, @"[^a-zA-Z0-9_\-]", "_");

            // Ensure no directory separators or traversal sequences remain by taking only the filename
            safeName = Path.GetFileName(safeName);

            // Strip leading underscores/dots to prevent traversal or hidden files
            while (safeName.StartsWith("_") || safeName.StartsWith("."))
            {
                safeName = safeName.Substring(1);
            }

            if (string.IsNullOrWhiteSpace(safeName))
            {
                safeName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            return safeName;
        }
    }
}
