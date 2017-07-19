using UnityEngine;
using UnityEngine.UI;

public class BallManager : Singleton<BallManager> {
    public GameObject BallPrefabObj;
    public Text BallText;
    public string ballTag = "Ball";

    private Ball[] ball;
    
    /// <summary>
    /// 每回合總球數
    /// </summary>
    private int ballSum = 9;
    /// <summary>
    /// 剩餘球數 (投球時 - 1)
    /// </summary>
    private int remainingBallNum_pitch = 9;
    /// <summary>
    /// 剩餘球數 (球接觸到任何東西時 - 1，EX：宮格、界外區)：
    /// 原本的 ball 變數是在投球瞬間就立即更新，
    /// 當快速投出多顆球 並 Debug.Log(ball) 時，就會造成列印出已更新的剩餘球數
    /// EX：目前剩餘 9 顆球，快速投出 2 顆球且 2 顆都打中宮格時，都會列印出剩餘 7 顆球
    /// </summary>
    private int remainingBallNum_hitSome = 9;
    /// <summary>
    /// 投出第幾顆球
    /// </summary>
    private int ball_Count = 0;

    /// <summary>
    /// 旋轉角度速度
    /// </summary>
    public float rotationSpeed = 1f;
    /// <summary>
    /// 最小亂數角度
    /// </summary>
    public int minRandomAngle = 50;
    /// <summary>
    /// 最大亂數角度
    /// </summary>
    public int maxRandomAngle = 270;

    void Awake() {
        Reload();
    }

    void Start() {
        ResetBall();
    }

    /// <summary>
    /// 更新 球數 + 設定 球的狀態：空中
    /// </summary>
    public void Update_Ball() {
        Set_BallStatus(ball_Count, BallStatus.Mid_air); // 設定 球的狀態：空中
        Minus_remainingBallNum_pitch();         // 更新 剩餘球數 (投球時 - 1)
        Add_Ball_Count();                       // 設定 投出第幾顆球 + 1
        Set_BallText();                         // 更新 球數
        BallController.Instance.WhenNoBall_Close_BallSpawn_Script();
    }

    private void Add_Ball_Count() {
        ball_Count++;
    }

    /// <summary>
    /// 更新 剩餘球數 (投球時 - 1)
    /// </summary>
    public void Minus_remainingBallNum_pitch() {
        remainingBallNum_pitch--;
    }

    /// <summary>
    /// 更新 剩餘球數 (球接觸到任何東西時 - 1，EX：宮格、界外區)
    /// </summary>
    public void Minus_remainingBallNum_hitSome() {
        remainingBallNum_hitSome--;
    }

    /// <summary>
    /// 取得 剩餘球數 (投球時 - 1)
    /// </summary>
    public int Get_remainingBallNum_pitch() {
        return remainingBallNum_pitch;
    }

    /// <summary>
    /// 取得 剩餘球數 (球接觸到任何東西時 - 1，EX：宮格、界外區)
    /// </summary>
    public int Get_remainingBallNum_hitSome() {
        return remainingBallNum_hitSome;
    }

    /// <summary>
    /// 是否 已沒球 (投球時 - 1)
    /// </summary>
    public bool IsNoRemainingBall_pitch() {
        return remainingBallNum_pitch == 0;
    }

    /// <summary>
    /// 是否 已沒球 (球接觸到任何東西時 - 1，EX：宮格、界外區)
    /// </summary>
    public bool IsNoRemainingBall_hitSome() {
        return remainingBallNum_hitSome == 0;
    }

    /// <summary>
    /// 取得 該球物件的編號，EX：原本叫 Ball(Clone)_5 -> 刪掉 "Ball(Clone)_" -> 取得 5 -> 5 - 1 = 4
    /// </summary>
    public int Get_BallID(GameObject BallGameObj) {
        return System.Int32.Parse(BallGameObj.name.Replace("Ball(Clone)_", "")) - 1;
    }

    /// <summary>
    /// 設定 該球物件的編號
    /// </summary>
    public string Set_BallID() {
        return "Ball(Clone)_" + (ball_Count + 1);
    }

    /// <summary>
    /// 更新 球數文字
    /// </summary>
    private void Set_BallText() {
        BallText.text = remainingBallNum_pitch.ToString();
    }

    /// <summary>
    /// 是否是球
    /// </summary>
    public bool IsBall_CheckTag(GameObject obj) {
        return obj.tag == ballTag;
    }

    /// <summary>
    /// 設定第 i 球的狀態 = status
    /// </summary>
    public void Set_BallStatus(int i, BallStatus status) {
        ball[i].status = status;
    }

    /// <summary>
    /// 第 i 球的狀態是否 == status
    /// </summary>
    public bool Compare_BallStatus(int i, BallStatus status) {
        return ball[i].status == status;
    }

    /// <summary>
    /// 重設 所有球資料：剩餘球數、投出第幾顆球、球數文字、狀態
    /// </summary>
    public void ResetBall() {
        remainingBallNum_pitch = 9;     // 重設 剩餘球數 (投球時 - 1)
        remainingBallNum_hitSome = 9;   // 重設 剩餘球數 (球接觸到任何東西時 - 1，EX：宮格、界外區)
        ball_Count = 0;                 // 重設 投出第幾顆球
        Set_BallText();                 // 重設 球數文字
        
        // 重設 所有球資料
        ball = new Ball[ballSum];
        for (int i = 0; i < ballSum; ++i) {
            ball[i] = new Ball();
            ball[i].id = i + 1;
            ball[i].status = BallStatus.None;   // 重設 球的狀態：無
        }
    }
}
