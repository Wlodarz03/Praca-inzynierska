using System.Collections;
using TMPro;
using UnityEngine;

public class GameManagerS : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI Score;
    private EnemyFactory enemyFactory;

    [Header("Enemy Spawning Settings")]
    public float spawnInterval = 15f;       // co ile sekund fala
    public int enemiesPerWave = 3;         // ilu wrogów na falę
    public float spawnDelayBetweenEnemies = 4f; // opóźnienie między spawnem poszczególnych wrogów w fali
    private bool spawningActive = true;

    PlayerController player;

    void Awake()
    {
        enemyFactory = EnemyFactory.Instance;
    }

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        if (enemyFactory == null)
            return;

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (spawningActive)
        {
            yield return StartCoroutine(SpawnWave());
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator SpawnWave()
    {
        for (int i = 0; i < enemiesPerWave; i++)
        {
            EnemyFactory.EnemyType type = (EnemyFactory.EnemyType)Random.Range(0, System.Enum.GetValues(typeof(EnemyFactory.EnemyType)).Length);
            enemyFactory.CreateEnemy(type);
            yield return new WaitForSeconds(spawnDelayBetweenEnemies);
        }
    }

    public void StopSpawning()
    {
        spawningActive = false;
        StopAllCoroutines();
    }

    // public void StartNewGame()
    // {
    //     crosshair.SetActive(true);
    //     Cursor.visible = false;
    //     StopSpawning();
    //     enemyFactory.ResetFactory();
    //     player.ResetAtttributes();
    //     spawningActive = true;
    //     Time.timeScale = 1f;
    //     StartCoroutine(SpawnWaves());
    // }

    public void EndGame()
    {
        StopSpawning();
        Time.timeScale = 0f;
        Score.text = "Kills: " + player.GetKills();
        Cursor.visible = true;
        crosshair.SetActive(false);
        GameOverUI.SetActive(true);
    }

    // public void ResetGame()
    // {
    //     Time.timeScale = 1f;
    //     GameOverUI.SetActive(false);
    //     //StartNewGame();
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }

    // public void BackToMenu()
    // {
    //     EnemyFactory.Instance.StopAllBeltMovements();
    //     EnemyFactory.Instance.ResetFactory();
    //     Cursor.visible = true;
    //     GameOverUI.SetActive(false);
    //     Time.timeScale = 1f;
    //     SceneManager.LoadScene(0);
    // }
}