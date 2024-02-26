using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destroyer : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private GameObject heartCanvas; // Reference to the heart canvas GameObject
    private Camera mainCamera; // Reference to the main camera GameObject
    private AudioManager audioManager;
    private GameObject crosshair;
    private void Start()
    {
        FindPlayerObjects();
    }
    private void FindPlayerObjects()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        heartCanvas = GameObject.FindGameObjectWithTag("HeartCanvas");
        mainCamera = Camera.main;
        audioManager = audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        crosshair = GameObject.FindGameObjectWithTag("crosshair");
    }

    public void Destroy()
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
        //if (audioManager != null)
        //{
         //   Destroy(audioManager.gameObject);
       // }
        if(crosshair != null)
        {
            Destroy(crosshair);
        }
    }
}