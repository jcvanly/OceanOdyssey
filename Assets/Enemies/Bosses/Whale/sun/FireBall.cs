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
        if(currTime - spawnTime >= 3f)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
