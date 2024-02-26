using UnityEngine;

public class Puddle : MonoBehaviour
{
        private SpriteRenderer spriteRenderer;
        public Color originalColor; // Store the original color of the puddle
        public Color electrifiedColor = Color.yellow;
        public float speedReduction = 350f; // Amount to reduce speed by
        public float minSpeed = 2f; // Minimum speed the player can be reduced to
        private bool isElectrified = false; // Flag to check if the puddle is electrified
        public PlayerHealth playerHealth;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            originalColor = spriteRenderer.color; // Save the original color of the puddle
        }

        public void Electrify()
        {
            isElectrified = true;
            spriteRenderer.color = electrifiedColor;
            Invoke(nameof(RevertColor), 1f);
        }

        void RevertColor()
        {
            spriteRenderer.color = originalColor;
            isElectrified = false;
        }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isElectrified)
        {
            PlayerHealth healthComponent = collision.gameObject.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(1); // Adjust damage value as necessary
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
                
        }
    }
}
