using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonsHandler : MonoBehaviour
{

    [SerializeField] private CodePanelManager panelManager;
    [SerializeField] private PatternContext context;
    [SerializeField] private NarrationData narration;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

    // ResetButton specjalny dla każdej gry

    void Start()
    {
        image.sprite = sprites[0];
    }
    
    public void MenuButtonHandler()
    {
        Cursor.visible = true;
        AudioManager.Instance.StopNarration();
        AudioManager.Instance.StopMusic();
        SceneManager.LoadScene(0);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DestroyGameManager();
        }
    }

    public void CodeButtonHandler()
    {
        AudioManager.Instance.StopNarration();
        Time.timeScale = 0f;
        panelManager.ShowCode(context.patternCode);
        if (Cursor.visible == false)
        {
            Cursor.visible = true;
        }
    }

    public void NarrationButtonHandler()
    {
        if (AudioManager.Instance.CurrentNarration == narration)
        {
            AudioManager.Instance.StopNarration();
        }
        else
        {
            AudioManager.Instance.PlayNarration(narration);
        }
    }

    public void ObserverNarrationButtonHandler()
    {
        if (AudioManager.Instance.CurrentNarration == narration)
        {
            Camera x = mainCamera.GetComponent<Camera>();
            x.orthographicSize = 5;
            mainCamera.transform.position = new Vector3(0, 0, -10);
            AudioManager.Instance.StopNarration();
        }
        else
        {
            Camera x = mainCamera.GetComponent<Camera>();
            x.orthographicSize = 7;
            mainCamera.transform.position = new Vector3(0, -1.5f, -10);
            AudioManager.Instance.PlayNarration(narration);
        }
    }

    public void PlayPauseButton()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            image.sprite = sprites[1];
        }
        else
        {
            Time.timeScale = 1f;
            image.sprite = sprites[0];
        }
    }

    public void PauseGameButton(){
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
        
    }

    public void ResumeGameButton(){
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
    }

    public void ToggleNarration()
    {
        GameManager.Instance.ToggleNarration();
    }
}