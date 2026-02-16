using TMPro;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;

    void OnGUI()
    {
        currencyText.text = TowerDefenseManager.Instance.currency.ToString();
    }
}
