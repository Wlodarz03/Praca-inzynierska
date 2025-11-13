using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private string[] obstacleTags;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 2f;
    [Range(0, 1)] public float obstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 1f;
    [Range(0, 1)] public float obstacleSpeedFactor = 0.2f;
    [SerializeField] private float offscreenX = -15f;
    private float _obstacleSpawnTime;
    private float _obstacleSpeed;
    private float timeAlive;
    private float timeUntilObstacleSpawn;

    private List<GameObject> activeObstacles = new List<GameObject>();

    private void Start()
    {
        GameManagerOP.Instance.onGameOver.AddListener(ClearObstacles);
        GameManagerOP.Instance.onPlay.AddListener(ResetFactors);
    }

    void Update()
    {
        if (GameManagerOP.Instance.isPlaying)
        {
            timeAlive += Time.deltaTime;

            CalculateFactors();

            SpawnLoop();

            CheckOffScreenObstacles();
        }
    }

    private void SpawnLoop()
    {
        timeUntilObstacleSpawn += Time.deltaTime;

        if (timeUntilObstacleSpawn >= _obstacleSpawnTime)
        {
            Spawn();
            timeUntilObstacleSpawn = 0f;
        }
    }

    private void ClearObstacles()
    {
        foreach (var obj in activeObstacles)
        {
            if (obj != null)
            {
                ObjectPool.Instance.ReturnToPool(obj.tag, obj);
            }
        }

        activeObstacles.Clear();
    }

    private void CalculateFactors()
    {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors()
    {
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
        activeObstacles.Clear();
    }

    private void Spawn()
    {
        string tag = obstacleTags[Random.Range(0, obstacleTags.Length)];

        GameObject spawnedObstacle = ObjectPool.Instance.GetPooledObject(tag);

        if (spawnedObstacle != null)
        {
            spawnedObstacle.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            spawnedObstacle.transform.localScale = new Vector3(1f, 1f, 1f);
            spawnedObstacle.transform.parent = obstacleParent;
            Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
            obstacleRB.linearVelocity = Vector2.left * _obstacleSpeed;
            activeObstacles.Add(spawnedObstacle);
        }
    }

    private void CheckOffScreenObstacles()
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--){

            GameObject obstacle = activeObstacles[i];

            if (obstacle == null) continue;

            if (obstacle.transform.position.x < offscreenX)
            {
                ObjectPool.Instance.ReturnToPool(obstacle.tag, obstacle);
                activeObstacles.RemoveAt(i);
            }
        }
    }
}
