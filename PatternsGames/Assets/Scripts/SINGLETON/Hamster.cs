using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hamster : Animal
{
    private int humor = 80;
    private int humorDecreaseValue = 1;
    private float humorDecreaseInterval = 2f;
    private float timeSinceLastDecrease = 0f;
    private float wheelTime = 5f;
    private float afterHugTime = 8f;
    private float restInterval = 20f;
    private float restTime = 10f;
    bool isResting = false;
    bool isOnWheel = false;
    bool wasHugged = false;

    [SerializeField] private Sprite[] hamsterImages;
    [SerializeField] private Image hamsterImage;
    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Message messageManager;
    [SerializeField] private Cat cat;
    [SerializeField] private Dog dog;
    [SerializeField] private Rabbit rabbit;
    [SerializeField] HoverManager hoverManager;

    void Start()
    {
        ChangeHumor();
    }
    void Update()
    {
        if (humor < 30 && !isResting && !isOnWheel && !wasHugged)
        {
            //GetComponent<Image>().sprite = hamsterImages[4];
            hamsterImage.sprite = hamsterImages[4];
        }

        if (!isResting && !isOnWheel && !wasHugged)
        {
            restInterval -= Time.deltaTime;

            if (restInterval <= 0f && !isOnWheel && !wasHugged)
            {
                isResting = true;
                //GetComponent<Image>().sprite = hamsterImages[3];
                hamsterImage.sprite = hamsterImages[3];
                restInterval = 20f;
                DisableActions();
            }
        }
        else
        {
            restTime -= Time.deltaTime;

            if (restTime <= 0f)
            {
                humorDecreaseValue = 1;
                isResting = false;
                EnableActions();
                //GetComponent<Image>().sprite = hamsterImages[0];
                hamsterImage.sprite = hamsterImages[0];
                restTime = 10f;
            }
        }
        if (isOnWheel)
        {
            wheelTime -= Time.deltaTime;

            if (wheelTime <= 0f)
            {
                isOnWheel = false;
                humorDecreaseValue = 1;
                //GetComponent<Image>().sprite = hamsterImages[0];
                hamsterImage.sprite = hamsterImages[0];
                EnergyManager.instance.AddEnergy(30);
                wheelTime = 5f;
                RemoveHumor(10);
            }
        }

        if (wasHugged)
        {
            afterHugTime -= Time.deltaTime;

            if (afterHugTime <= 0f)
            {
                wasHugged = false;
                afterHugTime = 8f;
                EnergyManager.instance.AddEnergy(10);
                hamsterImage.sprite = hamsterImages[0];
                cat.SetDecreaseValue(cat.GetDecreaseValue() + 1);
                dog.SetDecreaseValue(dog.GetDecreaseValue() + 1);
                rabbit.SetDecreaseValue(rabbit.GetDecreaseValue() + 1);
                EnableActions();
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
        bool result = EnergyManager.instance.SpendEnergy(8);
        if (!result)
        {
            return;
        }
        AddHumor(5);
        isOnWheel = true;
        humorDecreaseValue = 0;
        //GetComponent<Image>().sprite = hamsterImages[1];
        hamsterImage.sprite = hamsterImages[1];
        
    }

    public void Hug()
    {
        bool result = EnergyManager.instance.SpendEnergy(15);
        if (!result)
        {
            return;
        }
        AddHumor(25);
        wasHugged = true;
        cat.SetDecreaseValue(cat.GetDecreaseValue() - 1);
        dog.SetDecreaseValue(dog.GetDecreaseValue() - 1);
        rabbit.SetDecreaseValue(rabbit.GetDecreaseValue() - 1);
        //GetComponent<Image>().sprite = hamsterImages[2];
        hamsterImage.sprite = hamsterImages[2];
        DisableActions();
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

    public void DisableActions()
    {
        if (hoverManager.GetHamsterFlag())
        {
            hoverManager.SetHamsterFlag(false);
            hoverManager.OnAnyButtonHoverExit();
        }
        // To do naprawy przycisków, po wyłączeniu ich
        foreach (var button in buttons.GetComponentsInChildren<Button>())
        {
            button.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        buttons.SetActive(false);
        
    }

    public void EnableActions()
    {
        buttons.SetActive(true);
    }

    public void OnWheelHover()
    {
        messageManager.OnButtonHover("Hamster : Wheel", "Humor + 5 points. Costs 8 energy. Does not lose humor for 5 seconds. After 5 seconds, lose 10 humor but gain 30 energy.");
    }

    public void OnHugHover()
    {
        messageManager.OnButtonHover("Hamster : Hug", "Humor + 25 points. Costs 15 energy. For 8 seconds, other animals get -1 to humor decreace value and Hamster cannot use any actions. After 8 seconds + 10 energy.");
    }

    public void OnPictureHover()
    {
        messageManager.OnPictureHover("Hamster", "Humor loss 1 per 2 seconds. Every 20 seconds, hamster rests for 10 seconds during which it cannot use any actions and does not lose humor.");
    }
}