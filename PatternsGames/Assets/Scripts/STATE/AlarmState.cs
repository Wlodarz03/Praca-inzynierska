using UnityEngine;
public class AlarmState : IEnemyState
{
    private float alarmTime = 1f;
    private float timer = 0f;
    private GameObject alertIcon => enemy.alertIcon;

    public EnemyMovement enemy;

    public AlarmState(EnemyMovement enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        timer = 0f;
        Debug.Log("Enemy entered ALARM state");
        enemy.alertIcon.transform.position = enemy.transform.position + new Vector3(0f, enemy.tileSize * 0.8f, 0f);
        alertIcon.SetActive(true);
    }

    public void Update()
    {
        timer += Time.deltaTime;
        
        if (enemy.PlayerHit())
        {
            Debug.Log("Game over");
            GameManager.Instance.GameOver();
        }

        if (timer >= alarmTime)
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
    }

    public void Exit()
    {
        alertIcon.SetActive(false);
        Debug.Log("Enemy exiting ALARM state");
    }
}