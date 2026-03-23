using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cat : Animal 
{
    private int humor = 80;
    private int humorDecreaseValue = 2;
    private float humorDecreaseInterval = 2f;
    private float timeSinceLastDecrease = 0f;
    private float feededDuration = 10f;
    private float timeWithoutFood = 20f;
    bool isFed = false;
    bool isTapped = false;
    private float tapDuration = 4f;
    [SerializeField]private HoverManager hoverManager;
    [SerializeField] private GameObject buttons;
    [SerializeField] private Sprite[] catImages;
    [SerializeField] private Image catImage;
    [SerializeField] private Scrollbar humorUI;
    [SerializeField] private GameObject humorHandle;
    [SerializeField] private TextMeshProUGUI humorText;
    [SerializeField] private Message messageManager;
    
    void Start()
    {
        ChangeHumor();
    }

    void Update()
    {
        if (humor <= 0)
        {
            GameManagerA.Instance.LoseGame();
        }
        if (humor < 40)
        {
            humorDecreaseValue = 3;
            GetComponent<Image>().sprite = catImages[3];
        }

        if (isFed && !isTapped)
        {
            feededDuration -= Time.deltaTime;
            if (feededDuration <= 0f)
            {
                isFed = false;
                humorDecreaseValue = 2;
                feededDuration = 10f;
                catImage.sprite = catImages[0];
            }
        }
        else
        {
            timeWithoutFood -= Time.deltaTime;
            if (timeWithoutFood <= 0f)
            {
                humorDecreaseValue = 4;
                catImage.sprite = catImages[2];
                timeWithoutFood = 20f;
            }
        }

        if (isTapped && !isFed)
        {
            tapDuration -= Time.deltaTime;
            if (tapDuration <= 0f)
            {
                isTapped = false;
                tapDuration = 4f;
                humorDecreaseValue = 2;
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
        bool result = EnergyManager.instance.SpendEnergy(10);
        if (!result)
        {
            return;
        }
        AddHumor(15);
        isTapped = true;
        humorDecreaseValue = 0;
    }

    public void Feed()
    {
        bool result = EnergyManager.instance.SpendEnergy(15);
        if (!result)
        {
            return;
        }
        isFed = true;
        humorDecreaseValue = 1; 
        AddHumor(20);
        catImage.sprite = catImages[1];
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

    public void OnTapHover()
    {
        messageManager.OnButtonHover("Cat : Tap", "Humor +15 points. Costs 10 energy.");
    }

    public void OnFeedHover()
    {
        messageManager.OnButtonHover("Cat : Feed", "Humor + 20 points. Costs 15 energy. If not fed for 20 seconds, the cat's humor will decrease 200% faster.");
    }

    public void OnPictureHover()
    {
        messageManager.OnPictureHover("Cat", "Humor loss 2 per 1 second. You have to feed the cat every 20 seconds, otherwise its humor will decrease 200% faster.");
    }
    
}