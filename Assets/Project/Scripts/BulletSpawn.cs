using UnityEngine;
using UnityEngine.UI;

public class BulletSpawn : MonoBehaviour {
    public float Throw_Power = 8.0f;    // ran.Next(6, 12)
    public System.Random ran = new System.Random();
    private Rigidbody HitRB;

    void Start() {
        GameController.Instance.ResetGame();
    }

    void Update() {
        ShootBall();
    }

    /// <summary>
    /// 發射子彈
    /// </summary>
    void ShootBall() {
        // 按 Cardboard 按鈕時，如果還有剩球 -> 發射球
        if (GameController.Instance.ball > 0 && Input.GetMouseButtonDown(0)) {
            GameObject obj = Instantiate(GameController.Instance.BallObj, transform.position, transform.rotation);
            HitRB = obj.GetComponent<Rigidbody>();
            HitRB.velocity = Camera.main.transform.forward * Throw_Power;
            GameController.Instance.UpdateBall();
        }
    }
}
