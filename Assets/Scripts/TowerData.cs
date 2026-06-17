using UnityEngine;

[CreateAssetMenu(
    fileName = "TowerData",
    menuName = "Tower Defense/Tower Data"
)]
public class TowerData : ScriptableObject
{
    [Header("Identity")]
    public string towerName = "Basic Tower";

    [Header("Prefab")]
    public Tower prefab;

    [Header("Build")]
    public int cost = 50;

    [Header("Attack")]
    public float attackRange = 4f;
    public float fireRate = 1f;
    public int damage = 10;
    public float projectileSpeed = 8f;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Effect")]
    public TowerEffectType effectType = TowerEffectType.None;
    public float slowPercent = 0.3f;
    public float slowDuration = 2f;
}

public enum TowerEffectType
{
    None,
    Slow,
    Burn,
    Splash
}