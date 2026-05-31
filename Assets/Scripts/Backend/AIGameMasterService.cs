// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Backend
{
    /// <summary>
    /// Async AIGameMasterService utilizing ClientWebSocket or optimized HttpClient polling.
    /// Manages a persistent connection to the inference server without blocking Unity's main thread.
    /// </summary>
    public class AIGameMasterService : MonoBehaviour
    {
        public static AIGameMasterService Instance { get; private set; } = null!;

        [Header("Configuration")]
        [SerializeField] private string websocketUrl = "wss://api.milehigh.world/v1/gm/ws";
        [SerializeField] private string pollingUrl = "https://api.milehigh.world/v1/gm/poll";
        [SerializeField] private bool useWebSocket = true;
        [SerializeField] private float pollInterval = 3.0f;

        private ClientWebSocket? _webSocket;
        private HttpClient? _httpClient;
        private CancellationTokenSource? _cts;

        private readonly ConcurrentQueue<AIGMResponse> _inboundResponses = new();

        public bool IsConnected => _webSocket?.State == WebSocketState.Open;

        private void Awake()
        {
            // SENTINEL: Singleton security
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            _httpClient = new HttpClient();
        }

        private async void Start()
        {
            _cts = new CancellationTokenSource();

            if (useWebSocket)
            {
                await ConnectWebSocketAsync();
            }
            else
            {
                _ = StartPollingLoopAsync(_cts.Token);
            }
        }

        private void Update()
        {
            // Palette: Process responses on the main thread for UI/Game logic safety
            while (_inboundResponses.TryDequeue(out var response))
            {
                HandleGMResponse(response);
            }
        }

        private void HandleGMResponse(AIGMResponse response)
        {
            if (response == null || !response.IsValid()) return;

            Debug.Log($"<color=cyan>[AI GM]:</color> {response.dialogue}");
        }

        private async Task ConnectWebSocketAsync()
        {
            if (_webSocket != null && _webSocket.State == WebSocketState.Open) return;

            _webSocket = new ClientWebSocket();
            try
            {
                Debug.Log($"[AIGM] Connecting to {websocketUrl}...");
                await _webSocket.ConnectAsync(new Uri(websocketUrl), _cts!.Token);
                Debug.Log("[AIGM] WebSocket Connected.");

                // Start background receive loop
                _ = ReceiveLoopAsync(_cts.Token);

                // Send initial handshake
                await SendHandshakeAsync();
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIGM] WebSocket Connection Failed: {e.Message}");
            }
        }

        private async Task SendHandshakeAsync()
        {
            var handshake = new AIGMHandshake
            {
                playerId = "PLAYER_09", // Mock ID
                sessionId = Guid.NewGuid().ToString()
            };

            var message = new AIGMMessage
            {
                type = "HANDSHAKE",
                payloadJson = JsonUtility.ToJson(handshake),
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            await SendMessageAsync(message);
        }

        public async Task SendMessageAsync(AIGMMessage message)
        {
            if (_webSocket == null || _webSocket.State != WebSocketState.Open)
            {
                Debug.LogWarning("[AIGM] Cannot send message: WebSocket is not open.");
                return;
            }

            string json = JsonUtility.ToJson(message);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            var segment = new ArraySegment<byte>(bytes);

            try
            {
                await _webSocket.SendAsync(segment, WebSocketMessageType.Text, true, _cts!.Token);
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIGM] Failed to send message: {e.Message}");
            }
        }

        private async Task ReceiveLoopAsync(CancellationToken token)
        {
            byte[] buffer = new byte[8192];
            var segment = new ArraySegment<byte>(buffer);
            var messageBuilder = new StringBuilder();

            while (!token.IsCancellationRequested && _webSocket?.State == WebSocketState.Open)
            {
                try
                {
                    WebSocketReceiveResult result = await _webSocket.ReceiveAsync(segment, token);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", token);
                        break;
                    }

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        messageBuilder.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));

                        if (result.EndOfMessage)
                        {
                            string json = messageBuilder.ToString();
                            messageBuilder.Clear();
                            ProcessInboundJson(json);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogError($"[AIGM] WebSocket Receive Error: {e.Message}");
                    await Task.Delay(1000, token); // Backoff before retry
                }
            }
        }

        private void ProcessInboundJson(string json)
        {
            try
            {
                var message = JsonUtility.FromJson<AIGMMessage>(json);
                if (message == null || !message.IsValid()) return;

                switch (message.type)
                {
                    case "GM_RESPONSE":
                        var response = JsonUtility.FromJson<AIGMResponse>(message.payloadJson);
                        if (response != null && response.IsValid())
                        {
                            _inboundResponses.Enqueue(response);
                        }
                        break;
                    case "ERROR":
                        Debug.LogError($"[AIGM] Server Error: {message.payloadJson}");
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIGM] Failed to parse inbound JSON: {e.Message}");
            }
        }

        private async Task StartPollingLoopAsync(CancellationToken token)
        {
            Debug.Log("[AIGM] Starting HttpClient Polling Loop...");
            while (!token.IsCancellationRequested)
            {
                try
                {
                    // Optimized polling: Only poll if the service is active
                    await PollGMActionsAsync(token);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Debug.LogError($"[AIGM] Polling Error: {e.Message}");
                }

                await Task.Delay(TimeSpan.FromSeconds(pollInterval), token);
            }
        }

        private async Task PollGMActionsAsync(CancellationToken token)
        {
            if (_httpClient == null) return;

            // BOLT: Zero-allocation - reuse message structures if possible, but HttpClient needs new ones per req
            var response = await _httpClient.GetAsync(pollingUrl, token);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                // Expecting an array of AIGMMessage for polling
                ProcessInboundBatch(json);
            }
        }

        private void ProcessInboundBatch(string json)
        {
            try
            {
                if (json.StartsWith("["))
                {
                    // Handle JSON array by wrapping it for JsonUtility
                    string wrappedJson = "{\"messages\":" + json + "}";
                    var batch = JsonUtility.FromJson<AIGMMessageBatch>(wrappedJson);
                    if (batch?.messages != null)
                    {
                        foreach (var msg in batch.messages)
                        {
                            string msgJson = JsonUtility.ToJson(msg);
                            ProcessInboundJson(msgJson);
                        }
                    }
                }
                else
                {
                    ProcessInboundJson(json);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AIGM] Failed to process batch: {e.Message}");
            }
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _webSocket?.Dispose();
            _httpClient?.Dispose();
        }
    }
}
