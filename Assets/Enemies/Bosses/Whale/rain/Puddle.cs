using UnityEngine;

public class Puddle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color originalColor; // Store the original color of the puddle
    public Color electrifiedColor = Color.yellow;
    public float speedReduction = 350f; // Amount to reduce speed by
    public float minSpeed = 2f; // Minimum speed the player can be reduced to
    public int damage = 10; // Damage to deal to the player
    private bool isElectrified = false; // Flag to check if the puddle is electrified

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Save the original color of the puddle
    }

    public void Electrify()
    {
        isElectrified = true;
        spriteRenderer.color = electrifiedColor;
        // Revert back to the original color after 1 second
        Invoke(nameof(RevertColor), 1f);
    }

    void RevertColor()
    {
        spriteRenderer.color = originalColor;
        isElectrified = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Reduce the player's speed
                playerMovement.moveSpeed = Mathf.Max(playerMovement.moveSpeed - speedReduction, minSpeed);
                
                // If the puddle is electrified, deal damage
                if (isElectrified)
                {
                    // Assuming the player has a method to take damage
                    // Replace 'PlayerHealth' with the actual component name that handles health
                    collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Reset the player's speed to original value
                playerMovement.moveSpeed += speedReduction;
            }
        }
    }
}
