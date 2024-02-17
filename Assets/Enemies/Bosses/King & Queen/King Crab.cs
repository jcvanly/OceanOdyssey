using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KingCrab : MonoBehaviour
{
    public bool CrabIsAlive = true;
    private Rigidbody2D rb;
    public int maxHealth = 300;
    public int currentHealth;
    public CrabHealthBar healthBar;
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 5f; // Movement speed of the crab
    public float stoppingDistance = 3f; // Distance at which the crab stops moving towards the player
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
    public int slamDamage = 1; // Damage dealt by the slam attack
    public GameObject verticalSlashPrefab; // The prefab for the vertical slash effect
    public float verticalSlashWarningDuration = 1f; // Time the effect is visible before the slash lands
    public float verticalSlashDamage = 20; // Damage dealt by the vertical slash
    public PlayerHealth playerHealth;
    public Image fadePanel;
    public float fadeDuration = 2f;
    public GameObject victoryText;
    public GameObject nextIslandButton;
    public float enragedAttackInterval = 0.5f;
    private SpriteRenderer spriteRenderer; // To change the color of the crab
    public float enragedMoveSpeed = 15f; // Increased move speed when enraged

    public float enragedStoppingDistance = 2f; // Distance at which the crab stops moving towards the player
    public float enragedAttackDistance = 2f; // Distance within which the crab will attack the player
    


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        HideVictoryScreen();
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
    }


    void Update()
    {
        MoveTowardsPlayer();

        CheckLobsterQueenStatus();

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
    if (!CrabIsAlive) yield break; // Add this line

    isAttacking = true;

    // Calculate direction and position for the attack based on the player's position
    Vector2 attackPosition = player.position - transform.position;
    attackPosition.Normalize();
    attackPosition *= attackRadius; // Place the circle at the edge of the attack radius
    attackPosition += (Vector2)transform.position;

    // Instantiate the attack circle
    GameObject attackCircle = Instantiate(attackCirclePrefab, attackPosition, Quaternion.identity);
    SpriteRenderer circleRenderer = attackCircle.GetComponent<SpriteRenderer>();
    AttackCircle attackCircleScript = attackCircle.GetComponent<AttackCircle>(); // Ensure you have this component attached to the prefab

    // Make the circle red and semi-transparent initially
    circleRenderer.color = new Color(1, 0, 0, 0.5f);
    
    // Wait for the warning duration
    yield return new WaitForSeconds(attackWarningDuration);

    // Make the circle fully opaque to signal the attack is about to hit
    circleRenderer.color = new Color(1, 0, 0, 1);

    // Activate the attack circle to start detecting the player and dealing damage
    if (attackCircleScript != null) {
        attackCircleScript.Activate();
    } else {
        Debug.LogError("AttackCircle script not found on the attack circle prefab!");
    }

    // Give some time for the player to potentially be hit
    yield return new WaitForSeconds(0.5f);

    // Destroy the attack effect
    Destroy(attackCircle);

    isAttacking = false;
}


