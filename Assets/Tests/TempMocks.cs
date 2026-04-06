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
        public static GameObject Find(string name) => null;
        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T();
            component.gameObject = this;
            return component;
        }
        public T GetComponent<T>() where T : Component => null;
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
        public Vector3 localScale { get; set; }
    }

    public class Component : Object
    {
        public GameObject gameObject { get; set; } = null!;
        public Transform transform => gameObject?.transform;
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
        public static bool anyKeyDown = false;
    }

    public class Random
    {
        public static Vector3 insideUnitSphere = new Vector3(0, 0, 0);
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
        public TextInfo textInfo = new TextInfo();
        public void ForceMeshUpdate() {}
    }

    public class TextInfo
    {
        public int characterCount;
        public CharacterInfo[] characterInfo = new CharacterInfo[0];
    }

    public struct CharacterInfo
    {
        public char character;
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
