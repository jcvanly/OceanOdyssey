using UnityEngine;

public class AttackCircle : MonoBehaviour
{
    public bool isActive = false; // Use this to activate the damage check after the circle becomes opaque
    public PlayerHealth playerHealth;
    public Transform player; // Reference to the player's transform
    public float damageCooldown = 1.0f; // Seconds between damages
    private float lastDamageTime = -1.0f;



    private void OnTriggerStay2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player") && (lastDamageTime < 0 || Time.time - lastDamageTime >= damageCooldown))
        {
            // Damage the player
            lastDamageTime = Time.time;
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Adjust damage value as necessary
            }
        }
    }

    // Call this method to activate the damage check
    public void Activate()
    {
        isActive = true;
    }
}
