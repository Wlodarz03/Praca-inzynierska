using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Collections;
using UnityEngine.UI;

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
    [SerializeField] private GameObject[] cursorsObjects; // 0: green, 1: blue, 2: red 
    [SerializeField] private GameObject cursor;
    private bool showingCursor = false;
    private Vector2 mousePos;
    [SerializeField] private Button play;
    [SerializeField] private Button pause;

    private NativeHashMap<int, int> costs = new NativeHashMap<int, int>(3, Allocator.Persistent);

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        enemySpawner = GetComponent<EnemySpawner>();
        heart.GetComponent<Animator>().ResetTrigger("Hit");
        costs[0] = 100;
        costs[1] = 500;
        costs[2] = 1000;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0f)
        {
            play.gameObject.ButtonDown();
            play.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1f)
        {
            pause.gameObject.ButtonDown();
            pause.onClick.Invoke();
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursor.transform.position = mousePos;

        float aspect = (float)Screen.width / Screen.height;

        // 16:10 ≈ 1.6
        if (Mathf.Abs(aspect - 1.6f) < 0.05f)
        {
            playerHealthText.rectTransform.anchoredPosition = new Vector3(-108.7f, 358.9f, 0);
        }
        else
        {
            playerHealthText.rectTransform.anchoredPosition = new Vector3(-208.7f, 338.9f, 0);
        }
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

    public void DisableHealth()
    {
        playerHealthText.gameObject.SetActive(false);
    }

    public void EnableHealth()
    {
        playerHealthText.gameObject.SetActive(true);
    }

    public void OnShopButtonClicked(int index)
    {
        if (currency >= costs[index])
        {
            cursor = cursorsObjects[index];
            cursor.SetActive(true);
            Cursor.visible = false;
            showingCursor = true;
        }
    }

    public void OnCellClicked()
    {
        cursor.SetActive(false);
        Cursor.visible = true;
        showingCursor = false;
    }

    public void OnButtonHoverEnter()
    {
        Cursor.visible = true;
        cursor.SetActive(false);
    }

    public void OnButtonHoverExit()
    {
        if (showingCursor)
        {
            Cursor.visible = false;
            cursor.SetActive(true);
        }
    }
}
