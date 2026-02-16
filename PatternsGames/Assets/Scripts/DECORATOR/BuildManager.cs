using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;
    private ITurret currentTurret;
    [SerializeField] private GameObject towerPrefab;
    public GameObject rangeIndicatorPrefab;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ResetCurrentTurret()
    {
        currentTurret = null;
    }

    public void CreateBase()
    {
        currentTurret = new BaseTurret(5f, 1f, 100);
        Debug.Log("Created Base Turret");
    }

    public void AddFireRateUpgrade()
    {
        currentTurret = new FireRateDecorator(new BaseTurret(5f, 1f, 100), 2f, 1f);
        Debug.Log("Added Fire Rate Upgrade" + currentTurret.FireRate);
    }

    public void AddRangeUpgrade()
    {
        currentTurret = new RangeDecorator(new BaseTurret(5f, 1f, 100), 2f, 0.3f);
        Debug.Log("Added Range Upgrade" + currentTurret.Range);
    }

    public ITurret GetCurrentTurret()
    {
        return currentTurret;
    }
    
    public GameObject GetSelectedTower()
    {
        if (currentTurret == null)
        {
            Debug.Log("No turret selected!");
            return null;
        }
        GameObject instance = Instantiate(towerPrefab);
        instance.GetComponent<Turret>().Init(currentTurret);
        return instance;
    }
}
