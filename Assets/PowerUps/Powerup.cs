using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private AudioSource pickupSound;

    private void Update()
    {
        // Find the GameObject with the "shootingsound" tag and get its AudioSource component
        GameObject soundObject = GameObject.FindWithTag("pickupsound");
        if (soundObject != null)
        {
            pickupSound = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'pickupsound' found.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            TextMeshProUGUI notificationText = canvas.GetComponentInChildren<TextMeshProUGUI>();

            if (notificationText != null)
            {
                // Apply the power-up effect
                powerupEffect.Apply(collision.gameObject, notificationText);

                // Play the shooting sound effect if AudioSource is found
                if (pickupSound != null)
                {
                    pickupSound.Play();
                }
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