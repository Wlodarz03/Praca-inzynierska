using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource narratorSource;
    // [SerializeField] private AudioSource sfxSource;

    [Header("Narration")]
    public NarrationData CurrentNarration { get; private set; }

    // EVENTY → UI / napisy / pasek postępu
    public UnityEvent<NarrationData> OnNarrationStarted;
    public UnityEvent OnNarrationStopped;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log("AudioManager initialized.");
        DontDestroyOnLoad(gameObject);
    }

    // ======================
    // NARRATOR
    // ======================
    public void PlayNarration(NarrationData narration)
    {
        if (narration == null || narration.audioClip == null)
        {
            Debug.LogWarning("NarrationData is null or has no AudioClip");
            return;
        }

        CurrentNarration = narration;

        narratorSource.clip = narration.audioClip;
        narratorSource.time = 0f;
        narratorSource.Play();

        OnNarrationStarted?.Invoke(narration);
    }

    public void PauseNarration() => narratorSource.Pause();
    public void ResumeNarration() => narratorSource.UnPause();

    public void StopNarration()
    {
        narratorSource.Stop();
        CurrentNarration = null;
        OnNarrationStopped?.Invoke();
    }

    public void SetNarrationTime(float time) => narratorSource.time = time;

    public void AddNarrationTime(float delta)
    {
        narratorSource.time += delta;
    } 

    public float GetNarrationTime() => narratorSource.time;

    public AudioSource GetNarratorSource()
    {
        return narratorSource;
    }
    public float GetNarrationLength() =>
        narratorSource.clip != null ? narratorSource.clip.length : 0f;

    // ======================
    // MUZYKA
    // ======================
    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic() => musicSource.Stop();

    // ======================
    // SFX
    // ======================
    // public void PlaySFX(AudioClip clip)
    // {
    //     sfxSource.PlayOneShot(clip);
    // }
}
