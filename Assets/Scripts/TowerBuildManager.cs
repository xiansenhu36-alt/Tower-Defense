using UnityEngine;

public class TowerBuildManager : MonoBehaviour
{
    public static TowerBuildManager Instance;

    [Header("Selected Tower")]
    public TowerData selectedTowerData;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectTower(TowerData towerData)
    {
        selectedTowerData = towerData;

        if (towerData != null)
        {
            Debug.Log("选择塔：" + towerData.towerName);
        }
    }
}