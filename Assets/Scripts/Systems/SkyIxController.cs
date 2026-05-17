using UnityEngine;

namespace Milehigh.World.Systems
{
    // Error 1 Fix: Rename from Sky.ixController
    public class SkyIxController : MonoBehaviour
    {
        [SerializeField] private GameObject beamFxPrefab;

        public void FireLogicParity()
        {
            // Error 11 Fix: Proper instantiation
            if (beamFxPrefab != null)
            {
                Instantiate(beamFxPrefab, transform.position, transform.rotation);
            }
        }

        public void RedirectVoidData(byte[] data)
        {
            // Error 6 Fix: Explicit logic for LittleEndian systems
            if (System.BitConverter.IsLittleEndian)
            {
                System.Array.Reverse(data);
            }
            // Process reversed data for Now-Reality stabilization
        }
    }
}
