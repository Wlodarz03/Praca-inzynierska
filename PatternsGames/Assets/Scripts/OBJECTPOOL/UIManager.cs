using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TextMeshProUGUI gameOverScoreUI;
    [SerializeField] private TextMeshProUGUI gameOverHighscoreUI;
    [SerializeField] private TextMeshProUGUI poolListUI;
    GameManagerOP gm;
    private void Start()
    {
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
