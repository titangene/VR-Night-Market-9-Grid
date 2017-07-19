using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 準備時間："預備、3、2、1、GO!!"
/// </summary>
public class PrepareTimer : MonoBehaviour {
    /// <summary>
    /// 準備時間文字
    /// </summary>
    public Text prepareTimer_Text;
    /// <summary>
    /// 初始倒數時間
    /// </summary>
    private int prepareTimer_Start = 3;
    /// <summary>
    /// 目前時間
    /// </summary>
    private int currentTime;

    /// <summary>
    /// 開始準備時間
    /// </summary>
    public IEnumerator Start_PrepareTimer() {
        ResetPrepareTimer();
        yield return new WaitForSeconds(2);

        while (currentTime > 0) {   // 3、2、1，顯示於準備時間文字
            Set_PrepareTimerText(currentTime.ToString());
            yield return new WaitForSeconds(1);
            currentTime--;
        }
        if (currentTime <= 0)       // GO!!，顯示於準備時間文字
            Set_PrepareTimerText("GO!!");

        yield return new WaitForSeconds(2);

        UIController.Instance.Switch_PrepareTimerObj(false);
        TimerManager.Instance.timer.StartTimer();   // 開始記錄該回合總投球時間
        GameController.Instance.StartGame();        // 開始遊戲：可開始投球

        StopCoroutine("Start_PrepareTimer");
    }

    private void Set_PrepareTimerText(string str) {
        prepareTimer_Text.text = str;
    }

    private void ResetPrepareTimer() {
        UIController.Instance.Switch_PrepareTimerObj(true);
        Set_PrepareTimerText("預備");
        currentTime = prepareTimer_Start;   // 設定 初始倒數時間
    }
}