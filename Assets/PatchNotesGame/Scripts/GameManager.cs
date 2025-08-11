using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }

    public static GameManager Instance { get; private set; }

    private GameState currentState = GameState.Playing;

    [Header("Player Settings")]
    [SerializeField] public GameObject playerPrefab;
    private int maxLives = 3;
    private int currentLives;

    //[SerializeField] public List<BugAbilityBase> unlockedAbilities = new List<BugAbilityBase>();

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

        currentLives = maxLives;
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.MainMenu:
                // Handle main menu logic
                break;
            case GameState.Playing:
                // Handle game playing logic
                break;
            case GameState.Paused:
                // Handle game paused logic
                break;
            case GameState.GameOver:
                // Handle game over logic
                break;
        }
    }

    public void OnPlayerDeath()
    {
        currentLives--;
        if (currentLives <= 0)
        {
            currentState = GameState.GameOver;
            // Trigger game over logic
        }
        else
        {
            PlayerController controller = playerPrefab.GetComponent<PlayerController>();
            controller.RespawnPlayer();
        }
    }
}
