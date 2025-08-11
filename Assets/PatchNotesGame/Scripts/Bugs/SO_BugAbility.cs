using UnityEngine;

public enum AbilityType
{
    Passive,
    Activation
}

[CreateAssetMenu(fileName = "SO_BugAbility", menuName = "Scriptable Objects/SO_BugAbility")]
public abstract class SO_BugAbility : ScriptableObject
{
    public string bugName;
    public string bugDescription;
    public AbilityType abilityType;

    public abstract void Acquire();
    public abstract void Use(GameObject player);
}
