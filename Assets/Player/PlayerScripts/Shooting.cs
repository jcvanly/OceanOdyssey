using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletSpeed = 25f;
    public float bulletRange = 15f;
    public float shootCooldown = 0.03f;
    private float shootTimer = 0f;

    AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {

        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        // Change this to GetButton to allow for auto-shooting while the mouse button is held down
        if (Input.GetButton("Fire1") && shootTimer <= 0)
        {
            Shoot();
            shootTimer = shootCooldown; // Reset the timer after each shot
        }
        

    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed;
        Destroy(bullet, bulletRange / bulletSpeed);

        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.shootNoise);
        }
    }
}