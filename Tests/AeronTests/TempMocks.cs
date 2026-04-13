using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "MockObject";
        public static void Destroy(Object obj) {}
        public static void DontDestroyOnLoad(Object obj) {}
        public static T FindObjectOfType<T>() where T : Object => null;
        public static T Instantiate<T>(T original) where T : Object => original;
        public static T Instantiate<T>(T original, Transform parent) where T : Object => original;
        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : Object => original;
    }

    public class MonoBehaviour : Object
    {
        public GameObject gameObject { get; } = new GameObject();
        public Transform transform => gameObject.transform;
    }

    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject => Activator.CreateInstance<T>();
    }

    public class GameObject : Object
    {
        public Transform transform { get; } = new Transform();
        public T GetComponent<T>() => default;
        public T AddComponent<T>() where T : Component => default;
        public string name { get; set; }
        public GameObject() {}
        public GameObject(string name) { this.name = name; }
        public static GameObject Find(string name) => null;
    }

    public class Component : Object
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
    }

    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one => new Vector3(1, 1, 1);
        public static Vector3 zero => new Vector3(0, 0, 0);
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x * b, a.y * b, a.z * b);
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 insideUnitSphere => new Vector3(0, 0, 0);
    }

    public struct Quaternion
    {
        public static Quaternion identity => new Quaternion();
    }

    public static class Mathf
    {
        public static float Clamp01(float value) => value < 0 ? 0 : (value > 1 ? 1 : value);
    }

    public static class Random
    {
        public static Vector3 insideUnitSphere => new Vector3(0, 0, 0);
    }

    public static class Application
    {
        public static string dataPath = "/mock/data";
        public static string streamingAssetsPath = "/mock/streaming";
    }

    public static class JsonUtility
    {
        public static T FromJson<T>(string json) => default;
    }

    public class CreateAssetMenuAttribute : Attribute
    {
        public string fileName { get; set; }
        public string menuName { get; set; }
    }

    public class TextAreaAttribute : Attribute
    {
        public int minLines { get; set; }
        public int maxLines { get; set; }
        public TextAreaAttribute(int min, int max)
        {
            minLines = min;
            maxLines = max;
        }
    }

    public static class Debug
    {
        public static List<string> Logs = new List<string>();
        public static void Log(object message)
        {
            Logs.Add(message?.ToString() ?? "null");
            Console.WriteLine(message);
        }
        public static void LogError(object message)
        {
            Logs.Add("ERROR: " + (message?.ToString() ?? "null"));
            Console.WriteLine("ERROR: " + message);
        }
    }
}

namespace UnityEditor
{
    public class EditorWindow : UnityEngine.Object
    {
    }

    public class MenuItem : Attribute
    {
        public MenuItem(string path) {}
    }

    public static class AssetDatabase
    {
        public static bool IsValidFolder(string path) => true;
        public static void CreateFolder(string parent, string name) {}
        public static void CreateAsset(UnityEngine.Object asset, string path) {}
        public static void SaveAssets() {}
        public static void Refresh() {}
    }
}
