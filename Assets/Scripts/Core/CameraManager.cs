using UnityEngine;

namespace Milehigh.Core
{
    public class CameraManager : MonoBehaviour
    {
        public Camera mainCamera = null!;

        public void SetCameraPosition(Vector3 position)
        {
            if (mainCamera != null)
            {
                mainCamera.transform.position = position;
            }
        }
    }
}
