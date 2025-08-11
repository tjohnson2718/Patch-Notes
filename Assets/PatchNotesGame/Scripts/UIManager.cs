using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject playerHud;
    [SerializeField] private Image healthFillImage;
    [SerializeField] private Image HeartOne;
    [SerializeField] private Image HeartTwo;
    [SerializeField] private Image HeartThree;

    private void Awake()
    {
        if (Instance == null)
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        healthFillImage.fillAmount = Mathf.Clamp01(GetHealthFillValue());
    }

    private float GetHealthFillValue()
    {
        PlayerController playerController = GameManager.Instance?.playerPrefab.GetComponent<PlayerController>();
        return playerController.currentHealth / playerController.maxHealth;
    }

    public void LoseLife(int num)
    {
        switch (num)
        {
            case 1:
                HeartOne.gameObject.SetActive(false);
                break;
            case 2:
                HeartTwo.gameObject.SetActive(false);
                break;
            case 3:
                HeartThree.gameObject.SetActive(false);
                break;
        }
    }
}
