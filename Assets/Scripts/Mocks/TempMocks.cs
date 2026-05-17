using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class Object
    {
        public static T FindObjectOfType<T>() where T : class => null;
        public static T[] FindObjectsOfType<T>() where T : class => new T[0];
        public static void Destroy(Object obj) {}
        public static void DontDestroyOnLoad(Object obj) {}
    }
    public class MonoBehaviour : Object
    {
        public GameObject gameObject { get; } = new GameObject();
        public Coroutine StartCoroutine(System.Collections.IEnumerator routine) => new Coroutine();
        public void StopCoroutine(Coroutine routine) {}
    }
    public class GameObject : Object
    {
        public string name { get; set; } = "";
        public Transform transform { get; } = new Transform();
        public T GetComponent<T>() where T : class => null;
        public bool TryGetComponent<T>(out T component) where T : class { component = null; return false; }
        public T AddComponent<T>() where T : class => null;
        public void SetActive(bool value) {}
        public static GameObject Find(string name) => null;
        public static T Instantiate<T>(T original, Transform parent) where T : class => null;
        public static T Instantiate<T>(T original) where T : class => null;
        public static T Instantiate<T>(T original, Vector3 position, Quaternion rotation) where T : class => null;
        public GameObject() {}
        public GameObject(string name) {}
        public int GetInstanceID() => 0;
    }
    public class Transform : Object
    {
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }
        public Vector3 localPosition { get; set; }
        public Vector3 localScale { get; set; }
        public Vector3 one => new Vector3(1, 1, 1);
        public Transform parent { get; set; }
        public Transform Find(string name) => null;
    }
    public struct Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 one => new Vector3(1, 1, 1);
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t) => a;
        public static Vector3 operator *(Vector3 a, float b) => new Vector3(a.x * b, a.y * b, a.z * b);
        public static Vector3 operator +(Vector3 a, Vector3 b) => new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
    public struct Quaternion
    {
        public float x, y, z, w;
    }
    public class Debug
    {
        public static void Log(object message) {}
        public static void LogWarning(object message) {}
        public static void LogError(object message) {}
    }
    public class JsonUtility
    {
        public static T FromJson<T>(string json) => default;
    }
    public class Application
    {
        public static string dataPath = "";
        public static string streamingAssetsPath = "";
    }
    public class ScriptableObject : Object
    {
        public static T CreateInstance<T>() where T : ScriptableObject, new() => new T();
    }
    public class CreateAssetMenuAttribute : Attribute
    {
        public string fileName;
        public string menuName;
    }
    public class TextAreaAttribute : Attribute
    {
        public int minLines;
        public int maxLines;
        public TextAreaAttribute(int min, int max) {}
    }
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute(string header) {}
    }
    public class SerializeFieldAttribute : Attribute {}
    public class TooltipAttribute : Attribute
    {
        public TooltipAttribute(string tooltip) {}
    }
    public static class Mathf
    {
        public static float Clamp01(float value) => value;
        public static int RoundToInt(float value) => (int)value;
        public const float PI = 3.14159265f;
        public static float Sin(float f) => 0;
    }
    public class Color
    {
        public static Color white => new Color();
        public static Color black => new Color();
        public static Color cyan => new Color();
        public Color() {}
        public Color(float r, float g, float b) {}
        public Color(float r, float g, float b, float a) {}
    }
    public static class ColorUtility
    {
        public static string ToHtmlStringRGB(Color color) => "";
    }
    public class WaitForSeconds
    {
        public WaitForSeconds(float seconds) {}
    }
    public class Coroutine {}
    public class CanvasGroup
    {
        public float alpha { get; set; }
    }
    public class RectTransform : Transform {}
    public class Rigidbody : MonoBehaviour
    {
        public float mass { get; set; }
    }
    public class Time
    {
        public static float deltaTime;
        public static float unscaledDeltaTime;
        public static float time;
        public static float unscaledTime;
        public static float timeScale { get; set; }
    }
    public class Renderer : MonoBehaviour
    {
        public Material material { get; set; }
        public void GetPropertyBlock(MaterialPropertyBlock block) {}
        public void SetPropertyBlock(MaterialPropertyBlock block) {}
    }
    public class Material : Object
    {
        public void SetFloat(string name, float value) {}
        public void SetFloat(int id, float value) {}
        public void SetColor(string name, Color color) {}
        public void SetColor(int id, Color color) {}
    }
    public class MaterialPropertyBlock
    {
        public void SetFloat(int id, float value) {}
        public void SetFloat(string name, float value) {}
    }
    public static class Shader
    {
        public static int PropertyToID(string name) => 0;
    }
    public class Input
    {
        public static bool anyKeyDown;
        public static bool GetKeyDown(KeyCode code) => false;
        public static bool GetMouseButtonDown(int button) => false;
    }
    public enum KeyCode { Space, Return }
}

