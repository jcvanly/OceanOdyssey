using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign in Inspector
    public Vector2 hotSpot = Vector2.zero; // Adjust if needed
    private Camera mainCamera;


    

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void Awake() {
        DontDestroyOnLoad(gameObject);
        mainCamera = Camera.main;
    }

    void LateUpdate() {
        Vector2 cursorPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPosition;
    }

    

}
