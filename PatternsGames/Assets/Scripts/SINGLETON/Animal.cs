using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    public abstract int Humor {get; set;}
    public void AddHumor(int amount)
    {
        Humor = Mathf.Min(Humor + amount, 100);
        Debug.Log("Added " + amount + " humor. Current humor: " + Humor);
    }

    public void RemoveHumor(int amount)
    {
        Humor = Mathf.Max(Humor - amount, 0);
        Debug.Log("Removed " + amount + " humor. Current humor: " + Humor);
    }
}