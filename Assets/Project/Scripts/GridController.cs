using UnityEngine;

/// <summary>
/// 球打中某宮格時：翻轉該宮格 + 加分
/// </summary>
public class GridController : MonoBehaviour {
    /// <summary>
    /// 該宮格的編號
    /// </summary>
    private int gridObj_ID;
    
    void Start () {
        gridObj_ID = GameController.Instance.get_GirdID(gameObject);
    }
	
    /// <summary>
    /// 該宮格是否開始翻轉
    /// </summary>
	void Update () {
		TurnGrid();
	}
    
    private void OnCollisionEnter(Collision collision) {
        if (IsBall(collision))
            HitGrid(collision);
    }

    /// <summary>
    /// 翻轉該宮格
    /// </summary>
    private void TurnGrid() {
        if (GameController.Instance.Get_GridIsTurn(gridObj_ID)) {
            if (transform.rotation.x > 80)  // 轉到 80 度時停止翻轉
                GameController.Instance.Set_GridIsTurn(gridObj_ID, false);
            else    // 平滑翻轉到約 90 度
                transform.rotation = Quaternion.Lerp(transform.rotation, 
                    GameController.Instance.gridTurnRotation,
                        Time.deltaTime * GameController.Instance.gridRotationSpeed);
        }
    }

    /// <summary>
    /// 是否是球打中某宮格
    /// </summary>
    private bool IsBall(Collision collision) {
        return collision.gameObject.tag == "Ball";
    }

    /// <summary>
    /// 是否打中某宮格
    /// </summary>
    private bool IsHitGrid() {
        return !GameController.Instance.Get_GridIsHit(gridObj_ID);
    }

    /// <summary>
    /// 球打中某宮格時：翻轉該宮格 + 加分
    /// </summary>
    private void HitGrid(Collision collision) {
        GameController.Instance.Set_GridIsTurn(gridObj_ID, true);
        GameController.Instance.AddScore();
    }
}