using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Powerups/SpeedBuff")]
public class SpeedBuff : PowerupEffect
{
    public float amount;
    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        target.GetComponent<PlayerMovement>().moveSpeed += amount;
        target.GetComponent<SpriteRenderer>().color = Color.yellow;

        if (notificationText != null)
        {
            notificationText.text = "+ " + amount + " speed";
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
