using UnityEngine;
/// <summary>
/// 宮格的資訊：宮格編號 (id)、宮格物件 (obj)、是否被丟中 (isHit)、是否已翻轉 (isTurn)
/// </summary>
public class Grid {
    /// <summary>
    /// 宮格編號
    /// </summary>
    public int id;
    /// <summary>
    /// 宮格物件
    /// </summary>
    public GameObject obj;
    /// <summary>
    /// 宮格是否被丟中
    /// </summary>
    public bool isHit { get; set; }
    /// <summary>
    /// 宮格是否已翻轉
    /// </summary>
    public bool isTurn { get; set; }
}