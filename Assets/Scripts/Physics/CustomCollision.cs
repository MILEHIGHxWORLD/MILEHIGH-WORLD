using UnityEngine;

namespace Milehigh.Physics
{
    public class CustomCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Custom collision detected!");
        }
    }
}
