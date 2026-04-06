import re
import os

def find_dupes(path):
    try:
        with open(path, 'r') as f:
            lines = f.readlines()
    except:
        return

    # Track current class
    current_class = None
    methods_in_class = {}

    for line in lines:
        class_match = re.search(r'class\s+(\w+)', line)
        if class_match:
            current_class = class_match.group(1)
            methods_in_class[current_class] = set()

        method_match = re.search(r'(public|private|protected|internal|virtual|override|static|abstract)\s+[\w<>\d\[\]]+\s+(\w+)\s*\(', line)
        if method_match and current_class:
            method_name = method_match.group(2)
            if method_name in methods_in_class[current_class]:
                print(f"Duplicate method '{method_name}' in class '{current_class}' in {path}")
            methods_in_class[current_class].add(method_name)

for root, dirs, files in os.walk("Assets/Scripts"):
    for file in files:
        if file.endswith(".cs"):
            find_dupes(os.path.join(root, file))
