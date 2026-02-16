using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyFactory : MonoBehaviour
{
    public enum EnemyType { Zombie, Skeleton, Alien }
    public static EnemyFactory Instance;
    public Transform[] spawnPoints;
    public Transform[] BeltPoints;
    private Dictionary<GameObject, Coroutine> activeBeltCoroutines = new Dictionary<GameObject, Coroutine>();

    [Header("Enemy Prefabs")]
    public GameObject zombiePrefab;
    public GameObject skeletonPrefab;
    public GameObject alienPrefab;

    [Header("Pooling")]
    public int PoolperEnemyType = 10;
    public Transform poolParent;
    public Transform spawnParent; 
    public float speedBelt;

    Dictionary<EnemyType, Queue<GameObject>> enemyPools = new Dictionary<EnemyType, Queue<GameObject>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        foreach (EnemyType t in System.Enum.GetValues(typeof(EnemyType)))
        {
            enemyPools[t] = new Queue<GameObject>();
        }

        PrewarmPool(EnemyType.Zombie, zombiePrefab, PoolperEnemyType);
        PrewarmPool(EnemyType.Skeleton, skeletonPrefab, PoolperEnemyType);
        PrewarmPool(EnemyType.Alien, alienPrefab, PoolperEnemyType);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
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
        StopBeltMovement(obj);

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

    public void StartBeltMovement(GameObject o, int spawnIndex)
    {
        if (o == null) return;
        if (activeBeltCoroutines.TryGetValue(o, out var existing) && existing != null)
        {
            StopCoroutine(existing);
            activeBeltCoroutines.Remove(o);
        }

        var c = StartCoroutine(BeltCoroutine(o, spawnIndex, speedBelt));
        activeBeltCoroutines[o] = c;
    }

    public void StopBeltMovement(GameObject o)
    {
        if (o == null) return;
        if (activeBeltCoroutines.TryGetValue(o, out var existing) && existing != null)
        {
            StopCoroutine(existing);
            activeBeltCoroutines.Remove(o);
        }
    }

    public void StopAllBeltMovements()
    {
        foreach (var kv in new List<KeyValuePair<GameObject, Coroutine>>(activeBeltCoroutines))
        {
            var c = kv.Value;
            if (c != null)
                StopCoroutine(c);
        }
        activeBeltCoroutines.Clear();
        StopAllCoroutines();
    }
    
    private IEnumerator BeltCoroutine(GameObject o, int spawnIndex, float speed)
    {
        if (o == null || BeltPoints == null || BeltPoints.Length < 3 + spawnIndex || spawnPoints == null || spawnPoints.Length == 0)
            yield break;
        
        if (activeBeltCoroutines.ContainsKey(o) && activeBeltCoroutines[o] == null)
            activeBeltCoroutines.Remove(o);

        var be = o.GetComponent<BaseEnemy>();
        if (be != null) be.onBelt = true;

        Vector3[] path = new Vector3[]
        {
            BeltPoints[0].position,
            BeltPoints[1].position,
            BeltPoints[2 + spawnIndex].position,
            spawnPoints[spawnIndex].position
        };

        Rigidbody2D rb = o.GetComponent<Rigidbody2D>();

        for (int i = 0; i < path.Length; i++)
        {
            Vector3 target = path[i];
            while (o != null &&(o.transform.position - target).sqrMagnitude > 0.0001f)
            {
                Vector3 newPos = Vector3.MoveTowards(o.transform.position, target, speed * Time.deltaTime);
                if (rb != null) rb.MovePosition(newPos);
                else o.transform.position = newPos;
                yield return null;
            }
        }

        if (be != null)
        {
            be.onBelt = false;
            be.Initialize();
        }
    }

    public GameObject CreateEnemy(EnemyType type)
    {
        var go = GetFromPool(type);
        var enemy = go.GetComponent<BaseEnemy>();

        enemy.EnemyType = type;
        enemy.enemyFactory = Instance;

        int spawnIndex = Random.Range(0, spawnPoints.Length);

        go.transform.position = BeltPoints[0].position;
        // Transform spawnPoint = spawnPoints[spawnIndex];
        // go.transform.position = spawnPoint.position;
        StartBeltMovement(go, spawnIndex);

        // enemy.Initialize();
        return go;
    }

    public void ResetFactory()
    {
        StopAllBeltMovements();

        StopAllCoroutines();
        
        activeBeltCoroutines.Clear();

        foreach (var pool in enemyPools.Values)
        {
            while (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();
                Destroy(obj);
            }
        }
        
        foreach (Transform child in spawnParent)
        {
            Destroy(child.gameObject);
        }

        enemyPools.Clear();

        foreach (EnemyType t in System.Enum.GetValues(typeof(EnemyType)))
        {
            enemyPools[t] = new Queue<GameObject>();
        }

        PrewarmPool(EnemyType.Zombie, zombiePrefab, PoolperEnemyType);
        PrewarmPool(EnemyType.Skeleton, skeletonPrefab, PoolperEnemyType);
        PrewarmPool(EnemyType.Alien, alienPrefab, PoolperEnemyType);

    }
}
