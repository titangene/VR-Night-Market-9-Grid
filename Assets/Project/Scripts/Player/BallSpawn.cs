using UnityEngine;

public class BallSpawn : MonoBehaviour {
    private GameObject ball_Obj;
    private Rigidbody ball_RB;

    void Update() {
        if (GlobalManager.Instance.IsClickDownGVRBtn()) {
            GenerateBall();
            ThrowBall();
            BallManager.Instance.Update_Ball();
        }
    }

    /// <summary>
    /// 產生球
    /// </summary>
    private void GenerateBall() {
        ball_Obj = Instantiate(BallManager.Instance.BallPrefabObj, 
            PlayerManager.Instance.Get_PlayerPosition(), Quaternion.identity);
        ball_Obj.name = BallManager.Instance.Set_BallID();
    }

    /// <summary>
    /// 投球
    /// </summary>
    private void ThrowBall() {
        ball_RB = ball_Obj.GetComponent<Rigidbody>();
#if !UNITY_EDITOR
        PlayerManager.Instance.throw_Power = 
            GlobalManager.Instance.randomCtrl.Get_Random(6, 12);
#endif
        ball_RB.velocity = PlayerManager.Instance.Get_PlayerMainCamera().forward * 
            PlayerManager.Instance.throw_Power;
    }
}
