using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelShot : MonoBehaviour
{
    private float spawnTime;
    private float currTime;
    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        currTime = Time.time;
        if(currTime - spawnTime >= 2f)
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