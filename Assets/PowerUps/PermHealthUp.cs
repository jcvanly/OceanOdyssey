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
            playerHealth.maxHealth += amount;
            if (playerHealth.maxHealth > 20)
            {
                playerHealth.maxHealth = 20;
            }
            if (playerHealth.health + amount > playerHealth.maxHealth)
            {
                playerHealth.health = playerHealth.maxHealth;
            }
            else
            {
                playerHealth.health += amount;
            }

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
        PowerUpCounter.instance.IncreaseCounter("PermHealthUp");
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