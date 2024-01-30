using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array to hold different enemy prefabs
    private GameObject currentEnemy; // To keep track of the spawned enemy

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        // Check if the current enemy has been destroyed
        if (currentEnemy == null)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Choose a random enemy prefab
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject randomEnemyPrefab = enemyPrefabs[randomIndex];

        // Spawn the random enemy
        currentEnemy = Instantiate(randomEnemyPrefab, transform.position, Quaternion.identity);
    }
}
