using System;
using System.Collections;
using System.Collections.Generic;

// These mocks are only for running tests in a standalone .NET environment
// and should not be included in the Unity build.
#if !UNITY_5_3_OR_NEWER
namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "";
        public static void Destroy(Object obj) {}
        public static void DestroyImmediate(Object obj) {}
        public static void DontDestroyOnLoad(Object obj) {}
        public static T Instantiate<T>(T original) where T : Object => original;
        public static T Instantiate<T>(T original, Transform parent) where T : Object => original;
        public static GameObject Instantiate(GameObject original, Transform parent) => original;
        public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation) => original;
        public static T FindObjectOfType<T>() where T : Object => null;
        public static T FindFirstObjectByType<T>() where T : Object => null;
    }

    public class GameObject : Object
    {
        public GameObject() {}
        public GameObject(string name) { this.name = name; }
        public Transform transform { get; set; } = new Transform();
        public static GameObject Find(string name) => null;
        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T();
            component.gameObject = this;
            return component;
        }
        public T GetComponent<T>() where T : Component => null;
        public void SetActive(bool value) {}
        public int GetInstanceID() => 0;
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
        public Quaternion rotation { get; set; }
    }

    public class Component : Object
    {
        public GameObject gameObject { get; set; } = null!;
        public Transform transform => gameObject?.transform;
        public T GetComponent<T>() where T : Component => gameObject?.GetComponent<T>();
    }

    public class MonoBehaviour : Component
    {
        public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine routine) {}
        public void StopCoroutine(IEnumerator routine) {}
    }

    public class Debug
    {
        public static List<string> Logs = new List<string>();
        public static void Log(object message)
        {
            Logs.Add(message?.ToString() ?? "");
            Console.WriteLine(message);
        }
        public static void LogError(object message)
        {
            Logs.Add("ERROR: " + (message?.ToString() ?? ""));
            Console.WriteLine("ERROR: " + message);
        }
        public static void LogWarning(object message)
        {
            Logs.Add("WARNING: " + (message?.ToString() ?? ""));
            Console.WriteLine("WARNING: " + message);
        }
    }

    public class CreateAssetMenuAttribute : Attribute
    {
        public string fileName { get; set; } = "";
        public string menuName { get; set; } = "";
    }

    public class TextAreaAttribute : Attribute
    {
        public TextAreaAttribute(int minLines, int maxLines) {}
    }

    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute(string header) {}
    }

    public class TooltipAttribute : Attribute
    {
        public TooltipAttribute(string tooltip) {}
    }

    public class SerializeField : Attribute {}

    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject, new() => new T();
    }

    public struct Vector3
    {
        public float x, y, z;
        public static Vector3 one = new Vector3(1, 1, 1);
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x * b, a.y * b, a.z * b);
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public struct Quaternion
    {
        public static Quaternion identity = new Quaternion();
    }

    public struct Color
    {
        public float r, g, b, a;
        public Color(float r, float g, float b) { this.r = r; this.g = g; this.b = b; this.a = 1; }
        public static Color white = new Color(1, 1, 1);
    }

    public class ColorUtility
    {
        public static string ToHtmlStringRGB(Color color) => "FFFFFF";
    }

    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) {}
    }

    public class Coroutine {}

    public class Application
    {
        public static string dataPath = "";
        public static string streamingAssetsPath = "";
    }

    public class JsonUtility
    {
        public static T FromJson<T>(string json) => default(T);
    }

    public static class Mathf
    {
        public static float Clamp01(float value) => Math.Max(0, Math.Min(1, value));
    }

    public class Input
    {
        public static bool GetKeyDown(KeyCode key) => false;
        public static bool GetMouseButtonDown(int button) => false;
    }

    public enum KeyCode
    {
        Space,
        Return
    }

    public static class Random
    {
        public static Vector3 insideUnitSphere = new Vector3(0, 0, 0);
    }

    public static class Time
    {
        public static float deltaTime = 0.016f;
    }
}

namespace UnityEngine.Serialization
{
    public class FormerlySerializedAsAttribute : Attribute
    {
        public FormerlySerializedAsAttribute(string oldName) {}
    }
}

namespace UnityEditor
{
    public class EditorWindow : UnityEngine.MonoBehaviour {}
    public class AssetDatabase
    {
        public static bool IsValidFolder(string path) => true;
        public static void CreateFolder(string parent, string name) {}
        public static void CreateAsset(UnityEngine.Object asset, string path) {}
        public static void SaveAssets() {}
        public static void Refresh() {}
    }
    public class MenuItemAttribute : Attribute
    {
        public MenuItemAttribute(string path) {}
    }
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.MonoBehaviour
    {
        public string text { get; set; } = "";
        public int maxVisibleCharacters { get; set; }
        public UnityEngine.Color color { get; set; }
        public TMP_TextInfo textInfo = new TMP_TextInfo();
        public void ForceMeshUpdate() {}
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

namespace NUnit.Framework
{
    public class TestFixtureAttribute : Attribute {}
    public class TestAttribute : Attribute {}
    public class SetUpAttribute : Attribute {}
    public class TearDownAttribute : Attribute {}

    public static class Assert
    {
        public static void IsTrue(bool condition, string message = "")
        {
            if (!condition) throw new Exception("Assertion failed: " + message);
        }
        public static void IsFalse(bool condition, string message = "")
        {
            if (condition) throw new Exception("Assertion failed: " + message);
        }
        public static void IsNull(object obj, string message = "")
        {
            if (obj != null) throw new Exception("Assertion failed: " + message);
        }
        public static void IsNotNull(object? obj, string message = "")
        {
            if (obj == null) throw new Exception("Assertion failed: " + message);
        }
        public static void AreEqual(object? expected, object? actual, string message = "")
        {
            if (!Equals(expected, actual)) throw new Exception($"Assertion failed: {message}. Expected: {expected}, Actual: {actual}");
        }
        public static void DoesNotThrow(Action action)
        {
            try { action(); }
            catch (Exception ex) { throw new Exception("Assertion failed: Action threw exception: " + ex.Message); }
        }
    }
}
#endif
