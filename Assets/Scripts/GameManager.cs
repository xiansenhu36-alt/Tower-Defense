using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Data")]
    public int gold = 100;
    public int maxLives=10;
    public int currentWave = 0;

    [Header("UI")]
    public TMP_Text goldText;
    public TMP_Text livesText;
    public TMP_Text waveText;
    public int lives;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        lives = maxLives;
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public bool TrySpendGold(int cost)
    {
        if (gold < cost)
        {
            Debug.Log("金币不足");
            return false;
        }

        gold -= cost;
        UpdateUI();
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    public void LoseLife(int amount)
    {
        lives = Mathf.Max(0, lives - amount);
        UpdateUI();

        if (lives <= 0)
        {
            GameObject crystal = GameObject.FindGameObjectWithTag("Crystal");
            if (crystal != null)
            {
                Destroy(crystal);
            }
            Debug.Log("游戏失败");
            Time.timeScale = 0f;
        }
    }

    public void SetWave(int wave)
    {
        currentWave = wave;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (goldText != null)
            goldText.text = "Gold coins: " + gold;

        if (livesText != null)
            livesText.text = "Lives: " + lives;

        if (waveText != null)
            waveText.text = "Wave: " + currentWave;
    }
}