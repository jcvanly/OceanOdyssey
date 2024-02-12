using UnityEngine;

public class LobsterQueen : MonoBehaviour
{
    public int maxHealth = 40;
    public int currentHealth;
    public LobsterHealthBar healthBar;
    public float speed = 2f; // Speed at which the LobsterQueen moves

    private Vector2[] points = new Vector2[4]; // Array to hold the 4 corner points
    private int currentPointIndex = 0; // Index to track the current target point

    void Start()
    {
        currentHealth = maxHealth;

        points[0] = new Vector2(-7, 4); // Top left
        points[1] = new Vector2(7, 4);  // Top right
        points[2] = new Vector2(7, -4); // Bottom right
        points[3] = new Vector2(-7, -4); // Bottom left
    }

    void Update()
    {
        MoveInRectangle();
    }

    void MoveInRectangle()
    {
        // Move towards the current target point
        transform.position = Vector2.MoveTowards(transform.position, points[currentPointIndex], speed * Time.deltaTime);

        // Check if the LobsterQueen has reached the current target point
        if (Vector2.Distance(transform.position, points[currentPointIndex]) < 0.1f)
        {
            // Move to the next point
            currentPointIndex = (currentPointIndex + 1) % points.Length;
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
