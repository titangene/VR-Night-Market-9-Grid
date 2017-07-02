using UnityEngine;

public class BallRotation : MonoBehaviour {
    /// <summary>
    /// 旋轉角度
    /// </summary>
    private Vector3 rotationAngle;
    /// <summary>
    /// 旋轉角度速度
    /// </summary>
    public float rotationSpeed = 1f;
    /// <summary>
    /// 最小亂數角度
    /// </summary>
    public int minRandomAngle = 50;
    /// <summary>
    /// 最大亂數角度
    /// </summary>
    public int maxRandomAngle = 270;

    private Rigidbody ball_RB;
    private Quaternion deltaRotation;

    void Awake () {
        ball_RB = gameObject.GetComponent<Rigidbody>();
        rotationAngle = new Vector3(GetRandom(), GetRandom(), GetRandom());
    }

	void Update () {
        BallRandomRotation();
    }

    private void BallRandomRotation() {
        deltaRotation = Quaternion.Euler(rotationAngle * rotationSpeed * Time.deltaTime);
        ball_RB.MoveRotation(ball_RB.rotation * deltaRotation);
    }

    private float GetRandom() {
        return GameController.Instance.randomCtrl.GetRandom(minRandomAngle, maxRandomAngle);
    }
}
