using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{
    public static void showGameOver()
    {
       SceneManager.LoadScene("GameOver");
    }
    public void restartLevel()
    {
        SceneManager.LoadScene("StartArea");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
