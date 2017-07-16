using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 碼表：紀錄每回合總投球時間
/// </summary>
public class Timer : MonoBehaviour {
    /// <summary>
    /// 碼表時間文字
    /// </summary>
    public Text timerText;
    /// <summary>
    /// 碼表是否正在跑
    /// </summary>
    private bool isTimerRun = false;
    /// <summary>
    /// 目前時間字串 (已自訂格式)
    /// </summary>
    private string t_str;
    /// <summary>
    /// 每回合總投球時間 (s)
    /// </summary>
    private float timer = 0f;
    // 分, 秒
    private int t_min, t_sec, t_ms;

    private System.TimeSpan timeSpan;

    void Update () {
        UpdateTimer();
    }

    private void UpdateTimer() {
        if (isTimerRun) {
            Get_EachRoundTotalPitchingTime();
            Format_Timer();
            Set_TimerText(t_str);
        }
    }

    /// <summary>
    /// 開始記錄該回合總投球時間
    /// </summary>
    public void StartTimer() {
        ResetTimer();
        isTimerRun = true;
    }

    /// <summary>
    /// 停止記錄該回合總投球時間
    /// </summary>
    public void StopTimer() {
        isTimerRun = false;
        Debug.Log("time : " + t_str);
    }

    /// <summary>
    /// 重設 碼表：紀錄每回合總投球時間
    /// </summary>
    public void ResetTimer() {
        timer = 0f;                     // 重設 開始時間
        timerText.text = "00:00:00";    // 重設 時間文字
    }

    /// <summary>
    /// 取得 每回合總投球時間 (s)
    /// </summary>
    private void Get_EachRoundTotalPitchingTime() {
        timer += Time.deltaTime;
    }

    /// <summary>
    /// 自訂時間格式
    /// </summary>
    private void Format_Timer() {
        timeSpan = System.TimeSpan.FromSeconds(timer);
        t_min = timeSpan.Minutes;           // get 分
        t_sec = timeSpan.Seconds;           // get 秒
        t_ms = timeSpan.Milliseconds / 10;  // get 毫秒 (傳回值範圍為 -999 ~ 999，所以需 / 10)
        t_str = string.Format("{0:00}:{1:00}:{2:00}", t_min, t_sec, t_ms);
    }

    /// <summary>
    /// 設定 碼表時間文字
    /// </summary>
    private void Set_TimerText(string str) {
        timerText.text = str;
    }
}
