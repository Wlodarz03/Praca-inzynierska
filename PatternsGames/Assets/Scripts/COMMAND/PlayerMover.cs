using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] playerSprites;
    [SerializeField] private LayerMask obstacleLayer;   // ściany
    [SerializeField] private LayerMask boxLayer;        // skrzynki
    private const float tileSize = 1f;

    public bool TryMove(Vector2Int dir, out Vector2 boxOldPos, out GameObject movedBox)
    {
        Vector2 move = new Vector2(dir.x, dir.y) * tileSize;
        Vector2 targetPos = (Vector2)transform.position + move;

        boxOldPos = Vector2.zero;
        movedBox = null;

        // sprawdzamy ścianę
        if (Physics2D.OverlapPoint(targetPos, obstacleLayer))
        {
            return false;
        }

        // sprawdzamy skrzynkę
        Collider2D box = Physics2D.OverlapPoint(targetPos, boxLayer);
        if (box != null)
        {
            Vector2 boxTarget = (Vector2)box.transform.position + move;
            if (Physics2D.OverlapPoint(boxTarget, obstacleLayer) ||
                Physics2D.OverlapPoint(boxTarget, boxLayer))
            {
                return false; //skrzynia zablokowana
            }

            boxOldPos = box.transform.position;
            movedBox = box.gameObject;
            box.transform.position = boxTarget;
        }

        transform.position = targetPos;
        return true;
    }
}
