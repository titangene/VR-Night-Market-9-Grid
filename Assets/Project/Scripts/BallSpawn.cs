using UnityEngine;

public class BallSpawn : MonoBehaviour {
    /// <summary>
    /// 球速
    /// </summary>
    public float throw_Power = 8.0f;    // GameController.Instance.randomCtrl.GetRandom(6, 12);

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
        GameController.Instance.ResetGame();
        mainCamera = Camera.main.transform;
        playerPosition = transform.position;
        playerRotation = transform.rotation;
    }

    void Update() {
        if (OnClick_And_hasBall()) {
            GenerateBall();
            ThrowBall();
            GameController.Instance.UpdateBall();
        }
    }

    /// <summary>
    /// 按 Cardboard 按鈕時，如果還有剩球 : true
    /// </summary>
    private bool OnClick_And_hasBall() {
        return GameController.Instance.ball > 0 && Input.GetMouseButtonDown(0);
    }

    /// <summary>
    /// 產生球
    /// </summary>
    private void GenerateBall() {
        ball_Obj = Instantiate(GameController.Instance.BallObj, playerPosition, playerRotation);
    }

    /// <summary>
    /// 投球
    /// </summary>
    private void ThrowBall() {
        ball_RB = ball_Obj.GetComponent<Rigidbody>();
        ball_RB.velocity = mainCamera.forward * throw_Power;
    }
}
