using UnityEngine;

[CreateAssetMenu(fileName = "SO_GravityFlip", menuName = "Scriptable Objects/SO_GravityFlip")]
public class SO_GravityFlip : SO_BugAbility
{
    private bool upsideDown = false;
    private bool canFlip = true;
    public override void Acquire()
    {
        canFlip = true;
    }

    public override void Use(GameObject player)
    {
        if (canFlip)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            PlayerController controller = player.GetComponent<PlayerController>();

            upsideDown = !upsideDown;
            rb.gravityScale = upsideDown ? -Mathf.Abs(rb.gravityScale) : Mathf.Abs(rb.gravityScale);
            player.transform.rotation = upsideDown ? Quaternion.Euler(0, 0, 180f) : Quaternion.identity;
            controller.controlsInverted = upsideDown;
        }
    }
}
