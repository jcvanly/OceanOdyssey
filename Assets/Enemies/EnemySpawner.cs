using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private GameObject currentEnemy;
    public static int activeEnemies = 0; // Static counter for active enemies
    public float spawnDelay = 500f; // Delay between spawns
    private float spawnTimer;

    void Start()
    {
        SpawnEnemy();
    }

    void Update()
    {
        // Check if it's time to spawn a new enemy
        if (currentEnemy == null && activeEnemies == 0)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnDelay)
            {
                SpawnEnemy();
                spawnTimer = 0; // Reset timer after spawning
            }
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

    public float getNumberOfEnemies ()
    {
        return activeEnemies;
    }
}
