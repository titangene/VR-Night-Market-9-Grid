using UnityEngine;

public class OutRangeCollisionEvents : MonoBehaviour {
    void OnCollisionEnter(Collision collision) {
        if (BallManager.Instance.IsBall_CheckTag(collision.gameObject))
            OutRangeController.Instance.Ball_OutRange(collision.gameObject);
    }
}
