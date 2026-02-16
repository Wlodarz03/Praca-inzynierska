using UnityEngine;
using System;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private SpriteRenderer turretBase;
    [SerializeField] private SpriteRenderer turretTop;
    [SerializeField] private Sprite[] sprites;
    

    //[Header("Attributes")]
    // [SerializeField] private float range = 1.5f;
    // [SerializeField] private float fireRate = 1f; // Bullets per Second

    private Transform target;
    private float timeUntilFire;
    private ITurret turretLogic;

    public ITurret GetTurretLogic => turretLogic;

    public void Init(ITurret logic)
    {
        turretLogic = logic;
        ApplyVisuals();
    }

    private void Update()
    {
        if (turretLogic == null) return;
        
        if (target == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetInRange())
        {
            target = null;
            return;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / turretLogic.FireRate)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    public int GetCost()
    {
        return turretLogic.Cost;
    }
    
    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletTD bulletScript = bullet.GetComponent<BulletTD>();
        bulletScript.SetTarget(target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, turretLogic.Range, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            Transform best = hits[0].transform;
            Vector3 bestPos = best.position;

            //target = hits[0].transform;

            foreach (var hit in hits)
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy == null) continue;

                if (enemy.GetCurrentPathIndex() > best.GetComponent<Enemy>().GetCurrentPathIndex())
                {
                    best = hit.transform;
                    bestPos = hit.transform.position;
                }
                
            }
            target = best;
        }
    }

    private bool CheckTargetInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= turretLogic.Range;
    }
    
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, 360f * Time.deltaTime);
    }

    void ApplyVisuals()
    {
        turretBase.sprite = sprites[0];
        turretTop.sprite = sprites[1];
        if (turretLogic.Visuals.HasFlag(TurretVisual.FireRate))
        {
            turretBase.sprite = sprites[2];
            turretTop.sprite = sprites[3];
        }

        if (turretLogic.Visuals.HasFlag(TurretVisual.Range))
        {
            turretBase.sprite = sprites[4];
            turretTop.sprite = sprites[5];
        }
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, turretLogic.Range);
    }
}
