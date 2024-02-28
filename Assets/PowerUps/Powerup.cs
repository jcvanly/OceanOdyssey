using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        DontDestroyOnLoad(gameObject); // Make the power-up object persistent
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
                powerupEffect.Apply(collision.gameObject, notificationText);
                audioManager.PlaySFX(audioManager.collectPowerUp);
            }
        }
    }
}
