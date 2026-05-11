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
        if (Instance == this)
        {
            Instance = null;
            DontDestroyOnLoad(gameObject);
            Destroy(gameObject);
        }
        
    }

    void Start()
    {
        //AudioManager.Instance.SetSFXSource(GameObject.FindWithTag("Player").GetComponent<AudioSource>());
        try
        {
            play = GameObject.Find("Play").GetComponent<Button>();
        }
        catch
        {
            // Debug.Log("Play button not found");  
        }
        finally
        {
            StartNewGame();
        }
    }

    private void Update()
    {
        if (play == null)
        {
            try
            {
                play = GameObject.Find("Play").GetComponent<Button>();
            }
            catch
            {
                // Debug.Log("Play button not found");
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            try
            {
                play.gameObject.ButtonDown();
                play.onClick.Invoke();
            }
            catch
            {
                // Debug.Log("Play button not found");
            }
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
        isNarrationPlaying = false;
        AudioManager.Instance.StopNarration();
        AudioManager.Instance.StopMusic();
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