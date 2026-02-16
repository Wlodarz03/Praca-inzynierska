using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject crosshair;

    public void ResetButtonHandler()
    {
        if (gameOverUI.activeSelf)
            gameOverUI.SetActive(false);

        Time.timeScale = 1f;
        Cursor.visible = false;
        crosshair.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnButtonHoverEnter()
    {
        Cursor.visible = true;
        crosshair.SetActive(false);
    }

    public void OnButtonHoverExit()
    {
        Cursor.visible = false;
        crosshair.SetActive(true);
    }
}
