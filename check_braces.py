import os

def check_file(filepath):
    with open(filepath, 'r') as f:
        content = f.read()

    open_braces = content.count('{')
    close_braces = content.count('}')

    if open_braces != close_braces:
        print(f"Unbalanced braces in {filepath}: {{ {open_braces}, }} {close_braces}")
        return False
    return True

all_pass = True
for root, dirs, files in os.walk('Assets/Scripts'):
    for file in files:
        if file.endswith('.cs'):
            if not check_file(os.path.join(root, file)):
                all_pass = False

if all_pass:
    print("All .cs files in Assets/Scripts have balanced braces.")
