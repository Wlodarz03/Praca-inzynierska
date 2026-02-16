using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject levelUpUI;
    private float fireRate = 0.5f;
    public float damage = 1f;
    private int killsToLevelUp = 10;
    private int killCount = 0;

    private GameManagerS gm;
    private float fireTimer;
    private Vector2 moveDirection;
    private Vector2 mousePosition;
    private float LevelUpTimer = 1.5f;

    void Start()
    {
        gm = FindFirstObjectByType<GameManagerS>();
    }

    void OnEnable()
    {
        if (Application.isPlaying)
            GameEvents.OnEnemyKilled += OnEnemyKilled;
    }

    void OnDisable()
    {
        GameEvents.OnEnemyKilled -= OnEnemyKilled;       
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        if (levelUpUI.activeSelf)
        {
            LevelUpTimer -= Time.deltaTime;
            if (LevelUpTimer <= 0f)
            {
                levelUpUI.SetActive(false);
                LevelUpTimer = 1.5f;
            }
        }

        if (Input.GetMouseButton(0) && fireTimer <= 0f)
        {
            weapon.Fire();
            fireTimer = fireRate;
        }
        else
        {
            fireTimer -= Time.deltaTime;
        }

        moveDirection = new Vector2(moveX, 0f).normalized;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;

        Vector2 clampedPosition = rb.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -Camera.main.orthographicSize * Camera.main.aspect + 0.5f, Camera.main.orthographicSize * Camera.main.aspect - 0.5f);
        rb.position = clampedPosition;
    }

    private void OnEnemyKilled()
    {
        killCount++;

        if (killCount % killsToLevelUp == 0)
        {
            fireRate = Mathf.Max(0.1f, fireRate - 0.02f); // szybsze strzelanie
            damage += 0.2f;
            levelUpUI.SetActive(true);
            Debug.Log($"Level up! FireRate: {fireRate}, Damage: {damage}");
        }

        if (killCount % 30 == 0)
        {
            gm.enemiesPerWave += 1;
            moveSpeed += 0.5f;
        }
        if (killCount % 50 == 0)
        {
            gm.spawnInterval -= 1;
        }
    }

    public void ResetAtttributes()
    {
        fireRate = 0.5f;
        damage = 1f;
        moveSpeed = 7f;
        killCount = 0;
    }

    public int GetKills()
    {
        return killCount;
    }
}
