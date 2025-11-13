using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Weapon weapon;
    private float fireRate = 0.5f;
    public float damage = 1f;
    private int killsToLevelUp = 10;
    private int killCount = 0;

    private float fireTimer;
    private Vector2 moveDirection;
    private Vector2 mousePosition;

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
    }

    private void OnEnemyKilled()
    {
        killCount++;

        if (killCount % killsToLevelUp == 0)
        {
            fireRate = Mathf.Max(0.1f, fireRate - 0.02f); // szybsze strzelanie
            damage += 0.2f;                               // większe obrażenia
            Debug.Log($"Level up! FireRate: {fireRate}, Damage: {damage}");
        }

        if (killCount % 20 == 0)
        {
            GameManagerS.Instance.enemiesPerWave += 1;
            moveSpeed += 0.5f;
        }
    }
}
