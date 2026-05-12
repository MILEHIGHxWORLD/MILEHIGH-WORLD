using System.Collections;
using System.Collections.Generic;

namespace UnityEngine
{
    public class PropertyAttribute : System.Attribute {}
    public class HeaderAttribute : PropertyAttribute {
        public HeaderAttribute(string header) {}
    }
    public class TooltipAttribute : PropertyAttribute {
        public TooltipAttribute(string tooltip) {}
    }

    public class MonoBehaviour {
        public Coroutine StartCoroutine(IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine routine) {}
        public GameObject gameObject { get; }
        public Transform transform { get; }
        public static T Instantiate<T>(T original, Transform parent) where T : Object => null;
    }
    public class Object {
        public string name { get; set; }
        public int GetInstanceID() => 0;
        public static T Instantiate<T>(T original, Transform parent) where T : Object => null;
        public static T[] FindObjectsByType<T>(FindObjectsSortMode sort) where T : Object => new T[0];
    }
    public class GameObject : Object {
        public bool activeInHierarchy { get; }
        public void SetActive(bool value) {}
        public T GetComponent<T>() where T : class => null;
        public Transform transform { get; }
        public static GameObject Find(string name) => null;
    }
    public class Transform : Object, IEnumerable {
        public Vector3 localScale { get; set; }
        public Vector3 position { get; set; }
        public IEnumerator GetEnumerator() => null;
        public Transform Find(string name) => null;
    }
    public struct Vector3 {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one => new Vector3(1, 1, 1);
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a;
        public static Vector3 operator *(Vector3 a, float b) => a;
        public static Vector3 operator +(Vector3 a, Vector3 b) => a;
        public static Vector3 operator -(Vector3 a, Vector3 b) => a;
    }
    public class AudioSource : Object {
        public Material fontMaterial { get; set; }
    }
    public class Material : Object {
        public void EnableKeyword(string name) {}
        public void SetColor(int id, Color c) {}
        public void SetFloat(int id, float f) {}
    }
    public class Coroutine {}
    public class WaitForSeconds {
        public WaitForSeconds(float time) {}
    }
    public static class Mathf {
        public static int RoundToInt(float f) => (int)f;
        public static float Sin(float f) => 0;
        public const float PI = 3.14159f;
        public static float Lerp(float a, float b, float t) => a;
    }
    public static class Input {
        public static bool anyKeyDown => false;
        public static bool GetMouseButtonDown(int i) => false;
        public static bool GetKeyDown(KeyCode k) => false;
    }
    public enum KeyCode { Space, Return }
    public static class Time {
        public static float deltaTime => 0.02f;
        public static float time => 0;
        public static float unscaledDeltaTime => 0.02f;
    }
    public class CanvasGroup : MonoBehaviour {
        public float alpha { get; set; }
    }
    public struct Color {
        public float r, g, b, a;
        public Color(float r, float g, float b) { this.r = r; this.g = g; this.b = b; this.a = 1.0f; }
        public static Color white => new Color(1, 1, 1);
        public static Color black => new Color(0, 0, 0);
        public static Color cyan => new Color(0, 1, 1);
        public static Color Lerp(Color a, Color b, float t) => a;
    }
    public static class ColorUtility {
        public static string ToHtmlStringRGB(Color c) => "";
    }
    public static class Debug {
        public static void Log(object m) {}
        public static void LogError(object m) {}
        public static void LogWarning(object m) {}
    }
    public enum FindObjectsSortMode { None }
    public class ScriptableObject : Object {
        public static T CreateInstance<T>() where T : ScriptableObject => null;
    }
}

namespace UnityEngine.SceneManagement {
    public class SceneManager {}
}

namespace UnityEngine.Serialization {
    public class FormerlySerializedAsAttribute : System.Attribute {
        public FormerlySerializedAsAttribute(string name) {}
    }
}

namespace TMPro {
    public class TextMeshProUGUI : UnityEngine.MonoBehaviour {
        public string text { get; set; }
        public int maxVisibleCharacters { get; set; }
        public UnityEngine.Color color { get; set; }
        public void ForceMeshUpdate() {}
        public TMP_TextInfo textInfo { get; }
        public UnityEngine.Material fontMaterial { get; set; }
    }
    public class TMP_TextInfo {
        public int characterCount { get; }
        public TMP_CharacterInfo[] characterInfo { get; }
    }
    public struct TMP_CharacterInfo {
        public char character { get; }
    }
    public static class ShaderUtilities {
        public static int ID_OutlineWidth;
        public static int ID_OutlineColor;
        public const string Keyword_Outline = "OUTLINE_ON";
    }
}

namespace Milehigh.Data {
    public class HorizonGameData {
        public List<CharacterProfile> characters;
        public List<SceneScenario> scenarios;
    }
    public class CharacterProfile {
        public string name;
        public string role;
        public string[] traits;
        public string behaviorScript;
    }
    public class CharacterData : UnityEngine.ScriptableObject {
        public string characterName;
        public string role;
        public string[] traits;
        public string behaviorScript;
    }
    public class SceneScenario {
        public List<ObjectInteraction> interactiveObjects;
    }
    public class ObjectInteraction {
        public string objectId;
        public string action;
        public bool isVector;
        public UnityEngine.Vector3 GetVectorValue() => new UnityEngine.Vector3();
    }
}

namespace Milehigh.Characters {
    public class CharacterControllerBase : UnityEngine.MonoBehaviour {
        public void Initialize(Milehigh.Data.CharacterData data) {}
    }
}
