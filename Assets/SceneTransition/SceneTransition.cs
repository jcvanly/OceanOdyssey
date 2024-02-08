using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string spawnPointTag = "SpawnPoint"; // Tag for the spawn point object

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger && GlobalEnemyManager.TotalEnemies == 0)
        {
            if (GlobalEnemyManager.ScenesVisited >= 3)
            {
                LoadSpecificScene(); // Load a specific scene after # of visits
            }
            else
            {
                LoadRandomScene(); // Continue loading random scenes otherwise
            }
        }
    }

    private void LoadRandomScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int randomSceneIndex;

        do
        {
            // Assuming you want to skip the first scene (index 0) and start from index 1
            randomSceneIndex = Random.Range(3, sceneCount);
            DontDestroyOnLoad(gameObject);
        }
        while (randomSceneIndex == currentSceneIndex);

        Debug.Log("Loading scene index: " + randomSceneIndex);
        GlobalEnemyManager.IncrementScenesVisited(); // Increment the counter
        SceneManager.LoadScene(randomSceneIndex);

        // Call a method to spawn the player at the designated spawn point
        StartCoroutine(SpawnPlayerAtSpawnPoint());
    }

    private void LoadSpecificScene()
    {
        int specificSceneIndex = 2;
        Debug.Log("Loading specific scene index: " + specificSceneIndex);
        SceneManager.LoadScene(specificSceneIndex);

        // Call a method to spawn the player at the designated spawn point
        StartCoroutine(SpawnPlayerAtSpawnPoint());
    }

    IEnumerator SpawnPlayerAtSpawnPoint()
    {
        // Wait for the next frame to make sure the scene is fully loaded
        yield return null;

        // Find the spawn point object using the specified tag
        GameObject spawnPoint = GameObject.FindGameObjectWithTag(spawnPointTag);

        if (spawnPoint != null)
        {
            // Assuming your player has a Rigidbody2D component
            Rigidbody2D playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

            // Set the player's position to the spawn point position
            playerRb.position = spawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("Spawn point not found in the scene.");
        }
    }
}
