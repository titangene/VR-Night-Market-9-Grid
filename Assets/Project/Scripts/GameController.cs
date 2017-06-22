using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 宮格的資訊：是否被丟中 (isHit)、是否已翻轉 (isTurn)
/// </summary>
public class Grid {
    private bool _isHit = false;
    private bool _isTurn = false;
    /// <summary>
    /// 初始化設定 宮格的資訊：是否被丟中 (isHit)、是否已翻轉 (isTurn)
    /// </summary>
    public Grid(bool isHit, bool isTurn) {
        _isHit = isHit;
        _isTurn = isTurn;
    }

    /// <summary>
    /// 宮格是否被丟中
    /// </summary>
    public bool isHit {
        get { return _isHit; }
        set { _isHit = value; }
    }

    /// <summary>
    /// 宮格是否已翻轉
    /// </summary>
    public bool isTurn {
        get { return _isTurn; }
        set { _isTurn = value; }
    }
}

public class GameController : Singleton<GameController> {
    public GameObject BallObj, GameOverObj, GridGroupObj;
    public GameObject[] GridObjs = new GameObject[9];
    public Text BallText, ScoreText;
    public int score = 0;
    /// <summary>
    /// 球數
    /// </summary>
    public int ball = 9;
    /// <summary>
    /// 宮格的資訊：是否被丟中 (isHit)、是否已翻轉 (isTurn)
    /// </summary>
    public Grid[] grid;
    /// <summary>
    /// 該宮格翻轉的角度 x : 90
    /// </summary>
    public Quaternion gridTurnRotation = Quaternion.Euler(90, 0, 0);
    /// <summary>
    /// 宮格被球丟到後，翻轉的速度
    /// </summary>
    public float gridRotationSpeed = 1.7f;

    /// <summary>
    /// 重設遊戲：分數、球數、分數文字、球數文字、關閉 GameOver Panel
    /// </summary>
    public void ResetGame() {
        score = 0;                      // 重設 分數
        ball = 9;                       // 重設 球數
        Set_ScoreText();                // 重設 分數文字
        Set_BallText();                 // 重設 球數文字
        ResetAllGrid();                // 重設 所有宮格的資訊
        ResetAllGridObjRotation();     // 重設 所有宮格的角度
        Switch_GameOver_Panel(false);   // 關閉 GameOver Panel
        Debug.Log("ResetGame");
    }

    /// <summary>
    /// 遊戲結束：沒球時，開啟 GameOver Panel
    /// </summary>
    public void GameOver() {
        if (ball == 0) {
            Switch_GameOver_Panel(true);
            Debug.Log("GameOver");
        }
    }

    /// <summary>
    /// 更新 球數
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
        score = score == 88 ? 100 : score + 11;
        Set_ScoreText();
    }

    private void Set_ScoreText() {
        ScoreText.text = score.ToString();
    }

    /// <summary>
    /// 開啟 / 關閉 GameOver Panel
    /// </summary>
    public void Switch_GameOver_Panel(bool isHit) {
        GameOverObj.SetActive(isHit);
    }

    /// <summary>
    /// 取得 某宮格是否被丟中
    /// </summary>
    public bool Get_GridIsHit(int grid_No) {
        return grid[grid_No].isHit;
    }

    /// <summary>
    /// 取得 某宮格是否已翻轉
    /// </summary>
    public bool Get_GridIsTurn(int grid_No) {
        return grid[grid_No].isTurn;
    }

    /// <summary>
    /// 設定 某宮格是否被丟中
    /// </summary>
    public void Set_GridIsHit(int grid_No, bool isHit) {
        grid[grid_No].isHit = isHit;
    }

    /// <summary>
    /// 設定 某宮格是否已翻轉
    /// </summary>
    public void Set_GridIsTurn(int grid_No, bool isTurn) {
        grid[grid_No].isTurn = isTurn;
    }

    /// <summary>
    /// 重設 所有宮格的資訊：是否被丟中 (isHit)、是否已翻轉 (isTurn)
    /// </summary>
    private void ResetAllGrid() {
        grid = new Grid[9];
        for(int i = 0; i < ball; ++i) {
            grid[i] = new Grid(false, false);
        }
    }

    /// <summary>
    /// 重設 所有宮格的角度
    /// </summary>
    private void ResetAllGridObjRotation() {
        foreach (GameObject GridObj in GridObjs)
            GridObj.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
