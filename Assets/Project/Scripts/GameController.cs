using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {
    public GameObject BallObj, GameOverObj;
    public Text BallText, ScoreText;
    public int score = 0;
    /// <summary>
    /// 球數
    /// </summary>
    public int ball = 9;
    /// <summary>
    /// 九宮格內各格是否被丟中
    /// </summary>
    public bool[] pointStatus = 
        new bool[] {false, false, false, false, false, false, false, false, false};
    /// <summary>
    /// 九宮格內各格被球丟到後，翻轉的速度
    /// </summary>
    public float Point_Rotation_Speed = 1.7f;
    
    /// <summary>
    /// 重設遊戲：分數、球數、分數文字、球數文字、關閉 GameOver Panel
    /// </summary>
    public void ResetGame() {
        score = 0;          // 重設分數
        ball = 9;           // 重設球數
        Set_ScoreText();    // 重設分數文字
        Set_BallText();     // 重設球數文字
        Switch_GameOver_Panel(false);  // 關閉 GameOver Panel
    }

    /// <summary>
    /// 遊戲結束：沒球時，開啟 GameOver Panel
    /// </summary>
    public void GameOver() {
        if (ball == 0)
            Switch_GameOver_Panel(true);
    }

    /// <summary>
    /// 更新球數
    /// </summary>
    public void UpdateBall() {
        ball--;             // 球數 - 1
        Set_BallText();     // 更新球數
    }

    private void Set_BallText() {
        BallText.text = ball.ToString();
    }

    /// <summary>
    /// 加分：打中一格 + 11 分，全部打中 100 分
    /// </summary>
    public void AddScore() {
        if (score == 88)    score = 100;
        else                score += 11;
        Set_ScoreText();
    }

    private void Set_ScoreText() {
        ScoreText.text = score.ToString();
    }

    /// <summary>
    /// 開啟 / 關閉 GameOver Panel
    /// </summary>
    public void Switch_GameOver_Panel(bool status) {
        GameOverObj.SetActive(status);
    }

    /// <summary>
    /// 設定 九宮格內某格是否被丟中
    /// </summary>
    public void Set_pointStatus(int point_No, bool status) {
        pointStatus[point_No] = status;
    }
}
