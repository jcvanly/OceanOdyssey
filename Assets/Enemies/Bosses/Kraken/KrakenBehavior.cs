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
     public float inkSpotShootInterval = 10f; // Interval for shooting ink spots
    private float inkSpotTimer;
    public GameObject inkSpotPrefab; // Reference to the Ink Spot prefab
    public HealthBar healthBar;





void Start()
    {
        shootTimer = shootInterval;
        tentacleShootTimer = tentacleShootInterval;
        inkSpotTimer = inkSpotShootInterval; // Initialize the ink spot timer
        currentHealth = maxHealth;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }


void Update()
    {
        shootTimer -= Time.deltaTime;
        tentacleShootTimer -= Time.deltaTime;
        inkSpotTimer -= Time.deltaTime;

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

        if (inkSpotTimer <= 0)
        {
            StartCoroutine(ShootInkSpot());
            inkSpotTimer = inkSpotShootInterval;
        }
    }


    IEnumerator ShootInkSpot()
{
    Vector3 targetPosition = playerTransform.position; // Target position is current player position
    GameObject inkSpot = Instantiate(inkSpotPrefab, transform.position, Quaternion.identity); // Instantiate at Kraken's position

    // Assuming the ink spot moves directly towards the player's current position
    float speed = 5f; // Adjust speed as necessary
    while (inkSpot.transform.position != targetPosition)
    {
        if (inkSpot == null) yield break; // Exit if ink spot was destroyed for some reason
        inkSpot.transform.position = Vector3.MoveTowards(inkSpot.transform.position, targetPosition, speed * Time.deltaTime);
        yield return null;
    }

    // Do not destroy the ink spot immediately after reaching the target. Wait for 5 seconds.
    yield return new WaitForSeconds(5f); // Wait for 5 seconds

    // Destroy the ink spot after 5 seconds without fading out
    if (inkSpot != null)
    {
        Destroy(inkSpot);
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
        yield return new WaitForSeconds(0.05f); // Adjust this to control the speed of extension
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
        healthBar.UpdateHealthBar(currentHealth, maxHealth); // This method needs to be implemented in your HealthBar script

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        
        // Destroy all ink spots. Assuming you have a tag "InkSpot" for all ink spot objects
        foreach (GameObject inkSpot in GameObject.FindGameObjectsWithTag("InkSpot"))
        {
            Destroy(inkSpot);
        }

        foreach (GameObject tentacle in GameObject.FindGameObjectsWithTag("Tentacle"))
        {
            Destroy(tentacle);
        }

        GameObject healthBarGameObject = GameObject.FindGameObjectWithTag("HealthBar");
        if (healthBarGameObject != null) {
            healthBarGameObject.SetActive(false);
        } else {
            Debug.LogError("HealthBar GameObject not found. Make sure it's tagged correctly.");
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }

}