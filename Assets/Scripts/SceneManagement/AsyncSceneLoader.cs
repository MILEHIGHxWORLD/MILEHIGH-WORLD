using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadAsync(sceneName));
        }

        private IEnumerator LoadAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
