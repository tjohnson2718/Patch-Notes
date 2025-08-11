using UnityEngine;
using System.Collections;

public class GravityFlipBug : BugBase
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float minCooldown = 2;
    [SerializeField] private float maxCooldown = 6;
    private float gravityFlipCooldown;

    private bool upsideDown = false;
    private bool canFlip = true;

    protected override void OnActivate()
    {
        if (playerRigidbody == null) playerRigidbody = GameManager.Instance?.playerPrefab.GetComponent<Rigidbody2D>();
        if (playerRigidbody == null) playerTransform = GameManager.Instance?.playerPrefab.GetComponent<Transform>();
        if (playerRigidbody == null) playerController = GameManager.Instance?.playerPrefab.GetComponent<PlayerController>();
        StartCoroutine(WatchForGravityFlip());
    }

    protected override void OnFix()
    {
        StopAllCoroutines();
        ResetGravity();
        playerController.controlsInverted = false;

        PlayerInventory.Instance?.AddUnlockedBug(abilityToAquire);
    }

    private IEnumerator WatchForGravityFlip()
    {
        while (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canFlip)
            {
                FlipGravity();
                StartCoroutine(FlipCooldown());
            }
            yield return null;
        }
    }

    private void FlipGravity()
    {
        upsideDown = !upsideDown;
        Physics2D.gravity = new Vector2(0, upsideDown ? 9.81f : -9.81f);
        playerTransform.rotation = Quaternion.Euler(0, 0, upsideDown ? 180f : 0f);
        playerController.controlsInverted = upsideDown;
    }

    private IEnumerator FlipCooldown()
    {
        canFlip = false;
        gravityFlipCooldown = Random.Range(minCooldown, maxCooldown);
        yield return new WaitForSeconds(gravityFlipCooldown);
        canFlip = true;
    }
    private void ResetGravity()
    {
        Physics2D.gravity = new Vector2(0, -9.81f);
        playerTransform.rotation = Quaternion.identity;
        upsideDown = false;
        canFlip = true;
    }
}
