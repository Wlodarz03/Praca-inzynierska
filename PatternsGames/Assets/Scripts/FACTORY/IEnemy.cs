public interface IEnemy
{
    public EnemyFactory.EnemyType EnemyType { get; set; }
    void Initialize();
    void TakeDamage(float damage);
}
