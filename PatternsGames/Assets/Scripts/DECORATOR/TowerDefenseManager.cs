using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using TMPro;

public class TowerDefenseManager : MonoBehaviour
{
    public static TowerDefenseManager Instance;

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private Canvas otherCanvas;
    [SerializeField] private TextMeshProUGUI wavesText;
    public Transform[] path;
    public Transform startPoint;
    public GameObject towerOptionsUI;

    [SerializeField] private EnemySpawner enemySpawner;
    public int currency;
    public int playerHealth;
    public TextMeshProUGUI playerHealthText;
    [SerializeField] private GameObject heart;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        enemySpawner = GetComponent<EnemySpawner>();
        heart.GetComponent<Animator>().ResetTrigger("Hit");
    }
    
    private void GameOver()
    {
        Time.timeScale = 0f;
        AudioManager.Instance.StopNarration();
        wavesText.text = "GAME OVER!\n\nYOU SURVIVED:\n" + enemySpawner.GetCurrentWave() + "\nWAVES";
        mainCanvas.gameObject.SetActive(false);
        otherCanvas.gameObject.SetActive(true);
    }
    public void PlayerGetHit()
    {
        playerHealth -= 1;
        playerHealthText.text = playerHealth.ToString() + " / 100";
        heart.GetComponent<Animator>().SetTrigger("Hit");

        if (playerHealth <= 0)
        {
            Debug.Log("Game Over!"); // TODO: Show Game Over UI, Survived: X waves
            GameOver();
        }
    }
    public void ResetGame()
    {
        // mainCanvas.gameObject.SetActive(true);
        // otherCanvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Start()
    {
        currency = 100; 
        playerHealth = 100;
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
    }

    public void SpendCurrency(int amount)
    {
        if (currency >= amount)
        {
            // Buy tower or upgrade
            currency -= amount;
        }
        else
        {
            Debug.Log("Not enough currency!");
        }
    }
}
