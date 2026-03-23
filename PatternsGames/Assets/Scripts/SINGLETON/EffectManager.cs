using Unity.VisualScripting;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    [SerializeField] private GameObject fireworkEffectPrefab;
    [SerializeField] private GameObject rainEffectPrefab;
    [SerializeField] private GameObject boomEffectPrefab;
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform centerSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    [SerializeField] private Transform boomLeft;
    [SerializeField] private Transform boomRight;
    [SerializeField] private Transform rainUp;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnFireworkEffect()
    {
        GameObject leftEffect = Instantiate(fireworkEffectPrefab, leftSpawnPoint.position, Quaternion.identity);
        GameObject centerEffect = Instantiate(fireworkEffectPrefab, centerSpawnPoint.position, Quaternion.identity);
        GameObject rightEffect = Instantiate(fireworkEffectPrefab, rightSpawnPoint.position, Quaternion.identity);

        Destroy(leftEffect, 4f);
        Destroy(centerEffect, 4f);
        Destroy(rightEffect, 4f);
    }

    public void SpawnStormEffect()
    {
        GameObject rainEffect = Instantiate(rainEffectPrefab, rainUp.position, Quaternion.identity);
        GameObject boomLeftEffect = Instantiate(boomEffectPrefab, boomLeft.position, Quaternion.identity);
        GameObject boomRightEffect = Instantiate(boomEffectPrefab, boomRight.position, Quaternion.identity);
        
        Destroy(rainEffect, 3f);
        Destroy(boomLeftEffect, 3f);
        Destroy(boomRightEffect, 3f);
    }
}
