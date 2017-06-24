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

public class GameController : MonoBehaviour {
    public GameObject BallObj, GameOverObj;
    /// <summary>
    /// 所有宮格物件
    /// </summary>
    private GameObject[] GridObjs = new GameObject[9];
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

    private Timer timer;

    private static GameController instance = null;

    public static GameController Instance {
        get {
#if UNITY_EDITOR
            if (instance == null && !Application.isPlaying)
                instance = FindObjectOfType<GameController>();
#endif
            if (instance == null) {
                Debug.LogError("No GameController instance found.  Ensure one exists in the scene, or call "
                    + "GameController.Create() at startup to generate one.\n"
                    + "If one does exist but hasn't called Awake() yet, "
                    + "then this error is due to order-of-initialization.\n"
                    + "In that case, consider moving "
                    + "your first reference to GameController.Instance to a later point in time.\n"
                    + "If exiting the scene, this indicates that the GameController object has already "
                    + "been destroyed.");
            }
            return instance;
        }
    }

    public static void Create() {
        if (instance == null && FindObjectOfType<GameController>() == null) {
            Debug.Log("Creating GameController object");
            var go = new GameObject("GameController", typeof(GameController));
            go.transform.localPosition = Vector3.zero;
            // sdk will be set by Awake().
        }
    }

    void Awake() {
        if (instance == null)
            instance = this;
        if (instance != this) {
            Debug.LogError("There must be only one GameController object in a scene.");
            DestroyImmediate(this);
            return;
        }
        Initial_Set_GridObjsToArray();
        timer = GameObject.FindWithTag("Timer").GetComponent<Timer>();
    }

    /// <summary>
    /// 初始化設定所有宮格物件至 GridObjs 陣列
    /// </summary>
    private void Initial_Set_GridObjsToArray() {
        // 找出所有有 "Grid" Tag 的物件
        GameObject[] _GridObjs = GameObject.FindGameObjectsWithTag("Grid");
        foreach (GameObject GridObj in _GridObjs) {
            int girdID = get_GirdID(GridObj);
            GridObjs[girdID] = GridObj;
        }
    }

    /// <summary>
    /// 重設遊戲：分數、球數、分數文字、球數文字、關閉 GameOver Panel
    /// </summary>
    public void ResetGame() {
        score = 0;                      // 重設 分數
        ball = 9;                       // 重設 球數
        Set_ScoreText();                // 重設 分數文字
        Set_BallText();                 // 重設 球數文字
        ResetAllGrid();                 // 重設 所有宮格的資訊
        ResetAllGridObjRotation();      // 重設 所有宮格的角度
        Switch_GameOver_Panel(false);   // 關閉 GameOver Panel
        timer.StartTimer();
        Debug.Log("ResetGame");
    }

    /// <summary>
    /// 遊戲結束：沒球時，開啟 GameOver Panel
    /// </summary>
    public void GameOver() {
        timer.StopTimer();
        Switch_GameOver_Panel(true);
        Debug.Log("GameOver");
    }

    /// <summary>
    /// 更新 球數
    /// </summary>
    public void UpdateBall() {
        ball--;             // 球數 - 1
        Set_BallText();     // 更新球數
        if (ball == 0)      // 如果沒球時，延遲 2 秒後開啟 GameOver Panel
            Invoke("GameOver", 2);
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
    /// 取得 該宮格物件的編號，EX：原本叫 Grid5 -> 刪掉 "Grid" -> 取得 5 -> 5 - 1 = 4
    /// </summary>
    public int get_GirdID(GameObject gameObj) {
        return System.Int32.Parse(gameObj.name.Replace("Grid", "")) - 1;
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

    void OnDestroy() {
        if (instance == this)
            instance = null;
    }
}
