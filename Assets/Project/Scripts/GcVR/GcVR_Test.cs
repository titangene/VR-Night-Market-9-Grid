using UnityEngine;
using UnityEngine.UI;
using GcVR;

public class GcVR_Test : MonoBehaviour {
    public Text TargetObj_PlayerDistance;
    public Text TargetObj;
    
    private GcVR_Gaze gcVR_Gaze;

    void Awake() {
#if !UNITY_EDITOR
        TargetObj_PlayerDistance.enabled = false;
        TargetObj.enabled = false;
#endif
    }

    void Start() {
        Camera MainCamera = Camera.main;
        gcVR_Gaze = MainCamera.GetComponent<GcVR_Gaze>();
    }

    void LateUpdate() {
#if UNITY_EDITOR
        // 目前準心對準的物件
        PrintGazeObj();
#endif
    }

    /// <summary>
    /// Gaze 任何物件時會顯示：(Gaze 某物件、Gaze 的該物件與玩家之間的距離)
    /// </summary>
    private void PrintGazeObj() {
        // 設定 Gaze 某物件 至文字物件上：GameObject
        TargetObj.text = gcVR_Gaze.GetObjName(gcVR_Gaze.CurrentObj_Infinity());
        // 設定 Gaze 的該物件與玩家之間的距離 至文字物件上：GameObject
        TargetObj_PlayerDistance.text =
            gcVR_Gaze.GetTargetObj_Player_Distance().ToString("0.0000");
    }
}
