using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    private static PlayerSpawnManager instance;
    private PlayerController player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        player = PlayerController.Instance;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (player == null) return;

        // Find spawn point by tag in the loaded scene
        GameObject spawnPointObj = GameObject.FindWithTag("PlayerSpawn");
        if (spawnPointObj != null)
        {
            player.SetSpawn(spawnPointObj.transform);
            player.RespawnPlayer();
        }
        else
        {
            Debug.LogWarning($"No spawn point found in scene '{scene.name}'. Player will not move.");
        }
    }
}
