using UnityEngine;

public class BallSpawn : MonoBehaviour {
    /// <summary>
    /// 球速
    /// </summary>
    public float throw_Power = 8.0f;

    private GameObject ball_Obj;
    private Rigidbody ball_RB;
    private Transform mainCamera;
    /// <summary>
    /// 玩家位置
    /// </summary>
    private Vector3 playerPosition;
    /// <summary>
    /// 玩家角度
    /// </summary>
    private Quaternion playerRotation;

    void Start() {
        mainCamera = Camera.main.transform;
        playerPosition = transform.position;
        playerRotation = transform.rotation;
    }

    void Update() {
        if (IsClickDownGVRBtn()) {
            GenerateBall();
            ThrowBall();
            GameController.Instance.UpdateBall();
        }
    }

    /// <summary>
    /// 產生球
    /// </summary>
    private void GenerateBall() {
        ball_Obj = Instantiate(GameController.Instance.BallObj, playerPosition, playerRotation);
        ball_Obj.name = GameController.Instance.Set_BallID();
    }

    /// <summary>
    /// 投球
    /// </summary>
    private void ThrowBall() {
        ball_RB = ball_Obj.GetComponent<Rigidbody>();
#if !UNITY_EDITOR
        throw_Power = GameController.Instance.randomCtrl.GetRandom(6, 12);
#endif
        ball_RB.velocity = mainCamera.forward * throw_Power;
    }

    /// <summary>
    /// 是否按下 Cardboard 按鈕
    /// </summary>
    private bool IsClickDownGVRBtn() {
        return Input.GetMouseButtonDown(0);
    }
}
