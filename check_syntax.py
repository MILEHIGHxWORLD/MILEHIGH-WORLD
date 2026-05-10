import sys
import re

def check_file(filename):
    with open(filename, 'r') as f:
        content = f.read()

    # Remove comments
    content = re.sub(r'//.*', '', content)
    content = re.sub(r'/\*.*?\*/', '', content, flags=re.DOTALL)

    # Remove strings
    content = re.sub(r'".*?"', '""', content)
    content = re.sub(r"'.*?'", "''", content)

    braces = 0
    for char in content:
        if char == '{':
            braces += 1
        elif char == '}':
            braces -= 1
        if braces < 0:
            return False, "Negative brace count"

    return braces == 0, f"Unbalanced braces: {braces}"

if __name__ == "__main__":
    for arg in sys.argv[1:]:
        ok, msg = check_file(arg)
        if not ok:
            print(f"{arg}: {msg}")
