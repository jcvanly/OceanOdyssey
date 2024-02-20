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
                notificationText.text = "+ " + amount + " health";
                notificationText.gameObject.SetActive(true);
                target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
            }
            PowerUpCounter.instance.IncreaseCounter("HealthUp");
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