using UnityEngine;

namespace Milehigh.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Milehigh/Character Data")]
    public class CharacterData : ScriptableObject
    {
        public string characterName = string.Empty;
        public string role = string.Empty;
        [TextArea(3, 10)]
        public string[] traits = System.Array.Empty<string>();
        [TextArea(10, 20)]
        public string behaviorScript = string.Empty;

        public float health = 100f;
        public float resonance = 1.0f;
        public float integrity = 1.0f;
        public float vanguardMultiplier = 1.0f;
    }
}
