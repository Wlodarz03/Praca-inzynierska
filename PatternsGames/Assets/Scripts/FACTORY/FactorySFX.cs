using UnityEngine;

public class FactorySFX : MonoBehaviour
{
    public static FactorySFX instance;
    [SerializeField] private AudioSource bulletAudioSource;
    [SerializeField] private AudioSource zombieDeathAudioSource;
    [SerializeField] private AudioSource skeletonDeathAudioSource;
    [SerializeField] private AudioSource alienDeathAudioSource;
    [SerializeField] private AudioSource levelUpAudioSource;
    [SerializeField] private AudioSource gameOverAudioSource;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBulletSFX()
    {
        bulletAudioSource.PlayOneShot(bulletAudioSource.clip);
    }

    public void PlayZombieDeathSFX()
    {
        zombieDeathAudioSource.PlayOneShot(zombieDeathAudioSource.clip);
    }

    public void PlaySkeletonDeathSFX()
    {
        skeletonDeathAudioSource.PlayOneShot(skeletonDeathAudioSource.clip);
    }

    public void PlayAlienDeathSFX()
    {
        alienDeathAudioSource.PlayOneShot(alienDeathAudioSource.clip);
    }

    public void PlayLevelUpSFX()
    {
        levelUpAudioSource.PlayOneShot(levelUpAudioSource.clip);
    }

    public void PlayGameOverSFX()
    {
        gameOverAudioSource.PlayOneShot(gameOverAudioSource.clip);
    }
}
