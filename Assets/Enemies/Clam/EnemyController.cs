using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject homingProjectilePrefab;
    public float projectileSpeed = 5f;
    public float cooldownTime = 2f; // Adjust this value based on your desired cooldown time

    private float lastShootTime;

    void Update()
    {
        // Check if enough time has passed since the last shot
        if (Time.time - lastShootTime >= cooldownTime)
        {
            // Check if the player is in sight, then shoot a homing attack
            if (PlayerIsInSight())
            {
                ShootHomingAttack();
            }
        }
    }

    void ShootHomingAttack()
    {
        // Update the last shoot time
        lastShootTime = Time.time;

        // Instantiate homing projectile
        GameObject homingProjectile = Instantiate(homingProjectilePrefab, transform.position, Quaternion.identity);

        // Get direction towards the player
        Vector3 direction = (PlayerPosition() - transform.position).normalized;

        // Set the homing attack's direction
        homingProjectile.GetComponent<HomingProjectile>().SetDirection(direction * projectileSpeed);
    }

    bool PlayerIsInSight()
    {
        // Implement your logic to check if the player is in sight
        // You can use raycasting or other methods for this check
        return true; // Placeholder, replace with your actual implementation
    }

    Vector3 PlayerPosition()
    {
        // Implement your logic to get the player's position
        // For example, you can use GameObject.FindWithTag("Player").transform.position
        return Vector3.zero; // Placeholder, replace with your actual implementation
    }
}
