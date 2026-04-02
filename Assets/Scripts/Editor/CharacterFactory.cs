using UnityEngine;
using UnityEditor;
using System.IO;
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

            string json = File.ReadAllText(path);
            HorizonGameData data = JsonUtility.FromJson<HorizonGameData>(json);

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
                // Standardized Path Sanitization Sequence:
                // 1. Replace invalid filename characters with underscores.
                // 2. Use Path.GetFileName to prevent directory traversal.
                // 3. Replace whitespace with underscores.
                string baseName = charProfile.name ?? "unnamed_character";
                string sanitizedName = baseName;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    sanitizedName = sanitizedName.Replace(c, '_');
                }
                sanitizedName = Path.GetFileName(sanitizedName);
                sanitizedName = sanitizedName.Replace(" ", "_");

                string assetPath = $"{folderPath}/{sanitizedName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path to avoid absolute path disclosure
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
