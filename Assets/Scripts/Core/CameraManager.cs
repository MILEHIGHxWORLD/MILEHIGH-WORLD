// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using UnityEngine;

namespace MilehighWorld.Core
{
    public class CameraManager : MonoBehaviour
    {
        public Camera mainCamera = null!;

        public void SwitchCamera(Camera newCamera)
        {
            if (mainCamera != null) mainCamera.enabled = false;
            mainCamera = newCamera;
            if (mainCamera != null) mainCamera.enabled = true;
        }
    }
}
