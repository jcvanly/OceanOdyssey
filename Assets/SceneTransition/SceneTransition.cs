using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneTransition : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            LoadRandomScene();
        }

        Debug.Log("Trigger entered");
    }

    private void LoadRandomScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int randomSceneIndex;

        do
        {
            // Assuming you want to skip the first scene (index 0) and start from index 1
            randomSceneIndex = Random.Range(2, sceneCount);
            DontDestroyOnLoad(gameObject);
        } 
        while (randomSceneIndex == currentSceneIndex);

        Debug.Log("Loading scene index: " + randomSceneIndex);
        SceneManager.LoadScene(randomSceneIndex);
    }
}
