using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private AudioSource shootingSound; // Change to private, as it will be assigned dynamically

    public float bulletSpeed = 25f;
    public float bulletRange = 15f;
    public float shootCooldown = 0.03f;
    private float shootTimer = 0f;

    void Start()
    {
        // Find the GameObject with the "shootingsound" tag and get its AudioSource component
        GameObject soundObject = GameObject.FindWithTag("shooting sound");
        if (soundObject != null)
        {
            shootingSound = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'shooting sound' found.");
        }
    }

    void Update()
    {
        // Find the GameObject with the "shootingsound" tag and get its AudioSource component
        GameObject soundObject = GameObject.FindWithTag("shooting sound");
        if (soundObject != null)
        {
            shootingSound = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'shooting sound' found.");
        }

        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire1") && shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 targetPosition = firePoint.position + firePoint.right * bulletRange;
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
        Destroy(bullet, bulletRange / bulletSpeed);

        // Play the shooting sound effect if AudioSource is found
        if (shootingSound != null)
        {
            shootingSound.Play();
        }
    }
}
