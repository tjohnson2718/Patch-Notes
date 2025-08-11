using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "SO_SonicAbility", menuName = "Scriptable Objects/SO_SonicAbility")]
public class SO_SonicAbility : SO_BugAbility
{
    [Header("Force Settings")]
    [SerializeField] private float minForce = 5f;
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float minInterval = 5f;
    [SerializeField] private float maxInterval = 15f;
    [SerializeField] private PhysicsMaterial2D normalMat;
    [SerializeField] private PhysicsMaterial2D bounceMat;

    private bool isActive = false;
    private PlayerController playerController;
    private Rigidbody2D playerRb;
    private CapsuleCollider2D playerCollider;

    private void Update()
    {
        if (isActive)
        {

        }
    }
    public override void Acquire()
    {
        isActive = true;
        Initialize(GameManager.Instance?.playerPrefab);
        if (playerController != null)
        {
            playerController.StartCoroutine(ForceRoutine());
        }
    }

    public void Initialize(GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
        playerRb = player.GetComponent<Rigidbody2D>();
        playerCollider = player.GetComponent<CapsuleCollider2D>();
    }

    private IEnumerator ForceRoutine()
    {
        while (isActive)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float forceAmount = Random.Range(minForce, maxForce);

            playerRb.AddForce(randomDir * forceAmount, ForceMode2D.Impulse);

            playerCollider.sharedMaterial = bounceMat;
            yield return new WaitForSeconds(2.5f);
            playerCollider.sharedMaterial = normalMat;
            Debug.Log("Launched Player");
        }
    }
    public override void Use(GameObject player)
    {
        throw new System.NotImplementedException();
    }
}
