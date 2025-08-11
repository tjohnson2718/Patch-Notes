using UnityEngine;
using System.Collections.Generic;
public class Bug_SonicBlast : BugBase
{
    [SerializeField] private List<GameObject> bugsToKill = new List<GameObject>();

    private void FixedUpdate()
    {
        if (isActive)
        {
            CheckComplete();
        }
    }

    protected override void OnActivate()
    {
        
    }

    protected override void OnFix()
    {
        Debug.Log($"{bugName} is fixed");
        abilityToAquire.Acquire();
        PlayerInventory.Instance?.AddUnlockedBug(abilityToAquire);
    }

    private void CheckComplete()
    {
        int numKilled = 0;
        foreach (var enemy in bugsToKill)
        {
            if (enemy.GetComponent<EnemyBase>().isDead) numKilled++;
        }

        if (numKilled == bugsToKill.Count)
        {
            Fix();
        }
    }
}
