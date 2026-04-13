using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object { }
    public class MonoBehaviour : Object
    {
        public GameObject gameObject => new GameObject();
        public Transform transform => new Transform();
        public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine routine) { }
        public static void Destroy(Object obj) { }
    }

    public class GameObject : Object
    {
        public Transform transform = new Transform();
        public string name;
        public T GetComponent<T>() where T : Component => default;
        public void SetActive(bool value) { }
    }

    public class Component : Object
    {
        public GameObject gameObject = new GameObject();
        public Transform transform = new Transform();
    }

    public class Transform : Component
    {
        public Vector3 localScale = Vector3.one;
        public Vector3 position = Vector3.zero;
    }

    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one = new Vector3(1, 1, 1);
        public static Vector3 zero = new Vector3(0, 0, 0);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x * b, a.y * b, a.z * b);
    }

    public struct Color
    {
        public float r, g, b, a;
        public Color(float r, float g, float b, float a = 1) { this.r = r; this.g = g; this.b = b; this.a = a; }
        public static Color cyan = new Color(0, 1, 1);
        public static Color white = new Color(1, 1, 1);
    }

    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) { }
    }

    public class Coroutine { }

    public static class Time
    {
        public static float unscaledDeltaTime = 0.02f;
    }

    public static class Input
    {
        public static bool anyKeyDown = false;
    }

    public static class Mathf
    {
        public const float PI = 3.14159265359f;
        public static float Sin(float f) => (float)Math.Sin(f);
    }

    public static class Debug
    {
        public static void Log(object message) { Console.WriteLine(message); }
        public static void LogError(object message) { Console.WriteLine("ERROR: " + message); }
    }

    public class PropertyAttribute : Attribute { }

    public class AudioSource : Component { }
    public class HeaderAttribute : PropertyAttribute { public HeaderAttribute(string header) { } }
    public class TooltipAttribute : PropertyAttribute { public TooltipAttribute(string tooltip) { } }
    public class TextAreaAttribute : PropertyAttribute { public TextAreaAttribute(int min, int max) { } }
    public class CreateAssetMenuAttribute : Attribute { public string fileName; public string menuName; }
    public class ScriptableObject : Object { public static T CreateInstance<T>() where T : ScriptableObject => default; }
    public static class JsonUtility { public static T FromJson<T>(string json) => default; }
}

namespace UnityEngine.Serialization
{
    public class FormerlySerializedAsAttribute : Attribute
    {
        public FormerlySerializedAsAttribute(string name) { }
    }
}

namespace TMPro
{
    public class TMP_Text : UnityEngine.MonoBehaviour
    {
        public string text;
        public int maxVisibleCharacters;
        public UnityEngine.Color color;
        public TMP_TextInfo textInfo = new TMP_TextInfo();
        public void ForceMeshUpdate() { }
    }

    public class TextMeshProUGUI : TMP_Text { }

    public class TMP_TextInfo
    {
        public int characterCount = 0;
        public TMP_CharacterInfo[] characterInfo = new TMP_CharacterInfo[256];
    }

    public struct TMP_CharacterInfo
    {
        public char character;
    }
}

namespace UnityEditor
{
    public class EditorWindow : UnityEngine.Object { }
    public class MenuItemAttribute : Attribute { public MenuItemAttribute(string path) { } }
    public static class AssetDatabase
    {
        public static bool IsValidFolder(string path) => true;
        public static void CreateFolder(string parentFolder, string newFolderName) { }
        public static void CreateAsset(UnityEngine.Object asset, string path) { }
        public static void SaveAssets() { }
        public static void Refresh() { }
    }
}
