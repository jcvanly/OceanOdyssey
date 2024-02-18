using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolShrimpShot : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;

    public float speed = 10f;
    private float spawnTime;
    private float currTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x,direction.y).normalized * speed;
        spawnTime = Time.time;
    }

    void Update()
    {
        currTime = Time.time;
        if(currTime - spawnTime >= 5.5f)
        {
                Debug.Log("destroy projectile");
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
