using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for UI elements

public class PistolShrimp : MonoBehaviour
{
    public GameObject projectilePrefab;
   // public Transform target;
    public float shootInterval = 2f;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    //public Image healthBar; // Reference to the UI health bar

    private float shootTimer;

    void Start()
    {
        shootTimer = shootInterval;
        currentHealth = maxHealth; // Initialize current health
        //target = GameObject.FindGameObjectWithTag("Player").transform;
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
        float speed = 10f;
        // Shooting in all diagonal directions
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            TakeDamage(10); // Assuming each hit decreases 10 health
            Destroy(collider.gameObject); // Destroy the projectile
        }
    }
    

    public void TakeDamage(int damage)
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
