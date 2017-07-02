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

    private string gridTag = "Grid", outRangeTag = "OutRange";
    private string log;

    void Start () {
        ballID = GameController.Instance.get_BallID(gameObject);
    }

    private void OnCollisionEnter(Collision collision) {
        BallIsHit_Grid(collision);
        BallIsHit_OutRange(collision);
    }

    /// <summary>
    /// 球打中某宮格：print log
    /// </summary>
    private void BallIsHit_Grid(Collision collision) {
        if (IsGrid_CheckTag(collision)) {
            if (GridHasBeenHit(collision) && Equal_BallStatus(BallStatus.Mid_air)) {
                GameController.Instance.Set_GridIsHit(gridObj_ID, true);
                GameController.Instance._ball--;    // 更新球數 (延遲版)
                GameController.Instance.Set_BallStatus_To_Hit(ballID);

                log = System.String.Format("Ball{0} : {1} - 剩 {2} 球 - {3} 分", (ballID + 1),
                    collision.gameObject.name, GameController.Instance._ball, GameController.Instance.score);
                Debug.Log(log);     // ballID : gridID - ball - score

                if (GameController.Instance._ball == 0)             // 最後一顆球 Lose 時
                    GameController.Instance.timer.StopTimer();      // 停止記錄該回合總投球時間
            }
        }
    }

    /// <summary>
    /// 球是否超過範圍
    /// </summary>
    private void BallIsHit_OutRange(Collision collision) {
        if (IsOutRange_CheckTag(collision)) {
            if (!Equal_BallStatus(BallStatus.Hit)) {
                GameController.Instance._ball--;    // 更新球數 (延遲版)
                GameController.Instance.Set_BallStatus_To_Lose(ballID);

                log = System.String.Format("Ball{0} : Lose  - 剩 {1} 球",
                    ballID + 1, GameController.Instance._ball);
                Debug.Log(log);     // ballID : Lose - ball

                if (GameController.Instance._ball == 0)             // 最後一顆球 Lose 時
                    GameController.Instance.timer.StopTimer();      // 停止記錄該回合總投球時間
            }
            Destroy(gameObject);  // 刪除超過範圍的球
        }
    }

    /// <summary>
    /// 球是否打中某宮格 (檢查 Tag)
    /// </summary>
    private bool IsGrid_CheckTag(Collision collision) {
        return collision.gameObject.tag == gridTag;
    }

    /// <summary>
    /// 球是否超過範圍 (檢查 Tag)
    /// </summary>
    private bool IsOutRange_CheckTag(Collision collision) {
        return collision.gameObject.tag == outRangeTag;
    }

    /// <summary>
    /// 此宮格是否已被打中
    /// </summary>
    private bool GridHasBeenHit(Collision collision) {
        gridObj_ID = GameController.Instance.get_GirdID(collision.gameObject);
        return !GameController.Instance.Get_GridIsHit(gridObj_ID);
    }

    /// <summary>
    /// 球的狀態是否等於某狀態
    /// </summary>
    private bool Equal_BallStatus(BallStatus status) {
        return GameController.Instance.Equal_BallStatus(ballID, status);
    }
}
