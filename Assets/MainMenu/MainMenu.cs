using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    public void PlayGame()
    {
        audioManager.PlaySFX(audioManager.mainMenuStart);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void Credits()
    {

        audioManager.PlaySFX(audioManager.mainMenuCredit);
        SceneManager.LoadScene("Credits");
    }
    public void QuitGame()
    {
        audioManager.PlaySFX(audioManager.quit);
        Application.Quit();
    }
    
    public void Back() 
    {
        //audioManager = audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
       // if (audioManager != null)
       // {
        //    Destroy(audioManager.gameObject);
       // }
        SceneManager.LoadScene("TitleScreen");
    }
}