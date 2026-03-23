using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Rabbit : Animal
{
    private int humor = 80;
    private int humorDecreaseValue = 2;
    private float humorDecreaseInterval = 2f;
    private float timeSinceLastDecrease = 0f;
    private bool isHidden = false;
    private float hideDuration = 10f;
    private float panicInterval = 35f;
    private float timeSinceLastPanic = 0f;
    private int carrotValue = 15;
    private float carrotDuration = 4f;
    private bool isEatingCarrot = false;

    [SerializeField] private GameObject buttons;
    [SerializeField] private Sprite[] rabbitImages;
    [SerializeField] private Image rabbitImage;
    [SerializeField] private Cat cat;
    [SerializeField] private Hamster hamster;
    [SerializeField] private Dog dog;
    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;
    [SerializeField] private Message messageManager;
    [SerializeField] private HoverManager hoverManager;

    void Start()
    {
        ChangeHumor();
    }

    void Update()
    {
        if (humor < 30 && !isHidden && !isEatingCarrot)
        {
            //GetComponent<Image>().sprite = rabbitImages[4];
            rabbitImage.sprite = rabbitImages[4];
        }
        else if (!isHidden && !isEatingCarrot)
        {
            //GetComponent<Image>().sprite = rabbitImages[0];
            rabbitImage.sprite = rabbitImages[0];
        }

        if (humor >= 80)
        {
            carrotValue = 5;
        }
        else
        {
            carrotValue = 15;
        }
        
        if (isEatingCarrot)
        {
            carrotDuration -= Time.deltaTime;
            if (carrotDuration <= 0f)
            {
                isEatingCarrot = false;
                carrotDuration = 4f;
                //GetComponent<Image>().sprite = rabbitImages[0];
                rabbitImage.sprite = rabbitImages[0];
            }
        }

        if (isHidden)
        {
            hideDuration -= Time.deltaTime;
            if (hideDuration <= 0f)
            {
                isHidden = false;
                hideDuration = 10f;
                humorDecreaseValue = 2;
                EnableActions();
                //GetComponent<Image>().sprite = rabbitImages[0];
                rabbitImage.sprite = rabbitImages[0];
            }
        }
        else
        {
            timeSinceLastPanic += Time.deltaTime;
            if (timeSinceLastPanic >= panicInterval)
            {
                //GetComponent<Image>().sprite = rabbitImages[3];
                rabbitImage.sprite = rabbitImages[3];
                RemoveHumor(8);
                timeSinceLastPanic = 0f;
            }
            if (timeSinceLastPanic == 2f)
            {
                //GetComponent<Image>().sprite = rabbitImages[0];
                rabbitImage.sprite = rabbitImages[0];
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
        bool result = EnergyManager.instance.SpendEnergy(10);
        if (!result)
        {
            return;
        }
        isEatingCarrot = true;
        //GetComponent<Image>().sprite = rabbitImages[2];
        rabbitImage.sprite = rabbitImages[2];
        AddHumor(carrotValue);
        dog.AddHumor(5);
        cat.AddHumor(5);
        hamster.AddHumor(5);
    }

    public void Hide()
    {
        bool result = EnergyManager.instance.SpendEnergy(15);
        if (!result)
        {
            return;
        }
        AddHumor(30);
        isHidden = true;
        humorDecreaseValue = 1;
        //GetComponent<Image>().sprite = rabbitImages[1];
        rabbitImage.sprite = rabbitImages[1];
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
        hoverManager.OnAnyButtonHoverExit();
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

    public int GetDecreaseValue()
    {
        return humorDecreaseValue;
    }

    public void SetDecreaseValue(int value)
    {
        humorDecreaseValue = value;
    }

    public void OnCarrotHover()
    {
        messageManager.OnButtonHover("Rabbit : Carrot", "Humor + " + carrotValue + "points. Costs 10 energy. Increase humor of other animals by 5 points.");
    }

    public void OnHideHover()
    {
        messageManager.OnButtonHover("Rabbit : Hide", "Humor + 30 points. Costs 15 energy. Humor decreases -50%for 10 seconds but cannot use any actions.");
    }

    public void OnPictureHover()
    {
        messageManager.OnPictureHover("Rabbit", "Humor loss 2 per 2 seconds. Once every 35 seconds, the rabbit panics and loses 8 humor points.");
    }
}