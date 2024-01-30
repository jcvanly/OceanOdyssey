using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    public int health;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;

    public Image[] hearts;
    public PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found.");
        }
    }


    // Update is called once per frame
    void Update()
    {

        health = playerHealth.health;
        maxHealth = playerHealth.maxHealth;


       for (int i = 0; i < hearts.Length; i++) 
        {
            if ( i < health)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else 
            {
                hearts[i].enabled=false;
            }
        }
    }
}