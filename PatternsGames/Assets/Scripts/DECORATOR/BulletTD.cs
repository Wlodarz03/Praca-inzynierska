using UnityEngine;

public class BulletTD : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [Header("Attributes")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private int bulletDamage = 1;

    private float lifeTime = 5f;

    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0f)
        {
            Destroy(gameObject);
        }
    } 

    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }
}
