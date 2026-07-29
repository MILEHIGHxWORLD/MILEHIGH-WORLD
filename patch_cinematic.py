import re

with open('Assets/Scripts/Story/Cinematic_IntoTheVoid.cs', 'r') as f:
    content = f.read()

# Fix braces in Update method
update_pattern = re.compile(
    r'(private void Update\(\)\s*\{.*?if \(_idleTimer >= 2f && skipHint != null && !skipHint\.activeSelf\)\s*\{\s*skipHint\.SetActive\(true\);\s*\})(.*?)(?=\s*private async Task ExecuteConvergenceSequenceAsync)',
    re.DOTALL
)

match = update_pattern.search(content)
if match:
    # Remove the extra stuff that makes it uncompilable / duplicated logic
    replacement = match.group(1) + "\n            }\n        }"
    new_content = content[:match.start()] + replacement + content[match.end():]

    with open('Assets/Scripts/Story/Cinematic_IntoTheVoid.cs', 'w') as f:
        f.write(new_content)
    print("Fixed Update method braces.")
else:
    print("Could not find Update method block.")
