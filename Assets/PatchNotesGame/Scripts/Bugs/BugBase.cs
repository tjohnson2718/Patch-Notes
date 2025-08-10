using UnityEngine;

public abstract class BugBase : MonoBehaviour
{
    public string bugName;
    public string description;
    public bool isActive { get; private set; }
    public SO_BugAbility abilityToAquire;

    public virtual void Activate()
    {
        isActive = true;
        OnActivate();
    }

    public virtual void Fix()
    {
        isActive = false;
        OnFix();
    }

    protected abstract void OnActivate();
    protected abstract void OnFix();
}
