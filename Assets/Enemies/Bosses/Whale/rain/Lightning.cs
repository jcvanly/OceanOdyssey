using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public float lifetime = 0.5f; // Duration before the lightning disappears, adjust as needed

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime); // Destroy the lightning after 'lifetime' seconds
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Puddle")) // Ensure your puddles are tagged appropriately
        {
            // Call a method on the puddle to change its appearance
            Puddle puddle = other.GetComponent<Puddle>();
            if (puddle != null)
            {
                puddle.Electrify();
            }
        }
    }
}
