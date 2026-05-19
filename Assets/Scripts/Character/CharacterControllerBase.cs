// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using MilehighWorld.Data;

namespace MilehighWorld.Characters
{
    public abstract class CharacterControllerBase : MonoBehaviour
    {
        public CharacterData characterData = null!;

        public virtual void Initialize(CharacterData data)
        {
            characterData = data;
            UnityEngine.Debug.Log($"{gameObject.name} initialized with role: {data.role}");
            Debug.Log($"Initialized with role: {data.role}");
        }

        public abstract void ExecuteBehavior();
    }
}
