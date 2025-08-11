using UnityEngine;

public class BugTrigger : MonoBehaviour
{
    [SerializeField] private BugBase bug;
    [SerializeField] private PuzzleBase puzzle = null;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!bug.isActive && !bug.isFixed) bug.Activate();
            if (puzzle != null) puzzle.Activate();
        }
    }
}
