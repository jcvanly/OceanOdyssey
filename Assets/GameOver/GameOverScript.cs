using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    AudioManager audioManager;
    public Destroyer destroyer;
    private GameObject crosshair;

    public void Start()
    {
        crosshair = GameObject.FindGameObjectWithTag("crosshair");
    }
    public static void showGameOver()
    {
       SceneManager.LoadScene("GameOver");
    }
    public void restartLevel()
    {

        //audioManager = audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //Destroy(audioManager.gameObject);
        if (GlobalEnemyManager.TotalEnemies != 0)
        {
            for (int i = 0; i <= GlobalEnemyManager.TotalEnemies; i++)
            {
                GlobalEnemyManager.EnemyDied();
                GlobalEnemyManager.EnemyDied();
            }
        }
        if (GlobalEnemyManager.ScenesVisited != 0)
        {
            for (int i = 0; i <= GlobalEnemyManager.ScenesVisited; i++)
            {
                GlobalEnemyManager.DecreaseScenesVisited();
                GlobalEnemyManager.DecreaseScenesVisited();
            }
        }
        if (crosshair != null)
        {
            Destroy(crosshair);
        }
        SceneManager.LoadScene("StartArea");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
        UnityEditor.EditorApplication.isPlaying = false;
    }
}