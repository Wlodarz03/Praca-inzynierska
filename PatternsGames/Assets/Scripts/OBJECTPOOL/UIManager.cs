using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("GameOver UI Elements")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI;

    [Header("In-Game UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private TextMeshProUGUI poolListUI;

    Spawner spawner;
    GameManagerOP gm;
    private void Start()
    {
        spawner = Spawner.Instance;
        gm = GameManagerOP.Instance;
        gm.onGameOver.AddListener(ActivateGameOverUI);
    }

    public void PlayButtonHandler()
    {
        gm.StartGame();
        gameOverUI.SetActive(false);
    }

    public void ActivateGameOverUI()
    {
        poolListUI.text = "";
        gameOverUI.SetActive(true);
        gameOverScoreUI.text = "Score: " + gm.PreetyScore(gm.currentScore);
        gameOverHighscoreUI.text = "Highscore: " + gm.PreetyScore(gm.data.highscore);
    }

    public void ResetButtonHandler()
    {
        Time.timeScale = 1f;
        spawner.ClearObstacles();
        gm.StartGame();
    }

    private void Update()
    {
        scoreUI.text = gm.PreetyScore(gm.currentScore); 

        if (poolListUI != null && ObjectPool.Instance != null && gm.isPlaying)
        {
            poolListUI.text = "OBJECT POOL:\n";
            var inactive = ObjectPool.Instance.GetInactiveObjectNames();
            poolListUI.text += inactive.Count > 0 ? string.Join("\n", inactive) : "pool empty";
        }
    }
}
