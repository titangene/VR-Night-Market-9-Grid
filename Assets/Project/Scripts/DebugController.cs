using UnityEngine;

public class DebugController : Singleton<DebugController> {
    private string Log_BallIsHit_Grid = "Ball{0} : {1} - 剩 {2} 球 - {3} 分";
    private string Log_BallIsHit_OutRange = "Ball{0} : Lose  - 剩 {1} 球";

    void Awake() {
        Reload();
    }

    /// <summary>
    /// 球打中宮格時列印 "BallID : GridID - 剩幾球 - 現在幾分"
    /// </summary>
    public void PrintLog_BallIsHit_Grid(int ballID, GameObject gridObj) {
        string _log = System.String.Format(Log_BallIsHit_Grid, ballID + 1, gridObj.name, 
            BallManager.Instance.Get_remainingBallNum_hitSome(),
            ScoreManager.Instance.Get_Score());
        Debug.Log(_log);    // ballID : GridID - Ball - Score
    }

    /// <summary>
    /// 球超過範圍時列印 "BallID : Lose - 剩幾球"
    /// </summary>
    public void PrintLog_BallIsHit_OutRange(int ballID) {
        string _log = System.String.Format(Log_BallIsHit_OutRange, ballID + 1,
            BallManager.Instance.Get_remainingBallNum_hitSome());
        Debug.Log(_log);    // ballID : Lose - ball
    }
}
