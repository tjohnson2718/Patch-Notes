using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Parallax : MonoBehaviour
{
    private float length;
    private float startpos;
    public GameObject cam;
    [SerializeField] public float parallaxEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        PersistCamera persistentCam = FindFirstObjectByType<PersistCamera>();

        if (persistentCam != null)
        {
            cam = persistentCam.gameObject;
        }
        else
        {
            Debug.LogWarning("Persistent camera not found");
        }
    }
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length)
        {
            startpos += length;
        }
        else if (temp < startpos - length)
        {
            startpos -= length;
        }
    }
}
