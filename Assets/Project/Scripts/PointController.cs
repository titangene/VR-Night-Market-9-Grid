using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointController : MonoBehaviour {
    private int gameObj_No;
    private bool isTurn = false;
    // Use this for initialization
    void Start () {
        gameObj_No = System.Int32.Parse(gameObject.name.Replace("Point", "")) - 1;
    }
	
	// Update is called once per frame
	void Update () {
		if (isTurn) {
            if (transform.rotation.x > 80) {
                isTurn = false;
            } else {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(90, 0, 0),
                    Time.deltaTime * GameController.Instance.Point_Rotation_Speed);
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.name == GameObject.FindWithTag("Bullet").name) {
            if (GameController.Instance.point[gameObj_No] == 0) {
                isTurn = true;
                print(gameObject.name);
                GameController.Instance.point[gameObj_No] = 1;
                if (GameController.Instance.score == 88) {
                    GameController.Instance.score = 100;
                    GameController.Instance.GameOver.SetActive(true);
                } else
                    GameController.Instance.score += 11;
                GameController.Instance.ScoreText.text = "分數：" + GameController.Instance.score.ToString();
            }
        }
    }
}