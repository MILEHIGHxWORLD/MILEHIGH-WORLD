namespace UnityEngine
{
    public class MonoBehaviour {}
    public class GameObject
    {
        public GameObject(string name) {}
        public T AddComponent<T>() where T : class, new() => new T();
    }
    public class ScriptableObject
    {
        public static T CreateInstance<T>() where T : ScriptableObject, new() => new T();
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public class Debug
    {
        public static void Log(object message) => System.Console.WriteLine(message);
        public static void LogError(object message) => System.Console.WriteLine("ERROR: " + message);
        public static void LogWarning(object message) => System.Console.WriteLine("WARNING: " + message);
    }
}
