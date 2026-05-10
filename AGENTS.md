# Agent Briefing: MILEHIGH-WORLD

This document provides instructions for agents (like Jules and Gemini CLI) working on the MILEHIGH-WORLD project.

## 🛠️ Build and Validation

Before submitting any code changes, agents must run the following validation script to ensure the project structure and data integrity are maintained:

```bash
python3 validate_implementation.py
```

Additionally, run the C# unit tests:
```bash
dotnet test Testing/Testing.csproj
```

## 🛡️ Security Compliance

All contributions must adhere to the standards defined in `GEMINI.md`, specifically:
- Implementation of the **Sentinel Protocol** for data ingestion (mandatory `IsValid()` calls).
- Adherence to the **Clean Logging Policy** (no absolute paths, sanitized exceptions).
- **Path Sanitization** for all dynamic file operations.

## 📦 Project Structure

- `Assets/Scripts/Core`: Core game management and logic (e.g., `CombatManager`, `CampaignManager`).
- `Assets/Scripts/Data`: Data models and JSON campaign data (e.g., `CharacterData`, `HorizonGameData`).
- `Assets/Scripts/Editor`: Editor-only tools and importers (e.g., `CharacterFactory`).
- `Assets/Scripts/Characters`: AI and character behavior scripts.

## 🚀 Jules Agent Workflows

### Task Multiplier Script
Use the provided bash scripts to deploy multiple Jules agents in parallel for complex tasks. Ensure `tasks.txt` is updated with specific sub-tasks.

### Infrastructure Guide
Follow the `setup_jules.sh` configurations to maintain a consistent Google Cloud VM environment, ensuring all system dependencies and baseline tests are correctly initialized.

### CLI Piping
Utilize GitHub CLI integration to pipe issues or data directly into Jules remote sessions for automated triage and implementation.

### Core Commands
- `jules login`: Authenticate the current session.
- `jules remote pull`: Fetch cloud-generated changes to your local branch.

### System Prompting
Always refer to the project root `AGENTS.md` (this file) and `GEMINI.md` for specific coding standards and security protocols.

## 🚀 Deployment Rules

- Do not modify files in the `Assets/StreamingAssets` directory directly unless instructed.
- Ensure all new ScriptableObjects are generated through the `CharacterFactory` or equivalent editor tools to maintain path sanitization.
- Strictly maintain the folder-to-namespace hierarchy (e.g., `Milehigh.Data`).
