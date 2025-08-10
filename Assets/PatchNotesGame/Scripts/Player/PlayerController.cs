using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    public bool isGrounded = true;
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

    public bool controlsInverted = false;

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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        UpdatePlayerSprite(horizontalInput);

        if (Input.GetButtonDown("Jump") && !isDead)
        {
            Jump();
        }

        if (!isGrounded && rb.linearVelocity.y < 0)
        {
            animator?.SetBool("isFalling", true);
        }

        if (!isDead && Input.GetKeyDown(KeyCode.Q))
        {
            PlayerInventory.Instance?.UseEquippedBug(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // Get horizontal input (A/D or Left/Right Arrow keys)

        if (controlsInverted) horizontalInput = -horizontalInput;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    #region Player Movement
    private void UpdatePlayerSprite(float horizontalInput)
    {
        animator?.SetFloat("MoveSpeed", Mathf.Abs(horizontalInput));
        if (horizontalInput > 0) spriteRenderer.flipX = false;
        else if (horizontalInput < 0) spriteRenderer.flipX = true;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            float currentJumpForce = controlsInverted ? -jumpForce : jumpForce;
            rb.AddForce(new Vector2(0, currentJumpForce), ForceMode2D.Impulse);
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
