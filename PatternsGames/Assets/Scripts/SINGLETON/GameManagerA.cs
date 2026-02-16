using TMPro;
using UnityEngine;

public class GameManagerA : MonoBehaviour
{
    public static GameManagerA Instance;

    [SerializeField] private TextMeshProUGUI gameTimeText;
    private int gameTimeInMinutes = 5;
    private float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        timer = gameTimeInMinutes * 60f;
        UpdateTimer();
    }

    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimer();
        }
        else
        {
            timer = 0;
            UpdateTimer();
            // Game Over logic here
            Debug.Log("Game Over! Time's up.");
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        gameTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }    
}
