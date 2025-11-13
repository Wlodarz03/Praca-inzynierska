using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private Dictionary<GameObject, int> objectToInstanceId = new Dictionary<GameObject, int>();
    private Dictionary<string, int> poolNextId = new Dictionary<string, int>();

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        objectToInstanceId.Clear();
        poolNextId.Clear();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            poolNextId[pool.tag] = 1;

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                obj.transform.parent = transform;

                int id = poolNextId[pool.tag]++;
                objectToInstanceId[obj] = id;
                Transform textChild = obj.transform.Find("Text");
                if (textChild != null)
                {
                    TextMeshPro tmp = textChild.GetComponent<TextMeshPro>();
                    if (tmp != null)
                    {
                        tmp.text = $"{pool.tag} {id}";
                    }
                }

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetPooledObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        Queue<GameObject> poolQueue = poolDictionary[tag];

        // roszerzanie puli
        if (poolQueue.Count == 0)
        {
            Pool poolInfo = pools.Find(p => p.tag == tag);
            GameObject newObj = Instantiate(poolInfo.prefab);
            newObj.SetActive(false);
            newObj.transform.parent = transform;

            int id = poolNextId[tag]++;
            objectToInstanceId[newObj] = id;
            Transform textChild = newObj.transform.Find("Text");
            if (textChild != null)
            {
                TextMeshPro tmp = textChild.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = $"{tag} {id}";
                }
            }

            poolQueue.Enqueue(newObj);
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public void ReturnToPool(string tag, GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.parent = transform;
        if (poolDictionary.ContainsKey(tag))
        {
            poolDictionary[tag].Enqueue(obj);
        }
        else
        {
            Destroy(obj); // na wszelki
        }
    }

    public void ResetPool()
    {
        foreach (var k in poolDictionary)
        {
            foreach (var obj in k.Value)
            {
                obj.SetActive(false);
            }
        }
    }

    public List<string> GetInactiveObjectNames()
    {
        List<string> names = new List<string>();

        foreach (var pair in poolDictionary)
        {
            string tag = pair.Key;
            Queue<GameObject> queue = pair.Value;

            foreach (var obj in queue)
            {
                if (obj == null) continue;

                int id = objectToInstanceId.ContainsKey(obj) ? objectToInstanceId[obj] : -1;
                if (id != -1)
                {
                    names.Add($"{tag} {id}");
                }
            }
        }

        return names;
    }
}