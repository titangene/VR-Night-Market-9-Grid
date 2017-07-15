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
            if (GridHasBeenHit(collision) && Compare_BallStatus(BallStatus.Mid_air)) {
                GameController.Instance.Set_GridIsHit(gridObj_ID, true);
                GameController.Instance.Set_BallStatus(ballID, BallStatus.Hit);
                GameController.Instance.Update_Ball_delay();
                GameController.Instance.PrintLog_BallIsHit_Grid(ballID, collision.gameObject);
                GameController.Instance.WhenNoBall_StopTimer();
            }
        }
    }

    /// <summary>
    /// 球是否超過範圍
    /// </summary>
    private void BallIsHit_OutRange(Collision collision) {
        if (IsOutRange_CheckTag(collision)) {
            if (!Compare_BallStatus(BallStatus.Hit)) {
                GameController.Instance.Set_BallStatus(ballID, BallStatus.Lose);
                GameController.Instance.Update_Ball_delay();
                GameController.Instance.PrintLog_BallIsHit_OutRange(ballID);
                GameController.Instance.WhenNoBall_StopTimer();
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
    private bool Compare_BallStatus(BallStatus status) {
        return GameController.Instance.Compare_BallStatus(ballID, status);
    }
}
