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
            string fileName = "campaign_master.json";
            string path = Path.Combine("Assets/Scripts/Data", fileName);
            if (!File.Exists(path))
            {
                Debug.LogError("Campaign master JSON not found at " + path);
                return;
            }

            string json = File.ReadAllText(path);
            HorizonGameData data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization
            if (data == null || !data.IsValid())
            {
                Debug.LogError($"[Security] Failed to parse or validate campaign data from {fileName}.");
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

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // We use Path.GetFileName to ensure only the final component is used, and replace invalid chars.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                }
                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path to avoid absolute path disclosure
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
