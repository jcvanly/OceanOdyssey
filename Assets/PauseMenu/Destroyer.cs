using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destroyer : MonoBehaviour
{
    public GameObject player; // Reference to your player GameObject
    public GameObject heartCanvas; // Reference to your heart canvas GameObject
    public Camera mainCamera; // Reference to your main camera GameObject

    public void DestroyPlayerAndCanvas()
    {

        if (player != null)
        {
            Destroy(player);
        }

        if (heartCanvas != null)
        {
            Destroy(heartCanvas);
        }
        if (mainCamera != null)
        {
            Destroy(mainCamera.gameObject); 
        }
    }


}
