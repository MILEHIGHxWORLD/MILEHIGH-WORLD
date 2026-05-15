using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "";
        public int GetInstanceID() => 0;
        public static void DontDestroyOnLoad(Object obj) {}
        public static void Destroy(Object obj) {}
        public static T FindObjectOfType<T>() where T : Object => null;
        public static T[] FindObjectsByType<T>(FindObjectsSortMode sort) where T : Object => new T[0];

        public static bool operator ==(Object a, Object b) => ReferenceEquals(a, b);
        public static bool operator !=(Object a, Object b) => !ReferenceEquals(a, b);
    }

    public class Component : Object
    {
        public GameObject gameObject { get; set; } = null!;
        public Transform transform { get; set; } = null!;
        public T GetComponent<T>() where T : class => null!;
    }

    public class MonoBehaviour : Component
    {
        public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine routine) {}
        public void StopCoroutine(IEnumerator routine) {}
    }

    public class GameObject : Object
    {
        public GameObject() {}
        public GameObject(string name) { this.name = name; }
        public Transform transform { get; set; } = new Transform();
        public bool activeInHierarchy { get; set; }
        public void SetActive(bool value) {}
        public T GetComponent<T>() where T : class => null!;
        public T AddComponent<T>() where T : Component => null!;
    }

    public class Transform : Component
    {
        public Vector3 localScale { get; set; } = new Vector3();
        public Vector3 position { get; set; } = new Vector3();
        public Transform Find(string name) => null!;
        public IEnumerator GetEnumerator() { yield break; }
    }

    public class Coroutine {}

    public class AudioSource : Component
    {
        public void Play() {}
        public void PlayOneShot(AudioClip clip) {}
    }

    public class AudioClip : Object {}

    public class Animator : Component
    {
        public void SetTrigger(string name) {}
    }

    public class CanvasGroup : Component
    {
        public float alpha { get; set; }
    }

    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one = new Vector3(1, 1, 1);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x * b, a.y * b, a.z * b);
    }

    public struct Color
    {
        public float r, g, b, a;
        public Color(float r, float g, float b) { this.r = r; this.g = g; this.b = b; this.a = 1; }
        public Color(float r, float g, float b, float a) { this.r = r; this.g = g; this.b = b; this.a = a; }
        public static Color white = new Color(1, 1, 1);
        public static Color cyan = new Color(0, 1, 1);
        public static Color black = new Color(0, 0, 0);
    }

    public static class Mathf
    {
        public const float PI = 3.14159f;
        public static int RoundToInt(float f) => (int)System.Math.Round(f);
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        public static float Sin(float f) => (float)System.Math.Sin(f);
        public static float PingPong(float t, float length) => 0;
        public static float Clamp01(float f) => f < 0 ? 0 : (f > 1 ? 1 : f);
    }

    public class WaitForSeconds
    {
        public WaitForSeconds(float time) {}
    }

    public static class Input
    {
        public static bool anyKeyDown => false;
    }

    public static class Time
    {
        public static float deltaTime => 0.02f;
        public static float time => 0f;
    }

    public static class Debug
    {
        public static void Log(string msg) {}
        public static void LogWarning(string msg) {}
        public static void LogError(string msg) {}
    }

    public static class ColorUtility
    {
        public static string ToHtmlStringRGB(Color c) => "FFFFFF";
    }

    public static class Application
    {
        public static string dataPath = "";
        public static string streamingAssetsPath = "";
    }

    public static class JsonUtility
    {
        public static T FromJson<T>(string json) => default!;
    }

    public static class PlayerPrefs
    {
        public static void SetString(string key, string val) {}
        public static string GetString(string key, string def) => "";
        public static void Save() {}
    }

    public static class SystemInfo
    {
        public static string? deviceUniqueIdentifier = "test";
    }

    public enum FindObjectsSortMode { None }

    public class DefaultExecutionOrderAttribute : System.Attribute
    {
        public DefaultExecutionOrderAttribute(int i) {}
    }

    public class TooltipAttribute : System.Attribute { public TooltipAttribute(string s) {} }
    public class TextAreaAttribute : System.Attribute { public TextAreaAttribute(int min, int max) {} }
    public class RangeAttribute : System.Attribute { public RangeAttribute(float min, float max) {} }
    public class CreateAssetMenuAttribute : System.Attribute { public string? fileName { get; set; } public string? menuName { get; set; } }
    public class ScriptableObject : Object {}
    public class HeaderAttribute : System.Attribute { public HeaderAttribute(string s) {} }
    public class SerializeFieldAttribute : System.Attribute {}
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.Component
    {
        public string text { get; set; } = "";
        public Material fontMaterial { get; set; } = new Material();
        public UnityEngine.Color color { get; set; }
        public int maxVisibleCharacters { get; set; }
        public TMP_TextInfo textInfo { get; set; } = new TMP_TextInfo();
        public void ForceMeshUpdate() {}
    }

    public class Material
    {
        public void SetFloat(string id, float f) {}
        public void SetColor(string id, UnityEngine.Color c) {}
    }

    public static class ShaderUtilities
    {
        public const string ID_OutlineWidth = "_OutlineWidth";
        public const string ID_OutlineColor = "_OutlineColor";
    }

    public class TMP_TextInfo
    {
        public int characterCount;
        public TMP_CharacterInfo[] characterInfo = new TMP_CharacterInfo[0];
    }

    public struct TMP_CharacterInfo
    {
        public char character;
    }
}
