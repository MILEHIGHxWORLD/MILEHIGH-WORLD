## 2024-05-24 - Path Traversal in Editor Extension Scripts
**Vulnerability:** Found a Path Traversal vulnerability in an Editor window script (`CharacterFactory.cs`). The script trusted the `name` field from a parsed JSON file (`campaign_master.json`) to construct the file path for creating new `.asset` files (`string assetPath = $"{folderPath}/{charProfile.name...}.asset";`).
**Learning:** Even though the JSON is a "local" file used by a developer in the Editor, trusting external data to construct file paths without sanitization is a security risk. If a malicious JSON file is imported, it could contain names like `../../Scripts/Core/ImportantScript` which would overwrite arbitrary files in the project.
**Prevention:** Always sanitize any data sourced from external files (like JSON, XML) before using it in path construction. Use `Path.GetInvalidFileNameChars()` to filter out OS-level invalid characters and `Path.GetFileName()` to strip directory traversal sequences like `../` to ensure the resulting string is strictly a file name.
## 2024-05-24 - Path Traversal via Unsanitized JSON Data in Unity Editor
**Vulnerability:** Path traversal vulnerability in `CharacterFactory.cs` where imported JSON `charProfile.name` was used directly in an `AssetDatabase.CreateAsset` path without sanitization.
**Learning:** Unity Editor scripts that dynamically generate files or paths from imported data (like JSON) are susceptible to path traversal. Even seemingly safe strings like "character name" can be manipulated maliciously (e.g. `../MaliciousAsset`) to overwrite critical project assets or write files outside the intended folder.
**Prevention:** Always sanitize dynamically imported strings that are used in file paths. Use `Path.GetInvalidFileNameChars()` to strip/replace illegal characters, and ensure no directory separators are left before appending to a hardcoded base folder path.
## 2024-11-06 - Prevent Information Exposure via Error Logs in Unity Scripts
**Vulnerability:** Found `CampaignManager.cs` logging exact absolute file paths (using `Application.streamingAssetsPath` and `Application.dataPath`) and lacking exception handling around a JSON read/parse block, potentially leaking system info or stack traces.
**Learning:** Unity's built-in platform paths can expose underlying OS directory structures, usernames, or execution environments when logged directly, especially if logs are captured.
**Prevention:** Mask runtime exception stack traces (e.g. log only `ex.Message`) and replace absolute path logs with safe generic identifiers (like the filename) to avoid information exposure.
## 2024-05-18 - Information Disclosure via Absolute File Paths in Logs\n**Vulnerability:** Found `Debug.Log` statements outputting absolute paths like `Path.Combine(Application.dataPath, ...)` into Unity logs. In production, this leaks server or client filesystem structure, potentially aiding attackers in path traversal or deeper filesystem exploitation.\n**Learning:** `Application.dataPath` and `Application.streamingAssetsPath` resolve to complete absolute paths that should remain internal.\n**Prevention:** Only use filenames or safe identifiers when logging file operations, rather than the full `filePath`. Also, explicitly catch file parsing errors to avoid leaking internal stack traces.
## 2024-05-24 - [Information Leakage via Stack Trace]
**Vulnerability:** Public GameObjects and UI components (like `DialogueBox`, `SpeakerNameText`, and `DialogueText`) were being accessed directly in `Cinematic_IntoTheVoid.cs` without prior null checks.
**Learning:** If these components are unassigned in the Unity Editor, calling methods or properties on them triggers a `NullReferenceException`, which exposes the internal application call stack/stack trace. This violates "Fail securely - errors should not expose sensitive data."
**Prevention:** Always implement defensive programming by verifying that required object references are not null before accessing them. If they are missing, log a secure, generic error message (e.g., "Missing UI components required for cinematic") and safely abort the operation (e.g., using `return;`).
## 2024-05-24 - Unity File Path Information Disclosure and Unhandled Exception Logging
**Vulnerability:** Information Disclosure and Error Stack Trace Leak
**Learning:** `Application.dataPath` and `Application.streamingAssetsPath` resolve to physical file paths on the host system. Logging these directly in production environments or allowing unhandled exceptions from methods like `File.ReadAllText` and `JsonUtility.FromJson` to print to the console can expose internal file structures and stack traces to attackers.
**Prevention:** Always use relative file paths or file names (e.g. `fileName`) in logs. Wrap file I/O operations and JSON deserialization in `try-catch` blocks to fail securely and prevent unhandled exceptions from leaking internal details. Catch generic exceptions and provide a sanitized, secure error message.
## 2025-01-24 - Input Validation and Secure Exception Handling in Game Data Management
**Vulnerability:** The application lacked a systematic way to validate the integrity and security of deserialized JSON campaign data, potentially leading to logic errors or vulnerabilities if malicious data was loaded. Furthermore, previous "security fixes" in `CampaignManager.cs` and `CharacterFactory.cs` had introduced syntax errors and redundant/ineffective logging.
**Learning:** Security fixes applied without full contextual understanding or verification can lead to compilation errors and degraded maintainability. A centralized `IsValid()` pattern in the data model provides a clean, reusable way to enforce business constraints and security requirements immediately after deserialization.
**Prevention:** Implement an `IsValid()` method for all serializable data models that are populated from external sources. Use these methods at the point of ingestion (e.g., in `CampaignManager`). Ensure error handling in these ingestion points uses generic error messages and avoids logging absolute paths or stack traces.
## 2024-05-24 - Data Integrity and Security Validation for Deserialized Assets
**Vulnerability:** Untrusted external data (JSON) was being used directly by the application without validation, potentially leading to out-of-bounds values or corrupted application state.
**Learning:** Even if data is "local", it should be treated as untrusted input once it crosses the boundary from a file into the application.
**Prevention:** Implement an `IsValid()` pattern in data models to perform security and integrity checks immediately after deserialization. This ensures the application fails fast and securely when encountering malicious or corrupted data.

