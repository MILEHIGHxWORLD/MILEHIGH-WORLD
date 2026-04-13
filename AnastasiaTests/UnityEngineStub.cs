using System;

namespace UnityEngine
{
    public class MonoBehaviour
    {
        public GameObject gameObject { get; } = new GameObject();
        public Transform transform { get; } = new Transform();

        public static void Destroy(UnityEngine.Object obj) { }
        public static void DontDestroyOnLoad(UnityEngine.Object obj) { }
        public static T FindObjectOfType<T>() where T : MonoBehaviour => null;
        public static void Instantiate(GameObject prefab, Vector3 position, Quaternion rotation) { }
    }

    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject => Activator.CreateInstance<T>();
    }

    public class Object
    {
        public string name { get; set; }
    }

    public class GameObject : Object
    {
        public GameObject() { }
        public GameObject(string name) { this.name = name; }
        public T AddComponent<T>() where T : MonoBehaviour => Activator.CreateInstance<T>();
        public Transform transform { get; } = new Transform();
    }

    public class Transform
    {
        public Vector3 position { get; set; }
    }

    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3 operator *(Vector3 a, float d) => new Vector3(a.x * d, a.y * d, a.z * d);
        public static Vector3 insideUnitSphere => new Vector3(0, 0, 0);
    }

    public struct Quaternion
    {
        public static Quaternion identity => new Quaternion();
    }

    public static class Debug
    {
        public static void Log(object message) => Console.WriteLine(message);
        public static void LogError(object message) => Console.WriteLine("Error: " + message);
        public static void LogWarning(object message) => Console.WriteLine("Warning: " + message);
    }

    public class CreateAssetMenuAttribute : Attribute
    {
        public string fileName { get; set; }
        public string menuName { get; set; }
    }

    public class TextAreaAttribute : Attribute
    {
        public TextAreaAttribute(int minLines, int maxLines) { }
    }

    public static class Mathf
    {
        public static float Clamp01(float value) => value < 0 ? 0 : (value > 1 ? 1 : value);
    }

    public static class Application
    {
        public static string dataPath = "Assets";
        public static string streamingAssetsPath = "StreamingAssets";
        public static bool isEditor = true;
    }
}
