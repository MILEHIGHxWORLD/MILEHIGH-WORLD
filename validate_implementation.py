import json
import os

def validate_json():
    json_path = "Assets/Scripts/Data/campaign_master.json"
    if not os.path.exists(json_path):
        print(f"Error: {json_path} not found.")
        return False

    try:
        with open(json_path, 'r') as f:
            data = json.load(f)

        required_keys = ["sceneId", "metadata", "characters", "scenarios"]
        for key in required_keys:
            if key not in data:
                print(f"Error: Missing key '{key}' in campaign_master.json")
                return False

        # Check one interaction for the new structure
        first_scenario = data["scenarios"][0]
        first_interaction = first_scenario["interactiveObjects"][0]
        if "isVector" not in first_interaction:
             print("Error: interaction missing 'isVector' flag")
             return False

        print("campaign_master.json validation passed.")
        return True
    except Exception as e:
        print(f"Error parsing JSON: {e}")
        return False

def check_files():
    files_to_check = [
        "Assets/Scripts/Data/HorizonGameData.cs",
        "Assets/Scripts/Data/CharacterData.cs",
        "Assets/Scripts/Editor/CharacterFactory.cs",
        "Assets/Scripts/Core/CampaignManager.cs",
        "Assets/Scripts/Core/SceneDirector.cs",
        "Assets/Scripts/Characters/CharacterControllerBase.cs",
        "Assets/Scripts/Characters/DelilahAIController.cs",
        "Assets/Scripts/Narrative/ReverieDialogueSync.cs"
    ]

    all_exist = True
    for f in files_to_check:
        if not os.path.exists(f):
            print(f"Error: {f} is missing.")
            all_exist = False
        else:
            print(f"Confirmed: {f} exists.")

    return all_exist

def validate_compliance_assets():
    """Verify SOC 2 compliance directory structure and core files."""
    required_paths = [
        "Compliance/Scripts/evidence_collector.py",
        "Compliance/Scripts/identity_audit.py",
        "Compliance/Infrastructure/main.tf",
        "Compliance/Policies/information_security_policy.md",
        "Compliance/Policies/access_control_policy.md",
        ".github/workflows/compliance-monitor.yml"
    ]

    missing = []
    for path in required_paths:
        if not os.path.exists(path):
            missing.append(path)
            print(f"Error: Compliance asset missing: {path}")
        else:
            print(f"Confirmed: Compliance asset exists: {path}")

    return len(missing) == 0

if __name__ == "__main__":
    v_json = validate_json()
    v_files = check_files()
    v_compliance = validate_compliance_assets()

    if v_json and v_files and v_compliance:
        print("\nAll validation checks passed!")
    else:
        print("\nValidation failed.")
        exit(1)
