using UnityEngine;

namespace Milehigh.Cinematics
{
    public class CinematicController : MonoBehaviour
    {
        public void PlayCinematic(string cinematicId)
        {
            Debug.Log($"Playing cinematic: {cinematicId}");
        }
    }
}
