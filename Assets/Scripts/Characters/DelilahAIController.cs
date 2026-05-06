using UnityEngine;
using System.Collections.Generic;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class DelilahAIController : CharacterControllerBase
    {
        public GameObject shadowClonePrefab = null!;

        // BOLT: Pool for shadow clones to avoid Instantiate/Destroy overhead
        private Queue<GameObject> _shadowClonePool = new Queue<GameObject>();

        // BOLT: Shared cache for WaitForSeconds to eliminate GC allocations
        private static readonly Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

        private static WaitForSeconds GetWait(float seconds)
        {
            if (!_waitCache.TryGetValue(seconds, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[seconds] = wait;
            }
            return wait;
        }

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
                GameObject? clone = null;

                // BOLT: Try to get a valid object from the pool
                while (_shadowClonePool.Count > 0)
                {
                    clone = _shadowClonePool.Dequeue();
                    if (clone != null)
                    {
                        clone.SetActive(true);
                        break;
                    }
                }

                if (clone == null)
                {
                    clone = Instantiate(shadowClonePrefab);
                }

                clone.transform.position = transform.position + Random.insideUnitSphere * 5f;
                clone.transform.rotation = Quaternion.identity;

                // BOLT: Automatically recycle after 10 seconds
                StartCoroutine(RecycleClone(clone, 10f));
            }
        }

        private System.Collections.IEnumerator RecycleClone(GameObject? clone, float delay)
        {
            yield return GetWait(delay);

            if (clone != null)
            {
                clone.SetActive(false);
                _shadowClonePool.Enqueue(clone);
            }
        }

        private void CastSicklyGreenBlackVoidfire()
        {
            Debug.Log("Delilah: Casting sickly green-black Voidfire!");
        }
    }
}
