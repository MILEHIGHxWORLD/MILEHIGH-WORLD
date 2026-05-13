using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "";
        public static T FindObjectOfType<T>() where T : Object { return null!; }
        public static T[] FindObjectsByType<T>(FindObjectsSortMode sortMode) where T : Object { return new T[0]; }
        public static void DontDestroyOnLoad(Object obj) {}
        public static void Destroy(Object obj) {}
    }

    public enum FindObjectsSortMode { None, InstanceID }

    public class MonoBehaviour : Object
    {
        public GameObject gameObject { get; } = new GameObject("Mock");
        public Transform transform { get; } = new Transform();
        public bool enabled { get; set; }
    }

    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject, new() { return new T(); }
    }

    public class GameObject : Object
    {
        public GameObject(string name) { this.name = name; }
        public T AddComponent<T>() where T : MonoBehaviour, new() { return new T(); }
        public Transform transform { get; } = new Transform();
        public bool activeInHierarchy { get; set; }
        public static GameObject Find(string name) { return null; }
        public T GetComponent<T>() where T : class { return null; }
    }

    public class Transform : Object
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
        public GameObject gameObject { get; } = new GameObject("Mock");
        public int childCount { get; }
        public Transform GetChild(int i) { return null; }
    }

    public class Debug
    {
        public static void Log(string msg) { Console.WriteLine(msg); }
        public static void LogError(string msg) { Console.WriteLine("ERROR: " + msg); }
        public static void LogWarning(string msg) { Console.WriteLine("WARNING: " + msg); }
    }

    public class Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one = new Vector3(1,1,1);
    }

    public class JsonUtility
    {
        public static T FromJson<T>(string json) { return default!; }
    }

    public class Application
    {
        public static string dataPath = "/mock/data";
        public static string streamingAssetsPath = "/mock/streaming";
    }

    public class PlayerPrefs
    {
        public static void SetString(string key, string val) {}
        public static string GetString(string key, string def) { return def; }
        public static void Save() {}
    }

    public class SystemInfo
    {
        public static string deviceUniqueIdentifier = "mock-id";
    }

    public class Mathf
    {
        public static float Clamp01(float val) { return Math.Max(0, Math.Min(1, val)); }
        public static int RoundToInt(float val) { return (int)Math.Round(val); }
    }

    public class TextAreaAttribute : Attribute { public TextAreaAttribute(int min, int max) {} }
    public class TooltipAttribute : Attribute { public TooltipAttribute(string msg) {} }
    public class RangeAttribute : Attribute { public RangeAttribute(float min, float max) {} }
    public class DefaultExecutionOrderAttribute : Attribute { public DefaultExecutionOrderAttribute(int order) {} }

    public class Color
    {
        public static Color black = new Color();
        public static Color white = new Color();
    }

    public class Input
    {
        public static bool GetKeyDown(KeyCode key) { return false; }
        public static bool GetMouseButtonDown(int button) { return false; }
        public static bool anyKeyDown { get; }
    }

    public enum KeyCode { Space, Return }

    public class Coroutine {}

    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) {}
    }
}

namespace UnityEditor
{
    public class EditorWindow : UnityEngine.MonoBehaviour {}
    public class MenuItem : Attribute { public MenuItem(string path) {} }
    public class AssetDatabase
    {
        public static bool IsValidFolder(string path) { return true; }
        public static void CreateFolder(string parent, string name) {}
        public static void CreateAsset(UnityEngine.Object obj, string path) {}
    }
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.MonoBehaviour
    {
        public string text { get; set; } = "";
        public Color color { get; set; } = new UnityEngine.Color();
        public int maxVisibleCharacters { get; set; }
    }

    public class TMP_FontAsset : UnityEngine.Object
    {
        public UnityEngine.Material material { get; set; } = new UnityEngine.Material();
    }

    public static class ShaderUtilities
    {
        public static int ID_OutlineWidth = 1;
        public static int ID_OutlineColor = 2;
    }
}

namespace UnityEngine
{
    public class Material
    {
        public void SetFloat(int id, float val) {}
        public void SetColor(int id, Color color) {}
    }

    public class ColorUtility
    {
        public static bool TryParseHtmlString(string html, out Color color) { color = new Color(); return true; }
        public static string ToHtmlStringRGB(Color color) { return ""; }
    }
}

namespace UnityEngine.SceneManagement
{
    public struct Scene
    {
        public string name { get; set; }
        public bool isLoaded { get; set; }
        public UnityEngine.GameObject[] GetRootGameObjects() { return new UnityEngine.GameObject[0]; }
    }

    public class SceneManager
    {
        public static int sceneCount { get; }
        public static Scene GetSceneAt(int index) { return new Scene(); }
        public static void LoadScene(string name) {}
    }
}

public class CharacterData : UnityEngine.ScriptableObject
{
    public string characterName = "";
    public string role = "";
    public string[] traits = new string[0];
    public string behaviorScript = "";
}
