using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public string animalName;
    public string buttonName;
    public int energyCost;

    public void OnButtonPressed()
    {
        if (EnergyManager.instance.GetEnergy() >= energyCost)
        {
            HistoryManager.Instance.AddEntry(animalName, buttonName, energyCost);
        }  
    }
}