using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 360f;

    [SerializeField] private Slider healthSlider;

    private EnemyData enemyData;
    private Tower targetTower;

    private int currentHealth;
    private int maxHealth;
    private float moveSpeed;
    private int attackDamage;
    private float attackInterval;
    private float attackRange;

    private float attackTimer;
    private Transform[] waypoints;
    private int waypointIndex;

    private bool canMove;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        if (healthSlider != null)
        {
            healthSlider.minValue = 0f;
            healthSlider.maxValue = 1f;
            healthSlider.value = 1f;
        }
    }

    public void Init(
        EnemyData data,
        Transform[] path,
        int healthBonus = 0,
        float speedBonus = 0f)
    {
        if (data == null)
        {
            Debug.LogError("EnemyData 未设置", this);
            return;
        }

        enemyData = data;
        waypoints = path;

        maxHealth = Mathf.Max(
            1,
            data.maxHealth + healthBonus
        );
        currentHealth = maxHealth;

        moveSpeed = Mathf.Max(
            0.1f,
            data.moveSpeed + speedBonus
        );

        attackDamage = data.attackDamage;
        attackInterval = data.attackInterval;
        attackRange = data.attackRange;

        waypointIndex = 0;
        attackTimer = 0f;
        IsDead = false;
        canMove = false;

        gameObject.name = data.enemyName;

        if (waypoints != null &&
            waypoints.Length > 0 &&
            waypoints[0] != null)
        {
            transform.position = waypoints[0].position;
        }

        StartCoroutine(StartWalking());
    }

    private IEnumerator StartWalking()
    {
        SetAnimation(false, false);

        yield return new WaitForSeconds(
            enemyData.idleDuration
        );

        if (!IsDead)
        {
            canMove = true;
            SetAnimation(true, false);
        }
    }

    private void Update()
    {
        if (IsDead || !canMove)
            return;

        FindTower();

        if (targetTower != null)
        {
            AttackTower();
        }
        else
        {
            MoveAlongPath();
        }
    }

    private void FindTower()
    {
        if (targetTower != null &&
            !targetTower.IsDestroyed)
        {
            float currentDistance = Vector3.Distance(
                transform.position,
                targetTower.transform.position
            );

            if (currentDistance <= attackRange)
                return;
        }

        targetTower = null;

        Tower[] towers = FindObjectsByType<Tower>(
            FindObjectsSortMode.None
        );

        float nearestDistance = attackRange;

        foreach (Tower tower in towers)
        {
            if (tower == null || tower.IsDestroyed)
                continue;

            float distance = Vector3.Distance(
                transform.position,
                tower.transform.position
            );

            if (distance <= nearestDistance)
            {
                nearestDistance = distance;
                targetTower = tower;
            }
        }
    }

    private void AttackTower()
    {
        SetAnimation(false, true);
        LookAt(targetTower.transform.position);

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            attackTimer = 0f;
            targetTower.TakeDamage(attackDamage);
        }

        if (targetTower.IsDestroyed)
        {
            targetTower = null;
            SetAnimation(true, false);
        }
    }

    private void MoveAlongPath()
    {
        SetAnimation(true, false);

        if (waypoints == null ||
            waypoints.Length == 0)
        {
            return;
        }

        if (waypointIndex >= waypoints.Length)
        {
            ReachEnd();
            return;
        }

        Transform targetPoint =
            waypoints[waypointIndex];

        if (targetPoint == null)
        {
            waypointIndex++;
            return;
        }

        Vector3 targetPosition =
            targetPoint.position;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        LookAt(targetPosition);

        if (Vector3.Distance(
            transform.position,
            targetPosition) <= 0.05f)
        {
            waypointIndex++;
        }
    }

    private void LookAt(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
            return;

        currentHealth = Mathf.Max(
            0,
            currentHealth - damage
        );

        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;
        canMove = false;

        SetAnimation(false, false);
        animator?.SetTrigger("Die");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddGold(
                enemyData.goldReward
            );
        }

        DisableColliders();

        Destroy(
            gameObject,
            enemyData.deathDuration
        );
    }

    private void ReachEnd()
    {
        if (IsDead)
            return;

        IsDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife(
                enemyData.damageToPlayer
            );
        }

        Destroy(gameObject);
    }

    private void SetAnimation(
        bool walking,
        bool attacking)
    {
        if (animator == null)
            return;

        animator.SetBool(
            "IsWalking",
            walking
        );

        animator.SetBool(
            "IsAttacking",
            attacking
        );
    }

    private void DisableColliders()
    {
        Collider[] colliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider enemyCollider in colliders)
        {
            enemyCollider.enabled = false;
        }
    }
}