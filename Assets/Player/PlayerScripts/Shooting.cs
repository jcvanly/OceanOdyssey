using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 25f; // Speed of the bullet
    public float bulletRange = 15f; // Maximum range of the bullet in pixels
    public float shootCooldown = 0.03f; // Cooldown time between shots in seconds
    private float shootTimer = 0f; // Timer to track cooldown between shots

    // Update is called once per frame
    void Update()
    {
        // Decrement the shootTimer
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }

        // Check if the shoot button is pressed and if shootTimer is less than or equal to 0
        if (Input.GetButtonDown("Fire1") && shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootCooldown; // Reset the shootTimer
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
