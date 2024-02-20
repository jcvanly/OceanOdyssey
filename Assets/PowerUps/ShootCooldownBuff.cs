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
        target.GetComponent<Shooting>().shootCooldown -= amount;
        if (notificationText != null)
        {
            notificationText.text = "- " + amount + " shoot cooldown";
            notificationText.gameObject.SetActive(true);
            target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
        }
        PowerUpCounter.instance.IncreaseCounter("ShootCoolDown");
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