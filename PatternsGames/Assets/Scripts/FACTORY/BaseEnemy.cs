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

    public abstract void Initialize();

    public void Update()
    {
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
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * moveSpeed;
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
        if (collision.gameObject.CompareTag("Bullet"))
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            TakeDamage(player.damage);
            Debug.Log($"Enemy took {player.damage} damage. Remaining health: {health}");
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            GameManagerS.Instance.StopSpawning();
            Destroy(collision.gameObject);
            
        }
    }
}