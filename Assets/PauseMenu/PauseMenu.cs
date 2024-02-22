using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public Destroyer destroyer;
    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        audioManager.PlaySFX(audioManager.pause);
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        GameIsPaused = false;
        audioManager.PlaySFX(audioManager.unpause);
    }
    public void LoadMenu()
    {
         if (GlobalEnemyManager.TotalEnemies != 0)
        {
            for (int i = 0; i <= GlobalEnemyManager.TotalEnemies; i++)
            {
                GlobalEnemyManager.EnemyDied();
                GlobalEnemyManager.EnemyDied();
            }
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScreen");
        destroyer.Destroy();
        audioManager.PlaySFX(audioManager.mainMenu);
    }
    public void QuitMenu()
    {
        audioManager.PlaySFX(audioManager.quit);
        Debug.Log("Quiting Game.");
        Application.Quit();
    }
}