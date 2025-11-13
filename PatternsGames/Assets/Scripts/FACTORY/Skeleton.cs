using UnityEngine;

public class Skeleton : BaseEnemy
{
    public override void Initialize()
    {
        enemyFactory = EnemyFactory.Instance;
        EnemyType = EnemyFactory.EnemyType.Skeleton;
        health = 3;
        moveSpeed = 1.5f;
        rotationSpeed = 0.25f;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        Initialize();
    }
}