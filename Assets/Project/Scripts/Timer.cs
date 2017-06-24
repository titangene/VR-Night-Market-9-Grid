using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    /// <summary>
    /// 時間文字
    /// </summary>
    public Text timerText;
    /// <summary>
    /// 碼表是否正在跑
    /// </summary>
    public bool isTimerRun = false;
    /// <summary>
    /// 目前時間字串 (已自訂格式)
    /// </summary>
    private string t_str;
    /// <summary>
    /// 開始時間
    /// </summary>
    private float timer = 0f;
    // 分, 秒, 毫秒
    private float t_min, t_sec, t_ms;

	void Update () {
        UpdateTimer();
    }

    private void UpdateTimer() {
        if (isTimerRun) {
            timer += Time.deltaTime;                    // get current time (s)
            t_min = Mathf.Floor(timer / 60);            // get min
            t_sec = Mathf.Floor(timer % 60);            // get sec
            t_ms = Mathf.Floor((timer * 100) % 100);    // get millisec
            // 70.55s -> 01:10:55
            t_str = string.Format("{0:00}:{1:00}:{2:00}", t_min, t_sec, t_ms);
            timerText.text = t_str;     // 更新時間文字
        }
    }

    public void StartTimer() {
        timer = 0f;                     // 重設 開始時間
        timerText.text = "00:00:00";    // 重設 時間文字
        isTimerRun = true;
    }

    public void StopTimer() {
        isTimerRun = false;
    }
}
