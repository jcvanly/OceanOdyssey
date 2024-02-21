using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health; // player's current health
    public int maxHealth = 10; // total amount of health the player has

    public SpriteRenderer playerSr;
    public PlayerMovement playerMovement;

    public float flashDuration = 0.4f;
    private Color damageColor = new Color(1f, 0f, 0f, 1f);
    private Color originalColor;
    private float damageTime;
    private float timeLastFlash;
    private float currTime;
    private bool flashOnDamage = false;

    public GameObject player; // Reference to your player GameObject
    public GameObject heartCanvas; // Reference to your heart canvas GameObject
    public Camera mainCamera; // Reference to your main camera GameObject

    public float invincibilityDuration = 2f; // Duration of invincibility frames in seconds
    private bool isInvincible = false; // Flag to track if the player is currently invincible
    private float invincibilityEndTime; // Time when invincibility ends

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        health = maxHealth;
        originalColor = playerSr.color;
        FindPlayerObjects();
    }

    private void Update()
    {
        PlayerPrefs.SetInt("PlayerHealth", health);
        currTime = Time.time;

        if (flashOnDamage)
        {
            if (playerSr.color == originalColor && currTime >= (timeLastFlash + 0.1f))
            {
                timeLastFlash = currTime;
                playerSr.color = damageColor;
            }
            else if (playerSr.color == damageColor && currTime >= (timeLastFlash + 0.1f))
            {
                timeLastFlash = currTime;
                playerSr.color = originalColor;
            }

            if (currTime >= (damageTime + flashDuration))
            {
                flashOnDamage = false;
                playerSr.color = originalColor; // Restore original color after flashing
            }
        }
        else if (isInvincible)
        {
            playerSr.color = Color.green; // Set player color to green during invincibility
        }

        if (isInvincible && Time.time >= invincibilityEndTime)
        {
            isInvincible = false;
            playerSr.color = originalColor; // Restore original color
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isInvincible) // Only take damage if not already invincible
        {
            health -= amount;
            audioManager.PlaySFX(audioManager.takeDamage);
            if (health <= 0)
            {
                playerSr.enabled = false;
                playerMovement.enabled = false;
                DestroyPlayerAndCanvas();
                GameOverScript.showGameOver();
            }
            else
            {
                currTime = Time.time;
                flashOnDamage = true;
                damageTime = currTime;
                timeLastFlash = currTime;
                playerSr.color = damageColor;

                // Set invincibility
                isInvincible = true;
                invincibilityEndTime = Time.time + invincibilityDuration;
            }
        }
    }

    private void resetColor()
    {
        playerSr.color = originalColor;
        flashOnDamage = false;
    }

    private void DestroyPlayerAndCanvas()
    {
        if (player != null)
        {
            Destroy(player);
        }

        if (heartCanvas != null)
        {
            Destroy(heartCanvas);
        }

        if (mainCamera != null)
        {
            Destroy(mainCamera.gameObject);
        }
    }

    private void FindPlayerObjects()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        heartCanvas = GameObject.FindGameObjectWithTag("HeartCanvas");
    }
}