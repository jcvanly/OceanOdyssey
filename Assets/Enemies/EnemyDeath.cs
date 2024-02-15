using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for Action

public class EnemyDeath : MonoBehaviour
{
    public static event Action OnDeath;
    private AudioSource soundEffects;
    public AudioClip sound;
    // List of power-up prefabs
    public List<GameObject> powerUpPrefabs;

    private void Update()
    {
        GameObject soundObject = GameObject.FindWithTag("SoundEffects");
        if (soundObject != null)
        {
            soundEffects = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'SoundEffects' found.");
        }
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

        // Destroy the enemy gameObject
        Destroy(gameObject);

        if (soundEffects != null)
        {
            soundEffects.PlayOneShot(sound);
        }
    }


    // Function to spawn the power-up
    private void SpawnPowerUp(GameObject powerUpPrefab)
    {
        // Instantiate the selected power-up prefab at the enemy's position
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
    }
}
