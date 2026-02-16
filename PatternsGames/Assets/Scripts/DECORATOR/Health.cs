using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 5;
    [SerializeField] private int currencyReward = 25;
    private Animator animator; 
    private bool isDestroyed = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.ResetTrigger("Hit");
    }

    public void TakeDamage(int damage)
    {
        hitPoints -= damage;
        Debug.Log("Enemy took " + damage + " damage. Remaining HP: " + hitPoints);
        animator.SetTrigger("Hit");
        
        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroyed.Invoke();
            TowerDefenseManager.Instance.AddCurrency(currencyReward);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }

    public void IncreaseHealth(int amount)
    {
        hitPoints += amount;
    }
}
