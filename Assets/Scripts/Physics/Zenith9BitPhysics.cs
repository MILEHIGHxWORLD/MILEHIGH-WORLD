using UnityEngine;

namespace Milehigh.Physics
{
    public class Zenith9BitPhysics : MonoBehaviour
    {
        public void Apply9BitForce(Vector3 force)
        {
            Debug.Log($"Applying 9-bit quantized force: {force}");
        }
    }
}
