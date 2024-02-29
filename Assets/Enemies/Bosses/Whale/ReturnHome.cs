using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class ReturnHomeButton : MonoBehaviour
{
    public Destroyer destroyer;
    // Method to load the first scene
    public void LoadFirstScene()
    {
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
        if (GlobalEnemyManager.KrakenDefeated == true)
        {
            GlobalEnemyManager.KrakenNotDefeated();
        }
        if (GlobalEnemyManager.CrabDefeated == true)
        {
            GlobalEnemyManager.CrabNotDefeated();
        }

        destroyer.Destroy();
        SceneManager.LoadScene(0);
    }
}
