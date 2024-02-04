using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health; // player's current health
    public int maxHealth = 10; // total amount of health the player has

    public SpriteRenderer playerSr;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        health = maxHealth;
       // DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        PlayerPrefs.SetInt("PlayerHealth", health);
    }


    public void TakeDamage(int amount) // how much damage the player takes
    {

        health -= amount;

        if (health <= 0)
        {
            playerSr.enabled = false;
            playerMovement.enabled = false;
        }
    }
}