using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsHandler : MonoBehaviour
{

    [SerializeField] private CodePanelManager panelManager;
    [SerializeField] private PatternContext context;
    [SerializeField] private NarrationData narration;
    [SerializeField] private GameObject mainCamera;

    // ResetButton specjalny dla każdej gry
    
    public void MenuButtonHandler()
    {
        Cursor.visible = true;
        AudioManager.Instance.StopNarration();
        SceneManager.LoadScene(0);
    }

    public void CodeButtonHandler()
    {
        AudioManager.Instance.StopNarration();
        Cursor.visible = true;
        Time.timeScale = 0f;
        panelManager.ShowCode(context.patternCode);
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
}