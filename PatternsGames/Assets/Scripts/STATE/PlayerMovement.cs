using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public MazeMaker mazeMaker;
    public Sprite[] thiefSprites;
    public Sprite[] thiefRunSprites;
    public StaminaWheel staminaWheel;
    public float walkSpeed = 20f;
    public float runSpeed = 30f;
    public Vector2 inputDirection;
    public float tileSize { get; private set; }
    public TextMeshProUGUI stateText;
    private IPlayerState currentState;

    void Start()
    {
        tileSize = mazeMaker.tileSize;

        transform.position = new Vector2(1f * tileSize, 1f * tileSize); 

        ChangeState(new IdleState(this));
    }

    void Update()
    {
        currentState.Update();

        if (Finished())
        {
            Debug.Log("Gra ukończona");
            GameManager.Instance.NextLevel();
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public void ChangeState(IPlayerState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
        
        if (stateText != null)
            stateText.text = currentState.GetType().Name;
    }

    public bool Finished()
    {
        Vector2 pos = transform.position;

        int x = Mathf.RoundToInt(pos.x / mazeMaker.tileSize);
        int y = Mathf.RoundToInt(pos.y / mazeMaker.tileSize);

        int[,] maze = mazeMaker.GetMaze();
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        return x == width - 2 && y == height - 2;
    }
}
