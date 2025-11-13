using UnityEngine;

public class Alien : BaseEnemy
{
    public override void Initialize()
    {
        enemyFactory = EnemyFactory.Instance;
        EnemyType = EnemyFactory.EnemyType.Alien;
        health = 7;
        moveSpeed = 0.35f;
        rotationSpeed = 0.25f;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        Initialize();
    }


}