using UnityEngine;
using System.Collections.Generic;
public class PatrolState : IEnemyState
{
    private EnemyMovement enemy;
    private SpriteRenderer spriteRenderer => enemy.spriteRenderer;
    private Sprite[] securitySprites => enemy.securitySprites;
    private float moveCooldown => enemy.moveCooldown;
    private float tileSize => enemy.tileSize;
    private Transform player => enemy.player;
    private int[,] maze => enemy.maze;
    private Vector2[] directions => enemy.directions;
    private Vector2 prevPos => enemy.prevPos;

    public PatrolState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {

    }

    public void Update()
    {
        enemy.moveTimer += Time.deltaTime;

        if (enemy.PlayerHit())
        {
            Debug.Log("Game over");
            GameManager.Instance.GameOver();
        }

        if (CanSeePlayer(2))
        {
            enemy.ChangeState(new AlarmState(enemy));
        }

        if (enemy.moveTimer >= moveCooldown)
        {
            MoveRandomly();
            enemy.moveTimer = 0f;
        }
    }

    public void Exit()
    {

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
        spriteRenderer.sprite = securitySprites[ind];
    }

    private bool CanSeePlayer(int tilesAhead)
    {
        Vector2 dir = ((Vector2)enemy.transform.position - prevPos).normalized;
        Vector2 check = enemy.transform.position;

        for (int i = 1; i <= tilesAhead; i++)
        {
            check += dir * tileSize;
            float dist = Vector2.Distance(check, player.position);
            if (dist < tileSize * 0.5f)
                return true;

            int x = Mathf.RoundToInt(check.x / tileSize);
            int y = Mathf.RoundToInt(check.y / tileSize);

            if (maze[x, y] != 0)
                break;
        }

        return false;
    }

    private void MoveRandomly()
    {
        List<Vector2> validDirs = new List<Vector2>();
        Vector2 currentPos = enemy.transform.position;

        Vector2 backwardDir = (prevPos - currentPos).normalized;

        foreach (Vector2 dir in directions)
        {
            Vector2 possiblePos = (Vector2)enemy.transform.position + dir * tileSize;
            int checkX = Mathf.RoundToInt(possiblePos.x  / tileSize);
            int checkY = Mathf.RoundToInt(possiblePos.y  / tileSize);

            if (checkX < 0 || checkY < 0 || checkX > maze.GetLength(1) || checkY > maze.GetLength(0))
            {
                continue;
            }

            if (maze[checkX, checkY] != 0)
            {
                continue;
            }

            if (dir != backwardDir)
            {
                validDirs.Add(dir);
            }
        }

        Vector2 chosenDir;

        if (validDirs.Count > 0)
        {
            chosenDir = validDirs[Random.Range(0, validDirs.Count)];
        }
        else
        {
            chosenDir = backwardDir;
        }

        Vector2 newPos = currentPos + chosenDir * tileSize;
        enemy.prevPos = currentPos;
        enemy.transform.position = newPos;
        UpdateSprite(chosenDir);
    }
}