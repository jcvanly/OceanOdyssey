using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenBehavior : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100;
    public int currentHealth;
    private float shootTimer;
    private Transform playerTransform;
    public int numberOfShots = 7; // Number of shots in the beam
    public float timeBetweenShots = 0.1f; // Time between each shot in the beam

    void Start()
    {
        shootTimer = shootInterval;
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            StartCoroutine(ShootBeam());
            shootTimer = shootInterval;
        }
    }

    IEnumerator ShootBeam()
    {
        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

        for (int i = 0; i < numberOfShots; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = directionToPlayer * 5f; // Adjust speed as necessary
            yield return new WaitForSeconds(timeBetweenShots); // Wait for the specified time before firing the next shot
        }
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
            //enemyDeath.Die();
            //Die();
        }
    }

    void Die()
    {
        // Add logic for enemy death, e.g., play animation, sound, etc.
        Destroy(gameObject); // Destroy the enemy object
    }
}