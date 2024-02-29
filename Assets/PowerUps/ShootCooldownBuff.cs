using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[CreateAssetMenu(menuName = "Powerups/ShootCooldownBuff")]
public class ShootCooldownBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        Shooting shootingComponent = target.GetComponent<Shooting>();
        if (shootingComponent != null)
        {
            // Ensure the cooldown does not go below 0.2 seconds
            float newCooldown = shootingComponent.shootCooldown - amount;
            shootingComponent.shootCooldown = Mathf.Max(newCooldown, 0.15f);

            if (notificationText != null)
            {
                notificationText.text = shootingComponent.shootCooldown == 0.2f ? "Cooldown minimized to 0.2s" : "- " + amount + " shoot cooldown";
                notificationText.gameObject.SetActive(true);
                target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
            }

            PowerUpCounter.instance.IncreaseCounter("ShootCoolDown");
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