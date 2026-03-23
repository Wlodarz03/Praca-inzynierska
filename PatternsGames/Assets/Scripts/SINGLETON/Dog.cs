using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dog : Animal
{
    private int humor = 80;
    private int humorDecreaseValue = 1;
    private float humorDecreaseInterval = 1f;
    private float timeSinceLastDecrease = 0f;
    private float walkDuration = 5f;
    private float ballDuration = 4f;
    private bool isWalking = false;
    private bool isPlayingWithBall = false;

    [SerializeField] private GameObject buttons;
    [SerializeField] private HoverManager hoverManager;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Image dogImage;
    [SerializeField] private Cat cat;
    [SerializeField] private Hamster hamster;
    [SerializeField] private Rabbit rabbit;
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
        if ((rabbit.Humor < 30 || cat.Humor < 30 || hamster.Humor < 30) && !isWalking && !isPlayingWithBall)
        {
            humorDecreaseValue = 2;
            dogImage.sprite = sprites[3];
        }
        else if (!isWalking && !isPlayingWithBall)
        {
            humorDecreaseValue = 1;
            dogImage.sprite = sprites[0];
        }

        if (isWalking)
        {
            walkDuration -= Time.deltaTime;
            if (walkDuration <= 0f)
            {
                isWalking = false;
                walkDuration = 5f;
                dogImage.sprite = sprites[0];
                EnableActions();
                cat.EnableActions();
                hamster.EnableActions();
                rabbit.EnableActions();
            }
        }
        if (isPlayingWithBall)
        {
            ballDuration -= Time.deltaTime;
            if (ballDuration <= 0f)
            {
                isPlayingWithBall = false;
                ballDuration = 4f;
                dogImage.sprite = sprites[0];
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

    public void Ball()
    {
        bool result = EnergyManager.instance.SpendEnergy(12);
        if (!result)
        {
            return;
        }
        dogImage.sprite = sprites[1];
        isPlayingWithBall = true;
        AddHumor(15);
        cat.AddHumor(5);
        hamster.AddHumor(5);
        rabbit.RemoveHumor(3);
    }
    public void Walk()
    {
        bool result = EnergyManager.instance.SpendEnergy(20);
        if (!result)
        {
            return;
        }
        AddHumor(25);
        isWalking = true;
        dogImage.sprite = sprites[2];
        cat.AddHumor(10);
        hamster.AddHumor(10);
        rabbit.AddHumor(10);
        // tutaj wylaczyc opcje klikania u wszystkich przez 5 sekund
        DisableActions();
        hamster.DisableActions();
        cat.DisableActions();
        rabbit.DisableActions();
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

    public void OnBallHover()
    {
        messageManager.OnButtonHover("Dog : Ball", "Humor + 15 points. Costs 12 energy. Cat, Hamster + 5 humor points, Rabbit -3 humor points.");
    }

    public void OnWalkHover()
    {
        messageManager.OnButtonHover("Dog : Walk", "Humor +25 points. Costs 20 energy. Every other animal +10 humor points, but cannot use any actions for 5 seconds.");
    }

    public void OnPictureHover()
    {
        messageManager.OnPictureHover("Dog", "Humor loss 1 per 1 second. If any of the other animals has humor below 30, humor loss increases to 2 per second.");
    }
}