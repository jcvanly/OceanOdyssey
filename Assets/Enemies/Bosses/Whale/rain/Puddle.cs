using UnityEngine;

public class Puddle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color electrifiedColor = Color.yellow; 

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Electrify()
    {
        spriteRenderer.color = electrifiedColor;
        // Add additional effects as needed, such as playing a particle system or animation
    }
}
