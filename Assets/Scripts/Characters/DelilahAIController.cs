using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Milehigh.Data;
using Milehigh.Core;

namespace Milehigh.Characters
{
    public class DelilahAIController : CharacterControllerBase
    {
        public GameObject? shadowClonePrefab;
        public float cloneDuration = 5f;

        private readonly Queue<GameObject> _clonePool = new Queue<GameObject>();

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
            Vector3 spawnPos = transform.position + UnityEngine.Random.insideUnitSphere * 5f;

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

            StartCoroutine(RecycleClone(clone, cloneDuration));
        }

        private IEnumerator RecycleClone(GameObject? clone, float delay)
        {
            yield return UnityUtils.GetWait(delay);

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
