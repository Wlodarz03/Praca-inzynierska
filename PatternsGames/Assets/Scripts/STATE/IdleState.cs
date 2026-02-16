using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerMovement player;

    public IdleState(PlayerMovement player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.spriteRenderer.sprite = player.thiefSprites[0];
    }

    public void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        player.inputDirection = input;

        if (input != Vector2.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !player.staminaWheel.staminaExhausted)
            {
                player.ChangeState(new RunState(player));
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                player.ChangeState(new WalkState(player));
            }
        }
    }

    public void Exit()
    {

    }
}