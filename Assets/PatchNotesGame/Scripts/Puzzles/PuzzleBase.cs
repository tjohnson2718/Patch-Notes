using UnityEngine;

public abstract class PuzzleBase : MonoBehaviour
{
    protected enum PuzzleState
    {
        Waiting,
        Activated,
        Failed,
        Complete
    }

    private string puzzleName;
    private string puzzleDescription;
    public BugBase bugToFix;

    protected PuzzleState state = PuzzleState.Waiting;


    public bool isActive { get; private set; }
    public bool isComplete { get; private set; }

    public virtual void Activate()
    {
        if (isActive || isComplete) return;
        isActive = true;
        OnActivate();
    }
    public virtual void Complete()
    {
        isActive = false;
        isComplete = true;
        OnComplete();
    }

    protected abstract void OnActivate();
    protected abstract void OnFailed();
    protected abstract void OnComplete();
}
