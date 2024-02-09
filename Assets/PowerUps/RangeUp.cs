using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/RangeUp")]
public class RangeUp : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        target.GetComponent<Shooting>().bulletRange += amount;

        if (notificationText != null)
        {
            notificationText.text = "+ " + amount + " range ";
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