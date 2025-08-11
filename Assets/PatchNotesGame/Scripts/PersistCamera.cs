using UnityEngine;

public class PersistCamera : MonoBehaviour
{
    private void Awake()
    {
        // If another camera of this type exists, destroy the duplicate
        var existing = FindObjectsByType<PersistCamera>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
