using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    private List<SO_BugAbility> unlockedBugs = new List<SO_BugAbility>();
    public SO_BugAbility equippedBug;

    private void Awake()
    {
        if (Instance == null)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddUnlockedBug(SO_BugAbility ability)
    {
        unlockedBugs.Add(ability);

        if (unlockedBugs.Count == 1 && ability.abilityType != AbilityType.Passive)
        {
            equippedBug = unlockedBugs[0];
        }
    }

    public void EquipBug(SO_BugAbility ability)
    {
        // Deal With Later
    }

    public void UseEquippedBug(GameObject player)
    {
        if (!equippedBug) return;
        equippedBug.Use(player);
    }
}
