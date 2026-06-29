using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class MonoBehaviour : Object {}
    public class Object {
        public string name;
        public GameObject gameObject;
        public Transform transform;
        public static void Destroy(Object obj) {}
        public static void DontDestroyOnLoad(Object obj) {}
        public static T FindAnyObjectByType<T>() where T : Object { return null; }
        public static Object Instantiate(Object original, Vector3 position, Quaternion rotation) { return null; }
    }
    public class GameObject : Object {
        public GameObject(string name) {}
        public T AddComponent<T>() where T : MonoBehaviour { return null; }
    }
    public class Transform : Object {
        public Vector3 position;
    }
    public struct Vector3 {
        public float x, y, z;
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3();
        public static Vector3 operator *(Vector3 a, float b) => new Vector3();
    }
    public struct Quaternion {
        public static Quaternion identity => new Quaternion();
    }
    public struct Rect {
        public Rect(float x, float y, float width, float height) {}
    }
    public class Debug {
        public static void Log(object message) {}
        public static void LogError(object message) {}
        public static void LogWarning(object message) {}
    }
    public class ScriptableObject : Object {}
    public class DefaultExecutionOrderAttribute : Attribute {
        public DefaultExecutionOrderAttribute(int order) {}
    }
    public class CreateAssetMenuAttribute : Attribute {
        public string fileName;
        public string menuName;
    }
    public class TextAreaAttribute : Attribute {
        public TextAreaAttribute(int minLines, int maxLines) {}
    }
    public class SerializeField : Attribute {}
    public class HeaderAttribute : Attribute {
        public HeaderAttribute(string header) {}
    }
    public static class Mathf {
        public static float Clamp01(float value) => 0;
        public static float Abs(float value) => 0;
    }
    public static class PlayerPrefs {
        public static void SetString(string key, string value) {}
        public static string GetString(string key, string defaultValue) => "";
        public static void Save() {}
    }
    public static class SystemInfo {
        public static string deviceUniqueIdentifier => "mock_id";
    }
    public static class Application {
        public static string dataPath => "/mock/path";
        public static string streamingAssetsPath => "/mock/path";
        public static string companyName => "MockCompany";
        public static string productName => "MockProduct";
    }
    public static class JsonUtility {
        public static T FromJson<T>(string json) => default(T);
        public static string ToJson(object obj) => "{}";
    }
    public static class Random {
        public static Vector3 insideUnitSphere => new Vector3();
    }
    public class Camera : MonoBehaviour {
        public RenderTexture targetTexture;
        public void Render() {}
    }
    public class RenderTexture : Object {
        public RenderTexture(int w, int h, int d) {}
        public static RenderTexture active;
    }
    public class Texture2D : Object {
        public Texture2D(int w, int h, TextureFormat f, bool m) {}
        public void ReadPixels(Rect r, int x, int y) {}
        public byte[] EncodeToJPG() => new byte[0];
    }
    public enum TextureFormat { RGB24 }
    public static class Time {
        public static int frameCount => 0;
    }
}

namespace UnityEngine.Networking
{
    public class UnityWebRequest : IDisposable {
        public UnityWebRequest(string url, string method) {}
        public UploadHandler uploadHandler;
        public DownloadHandler downloadHandler;
        public void SetRequestHeader(string k, string v) {}
        public void Abort() {}
        public UnityWebRequestAsyncOperation SendWebRequest() => null;
        public enum Result { Success }
        public Result result;
        public string error;
        public void Dispose() {}
    }
    public class UploadHandler {}
    public class UploadHandlerRaw : UploadHandler {
        public UploadHandlerRaw(byte[] d) {}
    }
    public class DownloadHandler {}
    public class DownloadHandlerBuffer : DownloadHandler {
        public string text;
    }
    public class UnityWebRequestAsyncOperation {
        public bool isDone;
    }
}

namespace MilehighWorld.Core {
    public class EncounterDirector : UnityEngine.MonoBehaviour {
        public static EncounterDirector Instance;
    }
}
