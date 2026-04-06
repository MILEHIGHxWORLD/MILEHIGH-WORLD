using System;

namespace UnityEngine
{
    public class Debug
    {
        public static void Log(object message) => Console.WriteLine(message);
        public static void LogError(object message) => Console.WriteLine($"ERROR: {message}");
        public static void LogWarning(object message) => Console.WriteLine($"WARNING: {message}");
    }

    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class SerializableAttribute : Attribute { }

    public class CreateAssetMenuAttribute : Attribute
    {
        public string fileName;
        public string menuName;
    }

    public class TextAreaAttribute : Attribute
    {
        public int minLines;
        public int maxLines;
        public TextAreaAttribute(int min, int max)
        {
            minLines = min;
            maxLines = max;
        }
    }

    public class MonoBehaviour { }
}
