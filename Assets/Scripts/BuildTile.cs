using UnityEngine;

public class BuildTile : MonoBehaviour
{
    public GameObject towerPrefab;
    public int towerCost = 50;
    public Vector3 towerOffset = new Vector3(0, 0.5f, 0);

    private GameObject currentTower;

    private void OnMouseDown()
    {
        if (currentTower != null)
        {
            Debug.Log("该格子已有塔");
            return;
        }

        if (TowerBuildManager.Instance == null ||
            TowerBuildManager.Instance.selectedTowerData == null)
        {
            Debug.Log("请先选择要建造的塔");
            return;
        }

        TowerData towerData =
            TowerBuildManager.Instance.selectedTowerData;

        if (towerData.prefab == null)
        {
            Debug.LogError(towerData.towerName + " 没有配置 Prefab");
            return;
        }

        if (GameManager.Instance == null)
        {
            Debug.LogError("找不到 GameManager");
            return;
        }

        if (!GameManager.Instance.TrySpendGold(towerData.cost))
            return;

        currentTower = Instantiate(
            towerData.prefab,
            transform.position + towerOffset,
            Quaternion.identity
        ).gameObject;

        Debug.Log("建造成功：" + towerData.towerName);
    }
}