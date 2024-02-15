using UnityEngine;
using System.Collections;

public class LobsterQueen : MonoBehaviour
{
    public int maxHealth = 40;
    public int currentHealth;
    public LobsterHealthBar healthBar;
    public float speed = 2f; // Speed at which the LobsterQueen moves
    public Transform[] waypoints; // Array to hold the Transform of the 4 waypoints
    public float stopTime = 1f; // Time to stop at each waypoint
    private int currentWaypointIndex = 0; // Index to track the current target waypoint
    private bool isMoving = true; // Flag to control movement
    public GameObject miniLobsterPrefab; // Reference to the mini lobster prefab
    public Transform spawnPoint; // The point from where mini lobsters will be spawned



    void Start()
    {
        currentHealth = maxHealth;
        if (waypoints.Length < 4)
        {
            Debug.LogError("Insufficient waypoints set for LobsterQueen. Please set 4 waypoints.");
        }
        else
        {
            StartCoroutine(MoveToNextWaypoint());
        }
        
            StartCoroutine(SpawnMiniLobster()); // Start spawning mini lobsters

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

    IEnumerator SpawnMiniLobster()
    {
        while (true) // Continuously spawn mini lobsters every 5 seconds
        {
            yield return new WaitForSeconds(3.5f); // Wait for 5 seconds
            Instantiate(miniLobsterPrefab, spawnPoint.position, Quaternion.identity); // Spawn a mini lobster
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
        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("LobsterHealth");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
