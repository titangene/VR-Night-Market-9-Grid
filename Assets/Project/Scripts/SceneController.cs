using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadScene("Game");
    }

    public void GoHome() {
        Debug.Log("Home");
		SceneManager.LoadScene("Home");
	}

	public void RestartGame() {
		GameController.Instance.ResetGame();
	}
}
