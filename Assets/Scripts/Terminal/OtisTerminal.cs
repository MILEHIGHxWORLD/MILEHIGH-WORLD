using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Collections;

namespace MilehighWorld.World.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField commandInput;
        [SerializeField] private TextMeshProUGUI outputDisplay;

        // 🛡️ Sentinel: Security constants for input validation to prevent DoS and malformed input processing.
        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);

        private Coroutine? _typewriterCoroutine;
        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = -1;

        // ⚡ Bolt: Cache for WaitForSeconds using millisecond keys to prevent floating-point precision issues
        // and eliminate redundant GC allocations during coroutine execution.
        private static readonly Dictionary<int, WaitForSeconds> _waitCache = new Dictionary<int, WaitForSeconds>();

        private static WaitForSeconds GetWait(float seconds)
        {
            int ms = Mathf.RoundToInt(seconds * 1000f);
            if (!_waitCache.TryGetValue(ms, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[ms] = wait;
            }
            return wait;
        }

        private void Start()
        {
            if (outputDisplay != null)
            {
                outputDisplay.text = "";
                WriteToTerminal("[SYSTEM]: OTIS Terminal Online. Type 'help' for commands.");
            }
        }

        private void OnEnable()
        {
            if (commandInput != null)
            {
                commandInput.ActivateInputField();
            }
        }

        private void Update()
        {
            if (commandInput == null || !commandInput.isFocused) return;

            // Palette: Productivity - Ctrl+L shortcut to clear the terminal for better workspace management.
            bool isControlPressed = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);
            if (isControlPressed && Input.GetKeyDown(KeyCode.L))
            {
                ClearTerminalDisplay();
                commandInput.text = "";
            }

            // Palette: Refined history navigation - ensure responsiveness by polling in Update.
            if (Input.GetKeyDown(KeyCode.UpArrow)) NavigateHistory(-1);
            else if (Input.GetKeyDown(KeyCode.DownArrow)) NavigateHistory(1);

            // Palette: Productivity - Tab-to-Autocomplete for common commands.
            if (Input.GetKeyDown(KeyCode.Tab)) HandleAutocomplete();
        }

        private void HandleAutocomplete()
        {
            if (commandInput == null || string.IsNullOrWhiteSpace(commandInput.text)) return;

            string input = commandInput.text.ToLower();
            string[] commands = { "help", "clear" };

            foreach (string cmd in commands)
            {
                if (cmd.StartsWith(input))
                {
                    commandInput.text = cmd;
                    commandInput.caretPosition = cmd.Length;
                    break;
                }
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;
            _historyIndex = Mathf.Clamp(_historyIndex + direction, 0, _commandHistory.Count);
            commandInput.text = _historyIndex < _commandHistory.Count ? _commandHistory[_historyIndex] : "";
            commandInput.caretPosition = commandInput.text.Length;
        }

        public void ProcessCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;
            if (_commandHistory.Count == 0 || _commandHistory[^1] != input) _commandHistory.Add(input);
            _historyIndex = _commandHistory.Count;

            // UX Enhancement: Clear input and refocus immediately for better flow
            if (commandInput != null)
            {
                commandInput.text = "";
                commandInput.ActivateInputField();
            }

            // 🛡️ Sentinel: Input validation and DoS protection
            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n[SECURITY]: <color=#FF0000>Input exceeds maximum length (256 characters).</color>");
                if (commandInput != null) StartCoroutine(ShakeInputField());
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal("\n[SECURITY]: <color=#FF0000>Invalid characters. Use only A-Z, 0-9, spaces, '.', '_', and '-'.</color>");
                if (commandInput != null) StartCoroutine(ShakeInputField());
                return;
            }

            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();

            if (command == "clear")
            {
                ClearTerminalDisplay();
                return;
            }

            if (command == "help")
            {
                WriteToTerminal("\n[SYSTEM]: <color=#FFFF00>Available Commands:</color>" +
                                "\n - <color=#00FFFF>help</color>: Show this message." +
                                "\n - <color=#00FFFF>clear</color>: Clear the terminal display (or Ctrl+L)." +
                                "\n - <color=#00FFFF>[cmd] [arg1] [arg2]</color>: Execute extended system commands." +
                                "\n\n[SYSTEM]: <color=#FFFF00>Shortcuts:</color> Up/Down Arrow for History, Tab to Autocomplete, Ctrl+L to Clear.");
                return;
            }

            if (parts.Length >= 3)
            {
                int index = input.IndexOf(parts[2]);
                if (index != -1)
                {
                    string argument = input.Substring(index);
                    ExecuteExtendedCommand(parts[0], argument);
                    WriteToTerminal($"\n[SYSTEM]: <color=#00FF00>Command '{parts[0]}' executed.</color>");
                }
            }
            else
            {
                WriteToTerminal($"\n[SYSTEM]: <color=#FF0000>Unknown command or invalid argument count: '{parts[0]}'</color>");
                if (commandInput != null) StartCoroutine(ShakeInputField());
            }
        }

        private void ClearTerminalDisplay()
        {
            if (outputDisplay == null) return;
            outputDisplay.text = "";
            outputDisplay.maxVisibleCharacters = 0;
        }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;

            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                outputDisplay.maxVisibleCharacters = int.MaxValue; // Reveal all current text
            }

            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            outputDisplay.ForceMeshUpdate();
            int startVisibleCount = outputDisplay.textInfo.characterCount;

            outputDisplay.text += message;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;

            int charactersToReveal = endVisibleCount - startVisibleCount;

            for (int i = 0; i <= charactersToReveal; i++)
            {
                outputDisplay.maxVisibleCharacters = startVisibleCount + i;

                // UX Learning: Punctuation delays trigger after character is visible
                if (i > 0 && i <= charactersToReveal)
                {
                    char c = outputDisplay.textInfo.characterInfo[startVisibleCount + i - 1].character;
                    if (c == '.' || c == ':' || c == '!')
                        yield return GetWait(0.15f);
                    else if (c == ',')
                        yield return GetWait(0.08f);
                }

                yield return GetWait(0.02f);
            }

            _typewriterCoroutine = null;
        }

        private IEnumerator ShakeInputField()
        {
            if (commandInput == null) yield break;
            Vector3 originalPos = commandInput.transform.localPosition;
            float elapsed = 0f;
            float duration = 0.2f;
            float magnitude = 5f;

            while (elapsed < duration)
            {
                float x = UnityEngine.Random.Range(-1f, 1f) * magnitude;
                commandInput.transform.localPosition = originalPos + new UnityEngine.Vector3(x, 0, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            commandInput.transform.localPosition = originalPos;
        }

        private void ExecuteExtendedCommand(string cmd, string args)
        {
            // Implementation for specific terminal logic
        }
    }
}
