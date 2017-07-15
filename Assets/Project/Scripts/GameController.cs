using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public GameObject BallObj, GameOverObj;
    public Text BallText, ScoreText;

    private GameObject Player;
    /// <summary>
    /// 所有宮格物件
    /// </summary>
    private GameObject[] GridObjs = new GameObject[9];

    private int score = 0;
    /// <summary>
    /// 球數
    /// </summary>
    private int ball = 9;
    /// <summary>
    /// 球數 (延遲版，Debug.Log 用)：原本的 ball 變數是在投球瞬間就立即更新，
    /// 當快速投出多顆球 並 Debug.Log(ball) 時，就會造成列印出已更新的剩餘球數
    /// EX：目前剩餘 9 顆球，快速投出 2 顆球且 2 顆都打中宮格時，都會列印出剩餘 7 顆球
    /// </summary>
    private int ball_delay = 9;
    /// <summary>
    /// 投出第幾顆球
    /// </summary>
    private int ball_Count = 0;
    /// <summary>
    /// 宮格被球丟到後，翻轉的速度
    /// </summary>
    public float gridRotationSpeed = 1.7f;
    /// <summary>
    /// 該宮格翻轉的角度 x : 90
    /// </summary>
    public Quaternion gridTurnRotation = Quaternion.Euler(90, 0, 0);

    /// <summary>
    /// 所有球的狀態：無、空中、打中、沒中
    /// </summary>
    private BallStatus[] balls_Status = new BallStatus[9];

    /// <summary>
    /// 亂數
    /// </summary>
    public RandomController randomCtrl = new RandomController();
    /// <summary>
    /// 宮格的資訊：是否被丟中 (isHit)、是否已翻轉 (isTurn)
    /// </summary>
    private Grid[] grid;
    /// <summary>
    /// 準備時間："預備、3、2、1、GO!!"
    /// </summary>
    private PrepareTimer prepareTimer;
    /// <summary>
    /// 碼表：紀錄每回合總投球時間
    /// </summary>
    private Timer timer;
    /// <summary>
    /// 投球 Script
    /// </summary>
    private BallSpawn ballSpawn;

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
        Player = GameObject.FindGameObjectWithTag("Player");
        ballSpawn = Player.GetComponent<BallSpawn>();
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
        Switch_BallSpawn_Script(true);  // 開啟 投球 Script
        Debug.Log("StartGame");
    }

    /// <summary>
    /// 重設遊戲：分數、球數、分數文字、球數文字、關閉 GameOver Panel
    /// </summary>
    public void ResetGame() {
        Switch_BallSpawn_Script(false); // 關閉 投球 Script
        ResetScore();                   // 重設 分數、分數文字
        ResetBall();                    // 重設 球數、球數 (延遲版)、投出第幾顆球、球數文字
        ResetAllBallStatus();           // 重設 所有球的狀態：無
        ResetAllGrid();                 // 重設 所有宮格的資訊：未被丟中 (isHit)、未翻轉 (isTurn)
        ResetAllGridObjRotation();      // 重設 所有宮格的角度：x, y, z = (0, 0, 0)
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
    public void Update_Ball() {
        Set_BallStatus(ball_Count, BallStatus.Mid_air); // 設定 球的狀態：空中
        ball--;                         // 設定 球數 - 1
        ball_Count++;                   // 設定 投出第幾顆球 + 1
        Set_BallText();                 // 更新 球數
        if (ball == 0) {                // 如果沒球時，延遲 2 秒後開啟 GameOver Panel
            Switch_BallSpawn_Script(false);  // 關閉 投球 Script
            Invoke("GameOver", 2);
        }
    }

    /// <summary>
    /// 更新球數 (延遲版)
    /// </summary>
    public void Update_Ball_delay() {
        ball_delay--;           // 設定 球數 (延遲版) - 1
        if (ball_delay == 0)    // 如果沒球時
            timer.StopTimer();  // 停止記錄該回合總投球時間
    }

    /// <summary>
    /// 球打中宮格時列印 "BallID : GridID - 剩幾球 - 現在幾分"
    /// </summary>
    public void PrintLog_BallIsHit_Grid(int ballID, GameObject gridObj) {
        string _log = System.String.Format("Ball{0} : {1} - 剩 {2} 球 - {3} 分",
                    ballID + 1, gridObj.name, ball_delay, Get_Score());
        Debug.Log(_log);    // ballID : GridID - Ball - Score
    }

    /// <summary>
    /// 球超過範圍時列印 "BallID : Lose - 剩幾球"
    /// </summary>
    public void PrintLog_BallIsHit_OutRange(int ballID) {
        string _log = System.String.Format("Ball{0} : Lose  - 剩 {1} 球",
                    ballID + 1, ball_delay);
        Debug.Log(_log);    // ballID : Lose - ball
    }

    private void Set_BallText() {
        BallText.text = ball.ToString();
    }

    public int Get_Score() {
        return score;
    }

    /// <summary>
    /// 加分：打中一格 + 11 分，全部打中 100 分
    /// </summary>
    public void AddScore() {
        score = score >= 88 ? 100 : score + 11;
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
    /// 開啟 / 關閉 投球 Script
    /// </summary>
    public void Switch_BallSpawn_Script(bool b_switch) {
        ballSpawn.enabled = b_switch;
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
    /// 設定第 i 球的狀態 = status
    /// </summary>
    public void Set_BallStatus(int i, BallStatus status) {
        balls_Status[i] = status;
    }

    /// <summary>
    /// 第 i 球的狀態是否 == status
    /// </summary>
    public bool Equal_BallStatus(int i, BallStatus status) {
        return balls_Status[i] == status;
    }

    /// <summary>
    /// 產生新的 min ~ max 範圍亂數
    /// </summary>
    public int Get_Random(int min, int max) {
        return randomCtrl.Get_Random(min, max);
    }

    /// <summary>
    /// 重設 球數、球數 (延遲版)、投出第幾顆球、球數文字
    /// </summary>
    private void ResetBall() {
        ball = 9;           // 重設 球數
        ball_delay = 9;     // 重設 球數 (延遲版)
        ball_Count = 0;     // 重設 投出第幾顆球
        Set_BallText();     // 重設 球數文字
    }

    /// <summary>
    /// 重設 分數、分數文字
    /// </summary>
    private void ResetScore() {
        score = 0;          // 重設 分數
        Set_ScoreText();    // 重設 分數文字
    }

    /// <summary>
    /// 重設 所有球的狀態：無
    /// </summary>
    private void ResetAllBallStatus() {
        balls_Status = new BallStatus[9];
    }

    /// <summary>
    /// 重設 所有宮格的資訊：未被丟中 (isHit)、未翻轉 (isTurn)
    /// </summary>
    private void ResetAllGrid() {
        grid = new Grid[9];
        for (int i = 0; i < grid.Length; ++i)
            grid[i] = new Grid();
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
