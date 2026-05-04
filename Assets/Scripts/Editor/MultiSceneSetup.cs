using UnityEditor;
using UnityEditor.SceneManagement;

namespace Milehigh.Editor
{
    public class MultiSceneSetup : EditorWindow
    {
        [MenuItem("Milehigh/Setup Multi-Scene")]
        public static void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Additive);
        }
    }
}
