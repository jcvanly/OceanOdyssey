using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class ReturnHomeButton : MonoBehaviour
{
    // Method to load the first scene
    public void LoadFirstScene()
    {

        SceneManager.LoadScene(0);
    }
}
