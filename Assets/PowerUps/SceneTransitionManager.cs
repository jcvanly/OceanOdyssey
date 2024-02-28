using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DestroyUnpickedPowerups();
    }

    private void DestroyUnpickedPowerups()
    {
        GameObject[] powerupObjects = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject powerupObject in powerupObjects)
        {
            Destroy(powerupObject);
        }
    }
}
