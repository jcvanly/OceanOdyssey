using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
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
        Destroy(gameObject); // Destroy the orb after collision
    }
}
