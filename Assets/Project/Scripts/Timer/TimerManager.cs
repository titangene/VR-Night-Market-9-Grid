using UnityEngine;

public class TimerManager : Singleton<TimerManager> {
    /// <summary>
    /// 準備時間："預備、3、2、1、GO!!"
    /// </summary>
    public PrepareTimer prepareTimer;
    /// <summary>
    /// 碼表：紀錄每回合總投球時間
    /// </summary>
    public Timer timer;

    void Awake() {
        Reload();
        prepareTimer = gameObject.GetComponent<PrepareTimer>();
        timer = gameObject.GetComponent<Timer>();
    }
}
