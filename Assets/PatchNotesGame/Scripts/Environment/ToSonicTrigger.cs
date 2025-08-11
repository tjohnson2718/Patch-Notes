using UnityEngine;

public class ToSonicTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.ChangeGameState("SonicBug");
        }
    }
}
