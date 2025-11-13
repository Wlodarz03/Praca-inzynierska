using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManagerOP : MonoBehaviour
{
    public static GameManagerOP Instance;
    private void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        if (Instance == null) Instance = this;
    }

    public float currentScore = 0f;
    public bool isPlaying = false;
    public SaveData data;

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
        if (isPlaying)
        {
            currentScore += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
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
    }

    public void GameOver()
    {
        if (data.highscore < currentScore)
        {
            data.highscore = currentScore;

            string saveString = JsonUtility.ToJson(data);

            SaveSystem.Save("save", saveString);
        }
        isPlaying = false;
        onGameOver.Invoke();
    }
}
