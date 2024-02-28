using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class LobsterQueen : MonoBehaviour
{
    public bool LobsterIsAlive = true;
    public int maxHealth = 40;
    public int currentHealth;
    public LobsterHealthBar healthBar;
    public float speed = 2f; // Speed at which the LobsterQueen moves
    public Transform[] waypoints; // Array to hold the Transform of the 4 waypoints
    public float stopTime = 1f; // Time to stop at each waypoint
    private int currentWaypointIndex = 0; // Index to track the current target waypoint
    public GameObject miniLobsterPrefab; // Reference to the mini lobster prefab
    public Transform spawnPoint; // The point from where mini lobsters will be spawned
     public Image fadePanel;
    public float fadeDuration = 2f;
    public GameObject victoryText;
    public GameObject nextIslandButton;
    public GameObject projectilePrefab; // Reference to the projectile prefab
    public Transform playerTransform; // To aim at the player
    public float projectileSpeed = 10f;
    public float alternateTime = 3f;
    private SpriteRenderer spriteRenderer; 

    



    void Start()
    {
    spriteRenderer = GetComponent<SpriteRenderer>();

    currentHealth = maxHealth;
    if (waypoints.Length < 4)
    {
        Debug.LogError("Insufficient waypoints set for LobsterQueen. Please set 4 waypoints.");
    }
    else
    {
        StartCoroutine(MoveToNextWaypoint());
    }
    
    StartCoroutine(AlternateAttack()); // Start alternating between spawning and shooting
    }

    void Update()
    {
        CheckKingCrabStatus();

    }


    IEnumerator MoveToNextWaypoint()
    {
        while (true) // Ensures continuous execution
        {
            // Find the current waypoint's position
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            // Move towards the current target waypoint
            while (Vector2.Distance(transform.position, targetWaypoint.position) > 0.1f)
            {
                
                Vector2 newPosition = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
                transform.position = newPosition;
                yield return null; // Wait until next frame
            }
            // Wait at the waypoint
            yield return new WaitForSeconds(stopTime);

            // Update the current waypoint index to the next waypoint, cycling back to 0 if at the end of the array
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    IEnumerator AlternateAttack()
    {
    bool spawnMiniLobsterNext = true; // Flag to alternate actions

    while (LobsterIsAlive) // Continue as long as the Lobster Queen is alive
    {
        yield return new WaitForSeconds(alternateTime); // Wait for 3 seconds between actions

        if (spawnMiniLobsterNext == true)
        {
            Instantiate(miniLobsterPrefab, spawnPoint.position, Quaternion.identity); // Spawn a mini lobster
        }
        else
        {
            ShootProjectileAtPlayer(); // Shoot a projectile at the player
        }

        Debug.Log(spawnMiniLobsterNext);

        spawnMiniLobsterNext = !spawnMiniLobsterNext; // Toggle the flag to alternate the action next time
    }
    }

    void ShootProjectileAtPlayer()
    {
    
        // Debug.Log("shooting projectile");
        // GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        // Vector2 direction = (playerTransform.position - spawnPoint.position).normalized;
        // projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed; // Set the speed of your projectile here
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(1, 1).normalized * speed;

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
        // Existing code to deactivate the health bar and set the lobster as dead
        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("LobsterHealth");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }

        GlobalEnemyManager.LobsterDead = true;

        if (GlobalEnemyManager.CrabAndLobsterDead()) {
            Debug.Log("lobster showing black screen");
            ShowVictoryScreen();
            StartCoroutine(FadeToBlack());
            DestroyAllMiniLobsters();
        } else {
            DestroyAllMiniLobsters(); // Destroy all mini lobsters
            Destroy(gameObject); // Destroy the Lobster Queen
        }
    }

    void DestroyAllMiniLobsters()
    {
        GameObject[] miniLobsters = GameObject.FindGameObjectsWithTag("MiniLobster");
        foreach (GameObject miniLobster in miniLobsters)
        {
            Destroy(miniLobster);
        }
    }
    public void ShowVictoryScreen()
    {
        victoryText.SetActive(true);
        nextIslandButton.SetActive(true);
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

    public void CheckKingCrabStatus()
    {
        if (GlobalEnemyManager.CrabDead == true)
        {
            // Apply red tint
            spriteRenderer.color = Color.red;
    
            speed = 8f; 
 
            stopTime = .25f;

            projectileSpeed = 15f;
            alternateTime = 1f;


        }
    }
    
}
