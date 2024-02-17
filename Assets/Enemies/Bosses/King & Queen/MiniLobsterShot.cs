using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniLobsterShot : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float speed = 6f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * speed;
        RotateTowardsPlayer(); // Rotate the spear to face the player at the start
    }

    void RotateTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = player.transform.position - transform.position;
            // Calculate the angle in radians and convert to degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
