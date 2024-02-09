using UnityEngine;
using TMPro;
using System.Collections;

[CreateAssetMenu(menuName = "Powerups/DamageBuff")]
public class DamageBuff : PowerupEffect
{
    public int amount;
    public override void Apply(GameObject target, TextMeshProUGUI notificationText)
    {
        target.GetComponent<Bullet>().damage += amount;
        target.GetComponent<SpriteRenderer>().color = Color.red;

        if (notificationText != null)
        {
            notificationText.text = "+ " + amount + " damage";
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