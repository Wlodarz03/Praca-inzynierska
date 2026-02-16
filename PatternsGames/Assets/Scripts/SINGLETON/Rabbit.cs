using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Rabbit : Animal
{
    private int humor = 80;
    private int humorDecreaseValue = 5;
    private float humorDecreaseInterval = 6f;
    private float timeSinceLastDecrease = 0f;
    private bool isHidden = false;
    private float hideDuration = 10f;

    [SerializeField] private Sprite[] rabbitImages;

    [SerializeField] private Cat cat;
    [SerializeField] private Hamster hamster;
    [SerializeField] private Dog dog;
    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;

    void Start()
    {
        ChangeHumor();
    }

    void Update()
    {
        if (isHidden)
        {
            hideDuration -= Time.deltaTime;
            if (hideDuration <= 0f)
            {
                isHidden = false;
                hideDuration = 10f;
                GetComponent<Image>().sprite = rabbitImages[0];
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

    public void Carrot()
    {
        EnergyManager.instance.SpendEnergy(10);
        AddHumor(15);
        dog.AddHumor(3);
        cat.AddHumor(3);
        hamster.AddHumor(3);
    }

    public void Hide()
    {
        EnergyManager.instance.SpendEnergy(15);
        AddHumor(30);
        isHidden = true;
        GetComponent<Image>().sprite = rabbitImages[1];
        // tutaj wyłączyć opcje klikania królika
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