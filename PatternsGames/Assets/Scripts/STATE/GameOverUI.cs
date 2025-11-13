using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        scoreText.text = "Twoj wynik: " + gm.score + "\n\n Doszedles do poziomu: " + gm.currentLevel;
    }

    public void PlayButtonHandler()
    {
        Time.timeScale = 1f;

        gameOverUI.SetActive(false);

        gm.StartNewGame();

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}