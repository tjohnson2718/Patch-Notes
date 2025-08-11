using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        Hub,
        GravityBug,
        SonicBug,
    }

    public static GameManager Instance { get; private set; }

    private GameState currentState = GameState.MainMenu;

    [Header("Player Settings")]
    [SerializeField] public GameObject playerPrefab;
    private int maxLives = 3;
    public int currentLives;

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

    private void Start()
    {
        playerPrefab = GameObject.FindWithTag("Player");
    }

    public void StartGame ()
    {
        currentState = GameState.Hub;
        SceneSet();
    }
    
    public void ChangeGameState (string sceneToChangeTo)
    {
        if(SceneManager.GetActiveScene().name.Equals("Hub") && sceneToChangeTo.Equals("GravityBug"))
        {
            currentState = GameState.GravityBug;
            SceneSet();
            return;
        }
        if (SceneManager.GetActiveScene().name.Equals("GravityBug") && sceneToChangeTo.Equals("Hub"))
        {
            currentState = GameState.Hub;
            SceneSet();
            return;
        }
        if (SceneManager.GetActiveScene().name.Equals("Hub") && sceneToChangeTo.Equals("SonicBug"))
        {
            currentState = GameState.SonicBug;
            SceneSet();
            return;
        }
        if (SceneManager.GetActiveScene().name.Equals("SonicBug") && sceneToChangeTo.Equals("Hub"))
        {
            currentState = GameState.Hub;
            SceneSet();
            return;
        }
    }

    private void SceneSet()
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
            case GameState.Hub:
                SceneManager.LoadScene("Hub");
                break;
            case GameState.GravityBug:
                SceneManager.LoadScene("GravityBug");
                break;
            case GameState.SonicBug:
                SceneManager.LoadScene("SonicBug");
                break;
        }
    }

    public void OnPlayerDeath()
    {
        UIManager.Instance?.LoseLife(currentLives);
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
