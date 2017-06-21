using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour {
    /// <summary>
    /// 九宮格內該格的編號
    /// </summary>
    private int pointObj_No;
    /// <summary>
    /// 九宮格內該格翻轉的角度 x: 90
    /// </summary>
    private Quaternion point_Turn_Rotation = Quaternion.Euler(90, 0, 0);
    /// <summary>
    /// 九宮格內該格是否開始翻轉
    /// </summary>
    private bool isTurn = false;
    void Start () {
        pointObj_No = Set_Point_No();
    }
	
    /// <summary>
    /// 九宮格內該格是否開始翻轉
    /// </summary>
	void Update () {
		TurnPoint();
	}

    /// <summary>
    /// 設定 九宮格內該格的編號
    /// </summary>
    private int Set_Point_No() {
        return pointObj_No = System.Int32.Parse(gameObject.name.Replace("Point", "")) - 1;
    }

    /// <summary>
    /// 翻轉九宮格內的該格
    /// </summary>
    private void TurnPoint() {
        if (isTurn) {
            if (transform.rotation.x > 80)
                isTurn = false;     // 轉道 80 度時停止翻轉
            else
                // 平滑翻轉
                transform.rotation = Quaternion.Lerp(transform.rotation, point_Turn_Rotation,
                    Time.deltaTime * GameController.Instance.Point_Rotation_Speed);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (IsBall(other))
            if (IsHitPoint())   HitPoint();
    }

    /// <summary>
    /// 是否是球打中九宮格內的某格
    /// </summary>
    private bool IsBall(Collider other) {
        return other.name == GameObject.FindWithTag("Ball").name;
    }

    /// <summary>
    /// 是否打中九宮格內的某格
    /// </summary>
    private bool IsHitPoint() {
        return !GameController.Instance.pointStatus[pointObj_No];
    }

    /// <summary>
    /// 球打中九宮格內的某格時：翻轉九宮格內的該格 + 加分
    /// </summary>
    private void HitPoint() {
        isTurn = true;
        print(gameObject.name);
        GameController.Instance.Set_pointStatus(pointObj_No, true);
        GameController.Instance.AddScore();
    }
}