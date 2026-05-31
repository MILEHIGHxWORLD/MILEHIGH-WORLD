// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Collections.Generic;

namespace MilehighWorld.Data
{
    /// <summary>
    /// SENTINEL: Data contracts for the AIGameMasterService.
    /// All inbound data must implement IsValid() for security verification.
    /// </summary>

    [Serializable]
    public class AIGMMessage
    {
        public string type = ""; // "HANDSHAKE", "GAME_STATE_UPDATE", "GM_RESPONSE", "ERROR"
        public string payloadJson = "";
        public long timestamp;

        public bool IsValid() => !string.IsNullOrEmpty(type);
    }

    [Serializable]
    public class AIGMHandshake
    {
        public string playerId = "";
        public string sessionId = "";
        public string clientVersion = "1.0.0";

        public bool IsValid() => !string.IsNullOrEmpty(playerId) && !string.IsNullOrEmpty(sessionId);
    }

    [Serializable]
    public class AIGMGameStateUpdate
    {
        public string currentScene = "";
        public float voidVariance;
        public string playerStance = "";
        public string lastSignificantAction = "";

        public bool IsValid() => !string.IsNullOrEmpty(currentScene);
    }

    [Serializable]
    public class AIGMResponse
    {
        public string dialogue = "";
        public string gmActionIntent = ""; // e.g. "SPAWN_ENEMY", "TRIGGER_GLITCH"
        public string[] narrativeOptions = Array.Empty<string>();
        public float voidVarianceDelta;

        public bool IsValid() => !string.IsNullOrEmpty(dialogue) || !string.IsNullOrEmpty(gmActionIntent);
    }

    [Serializable]
    public class AIGMMessageBatch
    {
        public AIGMMessage[] messages = Array.Empty<AIGMMessage>();
    }
}
