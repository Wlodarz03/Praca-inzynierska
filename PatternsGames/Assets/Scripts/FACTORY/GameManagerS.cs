using System.Collections;
using UnityEngine;

public class GameManagerS : MonoBehaviour
{
    public static GameManagerS Instance;
    private EnemyFactory enemyFactory;

    [Header("Enemy Spawning Settings")]
    public float spawnInterval = 5f;       // co ile sekund fala
    public int enemiesPerWave = 3;         // ilu wrogów na falę
    public float spawnDelayBetweenEnemies = 1f; // opóźnienie między spawnem poszczególnych wrogów w fali
    private bool spawningActive = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        enemyFactory = EnemyFactory.Instance;
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
}