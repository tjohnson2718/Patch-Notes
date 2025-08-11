using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerSpawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().SetSpawn(gameObject.transform);
        }
    }
}
