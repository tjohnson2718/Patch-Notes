using UnityEngine;

public abstract class BugBase : MonoBehaviour
{
    public string bugName;
    public string description;
    public bool isActive { get; private set; }
    public bool isFixed { get; private set; } = false;
    public SO_BugAbility abilityToAquire;

    public virtual void Activate()
    {
        Debug.Log($"{bugName} activated");
        isActive = true;
        OnActivate();
    }

    public virtual void Fix()
    {
        isActive = false;
        isFixed = true;
        Debug.Log($"{bugName} is fixed");
        OnFix();
    }

    protected abstract void OnActivate();
    protected abstract void OnFix();
}
