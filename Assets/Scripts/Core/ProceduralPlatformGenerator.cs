using UnityEngine;
using System.Collections.Generic;

namespace Milehigh.Core
{
    public class ProceduralPlatformGenerator : MonoBehaviour
    {
        public GameObject platformPrefab;
        public int platformCount = 10;
        public Vector3 spawnRange = new Vector3(20, 10, 20);

        private List<GameObject> generatedPlatforms = new List<GameObject>();

        public void GeneratePlatforms(int seed)
        {
            Random.InitState(seed);
            ClearPlatforms();

            for (int i = 0; i < platformCount; i++)
            {
                Vector3 randomPos = new Vector3(
                    Random.Range(-spawnRange.x, spawnRange.x),
                    Random.Range(0, spawnRange.y),
                    Random.Range(-spawnRange.z, spawnRange.z)
                );

                if (platformPrefab != null)
                {
                    GameObject platform = Instantiate(platformPrefab, randomPos, Quaternion.identity, transform);
                    generatedPlatforms.Add(platform);
                }
            }
            Debug.Log($"Generated {generatedPlatforms.Count} platforms with seed {seed}.");
        }

        public void ClearPlatforms()
        {
            foreach (var platform in generatedPlatforms)
            {
                if (platform != null) Destroy(platform);
            }
            generatedPlatforms.Clear();
        }
    }
}
