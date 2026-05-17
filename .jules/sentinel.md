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
