using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private AudioSource soundEffects; 
    public AudioClip sound;

    public float bulletSpeed = 25f;
    public float bulletRange = 15f;
    public float shootCooldown = 0.03f;
    private float shootTimer = 0f;


    void Update()
    {
        GameObject soundObject = GameObject.FindWithTag("SoundEffects");
        if (soundObject != null)
        {
            soundEffects = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'SoundEffects' found.");
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

        if (soundEffects != null)
        {
            soundEffects.PlayOneShot(sound);
        }
    }
}