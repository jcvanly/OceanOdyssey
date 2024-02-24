using UnityEngine;

public class IceSlidingEffect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player has a script named PlayerMovement which handles entering ice
            other.GetComponent<PlayerMovement>().EnterIce();
            Debug.Log("Player has started sliding on ice.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Assuming the player has a script named PlayerMovement which handles exiting ice
            other.GetComponent<PlayerMovement>().ExitIce();
            Debug.Log("Player has stopped sliding on ice.");

        }
    }
}
