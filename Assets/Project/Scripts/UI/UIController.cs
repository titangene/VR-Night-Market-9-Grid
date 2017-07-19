using UnityEngine;

public class UIController : Singleton<UIController> {
    /// <summary>
    /// 準備時間物件
    /// </summary>
    public GameObject prepareTimerObj;
    /// <summary>
    /// 遊戲結束物件
    /// </summary>
    public GameObject GameOverObj;

    void Awake() {
        Reload();
    }

    /// <summary>
    /// 開啟 / 關閉 準備時間物件
    /// </summary>
    public void Switch_PrepareTimerObj(bool active) {
        prepareTimerObj.SetActive(active);
    }

    /// <summary>
    /// 開啟 / 關閉 GameOver Panel
    /// </summary>
    public void Switch_GameOver_Panel(bool isHit) {
        GameOverObj.SetActive(isHit);
    }
}
