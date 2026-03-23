using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] private GameObject starsEffect;

    void Start()
    {
        Cursor.visible = true;
        var effect = Instantiate(starsEffect, Vector3.zero, Quaternion.identity);
        effect.transform.position = new Vector3(746f, 354f, 0f);
    }

    public void StartStateGame()
    {
        //GameManager.Instance.StartNewGame();
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

    public void StartSingletonGame(){
        SceneManager.LoadScene(7);
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
