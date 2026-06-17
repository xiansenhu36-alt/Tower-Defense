using System;
using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Serializable]
    public class EnemyGroup
    {
        public EnemyData enemyData;

        [Min(1)]
        public int count = 5;

        [Min(0f)]
        public float spawnInterval = 0.8f;
    }

    [Serializable]
    public class Wave
    {
        public string waveName = "Wave";
        public EnemyGroup[] enemyGroups;
    }

    [Header("References")]
    public EnemySpawner enemySpawner;

    [Header("Wave Settings")]
    public Wave[] waves;

    [Min(0f)]
    public float startDelay = 1f;

    [Min(0f)]
    public float timeBetweenWaves = 5f;

    [Header("Difficulty Growth")]
    [Min(0)]
    public int healthGrowthPerWave = 10;

    [Min(0f)]
    public float speedGrowthPerWave = 0.1f;

    private bool isRunning;

    private IEnumerator Start()
    {
        if (enemySpawner == null)
        {
            Debug.LogError(
                "WaveManager 没有设置 EnemySpawner",
                this
            );

            yield break;
        }

        if (waves == null || waves.Length == 0)
        {
            Debug.LogWarning(
                "WaveManager 没有配置波次",
                this
            );

            yield break;
        }

        yield return new WaitForSeconds(startDelay);

        isRunning = true;

        for (int waveIndex = 0;
             waveIndex < waves.Length;
             waveIndex++)
        {
            int waveNumber = waveIndex + 1;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetWave(waveNumber);
            }

            Debug.Log("开始第 " + waveNumber + " 波");

            yield return SpawnWave(
                waves[waveIndex],
                waveIndex
            );

            // 等待当前场景中的敌人全部消失。
            yield return WaitForAllEnemiesDefeated();

            Debug.Log("第 " + waveNumber + " 波结束");

            if (waveIndex < waves.Length - 1)
            {
                yield return new WaitForSeconds(
                    timeBetweenWaves
                );
            }
        }

        isRunning = false;
        Debug.Log("所有波次结束，游戏胜利");
    }

    private IEnumerator SpawnWave(
        Wave wave,
        int waveIndex)
    {
        if (wave == null || wave.enemyGroups == null)
        {
            yield break;
        }

        int healthBonus =
            waveIndex * healthGrowthPerWave;

        float speedBonus =
            waveIndex * speedGrowthPerWave;

        foreach (EnemyGroup group in wave.enemyGroups)
        {
            if (group == null ||
                group.enemyData == null)
            {
                continue;
            }

            for (int i = 0; i < group.count; i++)
            {
                enemySpawner.Spawn(
                    group.enemyData,
                    healthBonus,
                    speedBonus
                );

                yield return new WaitForSeconds(
                    group.spawnInterval
                );
            }
        }
    }

    private IEnumerator WaitForAllEnemiesDefeated()
    {
        while (FindObjectsByType<Enemy>(
                   FindObjectsSortMode.None
               ).Length > 0)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
}