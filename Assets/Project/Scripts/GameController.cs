using UnityEngine;

public class GameController : Singleton<GameController> {
    void Awake() {
        Reload();
    }

    void Start() {
        ResetGame();
    }

    /// <summary>
    /// 開始遊戲：可開始投球
    /// </summary>
    public void StartGame() {
        PlayerManager.Instance.Switch_BallSpawn_Script(true);
        Debug.Log("StartGame");
    }

    /// <summary>
    /// 重設遊戲：分數、球數、分數文字、球數文字、關閉 GameOver Panel
    /// </summary>
    public void ResetGame() {
        PlayerManager.Instance.Switch_BallSpawn_Script(false);
        ScoreManager.Instance.ResetScore();
        BallManager.Instance.ResetBall();
        GridManager.Instance.ResetAllGrid();
        GridManager.Instance.ResetAllGridObjRotation();
        UIController.Instance.Switch_GameOver_Panel(false);
        TimerManager.Instance.timer.ResetTimer();
        StartCoroutine(TimerManager.Instance.prepareTimer.Start_PrepareTimer());
        GlobalManager.Instance.randomCtrl.GeneratorRandom();
        Debug.Log("ResetGame");
    }

    /// <summary>
    /// 遊戲結束：沒球時，開啟 GameOver Panel
    /// </summary>
    public void GameOver() {
        UIController.Instance.Switch_GameOver_Panel(true);
    }
}