## 2024-04-22 - IDOR in Scene Object Lookups
**Vulnerability:** Insecure Direct Object Reference (IDOR) via unsanitized `GameObject.Find` queries in `SceneDirector.cs`.
**Learning:** External JSON data could provide malicious object IDs (like "CampaignManager") to interact with core architectural singletons, because `GameObject.Find` has uncontrolled access to any scene hierarchy object. Broad string heuristics were avoided to prevent functional regressions.
**Prevention:** Always sanitize and validate object IDs originating from external sources using exact matches before using them in scene traversal queries.

## 2024-04-22 - IDOR in Scene Object Interactions
**Vulnerability:** Insecure Direct Object Reference (IDOR) via unsanitized `interaction.objectId` queries in `SceneDirector.cs`.
**Learning:** External JSON data could provide malicious object IDs (like "CampaignManager") to interact with core architectural singletons, because `ApplyInteraction` uses the uncontrolled string ID to fetch and manipulate the game object. The validation must happen at the boundary where the untrusted input is used, not inside low-level generic retrieval utilities like `GetCachedObject` to avoid breaking internal game logic.
**Prevention:** Always sanitize and validate object IDs originating from external sources using exact matches before using them to interact with scene objects.
## 2025-05-14 - Code Rot as a Security Risk in Core Data Management
**Vulnerability:** Extreme code rot (redundant member declarations, syntax errors, and duplicate logic) in `CampaignManager.cs` and `CharacterData.cs` created a scenario where security validation could be accidentally bypassed. For example, `CampaignManager.cs` had overlapping `if` blocks that assigned deserialized data to global state even if validation failed on a different instance of the same data.
**Learning:** Code rot isn't just a maintainability issue; it's a security vulnerability. Contradictory or redundant logic paths make it difficult to verify that security controls (like `IsValid()` checks) are consistently and correctly applied.
**Prevention:** Consolidate data structures and logic into a single, clean "source of truth." Ensure that validation logic is performed on the exact object being assigned to the global state immediately after deserialization.

