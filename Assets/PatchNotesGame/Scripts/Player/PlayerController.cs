using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    public bool isGrounded = true;
    private Rigidbody2D rb;

    [Header("Sprite Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    private bool facingRight = true;

    [Header("Player Settings")]
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public Transform spawnPosition;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireCooldown = 1.0f;
    [SerializeField] private float maxFireAngle = 45f;
    [SerializeField] private float firePointDistFromPlayer = 1.0f;
    [SerializeField] private Transform firePoint;
    public bool isDead = false;
    public bool canTakeDamage = true;
    public float currentHealth = 100f;
    private bool canFire = true;

    [Header("Animations")]
    [SerializeField]
    public AnimationClip death;
    public AnimationClip jump;
    public AnimationClip walk;
    public AnimationClip idle;

    [Header("Player")]
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip jumpSound;
    public bool controlsInverted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        Vector2 aimDirection = (mouseWorldPos - firePoint.position).normalized;
        float angleToMouse = Vector2.Angle(facingRight ? Vector2.right : Vector2.left, aimDirection);

        if (angleToMouse <= maxFireAngle)
        {
            firePoint.position = transform.position + (Vector3)aimDirection * firePointDistFromPlayer;
        }

        if (Input.GetMouseButtonDown(0) && angleToMouse <= maxFireAngle)
        {
            Fire(aimDirection);
        }

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
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
            facingRight = true;
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
            facingRight = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            audio.clip = jumpSound;
            audio.Play();
            float currentJumpForce = controlsInverted ? -jumpForce : jumpForce;
            rb.AddForce(new Vector2(0, currentJumpForce), ForceMode2D.Impulse);
            animator?.SetBool("isJumping", true);
            isGrounded = false; // Prevent double jumping
        }
    }
    #endregion

    #region Attacking
    private void Fire(Vector2 direction)
    {
        if (!canFire) return;

        GameObject fireBall = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        fireBall.GetComponent<Fireball>().direction = direction;
    }
    #endregion

    #region Health Management
    public void TakeDamage(float damageAmount)
    {
        if (canTakeDamage)
        {
            currentHealth -= damageAmount;
            canTakeDamage = false;
            animator.SetTrigger("OnHit");
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
        animator?.SetBool("isDead", true);
        rb.linearVelocity = Vector2.zero;

        Debug.Log("Player is dead. Waiting for respawn animation time of: " + death.length + " seconds.");
        StartCoroutine(WaitAndRespawn(death.length));
    }
    #endregion

    #region Spawn Management
    public void SetSpawn(Transform transform)
    {
        spawnPosition = transform;
    }
    public void RespawnPlayer()
    {
        transform.position = spawnPosition.position;
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
