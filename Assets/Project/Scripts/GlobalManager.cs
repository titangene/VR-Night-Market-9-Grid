using UnityEngine;

public class GlobalManager : Singleton<GlobalManager> {
    /// <summary>
    /// 亂數
    /// </summary>
    public RandomController randomCtrl = new RandomController();

    void Awake() {
        Reload();
    }

    /// <summary>
    /// 產生新的 min ~ max 範圍亂數 (float)
    /// </summary>
    public float Get_Random_float(int min, int max) {
        return randomCtrl.Get_Random(min, max);
    }

    /// <summary>
    /// 是否按下 Cardboard 按鈕
    /// </summary>
    public bool IsClickDownGVRBtn() {
        return Input.GetMouseButtonDown(0);
    }
}
