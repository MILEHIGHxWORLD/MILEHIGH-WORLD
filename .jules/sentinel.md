## 2025-05-14 - [Information Disclosure in Unity Logs]
**Vulnerability:** Absolute file paths being logged during data loading.
**Learning:** Hardcoded paths or `Application.dataPath` in logs can reveal developer filesystem structure.
**Prevention:** Only log filenames or relative identifiers in production-facing logs.

## 2025-01-24 - Restoration of Data Integrity Framework and Unit Testing for Core Mechanics
**Vulnerability:** Critical syntax errors and redundant logic were found in `HorizonGameData.cs` and `CampaignManager.cs`, breaking the security validation framework and CI/CD pipelines.
**Learning:** Botched merges and inconsistent security updates can lead to "broken windows" in the codebase, where intended security features (like `IsValid()` checks) are bypassed or rendered inert by compilation errors. Unit testing core mechanics like `IncreaseVoidSaturation` is essential to ensure that even simple logic adheres to security-driven constraints (like clamping and range validation).
**Prevention:** Always verify the codebase's compilability after applying security patches. Implement isolated unit tests for state-changing logic to guarantee that constraints are enforced consistently and that future refactoring doesn't break security-critical behavior.
## 2025-11-23 - Null Reference Protection in OmegaOneController

**Vulnerability:** OmegaOneController.CheckStability would crash with a NullReferenceException if the passed 'nearestShard' was null, as it accessed 'shard.name' in a Debug.Log statement.

**Learning:** Even simple logging statements can cause crashes if they dereference objects without null checks.

**Prevention:** Implement explicit null checks for GameObjects before accessing their properties, especially in methods that might be called with dynamic scene references.
## 2024-03-26 - Path Traversal Vulnerability during Asset Generation
**Vulnerability:** A path traversal vulnerability was present when dynamically creating Unity scriptable objects from JSON input (`charProfile.name`). A malicious actor could provide a JSON payload containing `../` sequences in character names, leading to arbitrary file write outside of the intended `Assets/Data/Characters` directory.
**Learning:** The previous path sanitization attempt was flawed. It correctly used `Path.GetInvalidFileNameChars()` to replace invalid characters, but then used `Path.GetFileName()` on a new `sanitizedName` variable that didn't have the invalid chars removed, or otherwise applied `Path.GetFileName()` incorrectly, leaving it vulnerable or broken.
**Prevention:** When dynamically creating files based on unstrusted input, always use `Path.GetFileName()` to extract only the final file component, and then optionally sanitize the remaining string against invalid filename characters. Never use `Path.Combine` or format strings for file paths with unverified input before stripping directory traversal sequences.
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
## 2024-05-24 - Fail Securely on File I/O in Unity Editor Scripts
**Vulnerability:** Unhandled exceptions during file reading (`File.ReadAllText`) and JSON deserialization (`JsonUtility.FromJson`) in Unity Editor scripts leak full stack traces and absolute file paths in the Unity Console.
**Learning:** Unity's default exception handling exposes implementation details (stack traces and internal directory structures). While this is an Editor script and won't be in the final build, it's still a poor security practice to leak sensitive developer paths, especially in collaborative environments or CI/CD pipelines.
**Prevention:** Always wrap `File.ReadAllText` and `JsonUtility.FromJson` in a `try-catch` block. Log a generic error message (e.g., "Error parsing file") and catch the exception gracefully to prevent Unity from printing the stack trace.
## 2025-01-24 - Input Validation and Secure Exception Handling in Game Data Management
**Vulnerability:** The application lacked a systematic way to validate the integrity and security of deserialized JSON campaign data, potentially leading to logic errors or vulnerabilities if malicious data was loaded. Furthermore, previous "security fixes" in `CampaignManager.cs` and `CharacterFactory.cs` had introduced syntax errors and redundant/ineffective logging.
**Learning:** Security fixes applied without full contextual understanding or verification can lead to compilation errors and degraded maintainability. A centralized `IsValid()` pattern in the data model provides a clean, reusable way to enforce business constraints and security requirements immediately after deserialization.
**Prevention:** Implement an `IsValid()` method for all serializable data models that are populated from external sources. Use these methods at the point of ingestion (e.g., in `CampaignManager`). Ensure error handling in these ingestion points uses generic error messages and avoids logging absolute paths or stack traces.
## 2024-05-24 - Data Integrity and Security Validation for Deserialized Assets
**Vulnerability:** Untrusted external data (JSON) was being used directly by the application without validation, potentially leading to out-of-bounds values or corrupted application state.
**Learning:** Even if data is "local", it should be treated as untrusted input once it crosses the boundary from a file into the application.
**Prevention:** Implement an `IsValid()` pattern in data models to perform security and integrity checks immediately after deserialization. This ensures the application fails fast and securely when encountering malicious or corrupted data.

