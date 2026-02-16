using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using UnityEngine.UIElements;

public class NarrationUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text currentTimeText;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private Sprite[] iconsButton;
    [SerializeField] private Button playPauseButton;

    private AudioManager audioManager;
    private SubtitlePlayer subtitlePlayer;
    private Image buttonImg;
    private bool isPaused;
    private bool isDraggingSlider;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
        subtitlePlayer = GetComponent<SubtitlePlayer>();
        buttonImg = playPauseButton.GetComponent<Image>();
        buttonImg.sprite = iconsButton[0];
    }

    private void OnEnable()
    {
        if (audioManager == null)
            audioManager = AudioManager.Instance;
        audioManager.OnNarrationStarted.AddListener(OnNarrationStarted);
        audioManager.OnNarrationStopped.AddListener(OnNarrationStopped);
    }

    private void OnDisable()
    {
        audioManager.OnNarrationStarted.RemoveListener(OnNarrationStarted);
        audioManager.OnNarrationStopped.RemoveListener(OnNarrationStopped);
    }

    public void OnSliderDragStart()
    {
        isDraggingSlider = true;
    }

    public void OnSliderDragEnd()
    {
        isDraggingSlider = false;
        float targetTime = progressSlider.value * audioManager.GetNarrationLength();
        audioManager.SetNarrationTime(targetTime);
        subtitlePlayer.Seek(targetTime);
    }

    private void Update()
    {
        if (!audioManager || audioManager.GetNarrationLength() <= 0f)
            return;

        if (isDraggingSlider)
            return;

        float time = audioManager.GetNarrationTime();
        float length = audioManager.GetNarrationLength();

        if (time >= length)
            audioManager.StopNarration();
            
       
        //progressSlider.value = time / length;
        progressSlider.SetValueWithoutNotify(time / length);
        currentTimeText.text = FormatTime(time);
        totalTimeText.text = FormatTime(length);
    }

    private void OnNarrationStarted(NarrationData data)
    {
        root.SetActive(true);
        isPaused = false;

        subtitlePlayer.Load(data.subtitlesSRT);
    }

    private void OnNarrationStopped()
    {
        root.SetActive(false);
        subtitleText.text = "";
    }

    public void TogglePlayPause()
    {
        if (isPaused)
        {
            audioManager.ResumeNarration();
            buttonImg.sprite = iconsButton[0];
        }
        else
        {
            audioManager.PauseNarration();
            buttonImg.sprite = iconsButton[1];
        }

        isPaused = !isPaused;
    }

    public void Forward5Seconds()
    {
        audioManager.AddNarrationTime(5f);
        subtitlePlayer.Seek(audioManager.GetNarrationTime());
    }

    public void Rewind5Seconds()
    {
        audioManager.AddNarrationTime(-5f);
        subtitlePlayer.Seek(audioManager.GetNarrationTime());
    }

    public void Seek(float normalizedValue)
    {
        float targetTime = normalizedValue * audioManager.GetNarrationLength();
        audioManager.SetNarrationTime(targetTime);
        subtitlePlayer.Seek(targetTime);
    }

    private string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:00}:{sec:00}";
    }
}
