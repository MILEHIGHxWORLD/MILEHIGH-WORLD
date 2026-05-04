## 2025-05-14 - [Information Disclosure in Unity Logs]
**Vulnerability:** Absolute file paths being logged during data loading.
**Learning:** Hardcoded paths or `Application.dataPath` in logs can reveal developer filesystem structure.
**Prevention:** Only log filenames or relative identifiers in production-facing logs.
