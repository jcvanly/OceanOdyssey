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
    public GameObject inkSprayPrefab; // Reference to the Ink Spray prefab
    public float inkSprayInterval = 5f; // Interval for spraying ink
    private float inkSprayTimer;




    void Start()
{
    shootTimer = shootInterval;
    tentacleShootTimer = tentacleShootInterval;
    inkSprayTimer = inkSprayInterval; // Initialize the ink spray timer
    currentHealth = maxHealth;
    playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
}


    void Update()
{
    shootTimer -= Time.deltaTime;
    tentacleShootTimer -= Time.deltaTime;
    inkSprayTimer -= Time.deltaTime;

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

    if (inkSprayTimer <= 0)
    {
        StartCoroutine(SprayInkOverPlayer());
        inkSprayTimer = inkSprayInterval;
    }
}


    IEnumerator SprayInkOverPlayer()
{
    GameObject inkSpray = Instantiate(inkSprayPrefab, playerTransform.position + Vector3.up * 2, Quaternion.identity); 
    inkSpray.transform.localScale = new Vector3(5f, 5f, 1f); 
    yield return null; 
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

    }

    void ShootTentacleInDirection(Vector2 direction)
    {
        StartCoroutine(SpawnAndRetractTentacleSequence(direction));
    }

    IEnumerator SpawnAndRetractTentacleSequence(Vector2 direction)
{
    List<GameObject> tentacles = new List<GameObject>();

    // Determine the actual size of the tentacle prefab for accurate placement
    float segmentLength = 1.0f; // Adjust this value based on your prefab's actual dimensions
    int segmentsNeeded = Mathf.CeilToInt(tentacleLength / segmentLength);

    for (int i = 0; i < segmentsNeeded; i++)
    {
        Vector3 positionOffset = direction.normalized * segmentLength * i;
        Vector3 segmentPosition = transform.position + positionOffset;

        GameObject tentacle = Instantiate(tentaclePrefab, segmentPosition, Quaternion.identity);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tentacle.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        tentacles.Add(tentacle);

        // Introduce a delay to create the effect of the tentacle extending
        yield return new WaitForSeconds(0.05f); // Adjust this delay to control the speed of extension
    }

    // Wait for the tentacle to stay extended for the duration
    yield return new WaitForSeconds(tentacleDuration);

    // Retract tentacles starting from the last one
    for (int i = tentacles.Count - 1; i >= 0; i--)
    {
        Destroy(tentacles[i]);
        // Introduce a delay between destroying each segment to simulate retraction
        yield return new WaitForSeconds(0.05f); // Adjust this delay to control the speed of retraction
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