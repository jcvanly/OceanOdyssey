using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destroyer : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private GameObject heartCanvas; // Reference to the heart canvas GameObject
    private Camera mainCamera; // Reference to the main camera GameObject

    private void Start()
    {
        FindPlayerObjects();
    }

    private void FindPlayerObjects()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        heartCanvas = GameObject.FindGameObjectWithTag("HeartCanvas");
        mainCamera = Camera.main; // Assuming the main camera is tagged as "MainCamera"
    }

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
