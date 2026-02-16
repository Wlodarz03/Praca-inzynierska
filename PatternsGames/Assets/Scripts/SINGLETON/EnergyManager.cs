using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;

    private int energy;
    private float energyRegenTime = 3f;
    private float timeSinceLastRegen = 0f;

    [SerializeField] private Scrollbar energyUI;
    [SerializeField] private GameObject energryHandle;
    [SerializeField] private TextMeshProUGUI energyText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        energy = 100;
        energyUI.size = energy / 100f;
        ChangeColor();
        ChangeEnergyCount();
    }

    void Update()
    {
        timeSinceLastRegen += Time.deltaTime;
        if (timeSinceLastRegen >= energyRegenTime && energy < 100)
        {
            AddEnergy(5);
            timeSinceLastRegen = 0f;
        }
    }

    private void ChangeEnergyCount()
    {
        energyText.text = energy.ToString() + "/100";
    }

    private void ChangeColor()
    {
        if (energy >= 70)
        {
            energryHandle.GetComponent<Image>().color = Color.green;
        }
        else if (energy >= 30)
        {
            energryHandle.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            energryHandle.GetComponent<Image>().color = Color.red;
        }
    }

    public int GetEnergy()
    {
        return energy;
    }

    public void AddEnergy(int amount)
    {
        energy += amount;
        energy = Mathf.Min(energy, 100);
        energyUI.size = energy / 100f;
        ChangeColor();
        ChangeEnergyCount();
    }

    public void SpendEnergy(int amount)
    {
        if (energy >= amount)
        {
            energy -= amount;
            Debug.Log("Spent " + amount + " energy. Current energy: " + energy);
            energyUI.size = energy / 100f;
            ChangeColor();
            ChangeEnergyCount();
        }
        else
        {
            Debug.Log("Not enough energy to spend! Current energy: " + energy);
        }
    }
}
