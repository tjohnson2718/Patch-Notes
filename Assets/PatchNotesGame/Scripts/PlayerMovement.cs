using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjustable speed in the Inspector
    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get player input for horizontal and vertical movement
        movementInput.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow keys
        

        // Normalize the movement vector to prevent faster diagonal movement
        movementInput.Normalize();
    }

    void FixedUpdate()
    {
        // Apply movement using Rigidbody2D's velocity in FixedUpdate for physics consistency
        rb.linearVelocity = movementInput * moveSpeed;
    }
}