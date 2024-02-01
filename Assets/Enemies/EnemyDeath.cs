using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // Needed for Action

public class EnemyDeath : MonoBehaviour
{
    public static event Action OnDeath;

    public void Die()
    {
        //Debug.Log("I died");
        OnDeath?.Invoke();
        GlobalEnemyManager.EnemyDied(); // Decrement global counter
        Destroy(gameObject);
    }
}
