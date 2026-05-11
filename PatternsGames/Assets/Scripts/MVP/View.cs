using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class View : MonoBehaviour
{
    [Header("View panel")]
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public Button equipSwordButton;
    public Button equipRingButton;
    public Button drinkPotionButton;
    public Button equipArmorButton;
    public Button equipHelmetButton;
    public Button equipShieldButton;
    public Button resetButton;
    public Button nextStepButton;

    [Header("Presenter panel")]
    public TextMeshProUGUI logText;

    [Header("Model panel")]
    public TextMeshProUGUI modelRawDataText;

    public event Action OnEquipSwordButtonClicked;
    public event Action OnEquipRingButtonClicked;
    public event Action OnDrinkPotionButtonClicked;
    public event Action OnEquipArmorButtonClicked;
    public event Action OnEquipHelmetButtonClicked;
    public event Action OnEquipShieldButtonClicked;
    public event Action OnResetButtonClicked;
    public event Action OnNextStepButtonClicked;

    private void Awake()
    {
        equipSwordButton.onClick.AddListener(() => OnEquipSwordButtonClicked?.Invoke());
        equipRingButton.onClick.AddListener(() => OnEquipRingButtonClicked?.Invoke());
        drinkPotionButton.onClick.AddListener(() => OnDrinkPotionButtonClicked?.Invoke());
        equipArmorButton.onClick.AddListener(() => OnEquipArmorButtonClicked?.Invoke());
        equipHelmetButton.onClick.AddListener(() => OnEquipHelmetButtonClicked?.Invoke());
        equipShieldButton.onClick.AddListener(() => OnEquipShieldButtonClicked?.Invoke());
        resetButton.onClick.AddListener(() => OnResetButtonClicked?.Invoke());
        nextStepButton.onClick.AddListener(() => OnNextStepButtonClicked?.Invoke());

        nextStepButton.interactable = false;
    }

    public void UpdateStatusUI(int attack, int defense) {
        attackText.text = $"{attack}";
        defenseText.text = $"{defense}";
    }

    public void UpdateModelUI(string rawData) {
        modelRawDataText.text = rawData;
    }

    public void AddLog(string message)
    {
        logText.text += "\n" + message;
    }

    public void ClearLog(string rawData)
    {
        UpdateModelUI(rawData);
        logText.text = "\n" + "         Waiting for action...";
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void SetButtonsInteractable(bool interactable) {
        equipSwordButton.interactable = interactable;
        equipRingButton.interactable = interactable;
        drinkPotionButton.interactable = interactable;
        equipArmorButton.interactable = interactable;
        equipHelmetButton.interactable = interactable;
        equipShieldButton.interactable = interactable;
        resetButton.interactable = interactable;
    }

    public void SetButtonsActive(bool active)
    {
        equipSwordButton.gameObject.SetActive(active);
        equipRingButton.gameObject.SetActive(active);
        drinkPotionButton.gameObject.SetActive(active);
        equipArmorButton.gameObject.SetActive(active);
        equipHelmetButton.gameObject.SetActive(active);
        equipShieldButton.gameObject.SetActive(active);
        resetButton.gameObject.SetActive(active);
    }
}