using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Fireball : MonoBehaviour
{
    [SerializeField] private float damage = 5;
    [SerializeField] private float speed = 6;
    [SerializeField] private Animator animator;
    [SerializeField] private AnimationClip explosionClip;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float activeTime = 10f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireAudioClip;
    [SerializeField] private AudioClip explodeAudioClip;
    public Vector2 direction = Vector2.right;

    private Rigidbody2D rb;
    private GameObject explosion;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitToDestroy());
    }

    private void Start()
    {
        audioSource.clip = fireAudioClip;
        audioSource.Play();
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void HitPlayer(GameObject player)
    {
        rb.linearVelocity = Vector2.zero;

        player.GetComponent<PlayerController>().TakeDamage(damage);
        Explode();
    }

    private void HitEnemy(GameObject enemy)
    {
        rb.linearVelocity = Vector2.zero;
        enemy.GetComponent<EnemyBase>().TakeDamage(damage);
        Explode();
    }

    private void Explode()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);

        audioSource.clip = explodeAudioClip;
        audioSource.Play();
        StartCoroutine(WaitForExplosion());
    }

    IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(explosionClip.length);
        Destroy(explosion);
        Destroy(gameObject);
    }
    IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(activeTime);
        Explode();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string hitTag = collision.gameObject.tag;

        switch (hitTag)
        {
            case "Player":
                HitPlayer(collision.gameObject);
                break;
            case "Enemy":
                HitEnemy(collision.gameObject);
                break;
            case "Default":
                break;
        }
    }
}
