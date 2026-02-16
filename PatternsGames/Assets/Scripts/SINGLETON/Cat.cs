using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cat : Animal 
{
    private int humor = 80;
    private int humorDecreaseValue = 5;
    private float humorDecreaseInterval = 4f;
    private float timeSinceLastDecrease = 0f;
    private float feededDuration = 10f;
    bool isFed = false;

    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;
    
    void Start()
    {
        ChangeHumor();
    }

    void Update()
    {
        if (isFed)
        {
            feededDuration -= Time.deltaTime;
            if (feededDuration <= 0f)
            {
                isFed = false;
                humorDecreaseInterval = 4f; // Reset to normal decrease interval
                feededDuration = 10f; // Reset feeded duration for next time
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

    public void Tap()
    {
        EnergyManager.instance.SpendEnergy(10);
        AddHumor(15);
    }

    public void Feed()
    {
        isFed = true;
        humorDecreaseInterval = 10f;
        EnergyManager.instance.SpendEnergy(15);
        AddHumor(20);
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