using UnityEngine;

/// <summary>
/// 球打中某宮格時：翻轉該宮格 + 加分
/// </summary>
public class GridController : Singleton<GridController> {
    void Awake() {
        Reload();
    }

    /// <summary>
    /// 翻轉該宮格
    /// </summary>
    public void TurnGrid(int grid_ID, Transform GridObj) {
        if (GridManager.Instance.Get_GridIsTurn(grid_ID)) {
            if (GridObj.eulerAngles.x >= 80)   // 翻轉到 80 度時停止翻轉
                GridManager.Instance.Set_GridIsTurn(grid_ID, false);
            else    // 平滑翻轉到約 90 度
                GridObj.rotation = Quaternion.Lerp(GridObj.rotation,
                    GridManager.Instance.gridTurnRotation,
                        Time.deltaTime * GridManager.Instance.gridRotationSpeed);
        }
    }
}
