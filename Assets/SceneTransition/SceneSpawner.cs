using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSpawner : MonoBehaviour
{
    public GameObject[] spawnTransitions;
    private bool hasSpawned = false; // Flag to check if enemy has been spawned

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (!hasSpawned)
        {
            int randomIndex = Random.Range(0, spawnTransitions.Length);
            GameObject randomEnemyPrefab = spawnTransitions[randomIndex];
            Instantiate(randomEnemyPrefab, transform.position, Quaternion.identity);
            hasSpawned = true; // Set flag to true to prevent further spawning
        }
    }
}