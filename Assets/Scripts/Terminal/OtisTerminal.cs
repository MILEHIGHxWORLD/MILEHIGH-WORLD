// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using TMPro;

namespace MilehighWorld.Terminal
{
    public class OtisTerminal : MonoBehaviour
    {
        [Header("Terminal UI References")]
        [SerializeField] private TMP_InputField commandInput = null!;
        [SerializeField] private TextMeshProUGUI outputDisplay = null!;

        [Header("Terminal Settings")]
        [SerializeField] private int maxHistory = 100;
        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s\.\_\-]+$", RegexOptions.Compiled);

        private List<string> _commandHistory = new List<string>();
        private int _historyIndex = 0;
        private Coroutine? _typewriterCoroutine;
        private string _lastSuggestion = "";

        // ⚡ Bolt: Cache for WaitForSeconds using millisecond keys to prevent floating-point precision issues
        // and eliminate redundant GC allocations during coroutine execution.
        private static readonly Dictionary<int, WaitForSeconds> _waitCache = new Dictionary<int, WaitForSeconds>();

        private void Awake()
        {
            if (commandInput != null)
            {
                commandInput.onEndEdit.AddListener(ProcessCommand);
                commandInput.ActivateInputField();
            }

            ClearTerminalDisplay();
            WriteToTerminal("[SYSTEM]: <color=#00FF00>OTIS-V1 Terminal Online.</color>\n[SYSTEM]: Type '<color=#00FFFF>help</color>' for a list of available commands.");
        }

        private void Update()
        {
            if (commandInput == null || !commandInput.isFocused) return;

            if (Input.GetKeyDown(KeyCode.UpArrow)) NavigateHistory(-1);
            if (Input.GetKeyDown(KeyCode.DownArrow)) NavigateHistory(1);
            if (Input.GetKeyDown(KeyCode.Tab)) HandleAutocomplete();
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.L)) ClearTerminalDisplay();
        }

        private WaitForSeconds GetWait(float seconds)
        {
            int ms = Mathf.RoundToInt(seconds * 1000f);
            if (!_waitCache.TryGetValue(ms, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[ms] = wait;
            }
            return wait;
        }

        private void HandleAutocomplete()
        {
            if (string.IsNullOrEmpty(commandInput.text)) return;

            string input = commandInput.text.ToLower();
            string[] commands = { "help", "clear", "verify" };

            // Priority 1: Standard prefix matching.
            foreach (string cmd in commands)
            {
                if (cmd.StartsWith(input))
                {
                    commandInput.text = cmd;
                    commandInput.caretPosition = cmd.Length;
                    return;
                }
            }

            // Priority 2: Fuzzy-based autocomplete as a fallback for typos
            string suggestion = GetCommandSuggestion(input);
            if (!string.IsNullOrEmpty(suggestion))
            {
                commandInput.text = suggestion;
                commandInput.caretPosition = suggestion.Length;
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

            // Palette: Echo user command for better feedback and professional feel.
            WriteToTerminal($"\n<color=#00FFFF>> {input}</color>");

            if (_commandHistory.Count == 0 || _commandHistory[^1] != input)
            {
                _commandHistory.Add(input);
                if (_commandHistory.Count > maxHistory) _commandHistory.RemoveAt(0);
            }
            _historyIndex = _commandHistory.Count;

            if (commandInput != null)
            {
                commandInput.text = "";
                commandInput.ActivateInputField();
            }

            if (input.Length > MaxInputLength)
            {
                WriteToTerminal("\n[SECURITY]: <color=#FF0000>Input exceeds maximum length (256 characters).</color>");
                StartCoroutine(ShakeInputField());
                return;
            }

            if (!SafeCommandRegex.IsMatch(input))
            {
                WriteToTerminal("\n[SECURITY]: <color=#FF0000>Invalid characters detected.</color>");
                StartCoroutine(ShakeInputField());
                return;
            }

            string[] parts = input.Trim().Split(' ');
            string command = parts[0].ToLower();

            switch (command)
            {
                case "clear":
                    ClearTerminalDisplay();
                    break;
                case "help":
                    WriteToTerminal("\n[SYSTEM]: <color=#FFFF00>Available Commands:</color>" +
                                    "\n - <color=#00FFFF>help</color>: Show this message." +
                                    "\n - <color=#00FFFF>clear</color>: Clear terminal." +
                                    "\n - <color=#00FFFF>verify</color>: Run data integrity check." +
                                    "\n\n[SYSTEM]: <color=#FFFF00>Shortcuts:</color> Arrows (History), Tab (Autocomplete), Ctrl+L (Clear).");
                    break;
                case "verify":
                    WriteToTerminal("\n[SYSTEM]: Initiating Data Integrity Check..." +
                                    "\n[ECC]: <color=#00FF00>Reality parity at 100%.</color>");
                    break;
                default:
                    string suggestion = GetCommandSuggestion(command);
                    string error = $"\n[SYSTEM]: <color=#FF0000>Unknown command '{command}'.</color>";
                    if (!string.IsNullOrEmpty(suggestion))
                    {
                        error += $"\n[SYSTEM]: Did you mean: <color=#00FFFF>{suggestion}</color>? (Press Tab)";
                    }
                    WriteToTerminal(error);
                    StartCoroutine(ShakeInputField());
                    break;
            }
        }

        private string GetCommandSuggestion(string input)
        {
            string[] availableCommands = { "help", "clear", "verify" };
            string bestMatch = "";
            int minDistance = int.MaxValue;

            foreach (string cmd in availableCommands)
            {
                int distance = ComputeLevenshteinDistance(input, cmd);
                if (distance < minDistance && distance <= 2)
                {
                    minDistance = distance;
                    bestMatch = cmd;
                }
            }

            return bestMatch;
        }

        private void ClearTerminalDisplay()
        {
            if (outputDisplay != null)
            {
                outputDisplay.text = "";
                outputDisplay.maxVisibleCharacters = 0;
            }
            if (commandInput != null) commandInput.ActivateInputField();
        }

        private void WriteToTerminal(string message)
        {
            if (outputDisplay == null) return;

            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                outputDisplay.maxVisibleCharacters = int.MaxValue;
            }

            _typewriterCoroutine = StartCoroutine(TypewriterEffect(message));
        }

        private IEnumerator TypewriterEffect(string message)
        {
            int startVisibleCount = outputDisplay.textInfo.characterCount;
            outputDisplay.text += message;
            outputDisplay.ForceMeshUpdate();
            int endVisibleCount = outputDisplay.textInfo.characterCount;

            for (int i = startVisibleCount; i <= endVisibleCount; i++)
            {
                outputDisplay.maxVisibleCharacters = i;
                if (i > startVisibleCount)
                {
                    char c = outputDisplay.textInfo.characterInfo[i - 1].character;
                    if (c == '.' || c == ':' || c == '!') yield return GetWait(0.15f);
                    else if (c == ',') yield return GetWait(0.08f);
                }
                yield return GetWait(0.02f);
            }

            outputDisplay.maxVisibleCharacters = outputDisplay.textInfo.characterCount;
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
                commandInput.transform.localPosition = originalPos + new Vector3(x, 0, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }
            commandInput.transform.localPosition = originalPos;
        }

        private int ComputeLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0) return m;
            if (m == 0) return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Mathf.Min(Mathf.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[n, m];
        }
    }
}
