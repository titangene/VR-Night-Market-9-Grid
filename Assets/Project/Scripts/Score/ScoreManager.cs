using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager> {
    public Text ScoreText;

    private int score = 0;

    void Awake() {
        Reload();
    }

    /// <summary>
    /// 取得 分數
    /// </summary>
    public int Get_Score() {
        return score;
    }

    /// <summary>
    /// 加分：打中一格 + 11 分，全部打中 100 分
    /// </summary>
    public void AddScore() {
        score = score >= 88 ? 100 : score + 11;
        Set_ScoreText();
    }

    /// <summary>
    /// 更新 分數文字
    /// </summary>
    private void Set_ScoreText() {
        ScoreText.text = score.ToString();
    }

    /// <summary>
    /// 重設 分數、分數文字
    /// </summary>
    public void ResetScore() {
        score = 0;          // 重設 分數
        Set_ScoreText();    // 重設 分數文字
    }
}
