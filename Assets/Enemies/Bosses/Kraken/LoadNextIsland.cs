using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextIsland : MonoBehaviour
{
    // Method to be called when the button is clicked
    public void OnNextIslandButtonClick()
    {
        SceneManager.LoadScene(7); 
    }
}
