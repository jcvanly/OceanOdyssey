using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    private float shootTimer;

    void Start()
    {
        shootTimer = shootInterval;
    }

    void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        // Shooting in the four cardinal directions
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.up * 5f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.down * 5f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f;
        Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = Vector2.right * 5f;
    }
}
