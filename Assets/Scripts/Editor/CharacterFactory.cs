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

            HorizonGameData? data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);

                if (data == null || !data.IsValid())
                {
                    Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError($"[Security] Failed to load or parse campaign data: {ex.Message}");
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
                    CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                    asset.characterName = charProfile.name ?? "unnamed";
                    asset.role = charProfile.role ?? "none";
                    asset.traits = charProfile.traits ?? System.Array.Empty<string>();
                    asset.behaviorScript = charProfile.behaviorScript ?? "";

                    // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                    string baseName = charProfile.name ?? "unnamed_character";
                    string sanitizedName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                    // We use Path.GetInvalidFileNameChars to filter OS-level invalid characters and Path.GetFileName to strip traversal sequences.
                    string baseName = charProfile.name ?? "unnamed_character";

                    // 1. Replace invalid filename characters with underscores
                    string sanitizedName = baseName;
                    foreach (char c in Path.GetInvalidFileNameChars())
                    {
                        sanitizedName = sanitizedName.Replace(c, '_');
                    }

                    // 2. Ensure no directory separators or traversal sequences remain
                    string safeFileName = Path.GetFileName(sanitizedName).Replace(" ", "_");

                    if (string.IsNullOrEmpty(safeFileName))
                    {
                        safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                    }

                    string assetPath = $"{folderPath}/{safeFileName}.asset";
                    AssetDatabase.CreateAsset(asset, assetPath);
                    Debug.Log($"Created character asset: {assetPath}");
                }
            }
        }
    }
}
