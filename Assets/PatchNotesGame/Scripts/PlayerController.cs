using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    private bool isGrounded = true;
    private Rigidbody2D rb;

    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Player Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] public Vector2 spawnPosition = new Vector2(0, 0);
    public bool isDead = false;
    public bool canTakeDamage = true;
    private float currentHealth = 100f;

    [Header("Animations")]
    [SerializeField]
    public AnimationClip death;
    public AnimationClip jump;
    public AnimationClip walk;
    public AnimationClip idle;

    private void Awake()
    {
        if (!spriteRenderer)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (!spriteRenderer)
            {
                Debug.LogError("SpriteRenderer component not found on the player object.");
            }
        }
        if (!animator)
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogWarning("Animator component not found on the player object. Animation will not work.");
            }
        }

        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

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
        if (Input.GetButtonDown("Jump") && !isDead) // Check if the jump button is pressed (default is Space key)
        {
            Jump();
        }

        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            animator?.SetBool("isFalling", true);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right Arrow keys)
        MovePlayer(horizontalInput);
    }

    #region Player Movement
    private void MovePlayer(float horizontalInput)
    {
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        rb.linearVelocity = movement;

        if (animator)
        {
            if (Mathf.Abs(horizontalInput) > 1.0f)
            {
                Debug.LogWarning("Horizontal input is greater than 1.0f, which may cause unexpected behavior in animation speed.");
            }
            animator.SetFloat("MoveSpeed", Mathf.Abs(horizontalInput));
        }

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
            animator?.SetBool("isJumping", true);
            isGrounded = false; // Prevent double jumping
        }
    }
    #endregion

    #region Health Management
    public void TakeDamage(float damageAmount)
    {
        if (canTakeDamage)
        {
            currentHealth -= damageAmount;
            canTakeDamage = false;
            StartCoroutine(WaitForEnableDamage(1f));
        }

        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        isDead = true;
        // Notify game manager to check if the player is out of lives
        // GameManager.Instance?.CheckPlayerLives(); // Assuming GameManager has a method to check player lives
        animator?.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;

        //TogglePlayerPhysics(false);
        //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Player is dead. Waiting for respawn animation time of: " + death.length + " seconds.");
        StartCoroutine(WaitAndRespawn(death.length));
    }
    #endregion

    #region Spawn Management
    public void SetSpawnPosition(Vector2 position)
    {
        spawnPosition = position;
    }

    public void RespawnPlayer()
    {
        transform.position = spawnPosition;
        ResetPlayer();
    }
    #endregion

    #region Helper Methods
    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        rb.linearVelocity = Vector2.zero;

        //TogglePlayerPhysics(true);
        spriteRenderer.enabled = true;

        // Respawn logic

        // Reset animator states
        if (animator)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead", false);
            animator.SetFloat("MoveSpeed", 0f);
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        isDead = false;
    }

    public void TogglePlayerPhysics(bool enable)
    {
        if (enable) rb.bodyType = RigidbodyType2D.Dynamic;
        else rb.bodyType = RigidbodyType2D.Static;
    }
    #endregion

    #region Coroutine Methods
    IEnumerator WaitAndRespawn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        spriteRenderer.enabled = false;
        GameManager.Instance?.OnPlayerDeath();
    }

    IEnumerator WaitForEnableDamage(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canTakeDamage = true;
        canTakeDamage = true;
    }

    #endregion

    #region Collision and Trigger Events
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set grounded state when touching the ground
            animator?.SetBool("isJumping", false);
            animator?.SetBool("isFalling", false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    #endregion

}
