using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//hi
public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}