using UnityEngine;
using System.Collections;

public class Enemy_SonicBlast : EnemyBase
{
    [Header("Detection Settings")]
    public float detectionDistance = 5.0f;
    public float detectionAngleStep = 5f;
    public int rayCount = 5;
    public Transform rayOrigin;

    [Header("Enemy Settings")]
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private Transform patrolLocation_01;
    [SerializeField] private Transform patrolLocation_02; 
    [SerializeField, Range(0f, 5f)] private float attackCooldown = 3f;
    [SerializeField] private GameObject sonicBlastPrefab;
  
    [Header("Animation Settings")]
    [SerializeField] private AnimationClip attackStartClip;
    [SerializeField] private AnimationClip attackEndClip;

    private Transform target;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool movingToFirstPoint = true;

  
    private void Update()
    {
        Patrol();
    }

    private void FixedUpdate()
    {
        if (ScanForPlayer() && canAttack)
        {
            Attack();
        }
    }

    private bool ScanForPlayer()
    {
        bool found = false;

        Vector2 facingDir = spr.flipX ? Vector2.right : Vector2.left;
        float startAngle = -(rayCount - 1) / 2f * detectionAngleStep;
        for (int i = 0; i < rayCount; i++)
        {
            float angleOffset = startAngle + i * detectionAngleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angleOffset) * facingDir;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, direction, detectionDistance);
            Debug.DrawRay(rayOrigin.position, direction * detectionDistance, Color.yellow);

            if (hit && hit.collider.CompareTag("Player"))
            {
                found = true;
                target = hit.collider.transform;
            }
        }

        return found;
    }

    private void LaunchSonicBlast()
    {
        Vector2 direction = target.position - rayOrigin.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject blast = Instantiate(sonicBlastPrefab, rayOrigin.position, Quaternion.Euler(0, 0, angle));
        blast.GetComponent<SonicBubble>().direction = direction;
    }

    protected override void Attack()
    {
        canAttack = false;
        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(WaitForStartAttackAnim(attackStartClip.length));
        StartCoroutine(WaitForAttackCooldown(attackCooldown));
    }

    protected override void OnDeath()
    {
        isDead = true;
        animator.SetTrigger("Died");
    }

    protected override void Patrol()
    {
        if (isAttacking) return;
        Vector2 target = movingToFirstPoint ? patrolLocation_01.position : patrolLocation_02.position;

        Vector2 direction = (target - (Vector2)transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        if (direction.x > 0.01f)
        {
            spr.flipX = true; // backwards for this sprite
                              // Place rayOrigin in front of enemy when facing right
            rayOrigin.localPosition = new Vector3(Mathf.Abs(rayOrigin.localPosition.x), rayOrigin.localPosition.y, rayOrigin.localPosition.z);
        }
        else if (direction.x < -0.01f)
        {
            spr.flipX = false;
            // Place rayOrigin in front of enemy when facing left (mirror the x)
            rayOrigin.localPosition = new Vector3(-Mathf.Abs(rayOrigin.localPosition.x), rayOrigin.localPosition.y, rayOrigin.localPosition.z);
        }

        if (Vector2.Distance(transform.position, target) < 1.0f)
        {
            movingToFirstPoint = !movingToFirstPoint;
        }
    }

    public void AttackComplete()
    {
        isAttacking = false;
    }
    protected override void OnTakeDamage()
    {
        animator.SetTrigger("TakeHit");
    }

    public void DeactivateEnemy()
    {
        gameObject.SetActive(false);
    }

    IEnumerator WaitForStartAttackAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator WaitForAttackCooldown(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }
}
