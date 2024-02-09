using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for Action

public class EnemyDeath : MonoBehaviour
{
    public static event Action OnDeath;

    // Reference to the power-up prefab
    public GameObject powerUpPrefab;

    public void Die()
    {
        // Always spawn the power-up for testing purposes
        SpawnPowerUp();

        // Invoke the OnDeath event
        OnDeath?.Invoke();

        // Decrement global counter
        GlobalEnemyManager.EnemyDied();

        // Destroy the enemy gameObject
        Destroy(gameObject);
    }

    // Function to spawn the power-up
    private void SpawnPowerUp()
    {
        // Instantiate the power-up prefab at the enemy's position
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
    }
}
