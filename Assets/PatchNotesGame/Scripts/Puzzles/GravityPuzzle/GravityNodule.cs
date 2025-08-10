using UnityEngine;

public class GravityNodule : MonoBehaviour
{
    [Header("Location Settins")]
    public Transform destination;

    public bool complete = false;

    private void FixedUpdate()
    {
        //CheckLocation();
    }

    private void CheckLocation()
    {
        if (complete) return;
        if (transform.position == destination.position)
        {
            complete = true;
        }
    }

    public void OnComplete()
    {
        Destroy(gameObject);
    }

}