IEnumerator PerformSideSlash()
{
    if (!CrabIsAlive) yield break; // Add this line

    isAttacking = true;

    // Instantiate the swoosh effects on all sides of the crab
    GameObject leftSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.left * 2, Quaternion.identity);
    GameObject rightSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.right * 2, Quaternion.identity);
    GameObject upSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.up * 2, Quaternion.Euler(0, 0, 90));
    GameObject downSwoosh = Instantiate(swooshPrefab, transform.position + Vector3.down * 2, Quaternion.Euler(0, 0, -90));

    // Mirror the left swoosh by scaling it negatively on the x-axis
    leftSwoosh.transform.localScale = new Vector3(-1.6f, 1.6f, 1); // Adjusted scale for mirroring

    GameObject[] swooshes = new GameObject[] { leftSwoosh, rightSwoosh, upSwoosh, downSwoosh };

    foreach (GameObject swoosh in swooshes)
    {
        SpriteRenderer renderer = swoosh.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 0.5f); // Make swoosh semi-transparent to serve as a warning
        AttackSide attackSideScript = swoosh.AddComponent<AttackSide>(); // Add AttackSide script dynamically
        attackSideScript.player = player; 
        attackSideScript.playerHealth = playerHealth; 
    }

    // Wait for the warning duration
    yield return new WaitForSeconds(sideSlashWarningDuration);

    // Make the swoosh fully opaque and activate them for damage detection
    foreach (GameObject swoosh in swooshes)
    {
        SpriteRenderer renderer = swoosh.GetComponent<SpriteRenderer>();
        renderer.color = new Color(1, 1, 1, 1); // Make swoosh fully opaque
        AttackSide attackSideScript = swoosh.GetComponent<AttackSide>();
        if (attackSideScript != null)
        {
            attackSideScript.Activate();
        }
    }

    // Destroy the swoosh effects after a short delay
    foreach (GameObject swoosh in swooshes)
    {
        Destroy(swoosh, 0.5f);
    }

    isAttacking = false;
}

    IEnumerator PerformVerticalSlash()
    {
        if (!CrabIsAlive) yield break; // Add this line

        isAttacking = true;

        // Instantiate the vertical slash effect at the player's current location
        Vector3 playerPosition = player.position; // Capture the player's position at the moment the attack starts
        GameObject verticalSlash = Instantiate(verticalSlashPrefab, playerPosition, Quaternion.identity);

        SpriteRenderer slashRenderer = verticalSlash.GetComponent<SpriteRenderer>();
        // Make slash semi-transparent to serve as a warning
        slashRenderer.color = new Color(1, 1, 1, 0.5f);

        // Access the AttackSlash script on the vertical slash prefab
        AttackSlash attackSlashScript = verticalSlash.GetComponent<AttackSlash>();

        // Wait for the warning duration before the attack lands
        yield return new WaitForSeconds(verticalSlashWarningDuration);

        // Make the slash fully opaque to indicate the attack is landing
        slashRenderer.color = new Color(1, 1, 1, 1);

        // Activate the slash to start detecting the player and dealing damage
        if (attackSlashScript != null) {
            attackSlashScript.Activate();
        } else {
            Debug.LogError("AttackSlash script not found on the vertical slash prefab!");
        }

        // Wait a bit to give time for the player to potentially be hit
        yield return new WaitForSeconds(0.5f);

        // Destroy the slash effect after a short delay to clean up
        Destroy(verticalSlash);

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
        CrabIsAlive = false; // Ensure this flag is set to false
    
        StopAllCoroutines();

        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("CrabHealth");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }


        GlobalEnemyManager.CrabDead = true;

        if (GlobalEnemyManager.CrabAndLobsterDead()) {
            ShowVictoryScreen();
            StartCoroutine(FadeToBlack());
        } else {
            Destroy(gameObject);
        }
    }

    public void ShowVictoryScreen()
    {
        victoryText.SetActive(true);
        nextIslandButton.SetActive(true);
    }

    public void HideVictoryScreen()
    {
        victoryText.SetActive(false);
        nextIslandButton.SetActive(false);
    }

    public IEnumerator FadeToBlack()
    {
        Debug.Log("fade to black");
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            panelColor.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            Debug.Log(panelColor.a + elapsedTime);
            fadePanel.color = panelColor;
            yield return null;
        }

        ShowVictoryScreen();
        // Destroy the enemy object
        Destroy(gameObject);  
        
    }

    void CheckLobsterQueenStatus()
    {
        if (GlobalEnemyManager.LobsterDead == true)
        {
            // Apply red tint
            spriteRenderer.color = Color.red;

            // Increase move speed and decrease attack interval
            attackInterval = enragedAttackInterval;
            attackDistance = enragedAttackDistance;

            attackWarningDuration = 0.6f;
            sideSlashWarningDuration = 0.25f;
            verticalSlashWarningDuration = 0.4f;
        }
    }
}