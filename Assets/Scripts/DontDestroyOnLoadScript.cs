using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    private static DontDestroyOnLoadScript instance;

    void Awake()
    {
        // Ensure only one instance of this script exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this one
            Destroy(gameObject);
        }
    }
}
