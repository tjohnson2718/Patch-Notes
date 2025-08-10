using UnityEngine;
using System.Collections;

public class GravityNodeDestination : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private AnimationClip m_Clip;

    public GameObject orb;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Triggered by {collision.gameObject.name}");
        if (collision.gameObject == orb)
        {
            Debug.Log($"Orb Entered Trigger");
            orb.GetComponent<GravityNodule>().complete = true;

            DestinationComplete();
        }
    }

    private void DestinationComplete()
    {
        m_Animator?.SetBool("isComplete", true);
        StartCoroutine(WaitForAnim(m_Clip.length));
    }

    IEnumerator WaitForAnim(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
