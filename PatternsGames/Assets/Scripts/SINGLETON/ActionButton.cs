using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public string animalName;
    public string buttonName;
    public int energyCost;

    public void OnButtonPressed()
    {
        HistoryManager.Instance.AddEntry(animalName, buttonName, energyCost);
    }
}