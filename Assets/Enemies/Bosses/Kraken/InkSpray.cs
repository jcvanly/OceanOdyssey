using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkSpray : MonoBehaviour
{
    public float speedReduction = 350f; // Amount to reduce speed by
    public float minSpeed = 2f; // Minimum speed the player can be reduced to

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Make sure the player has a tag "Player"
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                // Reduce speed but ensure it doesn't go below a minimum speed
                //change to - when not testing
                playerMovement.moveSpeed = Mathf.Max(playerMovement.moveSpeed - speedReduction, minSpeed);
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
                // Reset speed to original value
                //change this to + when not testing
                playerMovement.moveSpeed += speedReduction;
            }
        }
    }
}
