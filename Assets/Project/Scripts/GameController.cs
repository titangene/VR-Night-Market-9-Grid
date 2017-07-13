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

/// <summary>
/// 球的狀態：無、空中、打中、沒中
/// </summary>
public enum BallStatus {
    /// <summary>
    /// 無
    /// </summary>
    None,
    /// <summary>
    /// 空中
    /// </summary>
    Mid_air,
    /// <summary>
    /// 打中
    /// </summary>
    Hit,
    /// <summary>
    /// 沒中
    /// </summary>
    Lose
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
    /// 球數 (延遲版，Debug.Log 用)：原本的 ball 變數是在投球瞬間就立即更新，
    /// 當快速投出多顆球 並 Debug.Log(ball) 時，就會造成列印出已更新的剩餘球數
    /// EX：目前剩餘 9 顆球，快速投出 2 顆球且 2 顆都打中宮格時，都會列印出剩餘 7 顆球
    /// </summary>
    public int _ball = 9;
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
    /// 是否可以投球
    /// </summary>
    public bool canThrowBall = false;
    /// <summary>
    /// 亂數
    /// </summary>
    public RandomController randomCtrl = new RandomController();
    /// <summary>
    /// 準備時間："預備、3、2、1、GO!!"
    /// </summary>
    private PrepareTimer prepareTimer;
    /// <summary>
    /// 碼表：紀錄每回合總投球時間
    /// </summary>
    public Timer timer;
    
    /// <summary>
    /// 所有球的狀態：無、空中、打中、沒中
    /// </summary>
    public BallStatus[] balls = new BallStatus[9];
    /// <summary>
    /// 投出第幾顆球
    /// </summary>
    public int ball_Count = 0;

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
        prepareTimer = gameObject.GetComponent<PrepareTimer>();
        timer = gameObject.GetComponent<Timer>();
    }

    void Start() {
        ResetGame();
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
    /// 開始遊戲：可開始投球
    /// </summary>
    public void StartGame() {
        canThrowBall = true;
    }

    /// <summary>
    /// 重設遊戲：分數、球數、分數文字、球數文字、關閉 GameOver Panel
    /// </summary>
    public void ResetGame() {
        canThrowBall = false;           // 設定 不可投球
        score = 0;                      // 重設 分數
        ball = 9;                       // 重設 球數
        _ball = 9;                      // 重設 球數 (延遲版)
        ball_Count = 0;                 // 重設 投出第幾顆球
        Set_ScoreText();                // 重設 分數文字
        Set_BallText();                 // 重設 球數文字
        ResetAllGrid();                 // 重設 所有宮格的資訊：未被丟中 (isHit)、未翻轉 (isTurn)
        ResetAllGridObjRotation();      // 重設 所有宮格的角度：x, y, z = (0, 0, 0)
        ResetAllBallStatus();           // 重設 所有球的狀態：無
        Switch_GameOver_Panel(false);   // 關閉 GameOver Panel
        timer.ResetTimer();             // 重設 碼表：紀錄每回合總投球時間
        StartCoroutine(prepareTimer.Start_PrepareTimer());  // 開始準備時間
        randomCtrl.GeneratorRandom();   // 產生新的亂數 value
        Debug.Log("ResetGame");
    }

    /// <summary>
    /// 遊戲結束：沒球時，開啟 GameOver Panel
    /// </summary>
    public void GameOver() {
        Switch_GameOver_Panel(true);    // 開啟 GameOver Panel
        Debug.Log("GameOver");
    }

    /// <summary>
    /// 更新 球數 + 設定 球的狀態：空中
    /// </summary>
    public void UpdateBall() {
        Set_BallStatus_To_Mid_air();    // 設定 球的狀態：空中
        ball--;                         // 球數 - 1
        ball_Count++;                   // 投出第幾顆球 + 1
        Set_BallText();                 // 更新球數
        if (ball == 0)                  // 如果沒球時，延遲 2 秒後開啟 GameOver Panel
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
    public int get_GirdID(GameObject GridGameObj) {
        return System.Int32.Parse(GridGameObj.name.Replace("Grid", "")) - 1;
    }

    /// <summary>
    /// 設定 該球物件的編號
    /// </summary>
    public string Set_BallID() {
        return "Ball(Clone)_" + (ball_Count + 1);
    }

    /// <summary>
    /// 取得 該球物件的編號，EX：原本叫 Ball(Clone)_5 -> 刪掉 "Ball(Clone)_" -> 取得 5 -> 5 - 1 = 4
    /// </summary>
    public int get_BallID(GameObject BallGameObj) {
        return System.Int32.Parse(BallGameObj.name.Replace("Ball(Clone)_", "")) - 1;
    }

    /// <summary>
    /// 設定 球的狀態：空中
    /// </summary>
    public void Set_BallStatus_To_Mid_air() {
        balls[ball_Count] = BallStatus.Mid_air;
    }

    /// <summary>
    /// 設定 球的狀態：打中
    /// </summary>
    public void Set_BallStatus_To_Hit(int id) {
        balls[id] = BallStatus.Hit;
    }

    /// <summary>
    /// 設定 球的狀態：沒中
    /// </summary>
    public void Set_BallStatus_To_Lose(int id) {
        balls[id] = BallStatus.Lose;
    }

    /// <summary>
    /// 球的狀態是否等於某狀態
    /// </summary>
    public bool Equal_BallStatus(int id, BallStatus ball) {
        return balls[id] == ball;
    }

    /// <summary>
    /// 重設 所有球的狀態：無
    /// </summary>
    private void ResetAllBallStatus() {
        for (int i = 0; i < ball; ++i)
            balls[i] = BallStatus.None;
    }

    /// <summary>
    /// 重設 所有宮格的資訊：未被丟中 (isHit)、未翻轉 (isTurn)
    /// </summary>
    private void ResetAllGrid() {
        grid = new Grid[9];
        for(int i = 0; i < ball; ++i)
            grid[i] = new Grid(false, false);
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
