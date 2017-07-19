using UnityEngine;

/// <summary>
/// 偵測球是否打中宮格、是否超過範圍 + Print 投球 Log
/// </summary>
public class BallCollisionEvents : MonoBehaviour {
    /// <summary>
    /// 該球的編號
    /// </summary>
    private int ballID;

    void Start () {
        ballID = BallManager.Instance.Get_BallID(gameObject);
    }

    void OnCollisionEnter(Collision collision) {
        if (GridManager.Instance.IsGrid_CheckTag(collision)) {
            BallController.Instance.BallIsHit_Grid(collision, ballID);
        }
    }
}
