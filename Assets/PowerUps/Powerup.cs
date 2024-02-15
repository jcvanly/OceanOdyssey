using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private AudioSource soundEffects;
    public AudioClip sound;
    private void Update()
    {
        GameObject soundObject = GameObject.FindWithTag("SoundEffects");
        if (soundObject != null)
        {
            soundEffects = soundObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'SoundEffects' found.");
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

                if (soundEffects != null)
                {
                    soundEffects.PlayOneShot(sound);
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