namespace UnityEngine.UI
{
    public class Selectable : UnityEngine.MonoBehaviour {}
    public class Graphic : UnityEngine.MonoBehaviour {}
}

namespace TMPro
{
    public class TextMeshProUGUI : UnityEngine.MonoBehaviour
    {
        public string text { get; set; } = "";
        public int maxVisibleCharacters { get; set; }
        public TMP_TextInfo textInfo { get; } = new TMP_TextInfo();
        public void ForceMeshUpdate() {}
        public UnityEngine.Material fontMaterial { get; } = new UnityEngine.Material();
        public UnityEngine.RectTransform rectTransform { get; } = new UnityEngine.RectTransform();
        public UnityEngine.Color color { get; set; }
    }
    public class TMP_InputField : UnityEngine.UI.Selectable
    {
        public string text { get; set; } = "";
        public void ActivateInputField() {}
        public UnityEngine.Transform transform { get; } = new UnityEngine.Transform();
        public UnityEngine.UI.Graphic placeholder { get; set; }
    }
    public class TMP_TextInfo
    {
        public int characterCount { get; set; }
        public TMP_CharacterInfo[] characterInfo { get; set; } = new TMP_CharacterInfo[0];
    }
    public struct TMP_CharacterInfo
    {
        public char character;
    }
    public static class ShaderUtilities
    {
        public static int ID_OutlineWidth;
        public static int ID_OutlineColor;
    }
}

namespace UnityEditor
{
    public class EditorWindow {}
    public class MenuItemAttribute : Attribute
    {
        public MenuItemAttribute(string itemName) {}
    }
    public class AssetDatabase
    {
        public static bool IsValidFolder(string path) => true;
        public static void CreateFolder(string parent, string name) {}
        public static void CreateAsset(object asset, string path) {}
        public static void SaveAssets() {}
        public static void Refresh() {}
    }
}

namespace UnityEngine.Internal
{
    public class FormerlySerializedAsAttribute : Attribute
    {
        public FormerlySerializedAsAttribute(string name) {}
    }
}

namespace MilehighWorld.Core
{
    public class EncounterDirector
    {
        public NovomindadCharacter GetAlly(string name) => new NovomindadCharacter();
        public EnemyCharacter GetEnemy(string name) => new EnemyCharacter();
    }

    public class NovomindadCharacter
    {
        public UnityEngine.GameObject PrefabReference { get; } = new UnityEngine.GameObject();
        public NovomindadCharacter() {}
        public NovomindadCharacter(string name, List<string> abilities) {}
        public void UseAbility(string abilityName) {}
        public void Speak(string message) {}
    }

    public class EnemyCharacter
    {
        public UnityEngine.GameObject PrefabReference { get; } = new UnityEngine.GameObject();
        public void UseAbility(string abilityName) {}
    }
}

namespace MilehighWorld.Simulation
{
    public class LatticeSynchronizer
    {
        public void SynchronizeShard(int node, float modifier) {}
    }
}

public static class EntityRotation
{
    public static Task ApplyPhaseShift(float degrees) => Task.CompletedTask;
}
