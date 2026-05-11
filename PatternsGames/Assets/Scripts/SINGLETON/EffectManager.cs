using System.Collections;
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
    [SerializeField] private AudioSource fireworkSound;
    [SerializeField] private AudioSource stormSound;
    [SerializeField] private GameObject gameOverUI;

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
        SpawnAndDestroyUnscaled(fireworkEffectPrefab, leftSpawnPoint.position, 4f);
        SpawnAndDestroyUnscaled(fireworkEffectPrefab, centerSpawnPoint.position, 4f);
        SpawnAndDestroyUnscaled(fireworkEffectPrefab, rightSpawnPoint.position, 4f);

        fireworkSound.PlayOneShot(fireworkSound.clip);
    }

    public void SpawnStormEffect()
    {
        SpawnAndDestroyUnscaled(rainEffectPrefab, rainUp.position, 3f);
        SpawnAndDestroyUnscaled(boomEffectPrefab, boomLeft.position, 3f);
        SpawnAndDestroyUnscaled(boomEffectPrefab, boomRight.position, 3f);

        stormSound.PlayOneShot(stormSound.clip);
    }

    // Nowa pomocnicza metoda
    private void SpawnAndDestroyUnscaled(GameObject prefab, Vector3 position, float lifetime)
    {
        GameObject effect = Instantiate(prefab, position, Quaternion.identity);
        
        // To jest klucz!
        StartCoroutine(DestroyAfterUnscaledTime(effect, lifetime));
    }

    private IEnumerator DestroyAfterUnscaledTime(GameObject obj, float delay)
    {
        float timer = 0f;
        
        while (timer < delay)
        {
            timer += Time.unscaledDeltaTime;   // <--- to ignoruje Time.timeScale
            yield return null;
        }
        
        if (obj != null)
            Destroy(obj);
    }

    // public void SpawnFireworkEffect()
    // {
    //     GameObject leftEffect = Instantiate(fireworkEffectPrefab, leftSpawnPoint.position, Quaternion.identity);
    //     GameObject centerEffect = Instantiate(fireworkEffectPrefab, centerSpawnPoint.position, Quaternion.identity);
    //     GameObject rightEffect = Instantiate(fireworkEffectPrefab, rightSpawnPoint.position, Quaternion.identity);

    //     Destroy(leftEffect, 4f);
    //     Destroy(centerEffect, 4f);
    //     Destroy(rightEffect, 4f);
    //     fireworkSound.PlayOneShot(fireworkSound.clip);
    // }

    // public void SpawnStormEffect()
    // {
    //     GameObject rainEffect = Instantiate(rainEffectPrefab, rainUp.position, Quaternion.identity);
    //     GameObject boomLeftEffect = Instantiate(boomEffectPrefab, boomLeft.position, Quaternion.identity);
    //     GameObject boomRightEffect = Instantiate(boomEffectPrefab, boomRight.position, Quaternion.identity);
        
    //     Destroy(rainEffect, 3f);
    //     Destroy(boomLeftEffect, 3f);
    //     Destroy(boomRightEffect, 3f);
    //     stormSound.PlayOneShot(stormSound.clip);
    // }
}
