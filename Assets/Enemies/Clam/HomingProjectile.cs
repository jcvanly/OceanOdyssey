using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private Transform playerTransform;
    public float homingSpeed = 5f;

    void Start()
    {
        // Find the player's transform
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // Destroy the projectile after a certain time (adjust as needed)
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Calculate the direction towards the player
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;

        // Smoothly rotate towards the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, homingSpeed * Time.deltaTime);

        // Move the projectile forward
        transform.Translate(Vector3.forward * Time.deltaTime * homingSpeed, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        // Handle collision with player or other objects
        if (other.CompareTag("Player"))
        {
            // Implement your logic for dealing damage to the player
            Destroy(gameObject);
        }
    }
}
