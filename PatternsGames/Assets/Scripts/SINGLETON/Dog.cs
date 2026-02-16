using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dog : Animal
{
    private int humor = 80;
    private int humorDecreaseValue = 5;
    private float humorDecreaseInterval = 3f;
    private float timeSinceLastDecrease = 0f;

    [SerializeField] private Cat cat;
    [SerializeField] private Hamster hamster;
    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;

    void Start()
    {
        ChangeHumor();
    }
    void Update()
    {
        timeSinceLastDecrease += Time.deltaTime;
        if (timeSinceLastDecrease >= humorDecreaseInterval)
        {
            RemoveHumor(humorDecreaseValue);
            timeSinceLastDecrease = 0f;
        }
        ChangeHumor();
    }

    public override int Humor 
    {
        get { return humor; }
        set { humor = value; }
    }

    public void Ball()
    {
        EnergyManager.instance.SpendEnergy(12);
        AddHumor(15);
        cat.AddHumor(5);
        hamster.AddHumor(5);
    }

    public void Walk()
    {
        EnergyManager.instance.SpendEnergy(20);
        AddHumor(25);
    }

    private void ChangeHumor()
    {
        humorUI.size = humor / 100f;
        humorText.text = humor.ToString() + "/100";
        if (humor >= 70)
        {
            humorHandle.GetComponent<Image>().color = Color.green;
        }
        else if (humor >= 30)
        {
            humorHandle.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            humorHandle.GetComponent<Image>().color = Color.red;
        }
    }
}