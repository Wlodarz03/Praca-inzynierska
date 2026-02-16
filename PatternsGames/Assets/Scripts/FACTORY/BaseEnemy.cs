using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour, IEnemy
{
    public EnemyFactory.EnemyType EnemyType { get; set; }
    protected float health;
    protected float moveSpeed;
    protected float rotationSpeed;
    protected Transform target;
    protected Rigidbody2D rb;
    public EnemyFactory enemyFactory;
    private GameManagerS gm;
    public bool onBelt;

    public abstract void Initialize();

    void Awake()
    {
        gm = FindFirstObjectByType<GameManagerS>();
    }

    public void Update()
    {
        if (onBelt) return;

        if (!target)
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget();
        }

        if (health <= 0)
        {
            Die();
            return;
        }
        if (transform.position.y < -5f)
        {
            enemyFactory.ReturnToPool(EnemyType, gameObject);
            return;
        }
    }

    private void FixedUpdate()
    {
        if (!onBelt)
        {
            rb.linearVelocity = transform.up * moveSpeed;
        }
    }

    public void Die()
    {
        GameEvents.OnEnemyKilled?.Invoke();
        enemyFactory.ReturnToPool(EnemyType, gameObject);
    }

    public void TakeDamage(float amount)
    {
        if (health <= 0)
        {
            Die();
        }
        else
        {
            health -= amount;
        }
    }

    public void RotateTowardsTarget()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotationSpeed);
    }

    public void GetTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !onBelt)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            TakeDamage(player.damage);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            gm.EndGame();
            Destroy(collision.gameObject);
        }
    }
}