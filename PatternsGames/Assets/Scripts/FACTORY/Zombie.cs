using UnityEngine;

public class Zombie : BaseEnemy
{
    public override void Initialize()
    {
        enemyFactory = EnemyFactory.Instance;
        EnemyType = EnemyFactory.EnemyType.Zombie;
        health = 5;
        moveSpeed = 1f;
        rotationSpeed = 0.25f;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        Initialize();
    }
}