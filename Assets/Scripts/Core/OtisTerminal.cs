using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

namespace Milehigh.Core
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField terminalInput;
        [SerializeField] private TextMeshProUGUI terminalOutput;

        private Dictionary<string, Action<string, string>> commands = new Dictionary<string, Action<string, string>>();

        private void Start()
        {
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            commands["void"] = (sub, args) => {
                if (sub == "purge") Debug.Log("Executing Void Purge...");
                else if (sub == "scan") Debug.Log("Scanning Void signatures...");
            };
            commands["system"] = (sub, args) => {
                if (sub == "status") Debug.Log("System status: INTEGRITY NOMINAL");
            };
        }

        public void OnSubmitCommand(string input)
        {
            if (string.IsNullOrEmpty(input)) return;

            string[] parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length > 0)
            {
                string cmd = parts[0].ToLower();
                string subCmd = parts.Length > 1 ? parts[1].ToLower() : "";
                string args = parts.Length > 2 ? string.Join(" ", parts, 2, parts.Length - 2) : "";

                if (commands.ContainsKey(cmd))
                {
                    commands[cmd](subCmd, args);
                    ExecuteTerminalCommand(cmd, subCmd, args);
                }
                else
                {
                    LogOutput($"Unknown command: {cmd}");
                }
            }

            if (terminalInput != null) terminalInput.text = "";
        }

        private void ExecuteTerminalCommand(string cmd, string subCmd, string args)
        {
            LogOutput($"> {cmd} {subCmd} {args}");
        }

        private void LogOutput(string message)
        {
            if (terminalOutput != null)
            {
                terminalOutput.text += $"\n{message}";
            }
            Debug.Log($"Otis Terminal: {message}");
        }
    }
}
