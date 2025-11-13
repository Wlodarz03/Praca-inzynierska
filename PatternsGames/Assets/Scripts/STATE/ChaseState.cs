using UnityEngine;

public class ChaseState : IEnemyState
{
    private float chaseTime = 5f;
    private float timer = 0f;
    private EnemyMovement enemy;
    private SpriteRenderer spriteRenderer => enemy.spriteRenderer;
    private Sprite[] securitySprites => enemy.securitySprites;
    private Sprite[] securityRunSprites => enemy.securityRunSprites;
    private float tileSize => enemy.tileSize;
    private float moveTimer => enemy.moveTimer;
    private float moveCooldown => enemy.moveCooldown;
    private Transform player => enemy.player;
    private int[,] maze => enemy.maze;
    private Vector2[] directions => enemy.directions;
    private Vector2 prevPos => enemy.prevPos;

    public ChaseState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        timer = 0f;
        Debug.Log("Enemy is chasing");
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (enemy.PlayerHit())
        {
            Debug.Log("Game over");
            GameManager.Instance.GameOver();
        }

        ChasePlayer();

        if (timer >= chaseTime)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
    }

    public void Exit()
    {
        Debug.Log("Enemy stopped chasing");
    }

    private int DirectionToInd(Vector2 dir)
    {
        if (dir.y > 0) return 2;
        if (dir.y < 0) return 0;
        if (dir.x < 0) return 1;
        if (dir.x > 0) return 3;
        return 0;
    }

    public void UpdateSprite(Vector2 dir)
    {
        int ind = DirectionToInd(dir);
        spriteRenderer.sprite = securityRunSprites[ind];
    }

    public void ChasePlayer()
    {
        if (moveTimer < moveCooldown * 0.3f) return;
        enemy.moveTimer = 0f;

        Vector2 bestDir = Vector2.zero;
        float bestDist = float.MaxValue;

        foreach (var dir in directions)
        {
            Vector2 pos = (Vector2)enemy.transform.position + dir * tileSize;
            if (pos == prevPos) continue;
            int x = Mathf.RoundToInt(pos.x / tileSize);
            int y = Mathf.RoundToInt(pos.y / tileSize);

            if (x < 0 || y < 0 || x >= maze.GetLength(0) || y >= maze.GetLength(1))
                continue;

            if (maze[x, y] != 0) continue;

            float dist = Vector2.Distance(pos, player.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                bestDir = dir;
            }
        }

        if (bestDir != Vector2.zero)
        {
            enemy.prevPos = enemy.transform.position;
            enemy.transform.position += (Vector3)(bestDir * tileSize);
            UpdateSprite(bestDir);
        }
    }
}