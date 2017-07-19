using UnityEngine;

public class BallRotation : MonoBehaviour {
    private Rigidbody ball_RB;
    /// <summary>
    /// 旋轉角度
    /// </summary>
    private Vector3 rotationAngle;

    void Awake () {
        ball_RB = gameObject.GetComponent<Rigidbody>();
        rotationAngle = new Vector3(GetRandom(), GetRandom(), GetRandom());
    }

	void Update () {
        BallRandomRotation();
    }

    /// <summary>
    /// 球隨機轉
    /// </summary>
    private void BallRandomRotation() {
        Quaternion deltaRotation = Quaternion.Euler(rotationAngle * 
            BallManager.Instance.rotationSpeed * Time.deltaTime);
        ball_RB.MoveRotation(ball_RB.rotation * deltaRotation);
    }

    private float GetRandom() {
        return GlobalManager.Instance.Get_Random_float(
            BallManager.Instance.minRandomAngle, BallManager.Instance.maxRandomAngle);
    }
}
