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

            HorizonGameData? data = null;
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

            foreach (var charProfile in data.characters)
            {
                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                string safeFileName = GetSafeFileName(charProfile.name);

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 🛡️ Sentinel: Path sanitization logic to prevent directory traversal and hidden file creation.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // Whitelist approach: only allow alphanumeric, underscores, and hyphens.
            // Strip leading dots and underscores to prevent hidden files/special names.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");
            sanitized = sanitized.TrimStart('.', '_');

            if (string.IsNullOrEmpty(sanitized))
            {
                sanitized = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            return sanitized;
        }
    }
}
