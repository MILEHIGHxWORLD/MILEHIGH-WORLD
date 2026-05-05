using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Milehigh.Data;

namespace Milehigh.Characters
{
    public class DelilahAIController : CharacterControllerBase
    {
        public GameObject shadowClonePrefab = null!;
        public float cloneDuration = 5f;

        // BOLT: Object Pool to eliminate Instantiate/Destroy overhead for shadow clones
        private Queue<GameObject> _clonePool = new Queue<GameObject>();

        // BOLT: Cached WaitForSeconds to avoid GC allocations in coroutines
        private static readonly Dictionary<float, WaitForSeconds> _waitCache = new Dictionary<float, WaitForSeconds>();

        private WaitForSeconds GetWait(float seconds)
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
            if (shadowClonePrefab == null) return;

            GameObject? clone = null;
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 5f;

            // BOLT: Reuse object from pool if available
            while (_clonePool.Count > 0 && clone == null)
            {
                clone = _clonePool.Dequeue();
            }

            if (clone != null)
            {
                clone.transform.position = spawnPos;
                clone.transform.rotation = Quaternion.identity;
                clone.SetActive(true);
            }
            else
            {
                clone = Instantiate(shadowClonePrefab, spawnPos, Quaternion.identity);
            }

            // BOLT: Auto-recycle mechanism via coroutine
            StartCoroutine(RecycleClone(clone, cloneDuration));
        }

        private IEnumerator RecycleClone(GameObject clone, float delay)
        {
            yield return GetWait(delay);

            if (clone != null)
            {
                clone.SetActive(false);
                _clonePool.Enqueue(clone);
            }
        }

        private void CastSicklyGreenBlackVoidfire()
        {
            Debug.Log("Delilah: Casting sickly green-black Voidfire!");
        }
    }
}
