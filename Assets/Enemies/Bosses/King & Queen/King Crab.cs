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
    public float attackDistance = 3f; // Distance within which the crab will attack the player
    private bool isAttacking = false;
    private float attackTimer = 0f;
    public float attackInterval = 3f; // Time between attacks in the cycle
    private int attackPhase = 0; // To track which attack is to be performed next
    public GameObject swooshPrefab; // The prefab for the swoosh effect
    public float sideSlashWarningDuration = 1f; // Time the swoosh is visible before the slash lands
    public float sideSlashDamage = 15; // Damage dealt by the side slash
    public GameObject attackCirclePrefab; // The red circle prefab
    public float attackWarningDuration = 1f; // Time the circle is visible before the attack lands
    public float attackRadius = 3f; // Radius of the attack effect
    public int slamDamage = 20; // Damage dealt by the slam attack
    public GameObject verticalSlashPrefab; // The prefab for the vertical slash effect
    public float verticalSlashWarningDuration = 1f; // Time the effect is visible before the slash lands
    public float verticalSlashDamage = 20; // Damage dealt by the vertical slash

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }


    void Update()
    {
        MoveTowardsPlayer();

        if (!isAttacking) // Check if not already performing an attack
        {
        attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                PerformAttackCycle();
                attackTimer = 0f; // Reset timer after an attack is initiated
            }
        }
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

    void PerformAttackCycle()
    {
    switch (attackPhase)
    {
        case 0:
            StartCoroutine(PerformSlamAttack());
            break;
        case 1:
            StartCoroutine(PerformSideSlash());
            break;
        case 2:
            StartCoroutine(PerformVerticalSlash());
            break;
        default:
            break;
    }

    attackPhase = (attackPhase + 1) % 3; // Cycle through the attacks
    }

    IEnumerator PerformSlamAttack()
    {
    isAttacking = true;

    // Calculate direction and position for the attack based on the player's position
    Vector2 attackPosition = player.position - transform.position;
    attackPosition.Normalize();
    attackPosition *= attackRadius; // Place the circle at the edge of the attack radius
    attackPosition += (Vector2)transform.position;

    // Instantiate the attack circle
    GameObject attackCircle = Instantiate(attackCirclePrefab, attackPosition, Quaternion.identity);
    SpriteRenderer circleRenderer = attackCircle.GetComponent<SpriteRenderer>();
    circleRenderer.color = new Color(1, 0, 0, 0.5f); // Make the circle red and semi-transparent

    // Wait for the warning duration
    yield return new WaitForSeconds(attackWarningDuration);

    // Make the circle fully opaque
    circleRenderer.color = new Color(1, 0, 0, 1);

    // Check if player is within attack radius
    if (Vector2.Distance(player.position, transform.position) <= attackRadius)
    {
        // Apply damage to the player
        // Make sure your player has a method to take damage
        player.GetComponent<PlayerHealth>().TakeDamage(slamDamage);
    }

    // Destroy the attack effect after a short delay
    Destroy(attackCircle, 0.5f);

    isAttacking = false;
    }

    IEnumerator PerformSideSlash()
    {
        isAttacking = true;

        // Instantiate the swoosh effects on both sides of the crab
        GameObject leftSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.left * 2, Quaternion.identity);
        GameObject rightSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.right * 2, Quaternion.identity);
        // Instantiate the swoosh effects above and below the crab, rotating them to face up and down
        GameObject upSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.up * 2, Quaternion.Euler(0, 0, 90));
        GameObject downSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.down * 2, Quaternion.Euler(0, 0, -90));

        leftSwoosh.transform.localScale = new Vector3(-1.6f, 1.6f, 1); 

        SpriteRenderer leftRenderer = leftSwoosh.GetComponent<SpriteRenderer>();
        SpriteRenderer rightRenderer = rightSwoosh.GetComponent<SpriteRenderer>();
        SpriteRenderer upRenderer = upSwoosh.GetComponent<SpriteRenderer>();
        SpriteRenderer downRenderer = downSwoosh.GetComponent<SpriteRenderer>();

        // Set the color to make the swoosh semi-transparent
        Color semiTransparentColor = new Color(1, 1, 1, 0.5f);
        leftRenderer.color = semiTransparentColor;
        rightRenderer.color = semiTransparentColor;
        upRenderer.color = semiTransparentColor;
        downRenderer.color = semiTransparentColor;

        // Wait for the warning duration
        yield return new WaitForSeconds(sideSlashWarningDuration);

        // Make the swoosh fully opaque
        Color opaqueColor = new Color(1, 1, 1, 1);
        leftRenderer.color = opaqueColor;
        rightRenderer.color = opaqueColor;
        upRenderer.color = opaqueColor;
        downRenderer.color = opaqueColor;

        // Check for player within damage range on all sides
        if (Vector2.Distance(player.position, transform.position) <= 2f) // Assuming a simple range check for demonstration
        {
            // player.GetComponent<PlayerHealth>().TakeDamage(sideSlashDamage);
        }

        // Destroy the swoosh effects after a short delay
        Destroy(leftSwoosh, 0.5f);
        Destroy(rightSwoosh, 0.5f);
        Destroy(upSwoosh, 0.5f);
        Destroy(downSwoosh, 0.5f);

        isAttacking = false;
    }



    IEnumerator PerformVerticalSlash()
    {
        isAttacking = true;

        // Instantiate the vertical slash effect at the player's current location
        Vector3 playerPosition = player.position; // Capture the player's position at the moment the attack starts
        GameObject verticalSlash = Instantiate(verticalSlashPrefab, playerPosition, Quaternion.identity);

        SpriteRenderer slashRenderer = verticalSlash.GetComponent<SpriteRenderer>();
        slashRenderer.color = new Color(1, 1, 1, 0.5f); // Make slash semi-transparent to serve as a warning

        // Wait for the warning duration before the attack lands
        yield return new WaitForSeconds(verticalSlashWarningDuration);

        // Make the slash fully opaque to indicate the attack is landing
        slashRenderer.color = new Color(1, 1, 1, 1);

        if (Vector2.Distance(player.position, playerPosition) <= 2f) 
        {
            //player.GetComponent<PlayerHealth>().TakeDamage(verticalSlashDamage);
        }

        // Destroy the slash effect after a short delay to clean up
        Destroy(verticalSlash, 0.5f);

        isAttacking = false;
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