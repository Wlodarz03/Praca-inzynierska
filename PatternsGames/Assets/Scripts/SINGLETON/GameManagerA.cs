using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerA : MonoBehaviour
{
    public static GameManagerA Instance;

    [SerializeField] private TextMeshProUGUI gameTimeText;
    [SerializeField] private Rabbit rabbit;
    [SerializeField] private Cat cat;
    [SerializeField] private Dog dog;
    [SerializeField] private Hamster hamster;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button play;
    [SerializeField] private Button pause;

    private int gameTimeInMinutes = 5;
    private float timer;
    private int lastTriggeredMinute = -1;

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
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f)
        {
            play.gameObject.ButtonDown();
            play.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1f)
        {
            pause.gameObject.ButtonDown();
            pause.onClick.Invoke();
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int currentMinute = Mathf.FloorToInt(timer / 60);

            if (currentMinute != lastTriggeredMinute && currentMinute < gameTimeInMinutes-1)
            {
                lastTriggeredMinute = currentMinute;
                TriggerRandomEvent();
            }
            UpdateTimer();
        }
        else
        {
            timer = 0;
            UpdateTimer();
            WinGame();
        }
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        gameTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void Storm()
    {
        EnergyManager.instance.SpendEnergy(10);
        EffectManager.instance.SpawnStormEffect();
    }

    // void Charger()
    // {
    //     EnergyManager.instance.AddEnergy(10);
    // }

    void Fireworks()
    {
        EffectManager.instance.SpawnFireworkEffect();
        EnergyManager.instance.AddEnergy(20);
        rabbit.RemoveHumor(10);
        cat.RemoveHumor(10);
        dog.RemoveHumor(10);
        hamster.RemoveHumor(10);
    }

    // void PlayMusic()
    // {
    //     EnergyManager.instance.SpendEnergy(15);
    //     rabbit.AddHumor(15);
    //     cat.AddHumor(15);
    //     dog.AddHumor(15);
    //     hamster.AddHumor(15);
    // }

    private void TriggerRandomEvent()
    {
        int randomAction = Random.Range(0, 2);
        switch (randomAction)
        {
            case 0:
                Storm();
                Debug.Log("Storm action triggered!");
                break;
            case 1:
                Fireworks();
                Debug.Log("Fireworks action triggered!");
                break;
        }
    }

    public void LoseGame()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "Game Over";
    }

    public void WinGame()
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = "You Win!";
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

}
