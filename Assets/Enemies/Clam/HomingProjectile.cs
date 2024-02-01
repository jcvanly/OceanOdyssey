using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private Vector3 direction;

    void Update()
    {
        // Move the projectile in the specified direction
        transform.Translate(direction * Time.deltaTime, Space.World);
    }

    public void SetDirection(Vector3 newDirection)
    {
        // Set the direction of the homing attack
        direction = newDirection.normalized;
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
