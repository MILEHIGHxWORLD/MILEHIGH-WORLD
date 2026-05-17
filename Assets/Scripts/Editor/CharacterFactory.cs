using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
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
                UnityEngine.Debug.LogError("Campaign master JSON not found at " + path);
                Debug.LogError($"Campaign master JSON not found at {path}");
                return;
            }

            string json = File.ReadAllText(path);
            HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            // NRT Pattern: Explicitly mark deserialized object as nullable
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
                Debug.LogError($"Failed to load or parse campaign data: {ex.Message}");
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || !data.IsValid())
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || !data.IsValid() || data.characters == null)
            {
                UnityEngine.Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
            string json = File.ReadAllText(path);
            HorizonGameData? data = JsonUtility.FromJson<HorizonGameData>(json);

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization to ensure integrity and prevent out-of-bounds impacts.
            if (data == null || !data.IsValid())
            {
                Debug.LogError("[Security] Character import aborted: Campaign data failed validation or is malformed.");
                Debug.LogError($"[Security] Failed to load or parse campaign data: {ex.Message}");
                return;
            }

            string folderPath = "Assets/Data/Characters";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                if (!AssetDatabase.IsValidFolder("Assets/Data"))
            if (!UnityEditor.AssetDatabase.IsValidFolder(folderPath))
            {
                if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Data"))
                {
                    UnityEditor.AssetDatabase.CreateFolder("Assets", "Data");
                }
                UnityEditor.AssetDatabase.CreateFolder("Assets/Data", "Characters");
            }

            // NRT Pattern: Capture property in local variable before iteration
            var characters = data.characters;
            if (characters != null)
            {
                CharacterData asset = UnityEngine.ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;
                if (charProfile == null) continue;

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
                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name ?? "unnamed";
                asset.role = charProfile.role ?? "none";
                asset.traits = charProfile.traits ?? System.Array.Empty<string>();
                asset.behaviorScript = charProfile.behaviorScript ?? "";

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                string baseName = charProfile.name ?? "unnamed_character";
                string sanitizedName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = Path.GetFileName(sanitizedName).Replace(" ", "_");

                if (string.IsNullOrEmpty(safeFileName))
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                string safeFileName = charProfile.name ?? "unnamed_character";
                // Malicious JSON could use "../" to write assets outside the intended directory.
                string rawName = charProfile.name ?? "unnamed_character";

                // 1. Strip directory traversal sequences using Path.GetFileName.
                string safeFileName = Path.GetFileName(rawName);

                // 2. Replace OS-specific invalid filename characters.
                // We use Path.GetFileName to extract only the name part and replace OS-specific invalid characters.
                string safeFileName = charProfile.name ?? "unnamed_character";
                string safeFileName = charProfile.name ?? "unnamed_character";
                // Malicious JSON could use directory traversal sequences (e.g., "../") to write assets outside the intended directory.
                string safeFileName = charProfile.name ?? "unnamed_character";
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                string baseName = charProfile.name ?? "unnamed_character";
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    baseName = baseName.Replace(c, '_');
                }
                string safeFileName = Path.GetFileName(baseName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // Malicious JSON could use "../" or absolute paths to write assets outside the intended directory.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // Required sequence: replace invalid chars, use Path.GetFileName, then replace spaces.
                string safeFileName = charProfile.name ?? "unnamed_character";
                // We use Path.GetFileName to ensure only the final component is used, and replace invalid chars.
                string safeFileName = charProfile.name ?? "unnamed_character";
                string safeFileName = GetSafeFileName(charProfile.name);
                if (string.IsNullOrEmpty(safeFileName))
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                string safeFileName = charProfile.name ?? "unnamed_character";
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                if (string.IsNullOrEmpty(safeFileName))
                // Malicious JSON could use "../" to attempt writing assets outside the intended directory.
                // We sanitize by replacing invalid chars and ensuring only the filename component is used.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = Path.GetFileName(baseName);
                string safeFileName = baseName;
                foreach (char c in System.IO.Path.GetInvalidFileNameChars())

                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                }
                // Ensure no directory traversal sequences remain and normalize whitespace
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                safeFileName = System.IO.Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                UnityEditor.AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path to avoid absolute path disclosure
                UnityEngine.Debug.Log($"Created character asset: {assetPath}");
            }

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");


                // 3. Final cleanup: replace spaces with underscores and ensure extension.
                safeFileName = safeFileName.Replace(" ", "_");
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
                    // We use Path.GetInvalidFileNameChars to filter OS-level invalid characters and Path.GetFileName to strip traversal sequences.
                    string baseName = charProfile.name ?? "unnamed_character";
                    string sanitizedName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));

                    // Ensure no directory separators or traversal sequences remain
                    string safeFileName = Path.GetFileName(sanitizedName).Replace(" ", "_");

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // Ensure no directory traversal sequences remain and replace spaces for cleanliness
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // Ensure no directory separators or traversal sequences remain

                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");
                // Ensure no directory traversal sequences remain and replace spaces
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // Ensure no directory traversal sequences remain by using Path.GetFileName
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                // SECURITY: Log relative asset path to avoid absolute path disclosure
                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                safeFileName = safeFileName.Replace(" ", "_");
                // Use Path.GetFileName to ensure only the final component is used, and replace spaces.
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

            // Whitelist: Allow only alphanumeric, underscores, and hyphens.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

            // Strip leading dots/underscores to prevent hidden files or traversal.
            sanitized = sanitized.TrimStart('.', '_');

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // Ensure no directory traversal sequences remain and normalize spacing
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
                // SECURITY: Log relative asset path only to avoid internal path disclosure
                Debug.Log($"Created character asset: {assetPath}");
            }

            return sanitized;
        }

        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "");
            return sanitized.TrimStart('.', '_');
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