using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Tower Defense/Enemy Data"
)]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    public string enemyName = "Normal Enemy";

    [Header("Prefab")]
    public Enemy prefab;

    [Header("Stats")]
    [Min(1)]
    public int maxHealth = 30;

    [Min(0.1f)]
    public float moveSpeed = 2f;

    [Min(0)]
    public int goldReward = 10;

    [Min(1)]
    public int damageToPlayer = 1;

    [Header("Attack")]
    [Min(1)]
    public int attackDamage = 10;

    [Min(0.1f)]
    public float attackInterval = 1f;

    [Min(0.1f)]
    public float attackRange = 1.2f;

    [Header("Animation Timing")]
    [Min(0f)]
    public float idleDuration = 1f;

    [Min(0f)]
    public float deathDuration = 1.5f;
}