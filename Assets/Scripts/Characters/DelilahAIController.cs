using UnityEngine;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class DelilahAIController : CharacterControllerBase
    {
        public GameObject shadowClonePrefab;

        public override void ExecuteBehavior()
        {
            BossPhase();
        }

        public void BossPhase()
        {
            SpawnShadowClones();
            CastSicklyGreenBlackVoidfire();
        }

        private void SpawnShadowClones()
        {
            Debug.Log("Delilah: Spawning shadow clones...");
            if (shadowClonePrefab != null)
            {
                UnityEngine.Object.Instantiate(shadowClonePrefab, transform.position + UnityEngine.Random.insideUnitSphere * 5f, UnityEngine.Quaternion.identity);
            }
        }

        private void CastSicklyGreenBlackVoidfire()
        {
            Debug.Log("Delilah: Casting sickly green-black Voidfire!");
        }
    }
}
