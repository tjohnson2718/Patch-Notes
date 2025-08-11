using UnityEngine;

public class HostileObject : MonoBehaviour
{
    private bool playerIsOverlapping = false;
    private PlayerController player;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping)
        {
            if (!player.isDead) player?.TakeDamage(10f);
            else playerIsOverlapping = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsOverlapping = true;
            player = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsOverlapping = true;
            player = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsOverlapping = false;
            player = null;
        }
    }
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsOverlapping = false;
            player = null;
        }
    }
}
