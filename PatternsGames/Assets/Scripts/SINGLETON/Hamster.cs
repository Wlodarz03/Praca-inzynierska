using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hamster : Animal
{
    private int humor = 80;
    private int humorDecreaseValue = 5;
    private float humorDecreaseInterval = 5f;
    private float timeSinceLastDecrease = 0f;
    private float wheelTime = 5f;
    private float afterHugTime = 10f;
    bool isOnWheel = false;
    bool wasHugged = false;

    [SerializeField] private Sprite[] hamsterImages;
    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;

    void Start()
    {
        ChangeHumor();
    }
    void Update()
    {
        if (isOnWheel)
        {
            wheelTime -= Time.deltaTime;

            if (wheelTime <= 0f)
            {
                isOnWheel = false;
                GetComponent<Image>().sprite = hamsterImages[0];
                EnergyManager.instance.AddEnergy(5);
                wheelTime = 5f;
            }
        }

        if (wasHugged)
        {
            afterHugTime -= Time.deltaTime;

            if (afterHugTime <= 0f)
            {
                wasHugged = false;
                afterHugTime = 10f;
            }
        }

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

    public void Wheel()
    {
        EnergyManager.instance.SpendEnergy(8);
        AddHumor(10);
        isOnWheel = true;
        GetComponent<Image>().sprite = hamsterImages[1];
    }

    public void Hug()
    {
        EnergyManager.instance.SpendEnergy(15);
        AddHumor(25);
        wasHugged = true;
        // Tutaj wylaczyc opcje klikania na Hamstera przez 10 sekund + inny sprite
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