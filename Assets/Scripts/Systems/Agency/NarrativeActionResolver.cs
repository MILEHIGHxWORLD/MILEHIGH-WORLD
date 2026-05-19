// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System.Threading;
using System.Threading.Tasks;

namespace MilehighWorld.Systems.Agency
{
    public class NarrativeActionContext
    {
        public enum ActionType
        {
            HACK_TERMINAL,
            COMBAT_ACTION,
            DIALOGUE_CHOICE
        }

        public ActionType ActionType;
        public string TargetId = "";
        public bool RequiresVisualValidation;
        public string CurrentDimension = "";
    }

    public class NarrativeActionResolver
    {
        public static NarrativeActionResolver Instance { get; } = new NarrativeActionResolver();

        public async Task ExecuteLoreBoundChoiceAsync(NarrativeActionContext context, CancellationToken ct)
        {
            // Implementation of lore-bound choice execution
            await Task.Yield();
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Milehigh.Core;
using Milehigh.Data;

namespace Milehigh.Systems.Agency
{
    public class NarrativeActionResolver : MonoBehaviour
    {
        public static NarrativeActionResolver Instance;
        [SerializeField] private Camera playerEyeCamera;
        private const string MUDP_RESOLVE_URL = "https://api.milehigh.world/v1/udp/resolve-action";

        private void Awake()
        {
            // SENTINEL: Security & Robustness - Singleton pattern with null check
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);

            if (playerEyeCamera == null)
            {
                Debug.LogError("[SENTINEL] NarrativeActionResolver: playerEyeCamera is not assigned!");
            }
        }

        public async Task ExecuteLoreBoundChoiceAsync(NarrativeActionContext context, RuntimeCharacterData playerData, CancellationToken token)
        {
            // SENTINEL: Basic validation
            if (context == null)
            {
                Debug.LogError("[SENTINEL] NarrativeActionResolver: Context is null!");
                return;
            }

            string visualContextBase64 = context.RequiresVisualValidation ? await CapturePlayerViewAsync() : null;

            // The payload now forces the LLM to consider the exact narrative constraints of the current scene
            var payload = new ActionResolutionRequestPayload
            {
                playerId = "Player_01",
                actionType = context.ActionType.ToString(),
                targetId = context.TargetId,
                currentDimension = context.CurrentDimension,
                isTargetVoidCorrupted = context.IsTargetVoidCorrupted,
                activeSpiritualShields = context.HasMagenActive ? "Magen" : "None",
                proximityToOnalymNexus = context.DistanceToNexus,
                playerCurrentState = GetPlayerVitalsAndStance(playerData),
                visualFrame = visualContextBase64
            };

            string json = JsonUtility.ToJson(payload);
            using (UnityWebRequest req = new UnityWebRequest(MUDP_RESOLVE_URL, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");

                // Using custom extension for Task-based await with cancellation
                await req.SendWebRequest().WithCancellation(token);

                if (req.result == UnityWebRequest.Result.Success)
                {
                    ActionResolutionResponse resolution = JsonUtility.FromJson<ActionResolutionResponse>(req.downloadHandler.text);
                    EnactResolutionInGame(resolution);
                }
                else
                {
                    Debug.LogError($"[NarrativeActionResolver] Request failed: {req.error}");
                }
            }
        }

        private void EnactResolutionInGame(ActionResolutionResponse resolution)
        {
            if (resolution == null) return;

            // PALETTE: Rich text for speaker identification and dialogue
            if (!string.IsNullOrEmpty(resolution.DialogueGenerated))
                Debug.Log($"<color=cyan>[{resolution.EntityName}]:</color> {resolution.DialogueGenerated}");

            if (resolution.WasActionSuccessful)
                Debug.Log($"<color=#00FF00>Action Succeeded:</color> {resolution.MechanicalDescription}");
            else
                Debug.Log($"<color=red>Action Failed:</color> {resolution.MechanicalDescription}");

            if (Mathf.Abs(resolution.VoidVarianceDelta) > 0f && EncounterDirector.Instance != null)
                EncounterDirector.Instance.ApplyVoidVariance(resolution.VoidVarianceDelta);
        }

        private async Task<string> CapturePlayerViewAsync()
        {
            // SENTINEL: Ensure camera exists before capture
            if (playerEyeCamera == null) return null;

            // Capture logic using RenderTexture
            int width = 512;
            int height = 512;
            RenderTexture rt = new RenderTexture(width, height, 24);
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

            playerEyeCamera.targetTexture = rt;
            playerEyeCamera.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);

            playerEyeCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            byte[] bytes = screenShot.EncodeToJPG();
            Destroy(screenShot);

            // BOLT: Run Base64 conversion on background thread to avoid stalling main Unity thread
            string base64String = await Task.Run(() => Convert.ToBase64String(bytes));
            return base64String;
        }

        private string GetPlayerVitalsAndStance(RuntimeCharacterData playerData)
        {
            return $"Health: {playerData.HealthPercentage:P0}, TechAlignment: {playerData.TechAlignment}, MagenActive: {playerData.HasMagenActive}";
        }
    }
}
