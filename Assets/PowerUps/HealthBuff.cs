using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public int amount;

    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        PlayerHealth playerHealth = target.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            // Check if adding the health buff would exceed the maximum health
            if (playerHealth.health + amount > playerHealth.maxHealth)
            {
                // Set health to maximum health if adding the health buff would exceed it
                playerHealth.health = playerHealth.maxHealth;
            }
            else
            {
                // Add the health buff
                playerHealth.health += amount;
            }

            // Show notification
            if (notificationText != null)
            {
                notificationText.text = "+ " + amount + " health";
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
