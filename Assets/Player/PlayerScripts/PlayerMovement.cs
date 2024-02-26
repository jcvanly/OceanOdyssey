using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;
    Vector2 mousePos;
    private bool isOnIce = false;
    private float originalDrag;

    void Start()
    {
            originalDrag = rb.drag; // Store the original drag for later restoration

        if (cam == null)
        {
            cam = Camera.main;

            if (cam == null)
            {
                Debug.LogError("Main camera not found. Make sure there is a camera tagged as 'MainCamera' in the scene.");
            }
        }

        DontDestroyOnLoad(cam.gameObject);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize movement vector to ensure consistent speed in all directions
        movement.Normalize();

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        // Calculate the direction from the player to the mouse cursor position
        Vector2 lookDir = mousePos - rb.position;
        // Calculate the angle in radians from the player to the cursor using Atan2, and convert it to degrees
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Set the player's rotation to face towards the cursor, without subtracting 90 degrees
        rb.rotation = angle;

        // Movement and ice sliding logic remains the same
        if (isOnIce) {
            // Apply force for sliding effect
            rb.AddForce(movement * moveSpeed - rb.velocity, ForceMode2D.Force);
        } else {
            // Standard movement when not on ice
            rb.velocity = movement * moveSpeed;
        }
    }


    public void EnterIce() {
    isOnIce = true;
    rb.drag = 0.005f; // Lower drag for more slide
    }

    // Call this method when exiting ice
    public void ExitIce() {
        isOnIce = false;
        rb.drag = originalDrag; // Restore original drag
    }
}
