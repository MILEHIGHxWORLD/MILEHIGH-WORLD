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
            string fileName = "campaign_master.json";
            string path = Path.Combine("Assets/Scripts/Data", fileName);

            if (!File.Exists(path))
            {
                Debug.LogError($"Campaign master JSON not found at {path}");
                return;
            }

            HorizonGameData? data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
            }
            catch (System.Exception ex)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking absolute paths.
                Debug.LogError($"Error loading campaign data from {fileName}: {ex.Message}");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            // NRT Pattern: Captured local 'data' ensures safety.
            if (data == null || !data.IsValid())
            {
                Debug.LogError($"[Security] Character import aborted: Campaign data from {fileName} failed validation.");
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

            // NRT Pattern: Capture property in local variable before iteration
            var characters = data.characters;
            if (characters != null)
            {
                foreach (var charProfile in characters)
                {
                    if (charProfile == null) continue;

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
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 🛡️ Sentinel: Robust path sanitization helper.
        /// Uses a strict whitelist to prevent directory traversal and hidden files.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // Whitelist: Allow only alphanumeric, underscores, and hyphens.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

            // Strip leading dots/underscores to prevent hidden files or traversal.
            sanitized = sanitized.TrimStart('.', '_');

            if (string.IsNullOrEmpty(sanitized))
            {
                return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            return sanitized;
        }
    }
}
