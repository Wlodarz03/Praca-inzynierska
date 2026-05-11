using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerS : MonoBehaviour
{
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject crosshair;
    [SerializeField] private TextMeshProUGUI Score;
    [SerializeField] private Button playPause;
    [SerializeField] private AudioClip backgroundMusic;
    private EnemyFactory enemyFactory;

    [Header("Enemy Spawning Settings")]
    public float spawnInterval = 15f;       // co ile sekund fala
    public int enemiesPerWave = 3;         // ilu wrogów na falę
    public float spawnDelayBetweenEnemies = 4f; // opóźnienie między spawnem poszczególnych wrogów w fali
    private bool spawningActive = true;

    PlayerController player;

    void Awake()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(backgroundMusic);
        }
        enemyFactory = EnemyFactory.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playPause.gameObject.ButtonDown();
            playPause.onClick.Invoke();
        }
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


    public void EndGame()
    {
        FactorySFX.instance.PlayGameOverSFX();
        StopSpawning();
        Time.timeScale = 0f;
        Score.text = "Kills " + player.GetKills();
        Cursor.visible = true;
        AudioManager.Instance.StopNarration();
        AudioManager.Instance.StopMusic();
        crosshair.SetActive(false);
        GameOverUI.SetActive(true);
    }


}