## 2024-04-07 - Insecure Direct Object Reference (IDOR) via JSON Manipulation
**Vulnerability:** External JSON game data loaded via CampaignManager allowed applying scale and position modifications to any GameObject in the scene by supplying an exact name, including critical system objects like `CampaignManager`, `SceneDirector`, `CameraManager`, and `AlliancePowerManager`.
**Learning:** Even singleplayer/offline games loading external configuration files can be susceptible to IDOR-like data tampering vulnerabilities. Objects acting dynamically on names are a massive risk if those names come from an untrusted source. We must avoid broad heuristic matches (e.g., `Contains("Manager")`) to prevent functional regressions, utilizing explicit allowlists or denylists.
**Prevention:** Sanitize and validate object IDs originating from external sources before using them in dynamic systems. Maintain a strict denylist or allowlist to prevent manipulation of sensitive objects, and ensure specific tag matching rather than loose substring matching.
## 2025-01-24 - Ambiguity in Serializable Attribute and CI/Static Analysis Failures
**Vulnerability:** Static analysis (CodeQL) and standalone C# compilation checks can fail if `[Serializable]` is used while both `System` and `UnityEngine` namespaces are imported, as both define a `SerializableAttribute`.
**Learning:** While Unity's internal compiler handles this, standard .NET compilers and static analysis runners used in CI (like CodeQL) will flag it as a CS0104 ambiguity error. This can block security scans and CI pipelines.
**Prevention:** Always use the fully qualified `[System.Serializable]` attribute in Unity data models that might be processed by external tools or analyzed in CI to ensure consistent and reliable builds/scans.
## 2026-04-06 - Restoration of Hierarchical Security Validation and Resource Exhaustion Protection
**Vulnerability:** Found , , and  in a broken state due to poor merge edits, with duplicate  methods and broken logic. This disabled critical security checks for imported campaign data.
**Learning:** Security frameworks are fragile if not properly maintained. A broken  check is equivalent to no check at all, potentially allowing malformed or malicious data to compromise application state.
**Prevention:** Implement hierarchical  checks across all data models. Enforce resource limits (string lengths, collection sizes) at the point of deserialization to prevent DoS via resource exhaustion. Ensure each data class is responsible for its own validation.
## 2026-04-06 - Restoration of Hierarchical Security Validation and Resource Exhaustion Protection
**Vulnerability:** Found `HorizonGameData.cs`, `CampaignManager.cs`, and `CharacterFactory.cs` in a broken state due to poor merge edits, with duplicate `IsValid` methods and broken logic. This disabled critical security checks for imported campaign data.
**Learning:** Security frameworks are fragile if not properly maintained. A broken `IsValid` check is equivalent to no check at all, potentially allowing malformed or malicious data to compromise application state.
**Prevention:** Implement hierarchical `IsValid()` checks across all data models. Enforce resource limits (string lengths, collection sizes) at the point of deserialization to prevent DoS via resource exhaustion. Ensure each data class is responsible for its own validation.
## 2025-05-24 - Broken Security Validation Framework and Lack of Resource Limits
**Vulnerability:** The security validation framework was broken due to botched merge edits in `HorizonGameData.cs`, `CampaignManager.cs`, and `CharacterFactory.cs`, including duplicate `else` blocks and malformed methods. Additionally, the system lacked resource exhaustion protection for deserialized data.
**Learning:** Incomplete or unverified security patches can leave the system in a vulnerable state by breaking intended validation logic. Centralized recursive validation must include explicit limits on collection sizes and string lengths to mitigate DoS risks from external data.
**Prevention:** Maintain a single, robust `IsValid()` implementation per data class. Enforce string length and collection size limits in these methods. Always verify C# syntax and compilation (even via mocking) after applying security fixes to critical path components.

