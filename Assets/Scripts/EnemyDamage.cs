using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public int damage = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Assign the playerHealth reference directly in code.
            playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("PlayerHealth is still null after trying to assign it. Make sure the player GameObject has the PlayerHealth script.");
            }
        }
    }
}