using UnityEngine;

public class BallController : Singleton<BallController> {
    /// <summary>
    /// 該宮格編號
    /// </summary>
    private int gridObj_ID;

    /// <summary>
    /// 當沒球時，關閉 投球 Script
    /// </summary>
    public void WhenNoBall_Close_BallSpawn_Script() {
        if (BallManager.Instance.IsNoRemainingBall_pitch())     // 如果沒球時
            PlayerManager.Instance.Switch_BallSpawn_Script(false);
    }

    /// <summary>
    /// 如果沒球時，停止記錄該回合總投球時間
    /// </summary>
    public void WhenNoBall_StopTimer() {
        if (BallManager.Instance.IsNoRemainingBall_hitSome()) { // 如果沒球時
            TimerManager.Instance.timer.StopTimer();  // 停止記錄該回合總投球時間
            Invoke("GameOver", 1);  // 延遲 1 秒後開啟 GameOver Panel
        }
    }

    /// <summary>
    /// 遊戲結束：沒球時，開啟 GameOver Panel
    /// </summary>
    private void GameOver() {
        GameController.Instance.GameOver();
    }

    /// <summary>
    /// 球打中某宮格：加分 + print log
    /// </summary>
    public void BallIsHit_Grid(Collision collision, int ballID) {
        if (GridHasBeenHit(collision) &&
            BallManager.Instance.Compare_BallStatus(ballID, BallStatus.Mid_air)) {

            GridManager.Instance.Set_GridIsHit(gridObj_ID, true);
            BallManager.Instance.Set_BallStatus(ballID, BallStatus.Hit);
            BallManager.Instance.Minus_remainingBallNum_hitSome();
            ScoreManager.Instance.AddScore();
            DebugController.Instance.PrintLog_BallIsHit_Grid(ballID, collision.gameObject);
            WhenNoBall_StopTimer();
        }
    }

    /// <summary>
    /// 此宮格是否已被打中
    /// </summary>
    private bool GridHasBeenHit(Collision collision) {
        gridObj_ID = GridManager.Instance.Get_GirdID(collision.gameObject);
        return !GridManager.Instance.Get_GridIsHit(gridObj_ID);
    }
}
