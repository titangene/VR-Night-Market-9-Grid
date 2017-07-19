using UnityEngine;

public class Ball {
    /// <summary>
    /// 球號
    /// </summary>
    public int id;
    /// <summary>
    /// 球物件
    /// </summary>
    public GameObject obj;
    /// <summary>
    /// 球的狀態：無、空中、打中、沒中
    /// </summary>
    public BallStatus status;
}
