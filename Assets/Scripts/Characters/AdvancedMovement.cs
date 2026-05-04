using UnityEngine;

namespace Milehigh.Characters
{
    public class AdvancedMovement : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float jumpForce = 7f;

        public void Move(Vector3 direction)
        {
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }

        public void Jump()
        {
            Debug.Log($"{gameObject.name} jumped!");
        }
    }
}
