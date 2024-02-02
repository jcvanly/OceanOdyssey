using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenBehavior : MonoBehaviour
{
    
    public GameObject tentaclePrefab; // Reference to your tentacle prefab
    public float tentacleShootInterval = 6f; // Interval for shooting tentacles
     public float tentacleLength = 5f; // How far the tentacle should extend
    public float tentacleDuration = 5f; // How long the tentacle stays extended
    public int numberOfTentacles = 4; // Number of tentacles to shoot
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public int maxHealth = 100;
    public int currentHealth;
    private float shootTimer;
    private Transform playerTransform;
    public int numberOfShots = 7; // Number of shots in the beam
    public float timeBetweenShots = 0.1f; // Time between each shot in the beam
    private float tentacleShootTimer;


    void Start()
    {
        shootTimer = shootInterval;
        tentacleShootTimer = tentacleShootInterval; // Initialize the tentacle shoot timer
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        tentacleShootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            StartCoroutine(ShootBeam());
            shootTimer = shootInterval;
        }

        if (tentacleShootTimer <= 0)
        {
            StartCoroutine(ShootTentacles());
            tentacleShootTimer = tentacleShootInterval;
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

    IEnumerator ShootTentacles()
    {
        Vector2[] directions = new Vector2[]
        {
            Vector2.up, // North
            Vector2.right, // East
            Vector2.down, // South
            Vector2.left // West
        };

        foreach (var direction in directions)
        {
            ShootTentacleInDirection(direction);
        }

        yield return new WaitForSeconds(tentacleDuration);

        // Optionally retract tentacles here if needed
    }

    void ShootTentacleInDirection(Vector2 direction)
    {
        StartCoroutine(SpawnAndRetractTentacleSequence(direction));
    }

    IEnumerator SpawnAndRetractTentacleSequence(Vector2 direction)
{
    List<GameObject> tentacles = new List<GameObject>();

    // Assuming each segment is 1 unit long for example, adjust based on your prefab size
    float segmentLength = 1.0f; // You should adjust this based on the actual size of your tentacle prefab
    int segmentsNeeded = Mathf.CeilToInt(tentacleLength / segmentLength); // Calculate how many segments are needed based on the desired tentacle length

    for (int i = 0; i < segmentsNeeded; i++)
    {
        // Calculate position for each segment considering the segment size
        Vector3 positionOffset = direction * segmentLength * i;
        Vector3 segmentPosition = transform.position + positionOffset;

        GameObject tentacle = Instantiate(tentaclePrefab, segmentPosition, Quaternion.identity);
        // Adjust the rotation to match the direction of the tentacle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tentacle.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        tentacles.Add(tentacle);
        // Optionally, add a small delay here if you want each segment to appear one after another
    }

    yield return new WaitForSeconds(tentacleDuration);

    // Retract tentacles starting from the last one
    for (int i = tentacles.Count - 1; i >= 0; i--)
    {
        Destroy(tentacles[i]);
        yield return new WaitForSeconds(0.1f); // Delay between destroying each segment to simulate retraction
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