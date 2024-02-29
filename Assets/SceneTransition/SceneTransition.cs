using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string spawnPointTag = "SpawnPoint"; // Tag for the spawn point object

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(GlobalEnemyManager.TotalEnemies);
        if (other.CompareTag("Player") && !other.isTrigger && GlobalEnemyManager.TotalEnemies == 0)
        {
            if(GlobalEnemyManager.KrakenDefeated == false && GlobalEnemyManager.CrabDefeated == false){
                if (GlobalEnemyManager.ScenesVisited >= 0)
                {
                    LoadSpecificScene(); // Load a specific scene after # of visits
                }
                else
                {
                    LoadRandomScene(); // Continue loading random scenes otherwise
                }
            }

            if(GlobalEnemyManager.KrakenDefeated == true && GlobalEnemyManager.CrabDefeated == false){
                if (GlobalEnemyManager.ScenesVisited >= 8)
                    {
                        LoadSpecificScene(); // Load a specific scene after # of visits
                    }
                else
                    {
                        LoadRandomScene(); // Continue loading random scenes otherwise
                    }
            }

            if(GlobalEnemyManager.KrakenDefeated == true && GlobalEnemyManager.CrabDefeated == true){
                if (GlobalEnemyManager.ScenesVisited >= 12)
                    {
                        LoadSpecificScene(); // Load a specific scene after # of visits
                    }
                else
                    {
                        LoadRandomScene(); // Continue loading random scenes otherwise
                    }
            }
        }
    }

    private void LoadRandomScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int randomSceneIndex = 0;

        do
        {
            if(GlobalEnemyManager.KrakenDefeated == false && GlobalEnemyManager.CrabDefeated == false){
            randomSceneIndex = Random.Range(3, 7);
            }

            if(GlobalEnemyManager.KrakenDefeated == true && GlobalEnemyManager.CrabDefeated == false){
            randomSceneIndex = Random.Range(9, 14);
            }

            if(GlobalEnemyManager.KrakenDefeated == true && GlobalEnemyManager.CrabDefeated == true){
            randomSceneIndex = Random.Range(18, 23);
            }
        }
        while (randomSceneIndex == currentSceneIndex);

        Debug.Log("Loading scene index: " + randomSceneIndex);
        GlobalEnemyManager.IncrementScenesVisited(); // Increment the counter
        SceneManager.LoadScene(randomSceneIndex);
    }

    private void LoadSpecificScene()
    {

        int specificSceneIndex = 0;

        if(GlobalEnemyManager.KrakenDefeated == false && GlobalEnemyManager.CrabDefeated == false){
            specificSceneIndex = 15;
        }

        if(GlobalEnemyManager.KrakenDefeated == true && GlobalEnemyManager.CrabDefeated == false){
            specificSceneIndex = 8;
        }

        if(GlobalEnemyManager.KrakenDefeated == true && GlobalEnemyManager.CrabDefeated == true){
            specificSceneIndex = 2;
        }

        Debug.Log("Loading specific scene index: " + specificSceneIndex);
            SceneManager.LoadScene(specificSceneIndex);

    }

}
