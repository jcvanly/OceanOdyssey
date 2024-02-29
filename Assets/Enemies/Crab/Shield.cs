using UnityEngine;

public class Shield : MonoBehaviour
{
    private GameObject player;
    private GameObject crab; // Reference to the crab
    private Rigidbody2D rb;

    public float speed = 4f;
    public float maxDistance = 10f; // Max distance before returning
    private float distanceTraveled = 0f; // Track the distance traveled
    private Vector3 lastPosition;
    public float acceleration = 1f; // Acceleration towards the crab when returning

    private bool isReturning = false; // Flag to control the return logic
    private float lifetime = 1f; // Lifetime of the shield in seconds
    private float timeSinceSpawn; // Track how long the shield has been active

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        crab = GameObject.FindGameObjectWithTag("Crab"); // Ensure your crab GameObject has the "Crab" tag

        Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = directionToPlayer * speed;

        lastPosition = transform.position;
        timeSinceSpawn = Time.time; // Initialize the spawn time
    }

    void Update()
    {
        distanceTraveled += Vector3.Distance(transform.position, lastPosition);
        lastPosition = transform.position;

        if (!isReturning && distanceTraveled > maxDistance)
        {
            StartReturn();
        }
        else if (isReturning)
        {
            Vector2 directionToCrab = (crab.transform.position - transform.position).normalized;
            rb.velocity += directionToCrab * acceleration;
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, speed);
        }

        // Destroy the shield if it has been active for more than 4 seconds
        if (Time.time - timeSinceSpawn > lifetime)
        {
            Destroy(gameObject);
        }
    }

    void StartReturn()
    {
        isReturning = true;
        rb.velocity = Vector2.zero; // Stop the projectile momentarily
        Vector2 returnDirection = (crab.transform.position - transform.position).normalized;
        rb.velocity = returnDirection * (speed * 0.5f); // Start returning at a reduced speed
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision specifically with the crab
        if (collision.gameObject == crab)
        {
            Destroy(gameObject); // Destroy the shield upon collision with the crab
        }
    }
}
