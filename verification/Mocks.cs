namespace UnityEngine {
    public class MonoBehaviour {}
    public class GameObject {
        public string name;
        public Transform transform;
        public T GetComponent<T>() => default(T);
        public int GetInstanceID() => 0;
    }
    public class Transform {
        public Vector3 position;
        public Vector3 localScale;
    }
    public struct Vector3 {
        public float x, y, z;
        public static Vector3 one = new Vector3();
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
    }
    public class Debug {
        public static void Log(object m) {}
        public static void LogWarning(object m) {}
        public static void LogError(object m) {}
    }
    public class Object {
        public static GameObject Instantiate(GameObject p, Transform t) => new GameObject();
    }
    public enum FindObjectsSortMode { None }
}
namespace UnityEditor {
    public class EditorWindow : UnityEngine.Object {}
    public class AssetDatabase {
        public static bool IsValidFolder(string path) => true;
        public static void CreateFolder(string parent, string name) {}
        public static void CreateAsset(UnityEngine.Object obj, string path) {}
    }
}
namespace Milehigh.Data {
    public class ObjectInteraction {
        public string objectId;
        public string action;
        public bool isVector;
        public float floatValue;
        public UnityEngine.Vector3 GetVectorValue() => new UnityEngine.Vector3();
    }
    public class CharacterProfile {
        public string name;
        public bool IsValid() => true;
    }
    public class SceneScenario {
        public string scenarioId;
        public System.Collections.Generic.List<ObjectInteraction> interactiveObjects;
        public bool IsValid() => true;
    }
}
namespace Milehigh.Characters {
    public class CharacterControllerBase {}
}