## 2025-01-30 - Resource Exhaustion Protection in Data Validation
**Vulnerability:** The application was vulnerable to Denial of Service (DoS) attacks via maliciously crafted JSON campaign data containing extremely large strings or collections, which could exhaust system memory.
**Learning:** Simple integrity checks (like range checks) are insufficient for security; resource limits must be explicitly enforced during deserialization validation.
**Prevention:** Implement "Sentinel Standard" resource limits in all `IsValid()` methods, enforcing maximum lengths for strings and maximum counts for collections based on realistic application needs.
## 2025-05-24 - [Sentinel Standard] Resource Exhaustion Protection in Data Validation
**Vulnerability:** Lack of constraints on string lengths and collection sizes in deserialized JSON data could lead to Denial of Service (DoS) through memory exhaustion.
**Learning:** Input validation must include not just range and format checks, but also quantity and size limits for all externally sourced data.
**Prevention:** Enforce strict limits on all deserialized strings (e.g., 64-1024 chars) and collections (e.g., 10-100 items) within hierarchical `IsValid()` methods. Ensure high-level data models recursively validate all nested objects.
## 2024-05-24 - File I/O and JSON Deserialization Exception Handling
**Vulnerability:** Unhandled exceptions during file reading (`File.ReadAllText`) and JSON deserialization (`JsonUtility.FromJson`) in Editor scripts like `CharacterFactory.cs` could crash the editor or leak internal system paths and stack traces to logs.
**Learning:** Unity's built-in file reading and JSON utilities do not fail gracefully on malformed data or missing files. Uncaught exceptions propagate up, potentially exposing sensitive environment structure in the stack trace, which is a risk if logs are aggregated or shared.
**Prevention:** Always wrap file I/O operations and JSON deserialization in `try-catch` blocks. Fail securely by logging generic, safe error messages that do not expose absolute file paths or internal call stacks.
## 2025-05-15 - Resource Exhaustion (DoS) Protection in Data Deserialization
**Vulnerability:** The application was vulnerable to Denial of Service (DoS) attacks via resource exhaustion. Maliciously crafted JSON files with extremely long strings or massive collection sizes could lead to excessive memory consumption or processing time.
**Learning:** Input validation must go beyond just checking for nulls or range bounds; it must also enforce strict limits on the scale of the data being ingested to prevent "billion laughs" style or OOM attacks.
**Prevention:** Enforce maximum lengths for all strings and maximum counts for all collections (Lists/Arrays) in the `IsValid()` method of deserialized data models. For example: `environment` (128 chars), `dialogue` (50 items), `characters` (50 items).
## 2024-05-24 - Untrusted Input Path Traversal
**Vulnerability:** A path traversal vulnerability during dynamic Unity asset creation in `CharacterFactory.cs` due to using untrusted JSON data directly in file paths.
**Learning:** Even internal tool scripts like `CharacterFactory` are vulnerable when accepting unverified external inputs such as JSON. When resolving a merge conflict, the sanitization logic `Path.GetInvalidFileNameChars()` was bypassed because a subsequent `Path.GetFileName(sanitizedName)` declaration accidentally reverted to the original unsanitized string.
**Prevention:** Always ensure that file name sanitization logic accurately processes and transforms the input completely. Validate input strings and use `Path.GetFileName` along with stripping directory traversal operators before embedding them into asset creation paths.
## 2025-05-24 - Syntax Regressions from Mangled Security Patches
**Vulnerability:** Redundant, overlapping, and syntactically invalid security checks (duplicate 'else' blocks and 'IsValid' methods) were found in core managers and data models.
**Learning:** Rapid, repeated application of targeted search-and-replace tools on similar code blocks can lead to "syntax soup" where security logic is present but broken or duplicated, potentially masking actual vulnerabilities or causing build failures.
**Prevention:** Always consolidate security validation into single, well-defined paths and verify the resulting source code for syntactic integrity after automated edits.
## 2025-05-15 - Consolidating Broken Security Validation and Resolving Path Traversal
**Vulnerability:** The project had multiple broken "security fixes" that introduced syntax errors and redundant logic, specifically around deserialized data validation and character asset creation paths. The `IsValid()` pattern was partially implemented but broken, and path traversal mitigation was duplicated and syntactically incorrect.
**Learning:** Incomplete or improperly merged security fixes can be as dangerous as the original vulnerabilities, as they may lead to compilation failures or bypassed security checks. Centralizing validation logic and ensuring clean path sanitization is critical.
**Prevention:** Always perform a full code review and basic sanity check (even if just manual brace counting) after applying security fixes to ensure no regressions or syntax errors are introduced.

