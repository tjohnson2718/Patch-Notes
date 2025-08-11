using UnityEngine;
public class LevelTransitionTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.ChangeGameState("GravityBug");
        }
    }
}
