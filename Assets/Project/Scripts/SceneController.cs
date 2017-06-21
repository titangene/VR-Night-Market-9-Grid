using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	public void GoHome() {
		SceneManager.LoadScene("Home");
	}

	public void RestartGame() {
		GameController.Instance.ResetGame();
	}

	public void PlayGame() {
		SceneManager.LoadScene("Game");
	}
}
