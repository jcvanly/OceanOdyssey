using UnityEngine;

public class IceOrb : MonoBehaviour
{
    public float speed = 10f;
    private GameObject player;
    private Rigidbody2D rb;
    private float spawnTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        spawnTime = Time.time;
    }

    void Update()
    {
        if (Time.time - spawnTime >= 5.5f)
        {
            Debug.Log("Destroy projectile");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Check if the orb collides with the player
        {
            // PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            // if (playerMovement != null)
            // {
            //     playerMovement.FreezePlayer(); // Freeze the player
            // }
        }

        Destroy(gameObject); // Destroy the orb after collision
    }
}
