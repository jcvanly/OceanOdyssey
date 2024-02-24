using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign in Inspector
    public Vector2 hotSpot = Vector2.zero; // Adjust if needed

    void Awake() {
    DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void Update() {
    Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    transform.position = cursorPosition;
    }

}
