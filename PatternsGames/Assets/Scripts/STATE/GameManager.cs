using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel = 1;
    public int score = 0;
    public bool isPlaying;
    public UnityEvent onGameOver = new UnityEvent();

    private void Awake()
    {
        Time.timeScale = 1f;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     SceneManager.LoadScene(0);
        // }
    }

    public void NextLevel()
    {
        currentLevel++;
        score += 100;
    }

    public void GameOver()
    {
        isPlaying = false;
        AudioManager.Instance.StopNarration();
        onGameOver.Invoke();
        Time.timeScale = 0f;
    }

    public void StartNewGame()
    {
        isPlaying = true;
        score = 0;
        currentLevel = 1;
        Time.timeScale = 1f;
    }

}