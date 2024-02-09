using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 16f; // Speed of the bullet
    public float bulletRange = 15f; // Maximum range of the bullet in pixels

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Calculate the target position based on the bullet range
        Vector2 targetPosition = firePoint.position + firePoint.right * bulletRange;

        // Calculate the direction to the target position
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;

        // Set the bullet velocity
        rb.velocity = direction * bulletSpeed;

        // Destroy the bullet after reaching the specified range
        Destroy(bullet, bulletRange / bulletSpeed);
    }
}
