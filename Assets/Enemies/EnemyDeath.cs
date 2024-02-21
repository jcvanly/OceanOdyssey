using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for Action

public class EnemyDeath : MonoBehaviour
{
    public static event Action OnDeath;
    public List<GameObject> powerUpPrefabs;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void Die()
    {
        // Randomly determine whether to spawn a power-up
        if (UnityEngine.Random.value <= 0.5f) // 10% chance
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
        Destroy(gameObject);

        audioManager.PlaySFX(audioManager.enemyDeath);
    }
    private void SpawnPowerUp(GameObject powerUpPrefab)
    {
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
    }
}