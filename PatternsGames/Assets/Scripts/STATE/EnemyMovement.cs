using UnityEngine;
using TMPro;
public class EnemyMovement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public MazeMaker mazeMaker;
    public Transform player;
    public Sprite[] securitySprites; // 0 - front, 1 - left, 2 - back, 3 - right
    public Sprite[] securityRunSprites;
    public GameObject alertIcon;
    public TextMeshProUGUI stateText;
    public float tileSize { get; private set; }
    public float moveCooldown = 1f;

    [HideInInspector] public int[,] maze;
    [HideInInspector] public float moveTimer = 0f;
    [HideInInspector] public Vector2 prevPos;
    [HideInInspector]
    public Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };
    private IEnemyState currentState;

    void Start()
    {
        maze = mazeMaker.GetMaze();

        tileSize = mazeMaker.tileSize;

        Vector2 StartPos = mazeMaker.RandomFreeSpace();
        prevPos = StartPos;

        transform.position = new Vector2(StartPos.x * tileSize, StartPos.y * tileSize);
        spriteRenderer.sprite = securitySprites[0];

        currentState = new PatrolState(this);
        currentState.Enter();
    }
    void Update()
    {
        moveTimer += Time.deltaTime;
        currentState.Update();
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();

        if (stateText != null)
            stateText.text = currentState.GetType().Name;
    }

    public bool PlayerHit()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        return distance < tileSize * 0.5;
        
    }
}