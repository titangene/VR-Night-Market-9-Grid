using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour {
    /// <summary>
    /// 該宮格的編號
    /// </summary>
    private int gridObj_No;
    
    /// <summary>
    /// 該宮格是否開始翻轉
    /// </summary>
    //private bool isTurn = false;
    void Start () {
        gridObj_No = Set_GridNo();
    }
	
    /// <summary>
    /// 該宮格是否開始翻轉
    /// </summary>
	void Update () {
		TurnGrid();
	}
    
    private void OnTriggerEnter(Collider other) {
        if (IsBall(other))
            if (IsHitGrid())
                HitGrid();
    }

    /// <summary>
    /// 設定 該宮格的編號
    /// </summary>
    private int Set_GridNo() {
        // EX：原本叫 Grid5 -> 刪掉 Grid -> 取得 5
        return gridObj_No = System.Int32.Parse(gameObject.name.Replace("Grid", "")) - 1;
    }

    /// <summary>
    /// 翻轉該宮格
    /// </summary>
    private void TurnGrid() {
        if (GameController.Instance.Get_GridIsTurn(gridObj_No)) {
            if (transform.rotation.x > 80)  // 轉到 80 度時停止翻轉
                GameController.Instance.Set_GridIsTurn(gridObj_No, false);
            else    // 平滑翻轉到約 90 度
                transform.rotation = Quaternion.Lerp(transform.rotation, 
                    GameController.Instance.gridTurnRotation,
                        Time.deltaTime * GameController.Instance.gridRotationSpeed);
        }
    }

    /// <summary>
    /// 是否是球打中某宮格
    /// </summary>
    private bool IsBall(Collider other) {
        return other.tag == "Ball";
    }

    /// <summary>
    /// 是否打中某宮格
    /// </summary>
    private bool IsHitGrid() {
        return !GameController.Instance.Get_GridIsHit(gridObj_No);
    }

    /// <summary>
    /// 球打中某宮格時：翻轉該宮格 + 加分
    /// </summary>
    private void HitGrid() {
        GameController.Instance.Set_GridIsTurn(gridObj_No, true);
        GameController.Instance.Set_GridIsHit(gridObj_No, true);
        GameController.Instance.AddScore();
        string log = System.String.Format("{0} - {1} - {2}", 
            gameObject.name, GameController.Instance.ball, GameController.Instance.score);
        Debug.Log(log);     // gridName - ball - score
    }
}