using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Powerups/NetSpeedBuff")]
public class NetSpeedBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        target.GetComponent<Shooting>().bulletSpeed += amount;

        if (notificationText != null)
        {
            notificationText.text = "+ " + amount + " net speed";
            notificationText.gameObject.SetActive(true);
            target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
        }
        PowerUpCounter.instance.IncreaseCounter("NetSpeedUp");
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