## 2024-05-25 - IDOR in Scene Interaction Logic
**Vulnerability:** External JSON data could directly address and manipulate critical architectural singletons (e.g., CampaignManager, SceneDirector) using unsanitized object IDs via GameObject.Find, creating an Insecure Direct Object Reference (IDOR) vulnerability.
**Learning:** Applying blocklists or validation in low-level lookup utility methods causes functional regressions. The check must be implemented at the application boundary where the untrusted external input is first processed.
**Prevention:** Always validate and sanitize untrusted external input against an explicit blocklist or allowlist before passing it to scene traversal or object retrieval logic like GameObject.Find.
## 2024-05-24 - IDOR Vulnerability via Unsanitized JSON Data in Scene Interactions
**Vulnerability:** Insecure Direct Object Reference (IDOR) in `SceneDirector.cs` where untrusted external data (JSON `interaction.objectId`) was used to locate and manipulate arbitrary GameObjects via `GameObject.Find`, potentially allowing tampering with critical system managers.
**Learning:** In data-driven Unity architectures, using unsanitized strings directly from external sources to look up GameObjects creates severe vulnerabilities if critical components lack protection boundaries.
**Prevention:** Always validate and sanitize external object IDs. Implement explicit blocklists or allowlists at the application boundary (e.g., `ApplyInteraction`) using exact string matching to protect core architectural singletons from unauthorized manipulation.
## 2024-05-24 - Insecure Direct Object Reference (IDOR) in Scene Interactions
**Vulnerability:** Untrusted external JSON data (`interaction.objectId`) was used directly in `ApplyInteraction` to query the scene hierarchy via `GameObject.Find` and manipulate arbitrary objects.
**Learning:** In data-driven architectures, granting external configuration files unrestricted access to the scene hierarchy allows malicious files to tamper with critical system managers (e.g., `CampaignManager`, `SceneDirector`), leading to unpredictable behavior or security bypasses.
**Prevention:** Apply validation and blocklists (e.g., for core singletons) at the application boundary where untrusted external input is first processed (e.g., in `ApplyInteraction`) before it reaches low-level object retrieval utilities.
## 2025-05-24 - Prevent IDOR in Unity Data-Driven Interactions
**Vulnerability:** Insecure Direct Object Reference (IDOR) in `SceneDirector.cs` where untrusted external input (JSON) was passed directly to `GameObject.Find` via `GetCachedObject` during `ApplyInteraction`, allowing manipulation of any scene object.
**Learning:** Using unsanitized external strings to look up and modify GameObjects grants uncontrolled access to the scene hierarchy. Validation and blocklists must be applied at the application boundary where untrusted input is first processed, rather than inside generic internal retrieval utilities, to prevent functional regressions in internal logic.
**Prevention:** Always sanitize and validate object IDs from external sources against an explicit allowlist or blocklist before using them to interact with the game world.
## 2024-05-24 - Information Disclosure via Exception Messages in Logs
**Vulnerability:** Found `Debug.LogError` statements in `CampaignManager.cs` outputting `ex.Message` directly in catch blocks. This can inadvertently leak absolute file paths or internal system details if the exception message contains them.
**Learning:** Exception messages often contain platform-specific paths or environment details that should not be exposed in production logs.
**Prevention:** Mask runtime exception details by logging generic error messages (e.g., "See logs for details") and ensure that any file-related logs only use relative names or identifiers rather than full paths.
## 2024-05-24 - IDOR Vulnerability in Unity Data-Driven Architecture
**Vulnerability:** External JSON data could directly manipulate core singleton managers via GameObject lookups.
**Learning:** Unsanitized string IDs used for scene object retrieval create an Insecure Direct Object Reference (IDOR) risk, granting unintended access to architectural singletons.
**Prevention:** Apply explicit blocklists or validation boundaries where untrusted external data is first handled, rather than inside generic retrieval utilities, to protect singletons without breaking internal logic.
## 2026-05-05 - IDOR in SceneDirector ApplyInteraction
**Vulnerability:** Data-driven external input (JSON) in `ApplyInteraction` directly passed unsanitized strings to `GetCachedObject` (which uses `GameObject.Find`), allowing uncontrolled access to manipulate any scene hierarchy object.
**Learning:** This created an Insecure Direct Object Reference (IDOR) vulnerability where core architectural managers could be maliciously relocated or resized via external payloads.
**Prevention:** Apply validation and blocklists (e.g., blocking 'CampaignManager', 'SceneDirector', 'CameraManager', 'AlliancePowerManager') at the application boundary where untrusted external input is first processed, not in low-level utilities.
## 2026-05-07 - Unity IDOR via GameObject.Find
**Vulnerability:** External JSON data could directly reference and manipulate critical singleton managers (like CampaignManager) via `GameObject.Find` wrappers.
**Learning:** Data-driven architectures that pass external IDs directly to generic lookup utilities without boundary validation create Insecure Direct Object References (IDOR), allowing untrusted data to tamper with global state.
**Prevention:** Apply blocklists or whitelists at the application boundary (e.g., interaction processing) to sanitize external object IDs before they reach low-level lookup utilities.
## 2024-05-24 - Insecure Direct Object Reference in SceneDirector
**Vulnerability:** External JSON data could arbitrarily access and manipulate any scene object via `GameObject.Find` when processing object interactions, including core system managers.
**Learning:** Using `GameObject.Find` directly on unsanitized strings from external sources creates an IDOR vulnerability, allowing untrusted data to bypass intended access controls.
**Prevention:** Apply validation and blocklists (e.g., protecting CampaignManager, SceneDirector, CameraManager, AlliancePowerManager) at the application boundary where untrusted external input is first processed.
## 2024-05-25 - Insecure Direct Object Reference (IDOR) via Unity GameObject.Find
**Vulnerability:** `ApplyInteraction` in `SceneDirector.cs` allowed manipulating arbitrary game objects by passing their names from external JSON directly into a method that eventually calls `GameObject.Find`.
**Learning:** Exposing `GameObject.Find` to unsanitized external strings allows attackers to manipulate core architectural singletons and managers (e.g., CampaignManager), leading to unauthorized state changes.
**Prevention:** Apply validation and blocklists at the application boundary where untrusted external input is first processed, rather than in generic internal retrieval utilities, to prevent IDOR while avoiding regressions in internal logic.
## 2025-05-24 - Consolidated Security Validation and Resource Exhaustion Protection
**Vulnerability:** Massive "code rot" with multiple redundant, conflicting, and syntactically invalid 'IsValid()' methods and 'try-catch' blocks across core data and management scripts. This led to unpredictable security postures, potential information disclosure (absolute paths in logs), and vulnerability to Resource Exhaustion (DoS) via unconstrained JSON payloads.
**Learning:** Overlapping and improperly merged security patches can create "syntax soup" that disables the very protections they intend to provide. High-frequency edits with automated tools without manual consolidation can lead to multiple method signatures and mismatched braces.
**Prevention:** Always consolidate security validation into a single, robust 'IsValid()' framework. Enforce strict 'Sentinel Standard' resource limits (string lengths, collection counts) and range checks at the point of ingestion. Consistently use the 'fail secure' pattern by nulling data on validation failure and masking internal system details in logs.
