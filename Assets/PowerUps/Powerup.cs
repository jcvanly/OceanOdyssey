using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public AudioClip pickupSound; // Assign your pickup sound effect in the Inspector

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component on the same GameObject
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component if not already present
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

                if (pickupSound != null && audioSource != null)
                {
                    Debug.Log("Attempting to play pickup sound.");
                    if (!audioSource.enabled)
                    {
                        audioSource.enabled = true;
                    }
                    audioSource.Play();
                    audioSource.PlayOneShot(pickupSound);
                    if (!audioSource.isPlaying)
                    {
                        audioSource.enabled = false;
                    }
                }
                else
                {
                    Debug.LogWarning("Pickup sound or AudioSource component not assigned.");
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