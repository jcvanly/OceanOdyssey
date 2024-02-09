using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for Action

public class EnemyDeath : MonoBehaviour
{
    public static event Action OnDeath;

    // List of power-up prefabs
    public List<GameObject> powerUpPrefabs;

    public void Die()
    {
        // Randomly determine whether to spawn a power-up
        if (UnityEngine.Random.value <= 0.1f) // 10% chance
        {
            // Randomly choose a power-up prefab from the list
            GameObject selectedPowerUpPrefab = powerUpPrefabs[UnityEngine.Random.Range(0, powerUpPrefabs.Count)];

            // Spawn the selected power-up prefab
            SpawnPowerUp(selectedPowerUpPrefab);
        }

        // Invoke the OnDeath event
        OnDeath?.Invoke();

        // Decrement global counter
        GlobalEnemyManager.EnemyDied();

        // Destroy the enemy gameObject
        Destroy(gameObject);
    }


    // Function to spawn the power-up
    private void SpawnPowerUp(GameObject powerUpPrefab)
    {
        // Instantiate the selected power-up prefab at the enemy's position
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
    }
}
