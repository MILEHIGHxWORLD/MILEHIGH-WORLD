// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;

namespace MilehighWorld.SceneManagement
{
    public class AsyncSceneLoader : MonoBehaviour
    {
        // AsyncSceneLoader code...
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
