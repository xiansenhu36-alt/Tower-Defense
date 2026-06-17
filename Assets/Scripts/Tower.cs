using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 4f;
    public float fireRate = 1f;
    public Transform firePoint;
    public Projectile projectilePrefab;

    [Header("Rotation Settings")]
    public float rotationSpeed = 180f;

    [Tooltip("防御塔与目标方向夹角小于该值时允许开火")]
    public float aimTolerance = 5f;

    private Enemy target;
    private float fireTimer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TowerData towerData;

    private int currentHealth;
    private Quaternion initialRotation;
    public bool IsDestroyed { get; private set; }
    private void Awake()
    {
        if (towerData != null)
        {
            attackRange = towerData.attackRange;
            fireRate = towerData.fireRate;
            maxHealth = towerData.maxHealth;
        }
        currentHealth = maxHealth;
        initialRotation = transform.rotation;
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
    }

    public void TakeDamage(int damage)
    {
        if (IsDestroyed || damage <= 0)
            return;

        currentHealth = Mathf.Max(
            0,
            currentHealth - damage
        );
    
        Debug.Log(
            name + " 受到伤害：" + damage +
            "，剩余生命：" + currentHealth
        );

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
        else
        {
            Debug.LogWarning("Tower 没有绑定 HealthSlider", this);
        }

        if (currentHealth <= 0)
        {
            IsDestroyed = true;
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        FindTarget();

        if (target == null)
        {
            ReturnToInitialRotation();
            return;
        }

        bool isAimed = AimAtTarget();

        if (!isAimed)
            return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= 1f / fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    private void ReturnToInitialRotation()
    {
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            initialRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void FindTarget()
    {
        Enemy[] enemies = FindObjectsByType<Enemy>(
            FindObjectsSortMode.None
        );

        Enemy nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Enemy enemy in enemies)
        {
            if (enemy == null || enemy.IsDead)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                enemy.transform.position
            );

            if (distance <= attackRange && distance < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = distance;
            }
        }

        target = nearestEnemy;
    }

    private bool AimAtTarget()
    {
        Vector3 direction =
            target.transform.position - transform.position;

        // 只进行水平方向旋转
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return true;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );

        float angle = Quaternion.Angle(
            transform.rotation,
            targetRotation
        );

        return angle <= aimTolerance;
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Tower 没有设置 Projectile Prefab");
            return;
        }

        Transform spawnPoint =
            firePoint != null ? firePoint : transform;

        Projectile projectile = Instantiate(
            projectilePrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        projectile.Init(
        target,
        towerData.damage,
        towerData.projectileSpeed
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}