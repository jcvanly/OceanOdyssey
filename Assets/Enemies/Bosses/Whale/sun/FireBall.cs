using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
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
        if(currTime - spawnTime >= 5f)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
