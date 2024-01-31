using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for Action

public class EnemyDeath : MonoBehaviour
{
    public static event Action OnDeath;

    public void Die()
    {
        // Other death related code...
        OnDeath?.Invoke();
        EnemySpawner.EnemyDied(); // Notify the spawner that an enemy has died
        Destroy(gameObject);
    }
}