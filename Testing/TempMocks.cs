using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public static T FindObjectOfType<T>() where T : class => null;
        public static void Destroy(Object obj) {}
        public static void DontDestroyOnLoad(Object obj) {}
    }

    public class GameObject : Object
    {
        public GameObject(string name) {}
        public T AddComponent<T>() where T : new() => new T();
    }

    public class MonoBehaviour : Object
    {
        public GameObject gameObject => new GameObject("Mock");
        public static T FindObjectOfType<T>() where T : class => Object.FindObjectOfType<T>();
        public static void Destroy(Object obj) => Object.Destroy(obj);
        public static void DontDestroyOnLoad(Object obj) => Object.DontDestroyOnLoad(obj);
    }

    public class Debug
    {
        public static void Log(object message) => Console.WriteLine(message);
        public static void LogError(object message) => Console.Error.WriteLine(message);
    }
    public class Mathf
    {
        public static float Clamp01(float value) => value < 0 ? 0 : (value > 1 ? 1 : value);
    }
    public class Application
    {
        public static string dataPath = "Assets";
        public static string streamingAssetsPath = "StreamingAssets";
    }
    public class JsonUtility
    {
        public static T FromJson<T>(string json) => default(T);
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
}
