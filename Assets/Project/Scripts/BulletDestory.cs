using UnityEngine;

public class BulletDestory : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
        if (GameController.Instance.ball == 0)
            GameController.Instance.GameOver.SetActive(true);
    }
}
