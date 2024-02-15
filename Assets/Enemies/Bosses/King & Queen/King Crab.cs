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
    public GameObject clawPrefab; // Prefab for the claw attack
    public Transform clawSpawnPoint; // The point from where the claw will be spawned/swung
    public float moveSpeed = 5f; // Movement speed of the crab
    public float stoppingDistance = 4f; // Distance at which the crab stops moving towards the player
    public float attackDistance = 3f; // Distance within which the crab will attack the player
    public float clawSpeed = 10f; // Speed at which the claw moves towards the player
    public float clawSwingDelay = 2f; // Time between claw swings
    public CircleAttack circleAttack;
    private bool isAttacking = false;




    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }


    void Update()
    {
        MoveTowardsPlayer();
        if (!isAttacking && Vector2.Distance(transform.position, player.position) <= attackDistance)
        {
            StartCoroutine(PerformAttackSequence());
        }
    }

    IEnumerator PerformAttackSequence()
{
    isAttacking = true;

    // Show the attack indicator at the player's position or another target area
    if (circleAttack != null)
    {
        circleAttack.WarnOfAttack(player.position);
    }

    // Wait for the indicator to complete its sequence
    yield return new WaitForSeconds(circleAttack.warningDuration + 0.5f); // Additional wait time after the indicator becomes fully visible

    isAttacking = false;
}
    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, player.position);
            if (distanceToPlayer > stoppingDistance)
            {
                Vector2 targetPosition = Vector2.MoveTowards(rb.position, player.position, moveSpeed * Time.deltaTime);
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