// Copyright 2026 MILEHIGH-WORLD LLC. All Rights Reserved.
// PROPRIETARY AND CONFIDENTIAL: DO NOT DISTRIBUTE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;

namespace MilehighWorld.Engine
{
    /// <summary>
    /// Ported implementation of the MILEHIGH-WORLD LLC Infinite Generation Engine.
    /// Utilizes Nine Core Frequencies for harmonic terrain resonance and a WFC structural solver.
    /// BOLT: Optimized using asynchronous Task-based generation to maintain high-performance runtime.
    /// </summary>
    public struct ChunkCoord
    {
        public int x, z;
        public ChunkCoord(int x, int z) { this.x = x; this.z = z; }
        public override bool Equals(object obj) => obj is ChunkCoord other && x == other.x && z == other.z;
        public override int GetHashCode()
        {
            unchecked
            {
                ulong hash = 14695981039346656037UL;
                hash ^= (ulong)x;
                hash *= 1099511628211UL;
                hash ^= (ulong)z;
                hash *= 1099511628211UL;
                return (int)hash;
            }
        }
    }

    public enum TileType
    {
        Empty = 0,
        Path = 1,
        Wall = 2,
        Plaza = 3,
        Count = 4
    }

    public class WFCCell
    {
        public bool Collapsed = false;
        public TileType FinalTile = TileType.Empty;
        public bool[] Possibilities;

        public WFCCell()
        {
            Possibilities = new bool[(int)TileType.Count];
            for (int i = 0; i < Possibilities.Length; i++)
            {
                Possibilities[i] = true;
            }
        }

