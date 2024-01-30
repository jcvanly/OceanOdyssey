using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}
