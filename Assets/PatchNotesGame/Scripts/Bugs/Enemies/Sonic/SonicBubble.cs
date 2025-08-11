using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class SonicBubble : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private PhysicsMaterial2D normalMaterial;
    [SerializeField] private PhysicsMaterial2D bounceMaterial;
    [SerializeField] private float launchForceMin = 2f;
    [SerializeField] private float launchForceMax = 10f;
    [SerializeField] private float bounceTimeMin = 2f;
    [SerializeField] private float bounceTimeMax = 5f;
    [SerializeField] private float damage = 5;

    private bool isBouncy = false;
    private Rigidbody2D rb;
    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private PlayerController playerController;
    public Vector2 direction = Vector2.right.normalized;

    [SerializeField] Animator animator;
    [SerializeField] AnimationClip explosionClip;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.linearVelocity = direction * speed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == gameObject) return;

        if (collider.gameObject.tag == "Player")
        {
            playerCollider = collider;
            playerRigidbody = collider.gameObject.GetComponent<Rigidbody2D>();
            playerController = collider.gameObject.GetComponent<PlayerController>();
            OnPlayerHit();
        }
        else if (collider.gameObject.tag == "Ground")
        {
            OnHit();
        }
    }

    private void OnHit()
    {
        animator.SetTrigger("OnHit");
        rb.linearVelocity = Vector2.zero;
        //StartCoroutine(WaitForExplosion(explosionClip.length));
    }

    private void OnPlayerHit()
    {
        OnHit();

        playerController.TakeDamage(damage);
        LaunchPlayer();
    }

    private void LaunchPlayer()
    {
        if (playerRigidbody == null || playerController == null)
        {
            Debug.Log("Player rigid body or controller not set");
        }

        playerRigidbody.linearVelocity = Vector2.zero;

        float upwardForce = Random.Range(launchForceMin, launchForceMax);
        playerRigidbody.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);

        float horizontalForceMagnitude = Random.Range(launchForceMin, launchForceMax);
        float horizontalDirection = Random.value < 0.5f ? -1f : 1f;
        rb.AddForce(Vector2.right * horizontalForceMagnitude * horizontalDirection, ForceMode2D.Impulse);

        playerCollider.sharedMaterial = bounceMaterial;

        rb.gravityScale = 0.5f;

        if (!isBouncy) StartCoroutine(ResetBounceAfterDelay());
    }

    IEnumerator ResetBounceAfterDelay()
    {
        isBouncy = true;
        yield return new WaitForSeconds(Random.Range(bounceTimeMin, bounceTimeMax));
        playerRigidbody.gravityScale = 1.0f;
        playerCollider.sharedMaterial = normalMaterial;
        isBouncy = false;
        Destroy(gameObject);
    }
}