        public int GetEntropy()
        {
            if (Collapsed)
            {
                return 9999;
            }
            int count = 0;
            foreach (bool p in Possibilities)
            {
                if (p)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public class TerrainChunk
    {
        public ChunkCoord Coord { get; }
        public float[] HeightMap { get; }
        public TileType[] StructureMap { get; }
        public bool IsReady { get; set; }

        public TerrainChunk(ChunkCoord coord, int size)
        {
            Coord = coord;
            HeightMap = new float[size * size];
            StructureMap = new TileType[size * size];
            IsReady = false;
        }
    }

    public class StructuralSolver
    {
        private readonly bool[,] _adjacencyRules;

        public StructuralSolver()
        {
            _adjacencyRules = new bool[(int)TileType.Count, (int)TileType.Count];
            InitializeRules();
        }

        private void InitializeRules()
        {
            _adjacencyRules[(int)TileType.Empty, (int)TileType.Empty] = true;
            _adjacencyRules[(int)TileType.Empty, (int)TileType.Wall] = true;

            _adjacencyRules[(int)TileType.Path, (int)TileType.Path] = true;
            _adjacencyRules[(int)TileType.Path, (int)TileType.Plaza] = true;
            _adjacencyRules[(int)TileType.Path, (int)TileType.Wall] = true;

            _adjacencyRules[(int)TileType.Wall, (int)TileType.Empty] = true;
            _adjacencyRules[(int)TileType.Wall, (int)TileType.Wall] = true;
            _adjacencyRules[(int)TileType.Wall, (int)TileType.Plaza] = true;

            _adjacencyRules[(int)TileType.Plaza, (int)TileType.Plaza] = true;
            _adjacencyRules[(int)TileType.Plaza, (int)TileType.Path] = true;
            _adjacencyRules[(int)TileType.Plaza, (int)TileType.Wall] = true;
        }

        public void CollapseChunkLayout(TerrainChunk chunk)
        {
            int size = HarmonicTerrainEngine.CHUNK_SIZE;
            WFCCell[] wfcGrid = new WFCCell[size * size];
            for (int i = 0; i < wfcGrid.Length; i++)
            {
                wfcGrid[i] = new WFCCell();
                // Prime based on heightmap
                if (chunk.HeightMap[i] > 0.4f)
                {
                    wfcGrid[i].Possibilities[(int)TileType.Empty] = false;
                }
            }

            while (FindLowestEntropy(wfcGrid, out int nextX, out int nextZ))
            {
                int idx = nextZ * size + nextX;
                List<TileType> options = new List<TileType>();
                for (int t = 0; t < (int)TileType.Count; t++)
                {
                    if (wfcGrid[idx].Possibilities[t])
                    {
                        options.Add((TileType)t);
                    }
                }

                TileType selected = options.Count > 0 ? options[0] : TileType.Empty;
                wfcGrid[idx].FinalTile = selected;
                wfcGrid[idx].Collapsed = true;
                for (int t = 0; t < (int)TileType.Count; t++)
                {
                    wfcGrid[idx].Possibilities[t] = ((TileType)t == selected);
                }

                PropagateConstraints(wfcGrid, nextX, nextZ);
            }

            for (int i = 0; i < wfcGrid.Length; i++)
            {
                chunk.StructureMap[i] = wfcGrid[i].FinalTile;
            }
        }

        private bool FindLowestEntropy(WFCCell[] grid, out int outX, out int outZ)
        {
            int size = HarmonicTerrainEngine.CHUNK_SIZE;
            int minEntropy = (int)TileType.Count + 1;
            List<int> candidates = new List<int>();

            for (int i = 0; i < grid.Length; i++)
            {
                if (grid[i].Collapsed)
                {
                    continue;
                }
                int entropy = grid[i].GetEntropy();
                if (entropy < minEntropy)
                {
                    minEntropy = entropy;
                    candidates.Clear();
                    candidates.Add(i);
                }
                else if (entropy == minEntropy)
                {
                    candidates.Add(i);
                }
            }

            if (candidates.Count == 0)
            {
                outX = outZ = -1;
                return false;
            }

            int chosenIdx = candidates[0];
            outX = chosenIdx % size;
            outZ = chosenIdx / size;
            return true;
        }

        private void PropagateConstraints(WFCCell[] grid, int startX, int startZ)
        {
            int size = HarmonicTerrainEngine.CHUNK_SIZE;
            Queue<Vector2Int> propagationQueue = new Queue<Vector2Int>();
            propagationQueue.Enqueue(new Vector2Int(startX, startZ));

            int[] dx = { 0, 0, -1, 1 };
            int[] dz = { -1, 1, 0, 0 };

            while (propagationQueue.Count > 0)
            {
                Vector2Int curr = propagationQueue.Dequeue();
                int currentIdx = curr.y * size + curr.x;

                for (int i = 0; i < 4; i++)
                {
                    int nx = curr.x + dx[i];
                    int nz = curr.y + dz[i];

                    if (nx >= 0 && nx < size && nz >= 0 && nz < size)
                    {
                        int neighborIdx = nz * size + nx;
                        if (grid[neighborIdx].Collapsed)
                        {
                            continue;
                        }

                        bool changed = false;
                        for (int t = 0; t < (int)TileType.Count; t++)
                        {
                            if (!grid[neighborIdx].Possibilities[t])
                            {
                                continue;
                            }

                            bool pathPossible = false;
                            for (int c = 0; c < (int)TileType.Count; c++)
                            {
                                if (grid[currentIdx].Possibilities[c] && _adjacencyRules[c, t])
                                {
                                    pathPossible = true;
                                    break;
                                }
                            }

                            if (!pathPossible)
                            {
                                grid[neighborIdx].Possibilities[t] = false;
                                changed = true;
                            }
                        }

                        if (changed)
                        {
                            propagationQueue.Enqueue(new Vector2Int(nx, nz));
                        }
                    }
                }
            }
        }
    }

    public class HarmonicTerrainEngine : MonoBehaviour
    {
        // ⚡ Bolt: Cache for WaitForSeconds using millisecond keys to prevent floating-point precision issues
        // What: Replaced new WaitForSeconds with a cached dictionary lookup.
        // Why: Instantiating new WaitForSeconds inside generation loops causes recurring GC allocations and heap fragmentation.
        // Impact: Eliminates O(N) GC allocations per chunk request cycle in the terrain engine.
        private static readonly System.Collections.Generic.Dictionary<int, WaitForSeconds> _waitCache = new System.Collections.Generic.Dictionary<int, WaitForSeconds>();

        private static WaitForSeconds GetWait(float seconds)
        {
            int msKey = Mathf.RoundToInt(seconds * 1000f);
            if (!_waitCache.TryGetValue(msKey, out var wait))
            {
                wait = new WaitForSeconds(seconds);
                _waitCache[msKey] = wait;
            }
            return wait;
        }

        public const int CHUNK_SIZE = 16;
        private static readonly float[] NINE_CORE_FREQUENCIES = {
            174.0f, 285.0f, 396.0f, 417.0f, 528.0f, 639.0f, 741.0f, 852.0f, 963.0f
        };

        private readonly ConcurrentDictionary<ChunkCoord, TerrainChunk> _activeChunks = new ConcurrentDictionary<ChunkCoord, TerrainChunk>();
        private readonly HashSet<ChunkCoord> _queuedChunks = new HashSet<ChunkCoord>();
        private readonly object _queueLock = new object();
        private readonly StructuralSolver _structuralSolver = new StructuralSolver();

        private void Start()
        {
            Debug.Log("Starting MILEHIGH-WORLD Layered Logical Generation Engine...");
            StartCoroutine(SimulationCoroutine());
        }

        private IEnumerator SimulationCoroutine()
        {
            Debug.Log("\n[Engine] Player spawned at Origin. Requesting local sectors...");
            RequestChunksAround(0, 0, 2);
            yield return GetWait(0.5f);

            Debug.Log("\n[Engine] Player traveling East. Streaming new sectors...");
            RequestChunksAround(10, 0, 2);
            yield return GetWait(0.5f);

            Debug.Log("\nShutting down engine...");
        }

        public void RequestChunksAround(int playerChunkX, int playerChunkZ, int radius)
        {
            for (int x = playerChunkX - radius; x <= playerChunkX + radius; x++)
            {
                for (int z = playerChunkZ - radius; z <= playerChunkZ + radius; z++)
                {
                    ChunkCoord coord = new ChunkCoord(x, z);
                    lock (_queueLock)
                    {
                        if (!_activeChunks.ContainsKey(coord) && !_queuedChunks.Contains(coord))
                        {
                            _queuedChunks.Add(coord);
                            Task.Run(() => GenerateChunkAsync(coord));
                        }
                    }
                }
            }
        }

        private async Task GenerateChunkAsync(ChunkCoord coord)
        {
            TerrainChunk chunk = new TerrainChunk(coord, CHUNK_SIZE);

            // Phase 1: Macro Terrain Assembly
            for (int z = 0; z < CHUNK_SIZE; z++)
            {
                for (int x = 0; x < CHUNK_SIZE; x++)
                {
                    int worldX = (chunk.Coord.x * CHUNK_SIZE) + x;
                    int worldZ = (chunk.Coord.z * CHUNK_SIZE) + z;
                    chunk.HeightMap[z * CHUNK_SIZE + x] = GetHarmonicElevation(worldX, worldZ);
                }
            }

            // Phase 2: Rule-Based Structural Constraint Processing
            _structuralSolver.CollapseChunkLayout(chunk);

            chunk.IsReady = true;
            _activeChunks[coord] = chunk;

            Debug.Log($"[Worker Pool] Compiled Sector ({coord.x}, {coord.z}) with structural architecture map.");

            lock (_queueLock)
            {
                _queuedChunks.Remove(coord);
            }
        }

        private static float GetHarmonicElevation(int worldX, int worldZ)
        {
            float elevation = 0.0f;
            float amplitude = 1.0f;
            float maxElevation = 0.0f;
            for (int i = 0; i < NINE_CORE_FREQUENCIES.Length; i++)
            {
                float harmonicFreq = NINE_CORE_FREQUENCIES[i] / 10000.0f;
                float sampleX = worldX * harmonicFreq;
                float sampleZ = worldZ * harmonicFreq;
                elevation += SmoothNoise(sampleX, sampleZ) * amplitude;
                maxElevation += amplitude;
                amplitude *= 0.5f;
            }
            return elevation / maxElevation;
        }

        private static float SmoothNoise(float x, float z)
        {
            float intX = Mathf.Floor(x);
            float fracX = x - intX;
            float intZ = Mathf.Floor(z);
            float fracZ = z - intZ;
            float v1 = HashNoise2D(intX, intZ);
            float v2 = HashNoise2D(intX + 1.0f, intZ);
            float v3 = HashNoise2D(intX, intZ + 1.0f);
            float v4 = HashNoise2D(intX + 1.0f, intZ + 1.0f);
            float ftx = fracX * 3.1415927f;
            float f1 = (1.0f - Mathf.Cos(ftx)) * 0.5f;
            float i1 = v1 * (1.0f - f1) + v2 * f1;
            float i2 = v3 * (1.0f - f1) + v4 * f1;
            float ftz = fracZ * 3.1415927f;
            float f2 = (1.0f - Mathf.Cos(ftz)) * 0.5f;
            return i1 * (1.0f - f2) + i2 * f2;
        }

        private static float HashNoise2D(float x, float z)
        {
            int n = (int)(x * 57.0f + z * 131.0f);
            n = (n << 13) ^ n;
            return (1.0f - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0f);
        }
    }
}
