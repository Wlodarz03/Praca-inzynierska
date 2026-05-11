using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerOP : MonoBehaviour
{
    public static GameManagerOP Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public float currentScore = 0f;
    public bool isPlaying = false;
    public SaveData data;
    [SerializeField] private Button play;
    [SerializeField] private AudioClip backgroundMusic;
    [SerializeField] private AudioSource newRecordSFX;
    [SerializeField] private AudioSource gameOverSFX;

    public UnityEvent onPlay = new UnityEvent();
    public UnityEvent onGameOver = new UnityEvent();

    private void Start()
    {
        string loadedData = SaveSystem.Load("save");
        if (loadedData != null)
        {
            data = JsonUtility.FromJson<SaveData>(loadedData);
        }
        else
        {
            data = new SaveData();
        }
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            play.gameObject.ButtonDown();
            play.onClick.Invoke();
        }

        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }
    }

    public string PreetyScore(float score)
    {
        return Mathf.RoundToInt(score).ToString();
    }

    public void StartGame()
    {
        onPlay.Invoke();
        isPlaying = true;
        currentScore = 0;
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
    }

    public void GameOver()
    {
        if (data.highscore < currentScore)
        {
            data.highscore = currentScore;

            string saveString = JsonUtility.ToJson(data);

            SaveSystem.Save("save", saveString);
            newRecordSFX.PlayOneShot(newRecordSFX.clip);
        }
        else
        {
            gameOverSFX.PlayOneShot(gameOverSFX.clip);
        }

        isPlaying = false;

        onGameOver.Invoke();
        AudioManager.Instance.StopNarration();
        AudioManager.Instance.StopMusic();
    }
}
