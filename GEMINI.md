# MILEHIGH-WORLD Project Standards

This document defines the architectural patterns, security protocols, and coding standards for the MILEHIGH-WORLD Unity project.

## 🛡️ The "Sentinel" Security Protocol

All data ingested from external sources (JSON, XML, etc.) must be treated as untrusted and validated immediately after deserialization.

- **Mandatory `IsValid()` Implementation:** Every data class that serves as a target for deserialization (e.g., in `HorizonGameData.cs`) must implement a `bool IsValid()` method.
- **Immediate Validation:** Call `IsValid()` immediately after `JsonUtility.FromJson` or similar operations. If validation fails, abort the operation and log a secure error message.
- **Constraint Enforcement:** Use `IsValid()` to check for null values, range bounds (e.g., saturation levels between 0 and 1), and list integrity.

## 📜 Clean Logging Policy

To prevent Information Disclosure vulnerabilities, adhere to the following logging rules:

- **No Absolute Paths:** Never log full system paths. Use `Path.GetFileName()` or relative paths to identify files in `Debug.Log`.
- **Sanitized Exceptions:** When catching exceptions, log only `ex.Message`. Avoid logging the full stack trace in production-facing code.
- **Fail Securely:** Wrap I/O and parsing logic in `try-catch` blocks to ensure the application fails gracefully without leaking internal details.

## 📂 Path Sanitization

Prevent Path Traversal vulnerabilities by sanitizing all strings used in file path construction:

- **Strip Traversal Sequences:** Use `Path.GetFileName()` to ensure a string cannot be used to navigate up the directory tree (e.g., `../`).
- **Replace Invalid Characters:** Use `Path.GetInvalidFileNameChars()` to identify and replace OS-level illegal characters with underscores.
- **Standard Formatting:** Replace spaces with underscores for consistent file naming.

## 🏗️ Unity Best Practices

- **Serialization:** Prefer `[SerializeField] private` over public variables for Inspector exposure.
- **Namespaces:** All scripts should be contained within the `Milehigh` namespace hierarchy (e.g., `Milehigh.Core`, `Milehigh.Data`).
