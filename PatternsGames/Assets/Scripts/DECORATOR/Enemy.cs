using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float speed = 2f;

    private Transform target;
    private int pathIndex = 0;

    private void Start()
    {
        target = TowerDefenseManager.Instance.path[pathIndex];
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            
            if (pathIndex == TowerDefenseManager.Instance.path.Length)
            {
                EnemySpawner.onEnemyDestroyed.Invoke();
                TowerDefenseManager.Instance.PlayerGetHit();
                Destroy(gameObject);
                return;
            }
            else
            {
                target = TowerDefenseManager.Instance.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    public int GetCurrentPathIndex()
    {
        return pathIndex;
    }

}
