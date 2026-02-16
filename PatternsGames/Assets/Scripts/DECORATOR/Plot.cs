using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    private GameObject tower;
    [SerializeField] private Color hoverColor;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }
    
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
        if (tower == null && BuildManager.Instance.GetCurrentTurret() != null &&
             TowerDefenseManager.Instance.currency >= BuildManager.Instance.GetCurrentTurret().Cost &&
             !TowerDefenseManager.Instance.towerOptionsUI.activeSelf)
        {
            float range = BuildManager.Instance.GetCurrentTurret().Range;
            GameObject rangeIndicator = BuildManager.Instance.rangeIndicatorPrefab;
            rangeIndicator.transform.localScale = new Vector3(range*2, range*2, 1);
            rangeIndicator.transform.position = transform.position;
            if (!rangeIndicator.activeSelf)
            {
                rangeIndicator.SetActive(true);
            }
        }

        else if (tower != null && !TowerDefenseManager.Instance.towerOptionsUI.activeSelf)
        {
            float range = tower.GetComponent<Turret>().GetTurretLogic.Range;
            GameObject rangeIndicator = BuildManager.Instance.rangeIndicatorPrefab;
            rangeIndicator.transform.localScale = new Vector3(range*2, range*2, 1);
            rangeIndicator.transform.position = transform.position;
            if (!rangeIndicator.activeSelf)
            {
                rangeIndicator.SetActive(true);
            }
        }
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
        if (BuildManager.Instance.rangeIndicatorPrefab.activeSelf)
        {
            BuildManager.Instance.rangeIndicatorPrefab.SetActive(false);
        }
    }

    private void OnMouseDown()
    {
        if (tower != null)
        {   
            GameObject g = TowerDefenseManager.Instance.towerOptionsUI;
            if (g.activeSelf)
            {
                g.SetActive(false);
                return;
            }

            g.GetComponent<TurretOptionsUI>().SetTurret(tower);
            g.GetComponent<TurretOptionsUI>().SetTSellText();
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(1.25f,0,0));
            g.transform.position = screenPos;
            TowerDefenseManager.Instance.towerOptionsUI.SetActive(true);
        }
        else
        {
            if (TowerDefenseManager.Instance.towerOptionsUI.activeSelf) return;

            GameObject towerToBuild = BuildManager.Instance.GetSelectedTower();
            if (towerToBuild == null) return;

            int cost = towerToBuild.GetComponent<Turret>().GetCost();
            if (TowerDefenseManager.Instance.currency < cost)
            {
                Debug.Log("Not enough currency to build tower!");
                Destroy(towerToBuild);
                return;
            }
            TowerDefenseManager.Instance.SpendCurrency(cost);
            towerToBuild.transform.position = transform.position;
            tower = towerToBuild;
            BuildManager.Instance.ResetCurrentTurret();
        }
        
    }
}
