using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health; // player's current health
    public int maxHealth = 5; // total amount of health the player has


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount) // how much damage the player takes
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}