## 2025-05-14 - Resource Exhaustion (DoS) via Unsanitized Trait Strings
**Vulnerability:** While `HorizonGameData.cs` limited the number of traits per character, it did not limit the length of individual trait strings. A malicious JSON could contain a few traits each with millions of characters, leading to memory exhaustion.
**Learning:** Defense-in-depth requires validating both the quantity and the size of individual elements in collections sourced from external data.
**Prevention:** Implement length constraints for every string field in a serializable data model, especially within lists.

## 2024-05-24 - IDOR in Data-Driven Scene Construction
**Vulnerability:** `GameObject.Find` was used directly on unsanitized external strings from JSON, granting uncontrolled access to any scene hierarchy object.
**Learning:** External data targeting interactive objects can specify core singletons (like `CampaignManager` or `SceneDirector`), allowing attackers to manipulate or destroy critical game components. Broad heuristics like `.Contains("Manager")` break legitimate objects.
**Prevention:** Use exact string matching or specific tagging to block unauthorized access to core architectural singletons when performing dynamic object lookups.

## 2026-04-14 - Denial of Service (DoS) mitigation for GameObject.Find
**Vulnerability:** Untrusted input from JSON was used in `GameObject.Find` in `SceneDirector.cs`. Maliciously long strings or path-traversal-like patterns (e.g., using `/`) could lead to expensive scene traversals or unexpected object access.
**Learning:** `GameObject.Find` is an O(N) operation that can be abused. While caching helps, the initial lookup using unsanitized strings is a risk.
**Prevention:** Implement strict string length limits (e.g., 128 chars) and character whitelists (alphanumeric, underscores, spaces, etc.) for any string passed to `GameObject.Find` or used as a key in object caches derived from external data.

## 2025-05-14 - [Information Disclosure in Unity Logs]
**Vulnerability:** Absolute file paths being logged during data loading.
**Learning:** Hardcoded paths or `Application.dataPath` in logs can reveal developer filesystem structure.
**Prevention:** Only log filenames or relative identifiers in production-facing logs.

## 2026-05-12 - [Resource Exhaustion via Untrusted JSON Data]
**Vulnerability:** Deserialized campaign data (`HorizonGameData.cs`) lacked uniform length and count constraints, making the application vulnerable to resource exhaustion (DoS) attacks if a malicious JSON with extremely large strings or lists was loaded. Additionally, severe code rot (redundant class definitions) masked these vulnerabilities and impeded security audits.
**Learning:** Code rot and structural duplication not only degrade maintainability but also provide vectors for bypassing security controls or complicating audits. Centralized, hardened `IsValid()` methods are essential for defense-in-depth when dealing with untrusted external data.
**Prevention:** Consolidate data structures to ensure a single source of truth and implement strict validation (length, range, count) for all fields in serializable models.
## 2026-05-15 - [UI Injection via Code Rot in OtisTerminal]
**Vulnerability:** Redundant command processing logic in `OtisTerminal.cs` echoed user input to the terminal before performing length or regex validation. Since the terminal supports Rich Text tags, an attacker could inject malicious tags (e.g., extremely large font sizes or overlapping colors) to disrupt the UI or bypass intended interface constraints.
**Learning:** Code rot (duplicate logic paths and redundant echoes) often hides missing security controls. In this case, one echo path was correctly placed but another was redundant and unvalidated.
**Prevention:** Consolidate interactive input processing into a single, linear pipeline: Validate -> Sanitize -> Echo -> Execute. Never echo untrusted input before it has passed all security checks.

## 2026-05-18 - [Security Validation Bypass via Code Rot]
**Vulnerability:** A missing closing brace in a redundant scenario check within `HorizonGameData.cs` caused the C# compiler to skip subsequent character and scenario validation loops, effectively bypassing deep-validation for untrusted JSON data.
**Learning:** Code rot, specifically orphaned or malformed conditional blocks, can lead to silent security failures where critical validation logic is bypassed without triggering immediate runtime errors.
**Prevention:** Regularly audit validation logic for redundancy and ensure that all validation paths are fully covered by unit tests and static analysis. Favor clean, linear validation pipelines over complex, nested, or redundant checks.
