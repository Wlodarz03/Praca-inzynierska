using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 5;
    [SerializeField] private float enemiesPerSecond = 0.5f; // z takim enemiesPerSecond i szybkoscia wroga co najwyzej 2f, w ObjectPool max 17 na ekranie na raz
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroyed = new UnityEvent();

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroyed.AddListener(EnemyDestroyed);
    }
    private void Start()
    {
        StartCoroutine(StartWave());
        waveText.text = $"Wave {currentWave}";
    }

    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void Update()
    {
        if (!isSpawning) return;

        waveText.text = $"Wave {currentWave}";
        
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        if (currentWave == 1)
        {
            enemyPrefabs[0].GetComponent<Health>().SetHealth(5);
            Debug.Log("Basic enemy health reset to 5!");
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        // if ((currentWave - 1) % 1 == 0)
        
        if (currentWave != 1)
        {
            enemyPrefabs[0].GetComponent<Health>().IncreaseHealth(3);
            Debug.Log("Increased enemy health!");
        }
        StartCoroutine(StartWave());
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[0];
        Instantiate(prefabToSpawn, TowerDefenseManager.Instance.startPoint.position, Quaternion.identity);
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies *Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}
