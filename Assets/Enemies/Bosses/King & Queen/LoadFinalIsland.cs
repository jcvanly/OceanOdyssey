using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadFinalIsland : MonoBehaviour
{
        // Method to be called when the button is clicked
    public void OnNextIslandButtonClick()
    {
        Debug.Log("loading scene 17");
        SceneManager.LoadScene(17); 
    }
}
