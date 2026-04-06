using System;
using System.Collections.Generic;

// These mocks are only for running tests in a standalone .NET environment
// and should not be included in the Unity build.
#if !UNITY_5_3_OR_NEWER
namespace UnityEngine
{
    public class Object
    {
        public string name { get; set; } = "";
    }

    public class GameObject : Object
    {
        public Transform transform { get; set; } = new Transform();
        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T();
            component.gameObject = this;
            return component;
        }
    }

    public class Transform : Component {}

    public class Component : Object
    {
        public GameObject gameObject { get; set; } = null!;
    }

    public class MonoBehaviour : Component {}

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

    public class ScriptableObject : Object {}

    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
}

namespace NUnit.Framework
{
    public class TestFixtureAttribute : Attribute {}
    public class TestAttribute : Attribute {}
    public class SetUpAttribute : Attribute {}

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
    }
}
#endif
