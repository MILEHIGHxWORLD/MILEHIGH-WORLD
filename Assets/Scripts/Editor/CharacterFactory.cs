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

                if (data == null || data.characters == null)
                {
                    Debug.LogError("Failed to parse campaign data.");
                    return;
                }
            }
            catch (System.Exception)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (!data.IsValid())
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

                if (string.IsNullOrEmpty(safeFileName))
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 🛡️ Sentinel: Robust filename sanitization to prevent Path Traversal exploits.
        /// Uses a strict whitelist-based regex and strips leading dots/underscores.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            // SECURITY: Use a strict whitelist to only allow alphanumeric characters, underscores, and hyphens.
            // This prevents directory traversal sequences like '..' and platform-specific path separators.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "");

            // SECURITY: Strip leading dots or underscores to prevent hidden files or other OS-level exploits.
            sanitized = sanitized.TrimStart('.', '_');

            return sanitized;
        }
    }
}
