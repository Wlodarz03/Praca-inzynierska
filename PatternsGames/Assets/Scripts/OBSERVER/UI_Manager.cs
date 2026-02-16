using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    public GameObject gameOverUI;
    public GameObject buttons;
    public GameObject LevelUI;
    public TextMeshProUGUI finalTime;
    private float t;
    private int minutes;
    private int seconds;
    private bool isChosen = false;
    private GridManager gm;

    void Start()
    {
        gm = GridManager.Instance;
    }

    void Update()
    {
        t = GameManagerO.Instance.timer;
        minutes = Mathf.FloorToInt(t / 60f);
        seconds = Mathf.FloorToInt(t % 60f);

        if (!GameManagerO.Instance.isFinished && isChosen)
        {
            timer.text = $"Time: {minutes:00}:{seconds:00}";
        }
        else if (isChosen)
        {
            finalTime.text = $"Your time: {minutes} minutes {seconds} seconds";
            Time.timeScale = 0f;
            gameOverUI.SetActive(true);
        }

    }

    public void ResetButton()
    {
        if (gameOverUI.activeSelf)
            gameOverUI.SetActive(false);

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
    }

    public void EasyButton()
    {
        gm.width = 5;
        gm.height = 5;
        gm.spacing = 1.4f;
        gm.tilePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
        LevelUI.SetActive(false);
        buttons.SetActive(true);
        gm.GridPrep();
        isChosen = true;
        t = 0f;
    }

    public void HardButton()
    {
        gm.width = 8;
        gm.height = 8;
        gm.spacing = 1f;
        gm.randomizeSteps = 24;
        gm.tilePrefab.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        LevelUI.SetActive(false);
        buttons.SetActive(true);
        gm.GridPrep();
        isChosen = true;
        t = 0f;
    }
}