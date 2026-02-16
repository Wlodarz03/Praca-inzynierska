using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    public TextMeshProUGUI scoreText;
    GameManager gm;

    public void Start()
    {
        gm = GameManager.Instance;
        gm.onGameOver.AddListener(ActivateGameOverUI);
    }

    public void ActivateGameOverUI()
    {
        gameOverUI.SetActive(true);
        scoreText.text = "Your score: " + gm.score + "\n\n Reached level: " + gm.currentLevel;
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopNarration();
        gm.StartNewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayButtonHandler()
    {
        Time.timeScale = 1f;

        gameOverUI.SetActive(false);

        gm.StartNewGame();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}