using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//hi
public class Bullet : MonoBehaviour
{
    public int damage = 10;
    void Start()
    {
        Destroy(gameObject, 2);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Enemy enemy = collision.GetComponent<Enemy>();
        // if(enemy != null)
        // {
        //     enemy.TakeDamage(damage);
        // }

        Debug.Log("Bullet collided with: " + collision.gameObject.name);
        Destroy(gameObject);
    }

    

 
}
