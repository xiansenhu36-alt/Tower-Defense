using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Path")]
    public Transform[] waypoints;

    public Enemy Spawn(
        EnemyData enemyData,
        int healthBonus = 0,
        float speedBonus = 0f)
    {
        if (enemyData == null)
        {
            Debug.LogError(
                "EnemySpawner:EnemyData no configured",
                this
            );

            return null;
        }

        if (enemyData.prefab == null)
        {
            Debug.LogError(
                enemyData.enemyName + "no configured Enemy Prefab",
                enemyData
            );

            return null;
        }

        Vector3 spawnPosition = transform.position;
        Quaternion spawnRotation = transform.rotation;

        if (waypoints != null &&
            waypoints.Length > 0 &&
            waypoints[0] != null)
        {
            spawnPosition = waypoints[0].position;
        }

        Enemy enemy = Instantiate(
            enemyData.prefab,
            spawnPosition,
            spawnRotation
        );

        enemy.Init(
            enemyData,
            waypoints,
            healthBonus,
            speedBonus
        );

        return enemy;
    }
}