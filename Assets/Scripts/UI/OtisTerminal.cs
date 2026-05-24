// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using TMPro; // Use TextMesh Pro equivalents
using MilehighWorld.Systems.Agency;
using MilehighWorld.Data;
using System.Threading;
using System.Text.RegularExpressions;

namespace MilehighWorld.UI
{
    public class OtisTerminal : MonoBehaviour
    {
        [SerializeField] private TMP_InputField terminalInput = null!;
        [SerializeField] private TextMeshProUGUI terminalOutput = null!;

        // 🛡️ Sentinel: Input validation constants to prevent DoS and malformed command injections
        private const int MaxInputLength = 256;
        private static readonly Regex SafeCommandRegex = new Regex(@"^[a-zA-Z0-9\s._\-]+$", RegexOptions.Compiled);

        public void OnSubmit(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return;

            // 🛡️ Sentinel: Input length and character validation
            if (input.Length > MaxInputLength || !SafeCommandRegex.IsMatch(input))
            {
                Debug.LogWarning($"[Security] Blocked invalid terminal input: {input}");
                if (terminalInput != null) terminalInput.text = "";
                return;
            }

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

            // 🛡️ Sentinel: Pass default player data to resolve compilation error and satisfy resolver requirements
            var playerData = new RuntimeCharacterData { Id = "Player_01", Archetype = "Engineer" };
            await NarrativeActionResolver.Instance.ExecuteLoreBoundChoiceAsync(context, playerData, CancellationToken.None);
        }
    }
}
