using UnityEngine;

namespace Milehigh.Data
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Milehigh/Character Data")]
    public class CharacterData : ScriptableObject
    {
        public string characterName;
        public string role;
        [TextArea(3, 10)]
        public string[] traits;
        [TextArea(10, 20)]
        public string behaviorScript;
    }
}
