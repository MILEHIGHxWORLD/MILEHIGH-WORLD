
## 2025-01-24 - Restoration of Data Integrity Framework and Unit Testing for Core Mechanics
**Vulnerability:** Critical syntax errors and redundant logic were found in `HorizonGameData.cs` and `CampaignManager.cs`, breaking the security validation framework and CI/CD pipelines.
**Learning:** Botched merges and inconsistent security updates can lead to "broken windows" in the codebase, where intended security features (like `IsValid()` checks) are bypassed or rendered inert by compilation errors. Unit testing core mechanics like `IncreaseVoidSaturation` is essential to ensure that even simple logic adheres to security-driven constraints (like clamping and range validation).
**Prevention:** Always verify the codebase's compilability after applying security patches. Implement isolated unit tests for state-changing logic to guarantee that constraints are enforced consistently and that future refactoring doesn't break security-critical behavior.
