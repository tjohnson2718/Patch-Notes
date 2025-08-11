using System.Collections;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class GravityBugLevelFinish : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        
        if (PlayerInventory.Instance?.equippedBug != null)
        {
            GetComponent<SpriteRenderer>().enabled = true;

        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (PlayerInventory.Instance?.equippedBug != null)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance?.ChangeGameState("Hub");
            }
        }
    }
}
