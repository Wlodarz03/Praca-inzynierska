using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public PlayerMover player;
    public Button upBtn, downBtn, leftBtn, rightBtn;
    public Button undoBtn, redoBtn;
    public CommandHistoryVisualizer visualizer;

    [Header("Colors")]
    [SerializeField] private Color normalTextColor = Color.black;
    [SerializeField] private Color futureTextColor = Color.red;

    [SerializeField] private Color normalBgColor = new Color(0.8302513f, 0.8584906f, 0.6762638f);
    [SerializeField] private Color futureBgColor = new Color(0.7f,0.7f,0.7f,0.6f);

    private void Start()
    {
        upBtn.onClick.AddListener(() => { RunMove(Vector2Int.up); UpdateVisualizer(); });
        downBtn.onClick.AddListener(() => { RunMove(Vector2Int.down); UpdateVisualizer(); });
        leftBtn.onClick.AddListener(() => { RunMove(Vector2Int.left); UpdateVisualizer(); });
        rightBtn.onClick.AddListener(() => { RunMove(Vector2Int.right); UpdateVisualizer(); });

        undoBtn.onClick.AddListener(() => { CommandInvoker.UndoCommand(); UpdateVisualizer(); });
        redoBtn.onClick.AddListener(() => { CommandInvoker.RedoCommand(); UpdateVisualizer(); });
    }

    private void Update()
    {
        // góra
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upBtn.gameObject.ButtonDown();
            upBtn.onClick.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            upBtn.gameObject.ButtonUp();
        }

        // dół
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            downBtn.gameObject.ButtonDown();
            downBtn.onClick.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            downBtn.gameObject.ButtonUp();
        }

        // lewo
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftBtn.gameObject.ButtonDown();
            leftBtn.onClick.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            leftBtn.gameObject.ButtonUp();
        }

        // prawo
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightBtn.gameObject.ButtonDown();
            rightBtn.onClick.Invoke();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rightBtn.gameObject.ButtonUp();
        }

        // powrót do menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void RunMove(Vector2Int dir)
    {
        if (player != null)
        {
            CommandInvoker.ExecuteCommand(new MoveCommand(player, dir));
        }
    }

    private void UpdateVisualizer()
    {
        // Pobieramy historię z invokera
        var history = CommandInvoker.GetHistory();
        var bufferField = typeof(CommandHistory).GetField("buffer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var startField = typeof(CommandHistory).GetField("start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var countField = typeof(CommandHistory).GetField("count", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var currentField = typeof(CommandHistory).GetField("currentIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var maxSizeField = typeof(CommandHistory).GetField("maxSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        ICommand[] buffer = (ICommand[])bufferField.GetValue(history);
        int start = (int)startField.GetValue(history);
        int count = (int)countField.GetValue(history);
        int currentIndex = (int)currentField.GetValue(history);
        int maxSize = (int)maxSizeField.GetValue(history);
        bool canRedo = history.CanRedo;

        redoBtn.gameObject.SetActive(canRedo);

        int absStart = maxSize > 0 ? ((start % maxSize) + maxSize) % maxSize : -1; 

        int absCurrent = (currentIndex >= 0 && maxSize > 0) ? ((start + currentIndex) % maxSize + maxSize) % maxSize : -1;

        // Czyścimy sloty
        visualizer.ClearBuffer();

        // Wypełniamy sloty nazwami komend
        for (int i = 0; i < maxSize; i++)
        {
            if (buffer[i] != null)
            {
                visualizer.UpdateText(i, buffer[i].ToString());
            }

            bool isNormal = false;

            if (absCurrent == -1)
            {
                isNormal = false;
            }
            else
            {
                if (absStart <= absCurrent)
                {
                    isNormal = i >= absStart && i <= absCurrent;
                }
                else
                {
                    isNormal = (i >= absStart && i < maxSize) || (i <= absCurrent);
                }
            }

            if (isNormal)
            {
                visualizer.TextColorChange(i, normalTextColor);
                visualizer.BackgroundColorChange(i, normalBgColor);
            }
            else
            {
                visualizer.TextColorChange(i, futureTextColor);
                visualizer.BackgroundColorChange(i, futureBgColor);
            }
        }

        if (currentIndex >= 0 && count > 0)
        {
            for (int rel = currentIndex + 1; rel < count; rel++)
            {
                int abs = (start + rel) % maxSize;
                visualizer.TextColorChange(abs, futureTextColor);
                visualizer.BackgroundColorChange(abs, futureBgColor);
            }
        }

        // Ustawianie strzałek
        visualizer.UpdateCurrentArrow(absCurrent, absStart, maxSize);

        if (count > 0)
        {
            visualizer.UpdateStartArrow(absStart, absCurrent, maxSize);
        }
        else
        {
            visualizer.UpdateStartArrow(-1, -1, maxSize);
        }
    }
}
