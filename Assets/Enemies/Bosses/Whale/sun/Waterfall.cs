using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    public float speed = 2f;
    private GameObject player;
    private Rigidbody2D rb;
    private float spawnTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        // Calculate direction to the player
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = direction.normalized * speed;

        // Calculate the angle to rotate towards the player
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Assuming that the bottom of the waterfall is initially facing right (0 degrees),
        // and you want it to face towards the player:
        // Subtract 90 degrees if the bottom of the waterfall object faces downwards when angle is 0
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);

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
        Destroy(gameObject); // Destroy the object after collision
    }
}
