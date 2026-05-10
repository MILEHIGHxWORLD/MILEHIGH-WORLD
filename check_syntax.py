import os
import re

def check_cs_files():
    for root, dirs, files in os.walk("Assets/Scripts"):
        for file in files:
            if file.endswith(".cs"):
                path = os.path.join(root, file)
                with open(path, "r") as f:
                    content = f.read()
                    # Check for brace balance
                    if content.count("{") != content.count("}"):
                        print(f"Brace mismatch in {path}: {content.count('{')} {{ vs {content.count('}')} }}")

                    # Check for artifacts
                    if "2w33 f." in content:
                        print(f"Artifact '2w33 f.' found in {path}")

                    # Check for common NRT issue: accessing property on singleton without local capture
                    # Pattern: CampaignManager.Instance.currentCampaignData.something
                    if re.search(r"CampaignManager\.Instance\.currentCampaignData\.\w+", content):
                        print(f"NRT issue: Direct access to CampaignManager.Instance.currentCampaignData in {path}")

                    # Check for duplicate fields/properties
                    lines = content.splitlines()
                    fields = []
                    current_class = ""
                    for line in lines:
                        if "class " in line:
                            current_class = line.split("class ")[1].split(":")[0].strip().split()[0]
                            fields = []
                        match = re.search(r"public \w+ (\w+)( =|;)", line)
                        if match:
                            field_name = match.group(1)
                            if field_name in fields:
                                print(f"Duplicate field '{field_name}' in class '{current_class}' in {path}")
                            fields.append(field_name)

if __name__ == "__main__":
    check_cs_files()
