using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController> {
    public GameObject Bullet, GameOver;
    public Text BallText, ScoreText;
    public int score = 0;
    public int ball = 9;
    public int[] point = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float Point_Rotation_Speed = 1.7f;
}
