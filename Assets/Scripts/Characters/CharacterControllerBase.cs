using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public abstract class CharacterControllerBase : MonoBehaviour
    {
        public CharacterData characterData;

        public virtual void Initialize(CharacterData data)
        {
            characterData = data;
            Debug.Log($"{gameObject.name} initialized with role: {data.role}");
        }

        public abstract void ExecuteBehavior();
    }
}
