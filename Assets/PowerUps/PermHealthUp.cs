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
        target.GetComponent<PlayerHealth>().maxHealth += amount;
        target.GetComponent<PlayerHealth>().health += amount;

        if (notificationText != null)
        {
            notificationText.text = "+ " + amount + " max health";
            notificationText.gameObject.SetActive(true);
            target.GetComponent<MonoBehaviour>().StartCoroutine(HideNotification(notificationText));
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
