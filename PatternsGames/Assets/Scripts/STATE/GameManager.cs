using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel = 1;
    public int score = 0;
    public bool isPlaying;
    public bool isNarrationPlaying = false;
    public UnityEvent onGameOver = new UnityEvent();
    private Button play;
    private Button pause;

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

    public void DestroyGameManager()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        play = GameObject.Find("Play").GetComponent<Button>();
        pause = GameObject.Find("Pause").GetComponent<Button>();

        StartNewGame();
    }

    private void Update()
    {
        if (play == null || pause == null)
        {
            play = GameObject.Find("Play").GetComponent<Button>();
            pause = GameObject.Find("Pause").GetComponent<Button>();
        }

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
    }

    public void NextLevel()
    {
        currentLevel++;
        score += 100;
    }

    public void GameOver()
    {
        isPlaying = false;
        ToggleNarration();
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

    public void ToggleNarration()
    {
        isNarrationPlaying = !isNarrationPlaying;
    }

}