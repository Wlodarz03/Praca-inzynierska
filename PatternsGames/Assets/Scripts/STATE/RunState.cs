using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerMovement player;
    private int[,] maze;

    public RunState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {
        maze = player.mazeMaker.GetMaze();
        UpdateSprite(player.inputDirection);
    }

    public void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;

        Vector2 input = new Vector2(horizontal, vertical);
        player.inputDirection = input;

        if (!AnyMovementKeyHold())
        {
            player.ChangeState(new IdleState(player));
            return;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || player.staminaWheel.staminaExhausted)
        {
            player.ChangeState(new WalkState(player));
        }

        player.staminaWheel.ConsumeStamina(20 * Time.deltaTime);

        float speed = player.runSpeed * Time.deltaTime;
        Vector2 move = input * speed;
        Vector2 nextPosition = (Vector2)player.transform.position + move;

        if (CanMoveTo(nextPosition))
        {
            player.transform.position = nextPosition;
            UpdateSprite(input);
        }
        else
        {
            Vector2 moveX = new Vector2(move.x, 0);
            Vector2 moveY = new Vector2(0, move.y);

            if (CanMoveTo((Vector2)player.transform.position + moveX))
            {
                player.transform.position += (Vector3)moveX;
            }
            else if (CanMoveTo((Vector2)player.transform.position + moveY))
            {
                player.transform.position += (Vector3)moveY;
            }

            UpdateSprite(input);
        }

    }

    public void Exit()
    {

    }
    private bool AnyMovementKeyHold()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    }
    private void UpdateSprite(Vector2 dir)
    {
        int ind = DirectionToInd(dir);
        player.spriteRenderer.sprite = player.thiefRunSprites[ind];
    }

    private int DirectionToInd(Vector2 dir)
    {
        if (dir.y > 0) return 2;
        if (dir.y < 0) return 0;
        if (dir.x < 0) return 1;
        if (dir.x > 0) return 3;
        return 0;
    }
    private bool CanMoveTo(Vector2 position)
    {
        float tileSize = player.mazeMaker.tileSize;

        int x = Mathf.RoundToInt(position.x  / tileSize);
        int y = Mathf.RoundToInt(position.y  / tileSize);

        return x >= 0 && x < maze.GetLength(0) &&
            y >= 0 && y < maze.GetLength(1) &&
            maze[x, y] == 0;
    }
}