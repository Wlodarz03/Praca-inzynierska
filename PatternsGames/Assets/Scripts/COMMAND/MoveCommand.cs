using UnityEngine;

public class MoveCommand : ICommand
{
    private PlayerMover player;
    private Vector2Int direction;

    private Vector2 oldPlayerPos;
    private GameObject movedBox;
    private Vector2 oldBoxPos;
    private Sprite prevSprite;
    private Sprite newSprite;

    public MoveCommand(PlayerMover player, Vector2Int dir)
    {
        this.player = player;
        this.direction = dir;
        prevSprite = player.spriteRenderer.sprite;

        if (dir == Vector2Int.up)
            newSprite = player.playerSprites[0];
        else if (dir == Vector2Int.right)
            newSprite = player.playerSprites[1];
        else if (dir == Vector2Int.down)
            newSprite = player.playerSprites[2];
        else if (dir == Vector2Int.left)
            newSprite = player.playerSprites[3];
        else
            newSprite = prevSprite;
    }

    public bool Execute()
    {
        oldPlayerPos = (Vector2)player.transform.position;
        bool moved = player.TryMove(direction, out oldBoxPos, out movedBox);

        if (!moved)
        {
            movedBox = null;
        }
        else
        {
            player.spriteRenderer.sprite = newSprite;
        }

        return moved;
    }

    public void Undo()
    {
        // cofniecie gracza
        player.transform.position = oldPlayerPos;
        player.spriteRenderer.sprite = prevSprite;

        //cofniecie skrzyni
        if (movedBox != null)
        {
            movedBox.transform.position = oldBoxPos;
        }
    }
    public override string ToString()
    {
        if (direction == Vector2Int.up) return "UP";
        if (direction == Vector2Int.down) return "DOWN";
        if (direction == Vector2Int.left) return "LEFT";
        if (direction == Vector2Int.right) return "RIGHT";
        return "Move";
    }
}
