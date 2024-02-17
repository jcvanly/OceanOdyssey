using UnityEngine;

public class MiniLobster : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign in inspector
    public float shootingCooldown = 2f; // Time between shots
    private float shootingTimer;
    private Transform playerTransform;
    public int maxHealth = 30; // Max health of the enemy
    public int currentHealth; // Current health of the enemy
    private EnemyDeath enemyDeath; // Reference to the EnemyDeath component
    private bool isDead = false;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        shootingTimer = shootingCooldown; // Start with the ability to shoot immediately
        currentHealth = maxHealth; // Initialize current health
        enemyDeath = GetComponent<EnemyDeath>();
    }

    void Update()
    {
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0)
        {
            ShootAtPlayer();
            shootingTimer = shootingCooldown;
        }
    }

    void ShootAtPlayer()
    {
        if (playerTransform != null && projectilePrefab != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f; // Adjust speed as needed
            
            // Optional: Adjust projectile rotation to face the player
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
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
        if (isDead == false)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                enemyDeath.Die();
                isDead = true;
            }
        }
    }

    
}
