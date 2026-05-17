using UnityEngine;

namespace Milehigh.Core
{
    public class CameraManager : MonoBehaviour
    {
        public Camera mainCamera;

        public void SwitchCamera(Camera newCamera)
        {
            if (mainCamera != null) mainCamera.enabled = false;
            mainCamera = newCamera;
            if (mainCamera != null) mainCamera.enabled = true;
        }
    }
}
