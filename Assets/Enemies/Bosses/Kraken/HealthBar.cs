using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public KrakenBehavior krakenBehavior;
    public Image fillImage;

    void Update() {
        float healthPercentage = (float)krakenBehavior.currentHealth / krakenBehavior.maxHealth;
        fillImage.fillAmount = healthPercentage;
    }
}
