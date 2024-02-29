using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMirage : MonoBehaviour
{
    private bool shootDiagonal = true;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    private float shootTimer;
    public Sprite normalSprite; // Assign this in the Inspector
    public Sprite enragedSprite; // Assign this in the Inspector
    private bool isEnraged = false; // Tracks whether the mirage is enraged
    public WhaleBehavior mainWhale;
    public GameObject waterOrbPrefab; // Assign this in the Unity Inspector\

    void Start()
    {
        shootTimer = shootInterval; // Initialize the shoot timer with the shoot interval
        currentHealth = maxHealth; // Set current health to max health at start
        GetComponent<SpriteRenderer>().sprite = normalSprite; // Set the sprite to the normal state sprite
    }

    void Update()
    {
        // Sync the enraged state and sprite with the main whale
        if (mainWhale != null && mainWhale.isEnraged != isEnraged)
        {
            isEnraged = mainWhale.isEnraged;
            GetComponent<SpriteRenderer>().sprite = isEnraged ? enragedSprite : normalSprite;
        }

        // Shoot at the defined interval
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            if (isEnraged)
            {
                // If enraged, shoot water orb
                ShootWaterOrb();
                Shoot();
            }
            else
            {
                // If not enraged, shoot normal projectile
                Shoot();
            }
            // Reset the shoot timer
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        float speed = 4f;
        if (shootDiagonal)
        {
            // Shooting in all diagonal directions
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1).normalized * speed;
        }
        else
        {
            // Shooting in cardinal directions
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 0).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 0).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1).normalized * speed;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1).normalized * speed;
        }

        // Toggle the firing pattern for the next call
        shootDiagonal = !shootDiagonal;
    }

    void ShootWaterOrb()
    {
        // Instantiate and shoot the water orb
        GameObject waterOrb = Instantiate(waterOrbPrefab, transform.position, Quaternion.identity);
        // Set velocity or direction for the water orb towards the target
    }


    public void SetEnragedState(bool enraged)
    {
        isEnraged = enraged;
        GetComponent<SpriteRenderer>().sprite = isEnraged ? enragedSprite : normalSprite;
    }

}
