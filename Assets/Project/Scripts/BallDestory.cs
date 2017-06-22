using UnityEngine;

public class BallDestory : MonoBehaviour {
    private string ballTag = "Ball";

    private void OnTriggerEnter(Collider other) {
        if (IsBall(other)) {
            Destroy(other.gameObject);  // 刪除碰觸到範圍外的球
        }
    }

    /// <summary>
    /// 球是否碰觸到範圍外
    /// </summary>
    private bool IsBall(Collider other) {
        return other.tag == ballTag;
    }
}
