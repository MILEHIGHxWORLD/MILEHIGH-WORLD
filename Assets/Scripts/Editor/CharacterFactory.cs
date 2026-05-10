using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using Milehigh.Data;
using System.Text.RegularExpressions;

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
            HorizonGameData data = null;
            try
            {
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<HorizonGameData>(json);
            }
            catch (Exception)

            // 🛡️ Sentinel: Security validation of deserialized data.
            // SECURITY: Always validate data after deserialization
                // 🛡️ Sentinel: Security validation of deserialized data.
                if (data == null || !data.IsValid())
                {
                    Debug.LogError("[Security] Character import aborted: Campaign data failed validation.");
                if (data == null)
                if (data == null || data.characters == null || !data.IsValid())
                {
                    Debug.LogError("[Security] Character import aborted: Campaign data is null or failed validation.");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking absolute paths or full stack traces.
                Debug.LogError($"Failed to load or parse campaign data: {ex.Message}");
                Debug.LogError("Failed to load or parse campaign data.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (data == null || data.characters == null || !data.IsValid())
            if (!data.IsValid())
                // 🛡️ Sentinel: Catch exceptions during file read/JSON parse to fail securely and avoid leaking stack traces
                Debug.LogError($"Failed to load or parse campaign data: {ex.Message}");
                Debug.LogError("Failed to load or parse campaign data. Error parsing file.");
                return;
            }

            // 🛡️ Sentinel: Security validation of deserialized data.
            if (!data.IsValid())
            // 🛡️ Sentinel: Security validation of deserialized data.            if (!data.IsValid())
            // SECURITY: Always validate data after deserialization to ensure integrity
                Debug.LogError($"Failed to load or parse campaign data. Error parsing file.");
                // SECURITY: Catch exceptions during file read/JSON parse to fail securely and avoid leaking internal stack traces or absolute paths.
                Debug.LogError($"Failed to read or parse campaign data from {Path.GetFileName(path)}: {ex.Message}");
                return;
            }

            // SECURITY: Always validate data after deserialization to ensure integrity and prevent resource exhaustion.
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
                if (charProfile == null) continue;

                CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                asset.characterName = charProfile.name;
                asset.role = charProfile.role;
                asset.traits = charProfile.traits;
                asset.behaviorScript = charProfile.behaviorScript;

                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities
                // Malicious JSON could use "../" to write assets outside the intended directory.
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = charProfile.name;
                string safeFileName = GetSafeFileName(charProfile.name);
                // Malicious JSON could use directory traversal sequences (e.g., "../") to write assets outside the intended directory.
                string safeFileName = charProfile.name ?? "unnamed_character";
                // We use Path.GetFileName to extract only the name part and replace OS-specific invalid characters.
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = string.Join("_", baseName.Split(Path.GetInvalidFileNameChars()));
                string safeFileName = string.IsNullOrEmpty(charProfile.name) ? "unnamed_character" : charProfile.name;
                string safeFileName = GetSafeFileName(charProfile.name);

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                Debug.Log($"Created character asset: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static string GetSafeFileName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            string safeName = name;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safeName = safeName.Replace(c, '_');
            }

            // Ensure no directory separators or traversal sequences remain
            safeName = Path.GetFileName(safeName).Replace(" ", "_");

            // SECURITY: Strip leading dots/underscores to prevent hidden files or traversal exploits
            safeName = safeName.TrimStart('.', '_');

            if (string.IsNullOrEmpty(safeName))
            {
                safeName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            return safeName;
                string baseName = charProfile.name ?? "unnamed_character";
                string safeFileName = baseName;
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    safeFileName = safeFileName.Replace(c, '_');
                }
                // Use Path.GetFileName to ensure only the final component is used, and replace spaces.
                safeFileName = safeFileName.Replace(" ", "_");

                // Ensure no directory separators or traversal sequences remain by extracting only the filename
                safeFileName = Path.GetFileName(safeFileName).Replace(" ", "_");
                // Ensure no directory separators or traversal sequences remain
                safeFileName = Path.GetFileName(safeFileName);
                string safeFileName = GetSafeFileName(charProfile.name);
                string safeFileName = GetSafeFileName(charProfile.name ?? "unnamed_character");
                string assetPath = $"{folderPath}/{safeFileName}.asset";

                if (string.IsNullOrEmpty(safeFileName) || safeFileName == "_")
                AssetDatabase.CreateAsset(asset, assetPath);
                string safeFileName = GetSafeFileName(charProfile.name);
                if (string.IsNullOrEmpty(safeFileName))
                {
                    safeFileName = "character_" + Guid.NewGuid().ToString().Substring(0, 8);
                }
                    safeFileName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
                // Malicious JSON could use directory traversal sequences (e.g., "../") to write assets outside the intended directory.
                // We use Path.GetFileName to extract only the name part and replace OS-specific invalid characters.
                string safeFileName = charProfile.name ?? "unnamed_character";

                foreach (char c in Path.GetInvalidFileNameChars())
                string safeFileName = GetSafeFileName(charProfile.name);
                string assetPath = $"{folderPath}/{safeFileName}.asset";

                AssetDatabase.CreateAsset(asset, assetPath);

                // SECURITY: Log relative asset path to avoid absolute path disclosure
                Debug.Log($"Created character asset: {assetPath}");
                // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                // We use a strict whitelist-based regex and strip leading dots/underscores.
                string safeFileName = GetSafeFileName(charProfile.name);
                foreach (var charProfile in data.characters)
                {
                    if (charProfile == null) continue;

                    CharacterData asset = ScriptableObject.CreateInstance<CharacterData>();
                    asset.characterName = charProfile.name;
                    asset.role = charProfile.role;
                    asset.traits = charProfile.traits;
                    asset.behaviorScript = charProfile.behaviorScript;
                    asset.health = charProfile.health;
                    asset.resonance = charProfile.resonance;
                    asset.integrity = charProfile.integrity;
                    asset.vanguardMultiplier = charProfile.vanguardMultiplier;

                    // 🛡️ Sentinel: Sanitize character name to prevent Path Traversal vulnerabilities.
                    string safeFileName = GetSafeFileName(charProfile.name);
                    string assetPath = $"{folderPath}/{safeFileName}.asset";

                    AssetDatabase.CreateAsset(asset, assetPath);
                    // SECURITY: Log relative asset path to avoid absolute path disclosure.
                    Debug.Log($"Created character asset: {assetPath}");
                }
            }

                string assetPath = $"{folderPath}/{safeFileName}.asset";
                AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 🛡️ Sentinel: Sanitizes a string for use as a file name.
        /// Prevents Path Traversal by using Path.GetFileName and a strict whitelist regex.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // 1. Replace invalid characters with underscores using a strict whitelist.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

                // SECURITY: Log relative asset path to avoid absolute path disclosure.
                Debug.Log($"Created character asset: {assetPath}");
            // 2. Strip leading dots/underscores and handle potential directory traversal.
            sanitized = sanitized.TrimStart('.', '_');

            // Ensure no directory traversal remains
            string safeName = Path.GetFileName(sanitized);

            if (string.IsNullOrEmpty(safeName))
            {
                return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            // 3. Replace whitespaces with underscores.
            safeName = safeName.Replace(" ", "_");

            return safeName;
        }

        /// <summary>
        /// 🛡️ Sentinel: Implementation of the Path Sanitization Standard.
        /// Uses a strict whitelist regex and strips leading dots/underscores.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            // Whitelist: letters, numbers, underscores, and hyphens.
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

            // Strip leading dots or underscores to prevent hidden files or traversal tricks.
            sanitized = sanitized.TrimStart('.', '_');

            return sanitized;
        }

        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // 🛡️ Sentinel: Strict whitelist-based sanitization to prevent Path Traversal
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");
            sanitized = sanitized.TrimStart('.', '_');

            if (string.IsNullOrEmpty(sanitized)) sanitized = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            return sanitized;
        }

        private static string GetSafeFileName(string? name)
        {
            if (string.IsNullOrEmpty(name)) return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // 🛡️ Sentinel: Path Sanitization Standard
            // Use a strict whitelist-based regex and strip leading dots/underscores.
            string sanitized = Regex.Replace(name, @"[^a-zA-Z0-9_\-]", "_");
            sanitized = sanitized.TrimStart('.', '_');

            if (string.IsNullOrEmpty(sanitized)) return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            return sanitized;
        }

        /// <summary>
        /// 🛡️ Sentinel: Generates a safe filename by stripping traversal sequences and invalid characters.
        /// Uses a whitelist-based approach for maximum security.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed";

            // Strip leading dots/underscores and use whitelist regex for characters
            string sanitized = input.TrimStart('.', '_');
            sanitized = Regex.Replace(sanitized, @"[^a-zA-Z0-9_\-]", "_");

            // Ensure we don't return an empty string or just underscores
            if (string.IsNullOrWhiteSpace(sanitized.Replace("_", ""))) return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            return sanitized;
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

        private static string GetSafeFileName(string input)
        {
            // BOLT: Optimized path sanitization using a whitelist approach
            string safeName = input.Replace(" ", "_");
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safeName = safeName.Replace(c, '_');
            }
            // Strip leading dots or underscores to prevent hidden files/traversal
            safeName = safeName.TrimStart('.', '_');

            if (string.IsNullOrEmpty(safeName))
            {
                safeName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }
            return safeName;
        }

        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // Sentinel Path Sanitization Standard: whitelist-based regex
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");
            sanitized = sanitized.TrimStart('.', '_');

            if (string.IsNullOrEmpty(sanitized))
            {
                return "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }
            return sanitized;
        }

        /// <summary>
        /// 🛡️ Sentinel: Robustly sanitizes a string for use as a file name.
        /// Prevents Path Traversal by using a strict whitelist regex and stripping leading dots/underscores.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 1. Remove all characters that are not alphanumeric, underscores, or hyphens (Strict Whitelist)
            string sanitized = Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");

            // 2. Strip leading dots or underscores to prevent hidden files or traversal tricks
            sanitized = sanitized.TrimStart('.', '_');

            // 3. Ensure we only return a filename, not a path
            sanitized = Path.GetFileName(sanitized);

            return sanitized;
        }

        /// <summary>
        /// 🛡️ Sentinel: Robust file name sanitization to prevent Path Traversal.
        /// </summary>
        private static string GetSafeFileName(string input)
        {
            if (string.IsNullOrEmpty(input)) return "unnamed_character_" + System.Guid.NewGuid().ToString().Substring(0, 8);

            // Remove invalid characters
            string safeName = input;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safeName = safeName.Replace(c, '_');
            }

            // Strip directory traversal sequences and hidden file markers
            safeName = Path.GetFileName(safeName).Replace("..", "").TrimStart('.', '_');

            if (string.IsNullOrEmpty(safeName))
            {
                safeName = "character_" + System.Guid.NewGuid().ToString().Substring(0, 8);
            }

            return safeName;
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
