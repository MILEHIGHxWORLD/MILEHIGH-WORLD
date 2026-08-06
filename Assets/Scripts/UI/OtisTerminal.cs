// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using TMPro; // Use TextMesh Pro equivalents
using MilehighWorld.Systems.Agency;
using MilehighWorld.Data;
using System.Threading;

namespace MilehighWorld.UI
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField terminalInput;
        [SerializeField] private TextMeshProUGUI terminalOutput;

        private readonly System.Collections.Generic.List<string> _commandHistory = new System.Collections.Generic.List<string>();
        private int _historyIndex = -1;

        private void OnEnable()
        {
            if (terminalInput != null)
            {
                terminalInput.ActivateInputField();
            }
        }

        private void Update()
        {
            if (terminalInput == null || !terminalInput.isFocused) return;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NavigateHistory(-1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                NavigateHistory(1);
            }
        }

        private void NavigateHistory(int direction)
        {
            if (_commandHistory.Count == 0) return;
            _historyIndex = Mathf.Clamp(_historyIndex + direction, 0, _commandHistory.Count);
            terminalInput.text = _historyIndex < _commandHistory.Count ? _commandHistory[_historyIndex] : "";
            terminalInput.caretPosition = terminalInput.text.Length;
        }

        public void OnSubmit(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            // Save to history list
            if (_commandHistory.Count == 0 || _commandHistory[^1] != input)
            {
                _commandHistory.Add(input);
            }
            _historyIndex = _commandHistory.Count;

            // Add bounds checking to the terminal's input parsing
            int spaceIndex = input.IndexOf(' ');

            // Fix: If no space is found, IndexOf returns -1.
            // We treat the whole string as the command.
            string command = (spaceIndex == -1) ? input : input.Substring(0, spaceIndex);
            string args = (spaceIndex == -1) ? "" : input.Substring(spaceIndex + 1);

            ExecuteCommand(command.ToLower(), args);

            if (terminalInput != null)
            {
                terminalInput.text = ""; // Clear input
                terminalInput.ActivateInputField(); // Refocus input field for continuous typing
            }
        }

        private async void ExecuteCommand(string command, string args)
        {
            if (terminalOutput != null)
            {
                terminalOutput.text += $"\n> {command} {args}";
            }

            Debug.Log($"[OtisTerminal]: Executing command: {command} with args: {args}");

            // Format this as a hacking/query attempt for the Universal Action Resolver
            var context = new NarrativeActionContext
            {
                ActionType = NarrativeActionContext.ActionType.HACK_TERMINAL,
                TargetId = "Otis_Mainframe",
                RequiresVisualValidation = false,
                CurrentDimension = "ŁĪNC"
            };

            await NarrativeActionResolver.Instance.ExecuteLoreBoundChoiceAsync(context, default(RuntimeCharacterData), CancellationToken.None);
        }
    }
}
