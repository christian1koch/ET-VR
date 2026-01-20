using UnityEngine;
using System.Collections.Generic;

public class TargetSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Prefabs to spawn as targets. Each prefab spawns exactly once.")]
    [SerializeField] private GameObject[] targetPrefabs;
    
    [Header("Spawn Points")]
    [Tooltip("Array of 10 spawn point transforms. Targets will be randomly placed at these locations.")]
    [SerializeField] private GameObject[] spawnPointsGameObjects;
    
    [Header("Options")]
    [Tooltip("If true, ensures Target component is added to spawned objects if missing.")]
    [SerializeField] private bool ensureTargetComponent = true;

    private void Start()
    {
        SpawnTargets();
    }

    private void SpawnTargets()
    {
        if (targetPrefabs == null || targetPrefabs.Length == 0)
        {
            Debug.LogError("TargetSpawner: No target prefabs assigned!");
            return;
        }

        if (spawnPointsGameObjects == null || spawnPointsGameObjects.Length == 0)
        {
            Debug.LogError("TargetSpawner: No spawn points assigned!");
            return;
        }

        if (spawnPointsGameObjects.Length < targetPrefabs.Length)
        {
            Debug.LogWarning($"TargetSpawner: Not enough spawn points ({spawnPointsGameObjects.Length}) for all prefabs ({targetPrefabs.Length})!");
        }
        
        // Get randomized spawn point indices
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < spawnPointsGameObjects.Length; i++)
        {
            if (spawnPointsGameObjects[i] != null)
            {
                availableIndices.Add(i);
            }
        }

        // Shuffle indices using Fisher-Yates algorithm
        ShuffleList(availableIndices);

        // Spawn one of each prefab at random positions
        int actualSpawnCount = Mathf.Min(targetPrefabs.Length, availableIndices.Count);
        
        for (int i = 0; i < actualSpawnCount; i++)
        {
            Transform spawnPoint = spawnPointsGameObjects[availableIndices[i]].transform;
            
            // Each prefab spawns exactly once
            GameObject prefab = targetPrefabs[i];
            
            // Instantiate at root level first with identity transform
            GameObject spawnedTarget = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                       
            Debug.Log($"TargetSpawner: Spawned {prefab.name} at {spawnPoint.position}");
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
