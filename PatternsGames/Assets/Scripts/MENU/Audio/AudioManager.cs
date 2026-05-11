using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup narratorMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;


    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource narratorSource;
    // [SerializeField] private AudioSource sfxSource;
    private AudioSource sfxSource;

    [Header("MainMenu")]
    [SerializeField] private AudioClip mainMenuMusic;

    [Header("Narration")]
    public NarrationData CurrentNarration { get; private set; }

    // EVENTY → UI / napisy / pasek postępu
    public UnityEvent<NarrationData> OnNarrationStarted;
    public UnityEvent OnNarrationStopped;

    public void SetSFXSource(AudioSource source)
    {
        sfxSource = source;
        sfxSource.outputAudioMixerGroup = sfxMixerGroup;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (!musicSource.isPlaying || musicSource.clip != mainMenuMusic)
            {
                musicSource.clip = mainMenuMusic;
                musicSource.loop = true;
                musicSource.Play();
            }
        }
    }

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
        if (musicSource) musicSource.outputAudioMixerGroup = musicMixerGroup;
        if (narratorSource) narratorSource.outputAudioMixerGroup = narratorMixerGroup;
        // if (sfxSource) sfxSource.outputAudioMixerGroup = sfxMixerGroup;
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

    public void SetNarrationVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        float dB = Mathf.Log10(volume) * 20f;
        mainMixer.SetFloat("NarrationVolume", dB);
        Debug.Log("Narration volume set to: " + volume);
        //narratorSource.volume = volume;
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

    public void SetNarrationSpeed(float speed)
    {
        narratorSource.pitch = speed;
        if (speed > 0f)
        {
            float pitchCorrection = 1f / speed;
            pitchCorrection = Mathf.Clamp(pitchCorrection, 0.5f, 2f);
            mainMixer.SetFloat("NarratorPitchFix", pitchCorrection);
        }
        else
        {
            mainMixer.SetFloat("NarratorPitchFix", 1f);
        }
    }

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

    public bool IsPlayingMusic(AudioClip clip)
    {
        return musicSource.isPlaying && musicSource.clip == clip;
    }

    public void SetMusicVolume(float volume)
    {
        mainMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20); // Konwersja na decybele
    }

    public void StopMusic() => musicSource.Stop();

    // ======================
    // SFX
    // ======================
    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.pitch = Mathf.Clamp(pitch, 0.5f, 2f);
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlaySFX(AudioClip clip) => PlaySFX(clip, 1f, 1f);
    // public void PlaySFX(AudioClip clip, float volume) => PlaySFX(clip, volume, 1f);

    // public void SetSFXVolume(float volume)
    // {
    //     mainMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20); // Konwersja na decybele
    // }
}
