using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float hitDistance = 0.2f;

    private Enemy target;

    public void Init(
        Enemy newTarget,
        int newDamage,
        float newSpeed)
    {
        target = newTarget;
        damage = newDamage;
        speed = newSpeed;
    }

    private void Update()
    {
        if (target == null || target.IsDead)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetPosition =
            target.transform.position + Vector3.up * 0.5f;

        Vector3 direction =
            targetPosition - transform.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (direction.sqrMagnitude > 0.001f)
        {
            transform.forward = direction.normalized;
        }

        if (Vector3.Distance(
            transform.position,
            targetPosition) <= hitDistance)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (target != null && !target.IsDead)
        {
            target.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}