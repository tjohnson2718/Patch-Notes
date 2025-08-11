using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistCamera : MonoBehaviour
{
    CinemachineCamera vcam;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        vcam = GetComponent<CinemachineCamera>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            vcam.Follow = player.transform;
    }
}
