using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100; // Max health of the enemy
    private int currentHealth; // Current health of the enemy
    //public Image healthBar; // Reference to the UI health bar

    private float shootTimer;

    void Start()
    {
        shootTimer = shootInterval;
        currentHealth = maxHealth; // Initialize current health
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        // Shooting in the four cardinal directions
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.up * 5f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.down * 5f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.right * 5f;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10); // Assuming each hit decreases 10 health
            Destroy(collider.gameObject); // Destroy the projectile
        }
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;
        //healthBar.fillAmount = (float)currentHealth / maxHealth; // Update health bar

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Add logic for enemy death, e.g., play animation, sound, etc.
        Destroy(gameObject); // Destroy the enemy object
    }
}
