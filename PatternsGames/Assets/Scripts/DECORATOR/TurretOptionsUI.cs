using TMPro;
using UnityEngine;

public class TurretOptionsUI : MonoBehaviour
{
    private GameObject turret;
    [SerializeField] private TextMeshProUGUI sellText;

    public void SetTSellText()
    {
        sellText.text = "Sell (+" + (turret.GetComponent<Turret>().GetTurretLogic.Cost - 75) + ")";
    }

    public void SetTurret(GameObject t)
    {
        turret = t;
    }

    public void OnClickUpgradeRange()
    {
        int curr = TowerDefenseManager.Instance.currency;
        if (curr < 900)
        {
            Debug.Log("Not enough currency to upgrade range!");
            return;
        }
        turret.GetComponent<Turret>().Init(new RangeDecorator(turret.GetComponent<Turret>().GetTurretLogic, 2f, 0.1f));
        TowerDefenseManager.Instance.SpendCurrency(900);
        Debug.Log("Upgraded Range");
        gameObject.SetActive(false);
    }

    public void OnClickUpgradeFireRate()
    {
        int curr = TowerDefenseManager.Instance.currency;
        if (curr < 400)
        {
            Debug.Log("Not enough currency to upgrade fire rate!");
            return;
        }
        turret.GetComponent<Turret>().Init(new FireRateDecorator(turret.GetComponent<Turret>().GetTurretLogic, 1.5f, 0.3f));
        TowerDefenseManager.Instance.SpendCurrency(400);
        Debug.Log("Upgraded Fire Rate");
        gameObject.SetActive(false);
            
    }

    public void OnClickSell()
    {
        int sellAmount = turret.GetComponent<Turret>().GetTurretLogic.Cost - 75;
        TowerDefenseManager.Instance.AddCurrency(sellAmount);
        Destroy(turret);
        gameObject.SetActive(false);
    }
}