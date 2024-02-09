using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);

        // Get a reference to the Canvas GameObject
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            // Search for the TextMeshPro component within the Canvas GameObject and its children
            TextMeshProUGUI notificationText = canvas.GetComponentInChildren<TextMeshProUGUI>();

            if (notificationText != null)
            {
                // Apply the power-up effect
                powerupEffect.Apply(collision.gameObject, notificationText);
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found within the Canvas GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Canvas GameObject not found in the scene.");
        }
    }
}
