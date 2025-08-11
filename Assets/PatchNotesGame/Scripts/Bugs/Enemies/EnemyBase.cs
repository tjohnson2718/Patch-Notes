using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField]
    public Animator animator;
    public SpriteRenderer spr;
    public Rigidbody2D rb;

    [SerializeField] protected float maxHealth = 15;
    protected float currentHealth;
    public bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) OnDeath();
        else OnTakeDamage();
    }

    protected abstract void OnDeath();
    protected abstract void Attack();
    protected abstract void Patrol();
    protected abstract void OnTakeDamage();
}
