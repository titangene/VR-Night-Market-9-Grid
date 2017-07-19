using UnityEngine;

public class OutRangeController : Singleton<OutRangeController> {
    void Awake() {
        Reload();
    }

    /// <summary>
    /// 球是否超過範圍
    /// </summary>
    public void Ball_OutRange(GameObject ballObj) {
        int ballID = BallManager.Instance.Get_BallID(ballObj);
        if (!Compare_BallStatus(ballID, BallStatus.Hit)) {
            BallManager.Instance.Set_BallStatus(ballID, BallStatus.Lose);
            BallManager.Instance.Minus_remainingBallNum_hitSome();
            DebugController.Instance.PrintLog_BallIsHit_OutRange(ballID);
            BallController.Instance.WhenNoBall_StopTimer();
        }
        Destroy(ballObj);  // 刪除超過範圍的球
    }

    /// <summary>
    /// 球的狀態是否等於某狀態
    /// </summary>
    private bool Compare_BallStatus(int ballID, BallStatus status) {
        return BallManager.Instance.Compare_BallStatus(ballID, status);
    }
}
