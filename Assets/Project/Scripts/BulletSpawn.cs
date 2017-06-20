using UnityEngine;
using UnityEngine.UI;

public class BulletSpawn : MonoBehaviour {
    private Rigidbody HitRB;
    public float Throw_Power = 8.0f;    // ran.Next(6, 12)
    public System.Random ran = new System.Random();

    void Start() {
        GameController.Instance.score = 0;
        GameController.Instance.ball = 9;
        GameController.Instance.BallText.text = "剩餘球數：" + GameController.Instance.ball.ToString();
        GameController.Instance.ScoreText.text = "分數：" + GameController.Instance.score.ToString();
    }

    void Update() {
        if (GameController.Instance.ball > 0 && Input.GetMouseButtonDown(0)) {
            GameObject obj = Instantiate(GameController.Instance.Bullet, transform.position, transform.rotation);
            HitRB = obj.GetComponent<Rigidbody>();
            HitRB.velocity = Camera.main.transform.forward * Throw_Power;
            GameController.Instance.ball--;
            GameController.Instance.BallText.text = "剩餘球數：" + GameController.Instance.ball.ToString();
        }
    }
}
