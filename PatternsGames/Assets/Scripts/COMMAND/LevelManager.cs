using System.Collections.Generic;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LevelManager : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject targetPrefab;
    public GameObject boxPrefab;
    public GameObject boxOnTargetPrefab;
    public GameObject floorPrefab;
    public TextMeshProUGUI currentLevelText;
    public PlayerMover player;
    public List<Sprite> boxSprites;
    public CommandHistoryVisualizer visualizer;

    private Dictionary<int, string[]> levels = new Dictionary<int, string[]>();
    private int currentLevel = 0;

    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        StartCoroutine(LoadLevelsCoroutine());
        //LoadLevels();
        visualizer.ResetVisualizer();
        //BuildLevel(currentLevel);
    }

    IEnumerator LoadLevelsCoroutine()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "levels.txt");

        if (path.Contains("://")) // WebGL
        {
            UnityWebRequest www = UnityWebRequest.Get(path);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Błąd ładowania levels.txt: " + www.error);
                yield break;
            }

            ParseLevels(www.downloadHandler.text);
        }
        else // PC / lokalnie
        {
            if (!File.Exists(path))
            {
                Debug.LogError("Brak pliku levels.txt");
                yield break;
            }

            string fileText = File.ReadAllText(path);
            ParseLevels(fileText);
        }

        visualizer.ResetVisualizer();
        BuildLevel(currentLevel);
    }

    void ParseLevels(string fileText)
    {
        levels.Clear();

        string[] lines = fileText.Split('\n');
        int lvlNum = -1;
        List<string> buffer = new List<string>();

        foreach (var raw in lines)
        {
            string line = raw.TrimEnd('\r');
            if (line.StartsWith(";"))
            {
                if (lvlNum >= 0 && buffer.Count > 0)
                {
                    levels[lvlNum] = buffer.ToArray();
                    buffer.Clear();
                }
                string[] parts = line.Split(' ');
                if (parts.Length > 1 && int.TryParse(parts[1], out int num))
                {
                    lvlNum = num;
                }
            }
            else if (!string.IsNullOrWhiteSpace(line))
            {
                buffer.Add(line);
            }
        }
        if (lvlNum >= 0 && buffer.Count > 0)
            levels[lvlNum] = buffer.ToArray();
    }

    // void LoadLevels()
    // {
    //     string path = Path.Combine(Application.streamingAssetsPath, "levels.txt");
    //     if (!File.Exists(path))
    //     {
    //         Debug.LogError("Brak pliku levels.txt");
    //         return;
    //     }

    //     string[] lines = File.ReadAllLines(path);
    //     int lvlNum = -1;
    //     List<string> buffer = new List<string>();

    //     foreach (var raw in lines)
    //     {
    //         string line = raw;
    //         if (line.StartsWith(";"))
    //         {
    //             if (lvlNum >= 0 && buffer.Count > 0)
    //             {
    //                 levels[lvlNum] = buffer.ToArray();
    //                 buffer.Clear();
    //             }
    //             string[] parts = line.Split(' ');
    //             if (parts.Length > 1 && int.TryParse(parts[1], out int num))
    //             {
    //                 lvlNum = num;
    //             }
    //         }
    //         else if (!string.IsNullOrWhiteSpace(line))
    //         {
    //             buffer.Add(line);
    //         }
    //     }
    //     if (lvlNum >= 0 && buffer.Count > 0)
    //         levels[lvlNum] = buffer.ToArray();
    // }

    void BuildLevel(int levelIndex)
    {
        if (!levels.ContainsKey(levelIndex))
        {
            Debug.Log("Koniec gry!");
            return;
        }

        string[] map = levels[levelIndex];

        for (int y = 0; y < map.Length; y++)
        {
            string line = map[y];

            for (int x = 0; x < line.Length; x++)
            {
                char c = line[x];
                Vector2 pos = new Vector2(x, -y);

                bool hasLeft = false;
                bool hasRight = false;
                bool hasUp = false;
                bool hasDown = false;

                for (int lx = x - 1; lx >= 0; lx--)
                {
                    if (line[lx] != ' ')
                    {
                        hasLeft = true;
                        break;
                    }
                }
                for (int rx = x + 1; rx < line.Length; rx++)
                {
                    if (line[rx] != ' ')
                    {
                        hasRight = true;
                        break;
                    }
                }
                for (int uy = y - 1; uy >= 0; uy--)
                {
                    if (x < map[uy].Length && map[uy][x] != ' ')
                    {
                        hasUp = true;
                        break;
                    }
                }
                for (int dy = y + 1; dy < map.Length; dy++)
                {
                    if (x < map[dy].Length && map[dy][x] != ' ')
                    {
                        hasDown = true;
                        break;
                    }
                }

                if (hasLeft && hasRight && hasUp && hasDown)
                {
                    var floor = Instantiate(floorPrefab, pos, Quaternion.identity);
                    spawnedObjects.Add(floor);
                }

                GameObject obj = null;

                switch (c)
                {
                    case '#':
                        obj = Instantiate(wallPrefab, pos, Quaternion.identity);
                        break;
                    case '.':
                        obj = Instantiate(targetPrefab, pos, Quaternion.identity);
                        break;
                    case '$':
                        obj = Instantiate(boxPrefab, pos, Quaternion.identity);
                        break;
                    case '@':
                        player.transform.position = pos;
                        break;
                    case '*': // skrzynia na celu
                        var target1 = Instantiate(targetPrefab, pos, Quaternion.identity);
                        spawnedObjects.Add(target1);
                        obj = Instantiate(boxOnTargetPrefab, pos, Quaternion.identity);
                        break;
                    case '+': // gracz na celu
                        var target2 = Instantiate(targetPrefab, pos, Quaternion.identity);
                        spawnedObjects.Add(target2);
                        player.transform.position = pos;
                        break;
                }

                if (obj != null)
                    spawnedObjects.Add(obj);
            }
        }
        CenterCameraRight(map);
    }
    public void ResetLevel()
    {
        PreviousLevel();
        NextLevel();
        visualizer.ResetVisualizer();
    }

    void CenterCameraRight(string[] map)
    {
        int width = 0;
        foreach (var line in map)
            if (line.Length > width)
                width = line.Length;

        int height = map.Length;

        Camera cam = Camera.main;
        if (cam == null) return;

        float screenRatio = ((float)Screen.width / 2f) / Screen.height;
        float targetRatio = (float)width / height;

        if (targetRatio > screenRatio)
            cam.orthographicSize = width / 2f / screenRatio;
        else
            cam.orthographicSize = height / 2f;

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * ((float)Screen.width / Screen.height);

        float rightOfMap = width;
        float cameraX = rightOfMap - halfWidth;

        float cameraY = -height / 2f;

        cam.transform.position = new Vector3(cameraX, cameraY, -10f);
    }

    public bool Completed()
    {
        int boxLayer = LayerMask.NameToLayer("Box");
        int targetLayer = LayerMask.NameToLayer("Target");
        var boxesPos = new List<Vector2Int>();
        var targetsPos = new HashSet<Vector2Int>();

        foreach (GameObject obj in spawnedObjects)
        {
            Vector2Int pos = new Vector2Int(
                Mathf.RoundToInt(obj.transform.position.x),
                Mathf.RoundToInt(obj.transform.position.y)
            );
            if (obj.layer == boxLayer)
            {
                boxesPos.Add(pos);
            }
            if (obj.layer == targetLayer)
            {
                targetsPos.Add(pos);
            }
        }

        if (boxesPos.Count == 0) return false;

        foreach (var box in boxesPos)
        {
            if (!targetsPos.Contains(box))
            {
                return false;
            }
        }
        return true;
    }

    public void FixBoxes()
    {
        int boxLayer = LayerMask.NameToLayer("Box");
        int targetLayer = LayerMask.NameToLayer("Target");
        var boxes = new List<GameObject>();
        var targets = new HashSet<Vector2>();

        foreach (GameObject obj in spawnedObjects)
        {
            if (obj.layer == boxLayer)
            {
                boxes.Add(obj);
            }
            if (obj.layer == targetLayer)
            {
                targets.Add((Vector2)obj.transform.position);
            }
        }

        foreach (var box in boxes)
        {
            if (targets.Contains(box.transform.position))
            {
                box.GetComponent<SpriteRenderer>().sprite = boxSprites[1];
            }
            else
            {
                box.GetComponent<SpriteRenderer>().sprite = boxSprites[0];
            }
        }
    }

    public void ClearLevel()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null) Destroy(obj);
        }
        spawnedObjects.Clear();
        CommandInvoker.ClearHistory();
    }

    public void NextLevel()
    {
        ClearLevel();
        currentLevel++;
        BuildLevel(currentLevel);
    }

    public void PreviousLevel()
    {
        ClearLevel();
        currentLevel--;
        BuildLevel(currentLevel);
    }

    void Update()
    {
        FixBoxes();

        if (currentLevelText.text != "LEVEL " + (currentLevel + 1))
        {
            currentLevelText.text = "LEVEL " + (currentLevel + 1);
        }

        bool complete = Completed();

        if (complete)
        {
            visualizer.ResetVisualizer();
            NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            visualizer.ResetVisualizer();
            NextLevel();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            visualizer.ResetVisualizer();
            PreviousLevel();
        }
    }
}
