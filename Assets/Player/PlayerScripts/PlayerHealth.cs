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
    public float flashDuration = .4f;
    private Color damageColor = new Color(1f, 0f, 0f, 1f);
    private Color originalColor;
    private float damageTime;
    private float timeLastFlash;
    private float currTime;
    private bool flashOnDamage = false;
    private AudioSource soundEffects;
    public AudioClip sound;

    public GameObject player; // Reference to your player GameObject
    public GameObject heartCanvas; // Reference to your heart canvas GameObject
    public Camera mainCamera; // Reference to your main camera GameObject
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        health = maxHealth;
        originalColor = playerSr.color;
        FindPlayerObjects();
        // DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        PlayerPrefs.SetInt("PlayerHealth", health);
        currTime = Time.time;

        if(currTime >= (damageTime + flashDuration) && flashOnDamage == true)
        {
            resetColor();
        }
        else if (flashOnDamage == true)
        {
            if(playerSr.color == originalColor && currTime >= (timeLastFlash + .1f))
            {
                timeLastFlash = currTime;
                playerSr.color = damageColor;
            }
            else if (playerSr.color == damageColor && currTime >= (timeLastFlash + .1f))
            {
                timeLastFlash = currTime;
                playerSr.color = originalColor;
            }
        }
        GameObject soundObject = GameObject.FindWithTag("SoundEffects");
        if (soundObject != null)
        {
            soundEffects = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'SoundEffects' found.");
        }
    }


    public void TakeDamage(int amount) // how much damage the player takes
    {

        health -= amount;
        if (soundEffects != null)
        {
            soundEffects.PlayOneShot(sound);
        }
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
        }

    }

    public void resetColor()
    {
        playerSr.color = originalColor;
        flashOnDamage = false;
    }

    public void DestroyPlayerAndCanvas()
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
        //  mainCamera = Camera.main; // Assuming the main camera is tagged as "MainCamera"
    }


}