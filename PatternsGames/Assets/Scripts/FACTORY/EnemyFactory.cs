using UnityEngine;
using System.Collections.Generic;

public class EnemyFactory : MonoBehaviour
{
    public enum EnemyType { Zombie, Skeleton, Alien }
    public static EnemyFactory Instance;
    public Transform[] spawnPoints;

    [Header("Enemy Prefabs")]
    public GameObject zombiePrefab;
    public GameObject skeletonPrefab;
    public GameObject alienPrefab;

    [Header("Pooling")]
    public int PoolperEnemyType = 10;
    public Transform poolParent;
    public Transform spawnParent; 

    Dictionary<EnemyType, Queue<GameObject>> enemyPools = new Dictionary<EnemyType, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        foreach (EnemyType t in System.Enum.GetValues(typeof(EnemyType)))
        {
            enemyPools[t] = new Queue<GameObject>();
        }

        PrewarmPool(EnemyType.Zombie, zombiePrefab, PoolperEnemyType);
        PrewarmPool(EnemyType.Skeleton, skeletonPrefab, PoolperEnemyType);
        PrewarmPool(EnemyType.Alien, alienPrefab, PoolperEnemyType);
    }

    void PrewarmPool(EnemyType type, GameObject prefab, int count)
    {
        var q = enemyPools[type];
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, poolParent);
            obj.SetActive(false);
            q.Enqueue(obj);
        }
    }

    public GameObject GetFromPool(EnemyType type)
    {
        var q = enemyPools[type];
        if (q.Count > 0)
        {
            GameObject obj = q.Dequeue();
            obj.transform.SetParent(spawnParent, true);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject prefab = PrefabFor(type);
            GameObject obj = Instantiate(prefab, spawnParent, true);
            obj.SetActive(true);
            return obj;
        }
    }

    public void ReturnToPool(EnemyType type, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
        enemyPools[type].Enqueue(obj);
    }

    GameObject PrefabFor(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Zombie:
                return zombiePrefab;
            case EnemyType.Skeleton:
                return skeletonPrefab;
            case EnemyType.Alien:
                return alienPrefab;
            default:
                return zombiePrefab;
        }
    }

    public GameObject CreateEnemy(EnemyType type)
    {
        var go = GetFromPool(type);
        var enemy = go.GetComponent<BaseEnemy>();

        enemy.EnemyType = type;
        enemy.enemyFactory = Instance;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        go.transform.position = spawnPoint.position;

        enemy.Initialize();
        return go;
    }
}
