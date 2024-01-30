using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private GameObject currentEnemy;
    public static int activeEnemies = 0; // Static counter for active enemies

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        if (currentEnemy == null)
        {
            SpawnEnemy();
        }

        // Perform actions when no enemies are left
        if (activeEnemies == 0)
        {
            // Perform your actions here
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject randomEnemyPrefab = enemyPrefabs[randomIndex];
        currentEnemy = Instantiate(randomEnemyPrefab, transform.position, Quaternion.identity);
        activeEnemies++; // Increment the counter when an enemy is spawned
    }

    // Call this method to decrement the counter
    public static void EnemyDied()
    {
        activeEnemies--;
    }
}