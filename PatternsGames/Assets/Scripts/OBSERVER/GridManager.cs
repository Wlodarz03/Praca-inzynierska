using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Prefab & Layout")]
    public GameObject tilePrefab;
    public int width = 5;
    public int height = 5;
    public float spacing = 1.15f;
    public Vector2 originOffset = Vector2.zero;

    [Header("Subscription rules")]
    public bool useDiagonals = false;

    [Header("Randomization")]
    public int randomizeSteps = 12;
    public Tile[,] tiles {get; private set;}
    public event Action OnWin;
    public event Action<int> OnMovesChanged;
    private int moves = 0;
    public bool gridCreated = false;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void GridPrep()
    {
        CreateGrid();
        SubscribeNeighbors(useDiagonals);
        RandomizeSolvable(randomizeSteps);
    }

    public void CreateGrid()
    {
        if (tilePrefab == null)
        {
            return;
        }

        if (gridCreated) ClearGrid();

        tiles = new Tile[width, height];
        
        Vector2 origin = originOffset - new Vector2((width - 1) * spacing / 2f, (height - 1) * spacing / 2f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(origin.x + x * spacing, origin.y + y * spacing, 0f);
                GameObject go = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                go.name = $"Tile_{x}_{y}";
                Tile t = go.GetComponent<Tile>();
                if (t == null)
                {
                    Destroy(go);
                    continue;
                }

                t.x = x; t.y = y;
                tiles[x, y] = t;

                t.ThingToggled += OnTileThingToggled;
            }
        }

        gridCreated = true;
    }

    public void ClearGrid()
    {
        if (!gridCreated || tiles == null) return;

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                Tile t = tiles[x, y];
                if (t == null) continue;

                t.UnsubscribeAll();
                t.ThingToggled -= OnTileThingToggled;

                Destroy(t.gameObject);
            }
        }

        tiles = null;
        gridCreated = false;
        moves = 0;
        OnMovesChanged?.Invoke(moves);
    }

    public void SubscribeNeighbors(bool diagonals = false)
    {
        if (tiles == null) return;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile t = tiles[x, y];
                if (t == null) continue;

                t.UnsubscribeAll();
                if (y + 1 < height) t.SubscribeTo(tiles[x, y + 1]);
                if (y - 1 >= 0)     t.SubscribeTo(tiles[x, y - 1]);
                if (x - 1 >= 0)     t.SubscribeTo(tiles[x - 1, y]);
                if (x + 1 < width)  t.SubscribeTo(tiles[x + 1, y]);

                if (diagonals)
                {
                    if (x - 1 >= 0 && y - 1 >= 0) tiles[x, y].SubscribeTo(tiles[x - 1, y - 1]);
                    if (x - 1 >= 0 && y + 1 < height) tiles[x, y].SubscribeTo(tiles[x - 1, y + 1]);
                    if (x + 1 < width  && y - 1 >= 0) tiles[x, y].SubscribeTo(tiles[x + 1, y - 1]);
                    if (x + 1 < width  && y + 1 < height) tiles[x, y].SubscribeTo(tiles[x + 1, y + 1]);
                }

            }
        }
    }

    public void RandomizeSolvable(int steps = 10)
    {
        if (tiles == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x,y] != null)
                {
                    tiles[x,y].isOn = true;
                    tiles[x, y].RefreshVisual();
                }
            }
        }

        System.Random rng = new System.Random();
        for (int i = 0; i < steps; i++)
        {
            int rx = rng.Next(0, width);
            int ry = rng.Next(0, height);

            tiles[rx, ry].Toggle(true);
        }

        moves = 0;
        OnMovesChanged?.Invoke(moves);
    }

    public bool AllOn()
    {
        if (tiles == null) return false;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (tiles[x,y] != null && !tiles[x, y].isOn)
                    return false;
        return true;
    }

    public void RestartLevel()
    {
        if (tiles == null) return;
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (tiles[x,y] != null)
                {
                    tiles[x, y].isOn = false;
                    tiles[x, y].SendMessage("RefreshVisual", SendMessageOptions.DontRequireReceiver);
                }

        moves = 0;
        OnMovesChanged?.Invoke(moves);
    }

    private void OnTileThingToggled(Tile source)
    {
        moves++;
        OnMovesChanged?.Invoke(moves);

        if (AllOn())
        {
            OnWin?.Invoke();
        }
    }
}
