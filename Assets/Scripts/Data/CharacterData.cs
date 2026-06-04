// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;

namespace MilehighWorld.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Milehigh/Character Data")]
    public class CharacterProfileData : ScriptableObject
    {
        public string characterName = "";
        public string role = "";
        [TextArea(3, 10)]
        public string[] traits = System.Array.Empty<string>();
        [TextArea(10, 20)]
        public string behaviorScript = "";
    }
}
