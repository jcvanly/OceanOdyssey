using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleMirage : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    //public Image healthBar; // Reference to the UI health bar
    private EnemyDeath enemyDeath; // Reference to the EnemyDeath component
    private Color originalColor;
    private float currTime;
    private float shootTimer;

    void Start()
    {
        shootTimer = shootInterval;
        currentHealth = maxHealth; // Initialize current health
        enemyDeath = GetComponent<EnemyDeath>();
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        currTime = Time.time;

        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        float speed = 5f;
        // Shooting in all diagonal directions
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, -1).normalized * speed;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, 1).normalized * speed;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-1, -1).normalized * speed;  
    }
}
