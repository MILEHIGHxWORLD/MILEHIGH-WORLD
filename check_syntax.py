import os
import re

def check_csharp_syntax(filepath):
    with open(filepath, 'r') as f:
        lines = f.readlines()

    content = "".join(lines)

    # Check braces
    open_braces = content.count('{')
    close_braces = content.count('}')
    if open_braces != close_braces:
        print(f"Unbalanced braces in {filepath}: {{ {open_braces}, }} {close_braces}")

    # Check for redundant else after return
    for i in range(len(lines)):
        stripped = lines[i].strip()
        if "return;" in stripped or "return " in stripped or "break;" in stripped or "continue;" in stripped:
            # Look for next line(s) that might contain "else"
            for j in range(i+1, min(i+5, len(lines))):
                if "} else" in lines[j] or "else {" in lines[j] or "else" == lines[j].strip():
                    print(f"Redundant else in {filepath} near line {j+1}")
                    break

    # Check for suspicious semicolon after if
    for i in range(len(lines)):
        match = re.search(r'if\s*\(.*\);', lines[i])
        if match:
            print(f"Suspicious semicolon after if in {filepath} at line {i+1}")

for root, dirs, files in os.walk('Assets/Scripts'):
    for file in files:
        if file.endswith('.cs'):
            check_csharp_syntax(os.path.join(root, file))
