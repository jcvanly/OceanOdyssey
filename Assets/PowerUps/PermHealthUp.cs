using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/PermHealthUp")]
public class PermHealthUp : PowerupEffect
{
    public int amount;

    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Increase the maximum health
            playerHealth.maxHealth += amount;

            // Ensure maximum health does not exceed 20
            if (playerHealth.maxHealth > 20)
            {
                playerHealth.maxHealth = 20;
            }

            // Check if adding the amount to the current health would exceed the new maximum health
            if (playerHealth.health + amount > playerHealth.maxHealth)
            {
                // Set health to the new maximum health if adding the amount would exceed it
                playerHealth.health = playerHealth.maxHealth;
            }
            else
            {
                // Add the amount to the current health
                playerHealth.health += amount;
            }

            // Show notification
            if (notificationText != null)
            {
                notificationText.text = "+ " + amount + " max health";
                notificationText.gameObject.SetActive(true);
                target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
            }
        }
        else
        {
            Debug.LogWarning("PlayerHealth component not found on target GameObject.");
        }
    }

    private IEnumerator HideNotification(TextMeshProUGUI notificationText)
    {
        yield return new WaitForSeconds(2f);

        if (notificationText != null)
        {
            notificationText.text = "";
        }
    }
}
