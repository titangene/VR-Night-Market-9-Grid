using UnityEngine;

public class BulletDestory : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);  // 刪除碰觸到範圍外的球
        GameController.Instance.GameOver();
    }
}
