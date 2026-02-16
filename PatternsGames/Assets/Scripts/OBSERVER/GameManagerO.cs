using System;
using UnityEngine;

public class GameManagerO : MonoBehaviour
{
    public static GameManagerO Instance;
    public bool isFinished = false;
    public int moves = 0;
    public float timer = 0f;
    public event Action<int> OnMovesChanged;
    public event Action OnGameFinished;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (!isFinished && moves > 0)
            timer += Time.deltaTime;
    }

    public void RegisterMove()
    {
        if (isFinished) return;
        moves++;
        OnMovesChanged?.Invoke(moves);
    }

    public void CheckForWin()
    {
        if (isFinished || !GridManager.Instance.AllOn()) return;

        isFinished = true;
        OnGameFinished?.Invoke();
    }
}