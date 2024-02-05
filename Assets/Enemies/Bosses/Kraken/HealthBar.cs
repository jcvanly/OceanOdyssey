using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public KrakenBehavior krakenBehavior;
    public Image fillImage;

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        float healthPercentage = (float)currentHealth / maxHealth;
        fillImage.fillAmount = healthPercentage;
            Debug.Log($"Updating Health Bar: {healthPercentage * 100}%"); // Debug the update

    }

}
