using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Milehigh.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        public void LoadSceneAsync(string sceneName)
        {
            StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
