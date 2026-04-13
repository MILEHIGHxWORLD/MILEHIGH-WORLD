using System;
using System.Collections.Generic;

namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "MockObject";
    }

    public class MonoBehaviour : Object
    {
        public GameObject gameObject { get; } = new GameObject();
    }

    public class ScriptableObject : Object
    {
    }

    public class GameObject : Object
    {
        public Transform transform { get; } = new Transform();
    }

    public class Transform : Object
    {
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
    }
}
