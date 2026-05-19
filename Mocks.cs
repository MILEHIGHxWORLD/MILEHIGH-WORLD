using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class Coroutine {}
    public class MonoBehaviour
    {
        public GameObject gameObject = new GameObject("Mock");
        public Transform transform = new Transform();
        public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine routine) {}
    }
    public class Transform
    {
        public Vector3 localPosition = new Vector3();
    }
    public class GameObject
    {
        public Transform transform = new Transform();
        public GameObject(string name) {}
        public T AddComponent<T>() where T : MonoBehaviour, new() => new T();
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public static class Object
    {
        public static void DontDestroyOnLoad(GameObject go) {}
        public static void Destroy(GameObject go) {}
        public static void Destroy(MonoBehaviour go) {}
        public static T FindAnyObjectByType<T>() where T : class => null;
    }
    public static class Debug
    {
        public static void Log(string msg) => Console.WriteLine(msg);
        public static void LogError(string msg) => Console.Error.WriteLine(msg);
    }
    public static class Application
    {
        public static string dataPath = "/mock/path";
        public static string streamingAssetsPath = "/mock/streaming";
        public static string companyName = "Milehigh";
        public static string productName = "World";
    }
    public static class JsonUtility
    {
        public static T FromJson<T>(string json) => default;
    }
    public static class Mathf
    {
        public static float Clamp01(float v) => Math.Max(0, Math.Min(1, v));
        public static int Clamp(int v, int min, int max) => Math.Max(min, Math.Min(max, v));
    }
    public static class PlayerPrefs
    {
        public static void SetString(string key, string val) {}
        public static string GetString(string key, string def) => def;
        public static void Save() {}
    }
    public static class SystemInfo
    {
        public static string deviceUniqueIdentifier = "n/a";
    }
    public class DefaultExecutionOrderAttribute : Attribute
    {
        public DefaultExecutionOrderAttribute(int order) {}
    }
    public class SerializeFieldAttribute : Attribute {}
    public static class Time
    {
        public static float deltaTime = 0.02f;
    }
    public enum KeyCode
    {
        LeftControl, RightControl, L, UpArrow, DownArrow, Tab
    }
    public static class Input
    {
        public static bool GetKey(KeyCode key) => false;
        public static bool GetKeyDown(KeyCode key) => false;
    }
    public static class Random
    {
        public static float Range(float min, float max) => min;
    }
    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) {}
    }
}

namespace TMPro
{
    public class TMP_InputField : UnityEngine.MonoBehaviour
    {
        public string text = "";
        public int caretPosition = 0;
        public bool isFocused = false;
        public void ActivateInputField() {}
    }
    public class TextMeshProUGUI : UnityEngine.MonoBehaviour
    {
        public string text = "";
        public int maxVisibleCharacters = 0;
        public TMP_TextInfo textInfo = new TMP_TextInfo();
        public void ForceMeshUpdate() {}
    }
    public class TMP_TextInfo
    {
        public int characterCount = 0;
        public TMP_CharacterInfo[] characterInfo = new TMP_CharacterInfo[256];
    }
    public struct TMP_CharacterInfo
    {
        public char character;
    }
}

namespace MilehighWorld.Data
{
    public class HorizonGameData
    {
        public Metadata metadata = new Metadata();
        public class Metadata { public float voidSaturationLevel; }
        public bool IsValid() => true;
    }
}

namespace MilehighWorld.Systems.Agency
{
    public class NarrativeActionContext
    {
        public enum ActionType { HACK_TERMINAL }
        public ActionType ActionTypeProperty { get; set; }
        public string TargetId = "";
        public bool RequiresVisualValidation;
        public string CurrentDimension = "";
    }

    public class NarrativeActionResolver
    {
        public static NarrativeActionResolver Instance = new NarrativeActionResolver();
        public Task ExecuteLoreBoundChoiceAsync(NarrativeActionContext context, CancellationToken token) => Task.CompletedTask;
    }
}
