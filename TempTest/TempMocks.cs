using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "";
        public static void DontDestroyOnLoad(Object obj) {}
        public static void Destroy(Object obj) {}
        public static T FindObjectOfType<T>() where T : Object => default!;
        public static GameObject Find(string name) => default!;
        public static T Instantiate<T>(T obj, Transform parent) where T : Object => default!;
        public static T Instantiate<T>(T obj) where T : Object => default!;
    }
    public class Component : Object
    {
        public GameObject gameObject => default!;
        public Transform transform => default!;
        public T GetComponent<T>() => default!;
    }
    public class MonoBehaviour : Component
    {
        public Coroutine StartCoroutine(IEnumerator routine) => default!;
        public void StopCoroutine(Coroutine routine) {}
        public void StopCoroutine(IEnumerator routine) {}
    }
    public class GameObject : Object
    {
        public GameObject(string name) {}
        public T AddComponent<T>() where T : Component => default!;
        public Transform transform => default!;
        public T GetComponent<T>() => default!;
        public new string name { get; set; } = "";
        public void SetActive(bool value) {}
    }
    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
        public int childCount => 0;
    }
    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject => (T)Activator.CreateInstance(typeof(T))!;
    }
    public static class JsonUtility
    {
        public static T FromJson<T>(string json) => default!;
    }
    public static class Debug
    {
        public static void Log(object message) {}
        public static void LogError(object message) {}
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one => new Vector3(1, 1, 1);
        public static Vector3 operator *(Vector3 v, float f) => v;
    }
    public struct Color
    {
        public float r, g, b, a;
        public Color(float r, float g, float b) { this.r = r; this.g = g; this.b = b; this.a = 1; }
        public static Color white => new Color(1, 1, 1);
        public static Color cyan => new Color(0, 1, 1);
    }
    public static class Mathf
    {
        public static float Clamp01(float f) => f;
    }
    public class AudioSource : Component {}
    public class Coroutine {}
    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) {}
    }
    public static class Input
    {
        public static bool anyKeyDown => false;
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class CreateAssetMenuAttribute : Attribute
    {
        public string? fileName { get; set; }
        public string? menuName { get; set; }
        public int order { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class TextAreaAttribute : Attribute
    {
        public TextAreaAttribute(int minLines, int maxLines) {}
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute(string header) {}
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class TooltipAttribute : Attribute
    {
        public TooltipAttribute(string tooltip) {}
    }

    public static class Application
    {
        public static string dataPath => "";
        public static string streamingAssetsPath => "";
    }
}

namespace UnityEngine.Serialization
{
    public class FormerlySerializedAsAttribute : Attribute
    {
        public FormerlySerializedAsAttribute(string name) {}
    }
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.Component
    {
        public string text { get; set; } = "";
        public Color color { get; set; }
        public int maxVisibleCharacters { get; set; }
        public void ForceMeshUpdate() {}
        public TMP_TextInfo textInfo => new TMP_TextInfo();
    }
    public class TMP_TextInfo
    {
        public int characterCount => 0;
        public TMP_CharacterInfo[] characterInfo => Array.Empty<TMP_CharacterInfo>();
    }
    public struct TMP_CharacterInfo
    {
        public char character;
    }
}
