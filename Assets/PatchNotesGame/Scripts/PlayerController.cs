using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]
    private float moveSpeed = 5f;
    private float jumpForce = 5f;
    private bool isGrounded = true;
    private Rigidbody2D rb;

    [Header("Sprite Settings")]
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
        {
            Debug.LogError("Rigidbody2D component not found on the player object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right Arrow keys)
        MovePlayer(horizontalInput);

        if (Input.GetButtonDown("Jump")) // Check if the jump button is pressed (default is Space key)
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        // Movement and jump
        
        
    }

    private void MovePlayer(float horizontalInput)
    {
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;
        // Flip the sprite based on movement direction
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false; // Facing right
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true; // Facing left
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false; // Prevent double jumping
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set grounded state when touching the ground
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
