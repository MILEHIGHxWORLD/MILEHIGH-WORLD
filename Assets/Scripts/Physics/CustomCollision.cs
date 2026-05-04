using UnityEngine;

namespace Milehigh.Physics
{
    public class CustomCollision : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Collision with {collision.gameObject.name}");
        }
    }
}
