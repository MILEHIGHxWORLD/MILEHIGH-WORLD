using System;

namespace UnityEngine
{
    public class MonoBehaviour {}
    public class ScriptableObject {}
    public class GameObject {}
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public static class Debug
    {
        public static void Log(object message) {}
        public static void LogError(object message) {}
        public static void LogWarning(object message) {}
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class TextAreaAttribute : Attribute { public TextAreaAttribute(int min, int max) {} }
    [AttributeUsage(AttributeTargets.Class)]
    public class CreateAssetMenuAttribute : Attribute { public string fileName; public string menuName; }
}
