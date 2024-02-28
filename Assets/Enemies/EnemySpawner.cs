using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private bool hasSpawned = false; // Flag to check if enemy has been spawned
    

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (!hasSpawned)
        {
            bool krakenDead = GlobalEnemyManager.KrakenDefeated;
            bool crabLobsterDead  = GlobalEnemyManager.CrabAndLobsterDead();
            int randomIndex;

            if(krakenDead && crabLobsterDead == false)
            {
                randomIndex = Random.Range(0, enemyPrefabs.Length - 2);
            }
            else if (krakenDead && crabLobsterDead)
            {
                randomIndex = Random.Range(0, enemyPrefabs.Length - 2);
            }
            else
            {
                randomIndex = Random.Range(0,enemyPrefabs.Length - 3);
            }
            
            GameObject randomEnemyPrefab = enemyPrefabs[randomIndex];
            Instantiate(randomEnemyPrefab, transform.position, Quaternion.identity);
            GlobalEnemyManager.EnemySpawned(); // Increment global counter
            hasSpawned = true; // Set flag to true to prevent further spawning
        }
    }
}