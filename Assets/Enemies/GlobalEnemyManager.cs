using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalEnemyManager
{
    public static int TotalEnemies { get; private set; } = 0;

    public static void EnemySpawned()
    {
        TotalEnemies++;
    }

    public static void EnemyDied()
    {
        TotalEnemies--;
    }
}
