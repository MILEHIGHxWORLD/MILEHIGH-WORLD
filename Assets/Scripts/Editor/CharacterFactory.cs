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
            HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to prevent processing of malicious or malformed content.
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
                asset.characterName = charProfile.name ?? "unnamed";
                asset.role = charProfile.role ?? "none";
                asset.traits = charProfile.traits ?? new string[0];
                asset.behaviorScript = charProfile.behaviorScript ?? "";

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                // Malicious JSON could use "../" or absolute paths to write assets outside the intended directory.
                string baseName = charProfile.name ?? "unnamed_character";

                // 1. Replace invalid filename characters with underscores
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    baseName = baseName.Replace(c, '_');
                }

                // 2. Use Path.GetFileName to strip any remaining directory traversal sequences (like "../")
                // and replace spaces with underscores for better compatibility.
                string safeFileName = Path.GetFileName(baseName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset((UnityEngine.Object)asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
