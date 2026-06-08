using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoStarter : MonoBehaviour
{
    private bool pressed = false;
    [SerializeField] private GameObject infoPanel;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (GameManager.Instance.currentLevel == 1)
            {
                infoPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else
        {
            infoPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !pressed)
        {
            infoPanel.SetActive(false);
            Time.timeScale = 1f;
            pressed = true;
        }
    }
}
