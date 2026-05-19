// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;
using UnityEditor;

namespace MilehighWorld.Editor
{
    public class MultiSceneSetup : EditorWindow
    {
        // MultiSceneSetup code...
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Milehigh.Editor
{
    public class MultiSceneSetup
    {
        [MenuItem("Milehigh/Setup Multi-Scene")]
        public static void Setup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
        }
    }
}
