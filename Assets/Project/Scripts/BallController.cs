using UnityEngine;

/// <summary>
/// 偵測球是否打中宮格、是否超過範圍 + Print 投球 Log
/// </summary>
public class BallController : MonoBehaviour {
    /// <summary>
    /// 該球的編號
    /// </summary>
    private int ballID;
    /// <summary>
    /// 該宮格的編號
    /// </summary>
    private int gridObj_ID;
    private string log;

    void Start () {
        ballID = GameController.Instance.get_BallID(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        Is_Grid(other);
        Is_OutRange(other);
    }

    /// <summary>
    /// 球是否打中某宮格
    /// </summary>
    private void Is_Grid(Collider other) {
        if (other.tag == "Grid") {
            if (GridHasBeenHit(other) && Equal_BallStatus(BallStatus.Mid_air)) {
                GameController.Instance.Set_GridIsHit(gridObj_ID, true);
                GameController.Instance._ball--;    // 更新球數 (延遲版)
                GameController.Instance.Set_BallStatus_To_Hit(ballID);

                log = System.String.Format("Ball{0} : {1} - 剩 {2} 球 - {3} 分", (ballID + 1),
                other.name, GameController.Instance._ball, GameController.Instance.score);
                Debug.Log(log);     // gridName - ball - score
            }
        }
    }

    /// <summary>
    /// 球是否超過範圍
    /// </summary>
    private void Is_OutRange(Collider other) {
        if (other.tag == "OutRange") {
            if (!Equal_BallStatus(BallStatus.Hit)) {
                GameController.Instance._ball--;    // 更新球數 (延遲版)
                GameController.Instance.Set_BallStatus_To_Lose(ballID);

                log = System.String.Format("Ball{0} : Lose  - 剩 {1} 球",
                    ballID + 1, GameController.Instance._ball);
                Debug.Log(log);
            }
            Destroy(gameObject);  // 刪除超過範圍的球
        }
    }

    /// <summary>
    /// 此宮格是否已被打中
    /// </summary>
    private bool GridHasBeenHit(Collider other) {
        gridObj_ID = GameController.Instance.get_GirdID(other.gameObject);
        return !GameController.Instance.Get_GridIsHit(gridObj_ID);
    }

    /// <summary>
    /// 球的狀態是否等於某狀態
    /// </summary>
    private bool Equal_BallStatus(BallStatus status) {
        return GameController.Instance.Equal_BallStatus(ballID, status);
    }
}
