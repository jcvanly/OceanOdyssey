using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    public TextMeshProUGUI notificationText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
        powerupEffect.Apply(collision.gameObject, notificationText);
    }
}
