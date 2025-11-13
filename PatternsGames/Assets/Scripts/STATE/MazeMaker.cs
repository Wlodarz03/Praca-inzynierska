using UnityEngine;
using System.Collections.Generic;

public class MazeMaker : MonoBehaviour
{
    public int mazeSize = 11;
    public GameObject wallPrefab;
    public GameObject finishPrefab;
    public GameObject enemyPrefab;
    public float tileSize = 4f;
    private int[,] maze;
    private int width;
    private int height;
    private int enemyCount;

    void Awake()
    {
        width = mazeSize + GameManager.Instance.currentLevel * 2;
        height = mazeSize + GameManager.Instance.currentLevel * 2;
        enemyCount = 1 + GameManager.Instance.currentLevel;
        maze = new int[width, height];

        GenerateMaze();
        AddExtraConnections(15 + GameManager.Instance.currentLevel * 2);
        DrawMaze();
        SpawnEnemies(enemyCount, enemyPrefab);
    }
    void SpawnEnemies(int count, GameObject prefab)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 freePos = RandomFreeSpace();

            Vector2 worldPos = new Vector2(freePos.x * tileSize, freePos.y * tileSize);
            GameObject enemyObj = Instantiate(prefab, worldPos, Quaternion.identity);

            EnemyMovement enemy = enemyObj.GetComponent<EnemyMovement>();
            if (enemy != null)
            {
                enemy.mazeMaker = this;
            }
        }
    }

    void GenerateMaze()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                maze[x, y] = 1;

        Carve(1, 1, new HashSet<Vector2Int>());
    }

    void Carve(int x, int y, HashSet<Vector2Int> visited)
    {
        maze[x, y] = 0;
        visited.Add(new Vector2Int(x, y));

        List<Vector2Int> dirs = new List<Vector2Int> {
            new Vector2Int(0, 2),
            new Vector2Int(2, 0),
            new Vector2Int(0, -2),
            new Vector2Int(-2, 0)
        };

        Shuffle(dirs);

        foreach (var dir in dirs)
        {
            int nx = x + dir.x;
            int ny = y + dir.y;

            if (nx > 0 && nx < width && ny > 0 && ny < height && !visited.Contains(new Vector2Int(nx, ny)))
            {
                maze[x + dir.x / 2, y + dir.y / 2] = 0;
                Carve(nx, ny, visited);
            }
        }
    }

    void DrawMaze()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == 1)
                {
                    Vector2 pos = new Vector2(x * tileSize, y * tileSize); 
                    Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                }
            }
        }

        maze[width - 2, height - 2] = 0;
        Vector2 fin = new Vector2((width - 2) * tileSize, (height - 2) * tileSize); 
        Instantiate(finishPrefab, fin, Quaternion.identity, transform);
    }

    void AddExtraConnections(int count)
    {
        int attempts = 0;
        while (count > 0 && attempts < 1000)
        {
            int x = Random.Range(1, width - 1);
            int y = Random.Range(1, height - 1);

            if (maze[x, y] == 1 &&
                ((maze[x - 1, y] == 0 && maze[x + 1, y] == 0) ||
                (maze[x, y - 1] == 0 && maze[x, y + 1] == 0)))
            {
                maze[x, y] = 0;
                count--;
            }

            attempts++;
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public int[,] GetMaze()
    {
        return maze;
    }

    public void SetMaze(int x, int y, int val)
    {
        maze[x, y] = val;
    }

    public Vector2 RandomFreeSpace()
    {
        int x = Random.Range(5, width - 1);
        int y = Random.Range(5, height - 1);

        while (maze[x, y] == 1)
        {
            x = Random.Range(5, width - 1);
            y = Random.Range(5, height - 1);
        }

        return new Vector2(x, y);
    }
}
