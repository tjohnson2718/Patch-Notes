using UnityEngine;
using System.Collections.Generic;

public class SonicLevelExit : MonoBehaviour
{
    private bool levelComplete = false;
    // Update is called once per frame
    void Awake()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if (PlayerInventory.Instance.CheckForUnlockedBug("SonicBlast"))
        {
            levelComplete = true;
            gameObject.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!levelComplete) return;

        if (collision.gameObject.tag == "Player")
        {
            GameManager.Instance?.ChangeGameState("Hub");
        }
    }
}
