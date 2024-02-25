using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GlobalEnemyManager
{
    public static int TotalEnemies { get; private set; } = 0;
    public static int ScenesVisited { get; private set; } = 0; // Tracker for scenes visited
    public static bool CrabDead = false;
    public static bool LobsterDead = false;
    public static bool KrakenDefeated = false;
    public static bool CrabDefeated = false;

    public static bool CrabAndLobsterDead() {
        return CrabDead && LobsterDead;
    }

    public static void EnemySpawned()
    {
        TotalEnemies++;
        //Debug.Log("Total number of enemies: " + TotalEnemies);
    }

    public static void EnemyDied()
    {
        TotalEnemies = Mathf.Max(0, TotalEnemies - 1); // Ensure TotalEnemies never goes below 0
        Debug.Log("Total number of enemies: " + TotalEnemies);
    }

    public static void IncrementScenesVisited()
    {
        ScenesVisited++;
    }

    public static void DecreaseScenesVisited()
    {
        if (ScenesVisited > 0)
        {
            ScenesVisited--;
        }
    }

}