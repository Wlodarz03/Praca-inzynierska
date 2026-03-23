using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HistoryManager : MonoBehaviour
{
    public static HistoryManager Instance;
    public List<GameObject> entries;
    private Queue<(string animal, string button, int energy)> historyQueue = new Queue<(string, string, int)>();
    private const int maxHistoryEntries = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        foreach (var entry in entries)
        {
            entry.SetActive(false);
        }
    }

    public void AddEntry(string animal, string button, int energy)
    {
        if (historyQueue.Count >= maxHistoryEntries)
        {
            historyQueue.Dequeue();
        }
        historyQueue.Enqueue((animal, button, energy));
        RefreshUI();
    }

    public void RefreshUI()
    {
        var reversed = new List<(string animal, string button, int energy)>(historyQueue);
        reversed.Reverse();

        for (int i = 0; i < entries.Count; i++)
        {
            if (i < reversed.Count)
            {
                var entry = reversed[i];
                entries[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{i+1}. {entry.animal}.{entry.button} \n  - {entry.energy} energy";
                entries[i].SetActive(true);
            }
            else
            {
                entries[i].SetActive(false);
            }
        }
    }
}

