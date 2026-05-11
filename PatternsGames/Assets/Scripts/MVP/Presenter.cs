using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Presenter : MonoBehaviour
{

    [SerializeField] private View view;
    private Model model;

    [SerializeField] private GameObject ViewBorder;
    [SerializeField] private GameObject PresenterBorder;
    [SerializeField] private GameObject ModelBorder;
    [SerializeField] private GameObject VtoPArrow;
    [SerializeField] private GameObject PtoMArrow;
    [SerializeField] private GameObject MtoPArrow;
    [SerializeField] private GameObject PtoVArrow;

    private bool isStepReady = false;

    private void Start()
    {
        model = new Model();
        view.OnEquipSwordButtonClicked += HandleEquipSword;
        view.OnEquipRingButtonClicked += HandleEquipRing;
        view.OnDrinkPotionButtonClicked += HandleDrinkPotion;
        view.OnEquipArmorButtonClicked += HandleEquipArmor;
        view.OnEquipHelmetButtonClicked += HandleEquipHelmet;
        view.OnEquipShieldButtonClicked += HandleEquipShield;

        view.OnResetButtonClicked += HandleReset;
        view.OnNextStepButtonClicked += () => isStepReady = true;

        view.UpdateStatusUI(model.Attack, model.Defense);
        view.UpdateModelUI(model.GetRawData());
        view.ClearLog(model.GetRawData());
    }

    private void HandleEquipSword() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(EquipItemSequence("Sword", "equip", () => model.EquipSword()));
    }

    private void HandleEquipRing() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(EquipItemSequence("Ring", "equip", () => model.EquipRing()));
    }

    private void HandleDrinkPotion() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(EquipItemSequence("Potion", "drink", () => model.DrinkPotion()));
    }

    private void HandleEquipArmor() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(EquipItemSequence("Armor", "equip", () => model.EquipArmor()));
    }

    private void HandleEquipHelmet() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(EquipItemSequence("Helmet", "equip", () => model.EquipHelmet()));
    }

    private void HandleEquipShield() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(EquipItemSequence("Shield", "equip", () => model.EquipShield()));
    }

    private void HandleReset() {
        view.SetButtonsActive(false);
        view.nextStepButton.interactable = true;

        StartCoroutine(ResetSequence(() => model.Reset()));
    }

    private void HighlightSection(string section = "") {
        if (section == "View") {
            ViewBorder.SetActive(true);
            PresenterBorder.SetActive(false);
            ModelBorder.SetActive(false);
        }
        else if (section == "Presenter") {
            ViewBorder.SetActive(false);
            PresenterBorder.SetActive(true);
            ModelBorder.SetActive(false);
        }
        else if (section == "Model") {
            ViewBorder.SetActive(false);
            PresenterBorder.SetActive(false);
            ModelBorder.SetActive(true);
        }
        else {
            ViewBorder.SetActive(false);
            PresenterBorder.SetActive(false);
            ModelBorder.SetActive(false);
        }
    }

    private IEnumerator EquipItemSequence(string itemName, string equipUse, System.Func<bool> equipAction) {
        HighlightSection();
        view.ClearLog(model.GetRawData());
        // Step 1
        view.AddLog($"<color=green>[Step 1]</color>\nView requests for {itemName}");
        HighlightSection("View");
        VtoPArrow.SetActive(true);
        SFX.instance.PlayNotificationSound();
        yield return WaitForKey();

        // Step 2
        view.AddLog($"<color=green>[Step 2]</color>\nPresenter calls Model to {equipUse} {itemName}");
        VtoPArrow.SetActive(false);
        PtoMArrow.SetActive(true);
        HighlightSection("Presenter");
        SFX.instance.PlayNotificationSound();
        yield return WaitForKey();

        // Step 3
        bool success = equipAction.Invoke(); // Metoda w modelu

        if (success)
        {
            // Scenariusz sukcesu
            view.UpdateModelUI(model.GetRawData(itemName));
            view.AddLog($"<color=green>[Step 3]</color>\nModel updates data and send Event");
            PtoMArrow.SetActive(false);
            MtoPArrow.SetActive(true);
            HighlightSection("Model");
            SFX.instance.PlayNotificationSound();
            yield return WaitForKey();

            // Step 4
            view.AddLog($"<color=green>[Step 4]</color>\nPresenter receives Event and updates View");
            MtoPArrow.SetActive(false);
            PtoVArrow.SetActive(true);
            HighlightSection("Presenter");
            SFX.instance.PlayNotificationSound();
            yield return WaitForKey();

            // Step 5
            view.UpdateStatusUI(model.Attack, model.Defense);
            PtoVArrow.SetActive(false);
            HighlightSection("View");
            SFX.instance.PlayNotificationSound();
            view.AddLog($"<color=green>[Step 5]</color>\nView updates UI with new data");
            view.AddLog($"<color=green>[Sequence End]</color>\n{itemName} {equipUse} successfully!");
            if (itemName == "Sword") {
                SFX.instance.PlaySwordEquipSound();
            }
            else if (itemName == "Potion") {
                SFX.instance.PlayPotionDrinkSound();
            }
            else if (itemName == "Shield") {
                SFX.instance.PlayShieldEquipSound();
            }
            else if (itemName == "Armor") {
                SFX.instance.PlayClothingEquipSound();
            }
            else {
                SFX.instance.PlayEquipItemSound();
            }
        }
        else
        {
            // Scenariusz porażki
            view.AddLog($"<color=red>[Step 3]</color>\nModel rejects action, {itemName} is already equipped");
            PtoMArrow.SetActive(false);
            HighlightSection("Model");
            SFX.instance.PlayNotificationSound();
            yield return WaitForKey();

            view.AddLog($"<color=red>[Sequence End]</color>\n{itemName} {equipUse} failed!");
        }

        view.AddLog($"         Waiting for action...");
        // Cleaning
        view.SetButtonsActive(true);
        VtoPArrow.SetActive(false);
        view.nextStepButton.interactable = false;
    }

    private IEnumerator ResetSequence(System.Action resetAction) {
        HighlightSection();
        view.ClearLog(model.GetRawData());

        // Step 1
        view.AddLog($"<color=green>[Step 1]</color>\nView requests for reset");
        VtoPArrow.SetActive(true);
        HighlightSection("View");
        SFX.instance.PlayNotificationSound();
        yield return WaitForKey();

        // Step 2
        view.AddLog($"<color=green>[Step 2]</color>\nPresenter calls Model to reset");
        VtoPArrow.SetActive(false);
        PtoMArrow.SetActive(true);
        HighlightSection("Presenter");
        SFX.instance.PlayNotificationSound();
        yield return WaitForKey();

        // Step 3
        resetAction.Invoke(); // Metoda w modelu
        view.UpdateModelUI(model.GetRawData("Reset"));
        view.AddLog($"<color=green>[Step 3]</color>\nModel updates data and send Event");
        PtoMArrow.SetActive(false);
        MtoPArrow.SetActive(true);
        HighlightSection("Model");
        SFX.instance.PlayNotificationSound();
        yield return WaitForKey();

        // Step 4
        view.AddLog($"<color=green>[Step 4]</color>\nPresenter receives Event and updates View");
        MtoPArrow.SetActive(false);
        PtoVArrow.SetActive(true);
        HighlightSection("Presenter");
        SFX.instance.PlayNotificationSound();
        yield return WaitForKey();

        // Step 5
        view.UpdateStatusUI(model.Attack, model.Defense);
        PtoVArrow.SetActive(false);
        HighlightSection("View");
        SFX.instance.PlayNotificationSound();
        view.AddLog($"<color=green>[Step 5]</color>\nView updates UI with new data");
        view.AddLog($"<color=green>[Sequence End]</color>\nReset successfull!");

        view.AddLog($"         Waiting for action...");
        // Cleaning
        view.SetButtonsActive(true);
        SFX.instance.PlayNotificationSound();
        view.nextStepButton.interactable = false;
    }

    private IEnumerator WaitForKey()
    {
        isStepReady = false;
        yield return new WaitUntil(() => isStepReady);
    }
}