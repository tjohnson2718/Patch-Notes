using UnityEngine;

public class TestKillPlayer : MonoBehaviour
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

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsOverlapping = true;
            player = collision.gameObject.GetComponent<PlayerController>();
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
