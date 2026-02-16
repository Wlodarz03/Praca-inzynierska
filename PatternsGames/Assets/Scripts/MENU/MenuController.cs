using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartStateGame()
    {
        GameManager.Instance.StartNewGame();
        SceneManager.LoadScene(1);
    }

    public void StartCommandGame()
    {
        SceneManager.LoadScene(2);
    }

    public void StartObjectPoolGame()
    {
        SceneManager.LoadScene(3);
    }

    public void StartFactoryGame()
    {
        SceneManager.LoadScene(4);
    }

    public void StartObserverGame()
    {
        SceneManager.LoadScene(5);
    }

    public void StartDecoratorGame()
    {
        SceneManager.LoadScene(6);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
