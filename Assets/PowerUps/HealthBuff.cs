using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        target.GetComponent<PlayerHealth>().health += amount;
        if (notificationText != null)
        {
            notificationText.text = "+ " + amount + " health";
            notificationText.gameObject.SetActive(true);
            target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
        }
    }
    private IEnumerator HideNotification(TextMeshProUGUI notificationText)
    {
        yield return new WaitForSeconds(2f);

        if (notificationText != null)
        {
            notificationText.gameObject.SetActive(false);
        }
    }
}
