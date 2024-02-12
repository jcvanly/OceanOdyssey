using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCrab : MonoBehaviour
{
    private Rigidbody2D rb;

    public int maxHealth = 40;
    public int currentHealth;
    public CrabHealthBar healthBar;
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 5f; // Movement speed of the crab
    public float stoppingDistance = 4f; // Distance at which the crab stops moving towards the player

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, player.position);
            if (distanceToPlayer > stoppingDistance)
            {
                // Calculate the next position towards the player only if outside stopping distance
                Vector2 targetPosition = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.deltaTime);
                // Use Rigidbody2D to move to this position to respect physics
                rb.MovePosition(targetPosition);
            }
        }   
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {            
            TakeDamage(10); 
            Destroy(collider.gameObject); // Destroy the projectile
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("Current health is now 0, die is called");
            Die();
        }
    }

    void Die()
    {
        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("CrabHealth");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}