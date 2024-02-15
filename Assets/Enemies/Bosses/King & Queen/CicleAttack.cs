using UnityEngine;
using System.Collections;

public class CircleAttack : MonoBehaviour
{
    public float warningDuration = 2.0f; // Duration before the attack happens
    public SpriteRenderer indicatorSprite; // Reference to the indicator's Sprite Renderer

    void Start()
    {
        // Initialize the indicator as transparent
        SetTransparency(0.3f); // Adjust the transparency as needed
    }

    // Call this method to start the attack warning
    public void WarnOfAttack(Vector2 attackPosition)
    {
        transform.position = attackPosition;
        StartCoroutine(ShowAttackIndicator());
    }

    IEnumerator ShowAttackIndicator()
    {
        // Make the indicator visible but transparent
        SetTransparency(0.3f); // Adjust to make it slightly visible

        // Wait for the specified warning duration
        yield return new WaitForSeconds(warningDuration);

        // Make the indicator fully visible right before the attack
        SetTransparency(1.0f);

        // Optionally, wait for a short moment before hiding the indicator or performing the attack
        yield return new WaitForSeconds(0.5f);

        // Perform the attack here (you can call another method or trigger an event)

        // Hide or reset the indicator's transparency after the attack
        SetTransparency(0.3f); // Or disable the GameObject if the indicator should disappear
    }

    void SetTransparency(float alpha)
    {
        Color color = indicatorSprite.color;
        color.a = alpha;
        indicatorSprite.color = color;
    }
}
