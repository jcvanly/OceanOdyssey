using UnityEngine;
using UnityEngine.UI;

public class CrabHealthBar : MonoBehaviour {
    public KingCrab kingCrab;
    public Image fillImage;

    public void UpdateHealthBar(int currentHealth, int maxHealth) {
        float healthPercentage = (float)currentHealth / maxHealth;
        fillImage.fillAmount = healthPercentage;
        Debug.Log($"Updating Health Bar: {healthPercentage * 100}%"); // Debug the update

        // Check if health is 0 or less and hide the health bar if so
        if (currentHealth <= 0) {
            HideHealthBar();
        }
    }

    public void HideHealthBar() {
        gameObject.SetActive(false); // You can disable the whole HealthBar GameObject or just the fillImage depending on your UI setup.
    }
}
