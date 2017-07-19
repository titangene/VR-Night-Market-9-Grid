using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {
    public Player player = new Player();
    /// <summary>
    /// 投球 Script
    /// </summary>
    private BallSpawn ballSpawn;
    /// <summary>
    /// 球速
    /// </summary>
    public float throw_Power = 8.0f;

    void Awake() {
        Reload();
        player.obj = GameObject.FindGameObjectWithTag("Player");
        player.mainCamera = Camera.main.transform;
        player.position = player.obj.transform.position;
        ballSpawn = player.obj.GetComponent<BallSpawn>();
    }

    /// <summary>
    /// 開啟 / 關閉 投球 Script
    /// </summary>
    public void Switch_BallSpawn_Script(bool b_switch) {
        ballSpawn.enabled = b_switch;
    }

    public GameObject Get_PlayerObj() {
        return player.obj;
    }

    public Vector3 Get_PlayerPosition() {
        return player.position;
    }

    public Transform Get_PlayerMainCamera() {
        return player.mainCamera;
    }
}
