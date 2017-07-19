using UnityEngine;

public class GridCollisionEvents : MonoBehaviour {
    private int gridID;

    void Start() {
        gridID = GridManager.Instance.Get_GirdID(gameObject);
    }

    void Update() {
        GridController.Instance.TurnGrid(gridID, transform);
    }

    /// <summary>
    /// 球打中某宮格時：翻轉該宮格 + 加分
    /// </summary>
    void OnCollisionEnter(Collision collision) {
        if (BallManager.Instance.IsBall_CheckTag(collision.gameObject)) {
            GridManager.Instance.Set_GridIsTurn(gridID, true);
            ScoreManager.Instance.AddScore();
        }
    }
}
