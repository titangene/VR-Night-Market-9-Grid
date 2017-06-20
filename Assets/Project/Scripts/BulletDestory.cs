using UnityEngine;

public class BulletDestory : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        Destroy(other.gameObject);
    }
}
