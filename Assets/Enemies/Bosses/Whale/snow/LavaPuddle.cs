using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPuddle : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        
            PlayerHealth healthComponent = collision.gameObject.GetComponent<PlayerHealth>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(1); // Adjust damage value as necessary
            }
        
    }
}
