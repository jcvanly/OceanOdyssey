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
            if (GlobalEnemyManager.ScenesVisited >= 1)
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
            //DontDestroyOnLoad(gameObject);
        }
        while (randomSceneIndex == currentSceneIndex);

        Debug.Log("Loading scene index: " + randomSceneIndex);
        GlobalEnemyManager.IncrementScenesVisited(); // Increment the counter
        SceneManager.LoadScene(randomSceneIndex);

        // Call a method to spawn the player at the designated spawn point

    }

    private void LoadSpecificScene()
    {
        int specificSceneIndex = 2;
        Debug.Log("Loading specific scene index: " + specificSceneIndex);
        SceneManager.LoadScene(specificSceneIndex);

        // Call a method to spawn the player at the designated spawn point

    }

}
